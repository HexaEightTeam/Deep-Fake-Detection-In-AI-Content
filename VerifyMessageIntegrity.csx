#r "nuget: HexaEightJWTLibrary, *"
#r "nuget: Newtonsoft.Json, *"

using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json.Linq;

#nullable enable

// Hash email function 
string HashEmail(string email)
{
    using var sha512 = SHA512.Create();
    byte[] emailBytes = Encoding.UTF8.GetBytes(email);
    byte[] hashedEmailBytes = sha512.ComputeHash(emailBytes);
    return BitConverter.ToString(hashedEmailBytes).Replace("-", "").ToLower();
}

// Compute SHA512 hash function
string ComputeSha512Hash(string input)
{
    using var sha512 = SHA512.Create();
    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
    byte[] hashBytes = sha512.ComputeHash(inputBytes);
    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
}

string EscapeQuotes(string input)
{
    // Remove any surrounding quotes or braces
    input = input.Trim('\'', '"', '{', '}');

    // Split the input into key-value pairs
    var pairs = input.Split(',');
    var jsonObject = new JObject();

    foreach (var pair in pairs)
    {
        var parts = pair.Split(':', 2);
        if (parts.Length == 2)
        {
            var key = parts[0].Trim().Trim('"');
            var value = parts[1].Trim().Trim('"');
            jsonObject[key] = value;
        }
    }

    // Return the properly formatted JSON string
    return jsonObject.ToString(Newtonsoft.Json.Formatting.None);
}

if (Args.Count() < 1)
{
    Console.WriteLine("Usage: dotnet script <json-message>");
}
else
{

    var jsonMessage = Args[0];
    var escpjsonMessage = EscapeQuotes(jsonMessage);

    try
    {
        // Parse the JSON message
        var parsedJson = JObject.Parse(escpjsonMessage);


        // Extract the message and signedjwt
        var msgid = parsedJson["Msg ID"]?.ToString();
        var message = parsedJson["message"]?.ToString();
        var signedjwt = parsedJson["signedjwt"]?.ToString();

        if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(signedjwt))
        {
            Console.WriteLine("Error: 'message' or 'signedjwt' is missing in the JSON.");
            return;
        }

	var msghash = ComputeSha512Hash(message);


	var messagejwt = new HexaEightJose.JWT("");
	var kgt = messagejwt.GetKGTFromJWT(signedjwt);
	var doublehash = ComputeSha512Hash(msghash.Trim()+kgt.ToString().Trim());

	var asharedkey = messagejwt.GetUserAuthSharedkeyFromJWT(signedjwt);
	messagejwt.SetHexaEightCredentials("", messagejwt.GetLoginTokenFromJWT(signedjwt), doublehash, "");
	

	dynamic? jsonresp = null;
	string decodeddata = "";
	decodeddata = messagejwt.ValidateTokenUsingSharedKey(signedjwt, asharedkey);		
	jsonresp = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(decodeddata);


	Console.WriteLine($"Message ID : {msgid}");		
        Console.WriteLine($"Message: {message}");
	Console.WriteLine(jsonresp);
	Console.WriteLine("Note : The Message ID in the JSON message should match with the decoded JSON properties above");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Message could not be verified: {ex.Message}");
    }

}
