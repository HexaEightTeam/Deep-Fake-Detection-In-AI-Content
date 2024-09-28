#r "nuget: HexaEightJWTLibrary, *"

using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

// Hash email function 
string HashEmail(string email)
{
    using var sha512 = SHA512.Create();
    byte[] emailBytes = Encoding.UTF8.GetBytes(email);
    byte[] hashedEmailBytes = sha512.ComputeHash(emailBytes);
    return BitConverter.ToString(hashedEmailBytes).Replace("-", "").ToLower();
}

// Generate session code function
string GenerateSessionCode()
{
    using var rng = RandomNumberGenerator.Create();
    byte[] randomBytes = new byte[2]; // 2 bytes for session code
    rng.GetBytes(randomBytes);
    return BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
}


// Compute SHA512 hash function
string ComputeSha512Hash(string input)
{
    using var sha512 = SHA512.Create();
    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
    byte[] hashBytes = sha512.ComputeHash(inputBytes);
    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
}

// Double hash session code function 
string DoubleHashSessionCode(string sessionCode)
{
    string firstHash = ComputeSha512Hash(sessionCode);
    return ComputeSha512Hash(firstHash);
}


string GenerateSignedJwt(string hashedEmail, string msg)
{

    // Fetch environment variables
    var signing_resource_name = Environment.GetEnvironmentVariable("HEXAEIGHT_SIGNINGRESOURCENAME");
    var signing_machinetoken = Environment.GetEnvironmentVariable("HEXAEIGHT_SIGNINGMACHINETOKEN");
    var signing_secret = Environment.GetEnvironmentVariable("HEXAEIGHT_SIGNINGSECRET");
    var signing_licensecode = Environment.GetEnvironmentVariable("HEXAEIGHT_SIGNINGLICENSECODE");

    // Set environment variables for the token service
    Environment.SetEnvironmentVariable("HEXAEIGHT_RESOURCENAME", signing_resource_name);
    Environment.SetEnvironmentVariable("HEXAEIGHT_MACHINETOKEN", signing_machinetoken);
    Environment.SetEnvironmentVariable("HEXAEIGHT_SECRET", signing_secret);
    Environment.SetEnvironmentVariable("HEXAEIGHT_LICENSECODE", signing_licensecode);

    var msghash = ComputeSha512Hash(msg);

    // Get the current timestamp
    Int64 timestampnow = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMinutes;
    Int64 kgt = timestampnow - (timestampnow % 15);

    Int64 timestampnowmsgid = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;

    // Assuming you have access to the HexaEightJose.JWT class
    var tokenservice = new HexaEightJose.JWT(true);

    var doublehash = ComputeSha512Hash(msghash.ToString().Trim()+kgt.ToString().Trim());


    // Generate the pre-shared key
    var skey = tokenservice.HEClient.GetPreSharedKeyByKnownName("::SIGNING!" + hashedEmail + "." + signing_resource_name + "!" + doublehash, kgt.ToString()).ConfigureAwait(false).GetAwaiter().GetResult();

    var sharedkey = skey.Split("!")[0];
    var hrauthsk = skey.Split("!")[1];


    // Add payload items
    tokenservice.AddPayloadItem("Signed By", tokenservice.GetOwner());
    tokenservice.AddPayloadItem("HashAlgorithm", "SHA512");
    tokenservice.AddPayloadItem("Contact : ", "admin@" + tokenservice.GetOwner());
    tokenservice.AddPayloadItem("User : ", hashedEmail);
    tokenservice.AddPayloadItem("MsgId : ", timestampnowmsgid);

    // Sign the document
    var msgid = timestampnowmsgid.ToString();
    var original_resource_name = signing_resource_name;  // Assuming this is the original resource name
    var signedjwt = tokenservice.SignDocument(
        hashedEmail + "." + original_resource_name, 
        525000, 
        kgt, 
        sharedkey, 
        hashedEmail + "." + original_resource_name, 
        hashedEmail + "." + original_resource_name, 
        hrauthsk, 
        ""
    );

    OutputSignedJwtAsJson(msg,signedjwt,timestampnowmsgid.ToString());


    // Return the signed JWT
    return signedjwt;
}

public void OutputSignedJwtAsJson(string message,string signedjwt,string msgid)
{
    // Create a dictionary to hold the message and signed JWT
    var jsonObject = new Dictionary<string, string>
    {
        { "Msg ID", msgid },
        { "message", message },
        { "signedjwt", signedjwt }
    };

    // Serialize the dictionary into a JSON string
    string jsonString = JsonSerializer.Serialize(jsonObject);

    // Output the JSON string
    Console.WriteLine(jsonString);
}


if (Args.Count() < 2)
{
    Console.WriteLine("Usage: dotnet script <Email> <msghash>");
}
else
{
    // Extract hashedEmail and msghash from arguments
    var Email = Args[0];
    var msg = Args[1];
    string hashedEmail = HashEmail(Email);

    // Call GenerateSignedJwt function with the extracted arguments
    var signedjwt = GenerateSignedJwt(hashedEmail, msg);


}
