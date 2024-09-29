# Deep Fake Detection in AI-Generated Content using HexaEight Signature Verification

Deep fake detection is becoming increasingly important as the sophistication of AI-generated content, such as manipulated images, videos, and text, continues to grow. Traditional detection methods often analyze visual artifacts, facial inconsistencies, or audio patterns. However, these approaches can struggle to keep up with rapidly evolving deep fake technologies.

## HexaEight Signature Verification: A Novel Approach

This repository provides a solution for detecting tampering or manipulation in AI-generated content by using **HexaEight Signature Verification**. The approach involves creating a **signed JSON Web Token (JWT)** based on a hash of the AI-generated message, which allows end users to verify the authenticity and integrity of the content.

### How It Works

1.  **Content Generation**: AI generates a piece of content (e.g., text, audio, video).
2.  **Hash Creation**: A SHA512 hash of the generated content is computed.
3.  **JWT Signing**: The hash is used to create a signed JWT using HexaEight’s Patent Pending Encryption Technology
4.  **JWT Association**: The signed JWT is stored alongside the generated content.
5.  **Verification**: End users can decode the JWT and verify the hash against the computed hash of the received content. If the two hashes match, the content is verified as unmodified.

This method provides a robust layer of security, ensuring that any alterations to AI-generated content are detectable, thereby reducing the risk of deep fakes.

## Features

-   **Content Integrity**: Ensure that AI-generated content has not been tampered with after creation.
-   **JWT Signing**: Use a secure, verifiable signature to associate content with its original hash.
-   **Hash Verification**: Compare the original content hash with the hash of the received content for integrity checking.

## Prerequisites

-   [.NET SDK](https://dotnet.microsoft.com/download)
    
-   [dotnet-script tool](https://github.com/dotnet-script/dotnet-script) to run `.csx` scripts
    
    `dotnet tool install -g dotnet-script` 
    
-   NuGet packages referenced in the scripts:
    
   
	#r "nuget: HexaEightJWTLibrary, *"
    #r "nuget: Newtonsoft.Json, *"` 
    

## Installation

1.  Clone this repository.
    
2.  Install the `dotnet-script` tool, which is required to run `.csx` scripts:
    
    `dotnet tool install -g dotnet-script` 
    
3.  Set the required environment variables before running the scripts.


## Usage

### Step 1: Generate Signed JWT for AI-Generated Content

Use the `CreateJWTForMessages.csx` script to generate a signed JWT for any AI-generated message.


`dotnet script CreateJWTForMessages.csx <end-user-email-id> <ai-generated-message>` 

Example:

`dotnet script CreateJWTForMessages.csx "user@example.com" "AI-generated video frame data"` 

This script computes a hash of the AI-generated message, signs it using HexaEightJWTLibrary, and outputs the signed JWT as JSON.

### Step 2: Verify Content Integrity

Use the `VerifyMessageIntegrity.csx` script to verify the integrity of any AI-generated content by comparing the original and received hashes.

bash

Copy code

`dotnet script VerifyMessageIntegrity.csx '<signed-json-message>'` 

This script extracts the signed JWT, decodes it, and verifies the message’s integrity by comparing the message hash.

## Example Workflow

1.  **AI Content Creation**: An AI model generates a video frame or text.
2.  **JWT Creation**: The content's hash is used to create a signed JWT using HexaEight's secure token service.
3.  **Content Sharing**: The generated content and its signed JWT are shared with others.
4.  **Content Verification**: Recipients of the content can use the `VerifyMessageIntegrity.csx` script to check if the content has been altered by verifying the content hash.

## Why Use HexaEight for Deep Fake Detection?

By binding the original content to a signed JWT, **any modification** of the content will result in a hash mismatch. This mismatch will immediately signal that the content is no longer authentic. This method provides an additional layer of trust and security, ensuring that deep fakes or tampered content cannot be easily passed off as genuine.

## Showcase

In the below code, we are asking a question to Gemini and converting the response to base64 that is passed to the CreateJWTForMessage.csx for creating a Signed JWT of the response.

```
epadmin@hexa8cinterpreter:~/gemini$ SIGNED_JWT="$(dotnet script CreateJWTForMessage.csx "bob@anyemail.chat" "$(curl -s "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key=$GOOGLE_API_KEY" -H 'Content-Type: application/json' -X POST -d '{"contents":[{"parts":[{"text":"What is insulin resistance in diabites"}]}]}' | base64 -w 0)" --no-cache)"
```
The contents of the Signed JWT is shown below

```
epadmin@hexa8cinterpreter:~/gemini$ echo $SIGNED_JWT
{
"Msg ID":"1727552365203",
"message":"ewogICJjYW5kaWRhdGVzIjogWwogICAgewogICAgICAiY29udGVudCI6IHsKICAgICAgICAicGFydHMiOiBbCiAgICAgICAgICB7CiAgICAgICAgICAgICJ0ZXh0IjogIkluc3VsaW4gcmVzaXN0YW5jZSBpcyBhIGNvbmRpdGlvbiBpbiB3aGljaCB0aGUgYm9keSdzIGNlbGxzIGRvIG5vdCByZXNwb25kIG5vcm1hbGx5IHRvIHRoZSBob3Jtb25lIGluc3VsaW4uIEluc3VsaW4gaXMgYSBob3Jtb25lIHRoYXQgaGVscHMgZ2x1Y29zZSAoc3VnYXIpIGdldCBmcm9tIHRoZSBibG9vZHN0cmVhbSBpbnRvIGNlbGxzIGZvciBlbmVyZ3kuIFdoZW4gY2VsbHMgYXJlIGluc3VsaW4gcmVzaXN0YW50LCBnbHVjb3NlIGJ1aWxkcyB1cCBpbiB0aGUgYmxvb2RzdHJlYW0gYW5kIGNhbiBsZWFkIHRvIHR5cGUgMiBkaWFiZXRlcy5cblxuSW4gcGVvcGxlIHdpdGggdHlwZSAyIGRpYWJldGVzLCB0aGUgYm9keSdzIGNlbGxzIGRvIG5vdCByZXNwb25kIHByb3Blcmx5IHRvIGluc3VsaW4sIGFuZCBnbHVjb3NlIGJ1aWxkcyB1cCBpbiB0aGUgYmxvb2RzdHJlYW0uIFRoaXMgY2FuIGxlYWQgdG8gYSBudW1iZXIgb2YgY29tcGxpY2F0aW9ucywgaW5jbHVkaW5nOlxuXG4qIEhlYXJ0IGRpc2Vhc2VcbiogU3Ryb2tlXG4qIEtpZG5leSBkaXNlYXNlXG4qIE5lcnZlIGRhbWFnZVxuKiBFeWUgZGFtYWdlXG5cbkluc3VsaW4gcmVzaXN0YW5jZSBpcyBhIGNvbW1vbiBwcm9ibGVtIGluIHBlb3BsZSB3aXRoIG9iZXNpdHkgYW5kIG92ZXJ3ZWlnaHQuIE90aGVyIHJpc2sgZmFjdG9ycyBmb3IgaW5zdWxpbiByZXNpc3RhbmNlIGluY2x1ZGU6XG5cbiogQSBmYW1pbHkgaGlzdG9yeSBvZiB0eXBlIDIgZGlhYmV0ZXNcbiogQmVpbmcgcGh5c2ljYWxseSBpbmFjdGl2ZVxuKiBFYXRpbmcgYSBkaWV0IGhpZ2ggaW4gc2F0dXJhdGVkIGZhdCBhbmQgY2hvbGVzdGVyb2xcblxuVHJlYXRtZW50IGZvciBpbnN1bGluIHJlc2lzdGFuY2UgdHlwaWNhbGx5IGluY2x1ZGVzIGxpZmVzdHlsZSBjaGFuZ2VzLCBzdWNoIGFzIGxvc2luZyB3ZWlnaHQsIGVhdGluZyBhIGhlYWx0aHkgZGlldCwgYW5kIGdldHRpbmcgcmVndWxhciBleGVyY2lzZS4gTWVkaWNhdGlvbnMgbWF5IGFsc28gYmUgcHJlc2NyaWJlZCB0byBoZWxwIGxvd2VyIGJsb29kIGdsdWNvc2UgbGV2ZWxzLiIKICAgICAgICAgIH0KICAgICAgICBdLAogICAgICAgICJyb2xlIjogIm1vZGVsIgogICAgICB9LAogICAgICAiZmluaXNoUmVhc29uIjogIlNUT1AiLAogICAgICAiaW5kZXgiOiAwLAogICAgICAic2FmZXR5UmF0aW5ncyI6IFsKICAgICAgICB7CiAgICAgICAgICAiY2F0ZWdvcnkiOiAiSEFSTV9DQVRFR09SWV9TRVhVQUxMWV9FWFBMSUNJVCIsCiAgICAgICAgICAicHJvYmFiaWxpdHkiOiAiTkVHTElHSUJMRSIKICAgICAgICB9LAogICAgICAgIHsKICAgICAgICAgICJjYXRlZ29yeSI6ICJIQVJNX0NBVEVHT1JZX0hBVEVfU1BFRUNIIiwKICAgICAgICAgICJwcm9iYWJpbGl0eSI6ICJORUdMSUdJQkxFIgogICAgICAgIH0sCiAgICAgICAgewogICAgICAgICAgImNhdGVnb3J5IjogIkhBUk1fQ0FURUdPUllfSEFSQVNTTUVOVCIsCiAgICAgICAgICAicHJvYmFiaWxpdHkiOiAiTkVHTElHSUJMRSIKICAgICAgICB9LAogICAgICAgIHsKICAgICAgICAgICJjYXRlZ29yeSI6ICJIQVJNX0NBVEVHT1JZX0RBTkdFUk9VU19DT05URU5UIiwKICAgICAgICAgICJwcm9iYWJpbGl0eSI6ICJORUdMSUdJQkxFIgogICAgICAgIH0KICAgICAgXQogICAgfQogIF0sCiAgInVzYWdlTWV0YWRhdGEiOiB7CiAgICAicHJvbXB0VG9rZW5Db3VudCI6IDcsCiAgICAiY2FuZGlkYXRlc1Rva2VuQ291bnQiOiAyMDUsCiAgICAidG90YWxUb2tlbkNvdW50IjogMjEyCiAgfQp9Cg==",
"signedjwt":"eyJhbGciOiJIUzI1NiIsImtpZCI6IkFiOVZVN3lNcFh2VU9XRXBEVm5ZSENTV0dWQjRKa0dUNXlYeWVYNTNKa0dHVHhHcDl3UnZvME1UYzA0bWI4a1pvSWlsQ0U2VGRMdWJvNFlHcVk0dVU2SlFHM24rSEtGOWU0VXhxS0UwWkg4TlhFTWFjVDVUYWgyczFnTjFaU3NyOWtMK241MW85TGhnYmdKYkowTUhRRU9ZQmJIWVhrb0lDNllmVU1abkFWU2lVSk1YWUlqUmhnbHcxUjVYWUF4NktsdnJkcTI2eWJUWnRwRjk3bzJvY2JBMFlickNiRUVsUzNLalBHSkZLVDdDajdOV1QwOW1JTlNkSVJoOFRwVlhvbEJsQ1VsRCtZZ1hTRFVuVVo1K1FqVTN3RHpOT00rOE5pQnljd3cwUVJFUUEyTzVkNHd3aFBhc2NnaTBqYzJWWVdLUm5nbDV2SGdQTHpheGNKT1hWS0pRRXFzeUh6d0tkalBMSHVBeWVRZHBTY1NjUDBkdGc3RUVnQSt3bEs0V2JJQXZTOWcwZW5oamtUa0FibHZiWjJPclNEK0lEZkFlS3h6YWxYRUpRMVdpVUdSOXhvRUdxSzRSNUl5ckFFUnVHZ3Btdm1GWWVCVElpYjB6dEZja2xHRWo0NG4zSFJpLzg2Z2FtenEvdmtIK2VIRTVxaVNaQnJwNXI3M293bFJWb2xDM2I4WVJCN1hRdmdxTitIaHdOWkNTVGtxUWtBOFF0SzNxQlE0YkFDbHNTOStyV0lQMkV6QXlJb0dJTmx0NGNGSUd2VmhFTElHRXA4OU94SHZHVXFKUSIsImlzc3VlciI6ImdlbWluaS5oZXhhZWlnaHQuY29tIiwidHlwIjoiSk9TRSIsImNoYW5uZWxTZWN1cml0eUNvbnRleHQiOiJIZXhhRWlnaHRFbmNyeXB0aW9uIiwiaWF0IjoyODc5MjU0MCwiZXhwIjoyOTMxNzUzMCwia2d0IjoyODc5MjUzMCwiTG9naW5Ub2tlbiI6IjNhNjBiNjZkMjE5YWJlNDg3Yjg1OTgwMWE3MDRmNTJhYWY3OGQ1YmFjMWZhODBjNmUyNjE5ZDU5MWVlNDI0MDg2OTI2ZGY2MTBlYTg5NDMwYjFkMzgwMDA2MDhkODBjOTljNzhjM2Y4YmEyMzc0MTg4Y2I1ZDYyMWYxMTBhY2Y2LmdlbWluaS5oZXhhZWlnaHQuY29tIiwiUmVzb3VyY2UiOiIzYTYwYjY2ZDIxOWFiZTQ4N2I4NTk4MDFhNzA0ZjUyYWFmNzhkNWJhYzFmYTgwYzZlMjYxOWQ1OTFlZTQyNDA4NjkyNmRmNjEwZWE4OTQzMGIxZDM4MDAwNjA4ZDgwYzk5Yzc4YzNmOGJhMjM3NDE4OGNiNWQ2MjFmMTEwYWNmNi5nZW1pbmkuaGV4YWVpZ2h0LmNvbSIsIkF1dGhTaGFyZWRrZXkiOiJUQXdCb29vUnE3OWZWV1NoeUpCUmVRVzlXUTZrQVRwSjl1RzY2SWRpT2EzbWhyaGdNdkdIRkF5T1o2NTBULzJJWW9XdDBCdlNUd1M3RjVIVXFRR2ZsbzNNMUNVcFFaWUdSd1ZjTGNpZWFKcXNjL3NwL21oRm1YTjcwcE1UeXlQekNhQnd2NklSYlpyUERqUlRhMWhUWnM1OFpQSFFOQnljQzdhRDNaU1FBRE9kT1FRMkZkdDlrTmNQdXEvUzBUeDlBQlIzM0lRWlhSSlllNnY1dFZGSi9BPT06eWdZR2xuZ1VqdXkrV1IrZjAzdWhvWll0MHFNaFdhbXFLb2dQam1neVB1ZmwwOGhtSHFYM3hCOUxyRmxhK3VkQ0lqTitUZ2hIQ0I3TklueEJZMmw5Tk9HZDh3aTR6QXY0TVQzd0dRNDJ1ZW5kb05uNWNVd3RhcDlCODU4dUhMNEtBdFhnUEY5eXZLcXEzUUIwdDlCSXVlbFh6b1A3K2hXVUYvbEdkQmZZdUw2T3JQeEFyQkhNOXhqZnordXZUU2lENHhzYlh5NVZmTXZTRjRTc0ZOYmZHWEFuUzRCQmg0QU9DSVBkOUlxeDFtMWx3aURvYlFTODdSSVJmWVFVMkRzSEk5SWN6OWZLYnhGTWhHSWU2MHZwMFlOMkVCVEkxUERsRDFTejRlK21jc2ExQS9lQklSSGI1MWlnRXpUZFlzOStKVjljcVJLWDJ0SWZPU2RJcUo2a0d6M1VrYXRZQnF4SERiM2wxRnJ3STNKT3BqMmJlZDEvTWYyU2xiSHpuTzA9OkZOdlNPL281VTlCUlNmbEJodHJuUVd5OWhYNXhjWlA3WG0rNHpZeEVyT1YvV1IwL3dhQ1JqN1FwUkNYd2p3enozL2ZKRzlvR2NrK0dIRXFKRWJia21oQm4waVJuejN5VS9oU2JkcnJhdW5iQ0E1MERXdnZwS1dtQzdDQjZHbHhjajFhTWJrb0hGOUN0WlJQRGM4MVpGWFBQOXE3VFN1VTZBMGFRM2RxRWZWQk5CVnRQb1c1MnRFeU5MNDMxVG9pclp1NkJvejN4d2c0MWFKbUdrdmQwMHV5QVMzSGE0Q1VFQ0p1S2VzSG4xcGNYRU1EYmV5eWVBc09XSVVLU2RHNFdiSDhDb1pOTE9RVkJ4RU9CQVRLZWtqZS9TTkJDaGxmem5qNzF3SGtpWW5LVStCSDh2TjFKTUU2LzJKZEZPbUVDdFVGSHJkclJTbHJBamJGdnI4SzJ5SVZYM0toTzg2ZHc1WVUwVmtvZnVCdGpzNFRKREVVQW5RWGExSndZeGlRPSIsIkNhcHRjaGFJZCI6IiIsInVuc2VjdXJlZCI6dHJ1ZX0.eyJlbmNyeXB0ZWQiOiJZTkdGbU8xUVlBdWtpNHF1TlZJYXRZMHA5bDRvSm01ZzByKzhxUUVkbGdBSkEyTWpKbHFtUjN3NE9pVjJaVEVjT240TGx5OXF5S2pyaXNESFd0UFBVS0pRWDFaUG4vQmJOVjBremNld1R4MmFqZFdHT01Ud2tQdHpMeEpZclRKQUxINE96Z05nUnhIQ1gxNFppQnJTaXMvTmdpOEVzZWpLb3BaZmVxbzdERkdpVUt0RkpKaHVGSlVpTVUyUWp0QTMvRkd2a0RuSDE1eW1UMnJSbHg2WmtMeXk4eGR6TDFLKzJiUTJEMXNTcHBmTVRnV3RSbXVjVGc2QVlORVZBTjlRb2xCVEx1ZUZZS2pycHFZaC9zYXJXc1BKd25HaVp2QnViNFVSYkFvbDZrMjNOL3RhK0NOOGluVksydzd1dDdXU1A1N2VIUUdlUGsyamhRZGYxbjhnVWFKUTRsYjJYUHQ0ZlF0TVJDVWlSZ2tEY3dvK1FzZTBHcktXaVdic2tDKzduejVkaGg2TmtLN1BYV0NobDNCY0NJMnRRU3NjVmRpQlhIdTRXd1dJSWxHaVVJTmFvejdFeUpnUytBdWh2WkMyNjM4Q09pYk5neDFUSjFrUVd5TXFLREY3TkNydHFTOXpWQzJURmdRWTdISkZyNkdPTEZyOFZxUXpkYng5UXN0UW9sQWdKOFFUVXI3TkxOU25GMDdwSzZZOC9UOTRSNXZRejVNZUR1cGZSNW5jUUtFL2pVOTRNTkU5d1FFdW5GUmlsSE0yRW45VkZRbjhnbHBtaDd3WlVhSlFKRnJycWxsTlU1cFdJV3VaRm5xZFNmaUg1aEFjU2MxQW5vWkZ5TTA5cmFJUUNuQm1Da3BScjVKRFJXN3h1V1pocWNCVFd1UlllQm5DQmVUZUdGR2lVUFkrQ0F2Z1FwbWJ0MzE0czU0M0RFZktIdjl3V05BNEJoSk9SSUROWjdwTmhTUk54RGpOR1JGbFlHRmJnQlk0aERiU3NsdG1yaW9QR3lZcmIrRlFvbENPT0hLM01qTzhFKzV0U1ZzTnZvb3RIcXlSZ1FuRzNZWXJ6K2NDeWwxSlZRb0NkVXBpSDRHczVuNVRiVDlRSTIreE1OaHZRcEgzdStRQ3RyRGdVS0pRVk1Ia01RZ1A4NXNJMExzZ2RtdUtLVmhJM1hMU2xVK1c5Y3hvVHB5US9HWjNxWjB5TzZob3d2cXZKcGJUTzF0OTJrSW9rc0ZJMXljTGc4RGJIVkdpVUFkYWlrSkZyelkvNEwzY0xVd0ZaRFJXek9WblZXSmViVGVzUEE2NlZxTEtza2cxb0lrK3puVU5ndEVwd0RZK09hS0p3aWFEdlNxS2gxT0xqaEJSb2xCaEVDc05NaTlKeVE4NEczV2plV2M4Y29yY24vcDZHYTdMYWZoemVEUUxnWHViYXNVK0I1dDJrVVFQbDNCNHpZQTdrZERHaXNyanVEL0QwSERaVUtKUTV3TURWc1ZiRlFxa0p2UFFucVV3aE9KZEpDRWZhU0VIeThJZ1ZxMGtYRjN2a3BSL0Jub2VGejRBTHEzdmNwUkQ0MmFyaUdKWlBWU0VYTHFPMmxDaVVFQ3B2bjY3TmdWcUVKYlBkVGh6d1JRd1JYUko0TWpvdVNOMmFKTlZtMVBFRHlrYnN0UUlWYUhhaFpOZ01rNWRBbGxFaHE3R0dkWE9pVWFsbmg5Um9sQmtjelNBU0dJSzBrakFUYzR1cmthaWtaaERmRGU4UUdhM1cxdXVuRWN4QU5oZE8wdHh2Y0NzWVFHcmNVS29DbWt3SFo5TDZncVJBS05Fa1ZQNlVLSlFGREtjYnlDMXVoZ0NSYVN3T0xuQWoxSTFIRm1JdHp1SVBqNWFSSXFqV3BFeWN5YWl1a0ZpcG02RldGMENSVlZPVXF6dkJqeHl3cGRXTWVoSTIxQ2lVQT09In0.cKIKx0LyAwBVO_BxD5HlQRg2DndSJQxkJYmBoW1XL9U"
}
```
In the subsequent step, we utilize the VerifyMessageIntegrity code to confirm that the received JSON message originates from Gemini AI. Upon successful verification, the contents are decoded for your review. This process guarantees that the AI-generated response remains unaltered.

Additionally, this method enables us to identify the intended recipient of the AI content. The SHA512 hash of the user is included in the signed message, facilitating content tracking and assisting in the fight against misinformation that users may exploit for harmful purposes.

```
epadmin@hexa8cinterpreter:~/gemini$ dotnet script VerifyMessageIntegrity.csx "$SIGNED_JWT" --no-cache && MESSAGE=$(echo $SIGNED_JWT | jq -r '.message') && echo $MESSAGE | base64 --decode
Message ID : 1727552365203
Message: ewogICJjYW5kaWRhdGVzIjogWwogICAgewogICAgICAiY29udGVudCI6IHsKICAgICAgICAicGFydHMiOiBbCiAgICAgICAgICB7CiAgICAgICAgICAgICJ0ZXh0IjogIkluc3VsaW4gcmVzaXN0YW5jZSBpcyBhIGNvbmRpdGlvbiBpbiB3aGljaCB0aGUgYm9keSdzIGNlbGxzIGRvIG5vdCByZXNwb25kIG5vcm1hbGx5IHRvIHRoZSBob3Jtb25lIGluc3VsaW4uIEluc3VsaW4gaXMgYSBob3Jtb25lIHRoYXQgaGVscHMgZ2x1Y29zZSAoc3VnYXIpIGdldCBmcm9tIHRoZSBibG9vZHN0cmVhbSBpbnRvIGNlbGxzIGZvciBlbmVyZ3kuIFdoZW4gY2VsbHMgYXJlIGluc3VsaW4gcmVzaXN0YW50LCBnbHVjb3NlIGJ1aWxkcyB1cCBpbiB0aGUgYmxvb2RzdHJlYW0gYW5kIGNhbiBsZWFkIHRvIHR5cGUgMiBkaWFiZXRlcy5cblxuSW4gcGVvcGxlIHdpdGggdHlwZSAyIGRpYWJldGVzLCB0aGUgYm9keSdzIGNlbGxzIGRvIG5vdCByZXNwb25kIHByb3Blcmx5IHRvIGluc3VsaW4sIGFuZCBnbHVjb3NlIGJ1aWxkcyB1cCBpbiB0aGUgYmxvb2RzdHJlYW0uIFRoaXMgY2FuIGxlYWQgdG8gYSBudW1iZXIgb2YgY29tcGxpY2F0aW9ucywgaW5jbHVkaW5nOlxuXG4qIEhlYXJ0IGRpc2Vhc2VcbiogU3Ryb2tlXG4qIEtpZG5leSBkaXNlYXNlXG4qIE5lcnZlIGRhbWFnZVxuKiBFeWUgZGFtYWdlXG5cbkluc3VsaW4gcmVzaXN0YW5jZSBpcyBhIGNvbW1vbiBwcm9ibGVtIGluIHBlb3BsZSB3aXRoIG9iZXNpdHkgYW5kIG92ZXJ3ZWlnaHQuIE90aGVyIHJpc2sgZmFjdG9ycyBmb3IgaW5zdWxpbiByZXNpc3RhbmNlIGluY2x1ZGU6XG5cbiogQSBmYW1pbHkgaGlzdG9yeSBvZiB0eXBlIDIgZGlhYmV0ZXNcbiogQmVpbmcgcGh5c2ljYWxseSBpbmFjdGl2ZVxuKiBFYXRpbmcgYSBkaWV0IGhpZ2ggaW4gc2F0dXJhdGVkIGZhdCBhbmQgY2hvbGVzdGVyb2xcblxuVHJlYXRtZW50IGZvciBpbnN1bGluIHJlc2lzdGFuY2UgdHlwaWNhbGx5IGluY2x1ZGVzIGxpZmVzdHlsZSBjaGFuZ2VzLCBzdWNoIGFzIGxvc2luZyB3ZWlnaHQsIGVhdGluZyBhIGhlYWx0aHkgZGlldCwgYW5kIGdldHRpbmcgcmVndWxhciBleGVyY2lzZS4gTWVkaWNhdGlvbnMgbWF5IGFsc28gYmUgcHJlc2NyaWJlZCB0byBoZWxwIGxvd2VyIGJsb29kIGdsdWNvc2UgbGV2ZWxzLiIKICAgICAgICAgIH0KICAgICAgICBdLAogICAgICAgICJyb2xlIjogIm1vZGVsIgogICAgICB9LAogICAgICAiZmluaXNoUmVhc29uIjogIlNUT1AiLAogICAgICAiaW5kZXgiOiAwLAogICAgICAic2FmZXR5UmF0aW5ncyI6IFsKICAgICAgICB7CiAgICAgICAgICAiY2F0ZWdvcnkiOiAiSEFSTV9DQVRFR09SWV9TRVhVQUxMWV9FWFBMSUNJVCIsCiAgICAgICAgICAicHJvYmFiaWxpdHkiOiAiTkVHTElHSUJMRSIKICAgICAgICB9LAogICAgICAgIHsKICAgICAgICAgICJjYXRlZ29yeSI6ICJIQVJNX0NBVEVHT1JZX0hBVEVfU1BFRUNIIiwKICAgICAgICAgICJwcm9iYWJpbGl0eSI6ICJORUdMSUdJQkxFIgogICAgICAgIH0sCiAgICAgICAgewogICAgICAgICAgImNhdGVnb3J5IjogIkhBUk1fQ0FURUdPUllfSEFSQVNTTUVOVCIsCiAgICAgICAgICAicHJvYmFiaWxpdHkiOiAiTkVHTElHSUJMRSIKICAgICAgICB9LAogICAgICAgIHsKICAgICAgICAgICJjYXRlZ29yeSI6ICJIQVJNX0NBVEVHT1JZX0RBTkdFUk9VU19DT05URU5UIiwKICAgICAgICAgICJwcm9iYWJpbGl0eSI6ICJORUdMSUdJQkxFIgogICAgICAgIH0KICAgICAgXQogICAgfQogIF0sCiAgInVzYWdlTWV0YWRhdGEiOiB7CiAgICAicHJvbXB0VG9rZW5Db3VudCI6IDcsCiAgICAiY2FuZGlkYXRlc1Rva2VuQ291bnQiOiAyMDUsCiAgICAidG90YWxUb2tlbkNvdW50IjogMjEyCiAgfQp9Cg==
{
  "Signed By": "gemini.hexaeight.com",
  "HashAlgorithm": "SHA512",
  "Contact : ": "admin@gemini.hexaeight.com",
  "User : ": "3a60b66d219abe487b859801a704f52aaf78d5bac1fa80c6e2619d591ee424086926df610ea89430b1d38000608d80c99c78c3f8ba2374188cb5d621f110acf6",
  "MsgId : ": 1727552365203,
  "iss": "gemini.hexaeight.com",
  "aud": "3a60b66d219abe487b859801a704f52aaf78d5bac1fa80c6e2619d591ee424086926df610ea89430b1d38000608d80c99c78c3f8ba2374188cb5d621f110acf6.gemini.hexaeight.com",
  "iat": 28792540,
  "nbf": 1727552373,
  "exp": 29317530,
  "signingkeyhash": "v0vAy+eHOddRZRiKz8kTbvkLy7Aaas7GpmYlXFIF8xQLKbceV+nwRL0ak9CGTW9GFahLmhZy3Ct/U20Dng9npA=="
}
Note : The Message ID in the JSON message should match with the decoded JSON properties above
{
  "candidates": [
    {
      "content": {
        "parts": [
          {
            "text": "Insulin resistance is a condition in which the body's cells do not respond normally to the hormone insulin. Insulin is a hormone that helps glucose (sugar) get from the bloodstream into cells for energy. When cells are insulin resistant, glucose builds up in the bloodstream and can lead to type 2 diabetes.\n\nIn people with type 2 diabetes, the body's cells do not respond properly to insulin, and glucose builds up in the bloodstream. This can lead to a number of complications, including:\n\n* Heart disease\n* Stroke\n* Kidney disease\n* Nerve damage\n* Eye damage\n\nInsulin resistance is a common problem in people with obesity and overweight. Other risk factors for insulin resistance include:\n\n* A family history of type 2 diabetes\n* Being physically inactive\n* Eating a diet high in saturated fat and cholesterol\n\nTreatment for insulin resistance typically includes lifestyle changes, such as losing weight, eating a healthy diet, and getting regular exercise. Medications may also be prescribed to help lower blood glucose levels."
          }
        ],
        "role": "model"
      },
      "finishReason": "STOP",
      "index": 0,
      "safetyRatings": [
        {
          "category": "HARM_CATEGORY_SEXUALLY_EXPLICIT",
          "probability": "NEGLIGIBLE"
        },
        {
          "category": "HARM_CATEGORY_HATE_SPEECH",
          "probability": "NEGLIGIBLE"
        },
        {
          "category": "HARM_CATEGORY_HARASSMENT",
          "probability": "NEGLIGIBLE"
        },
        {
          "category": "HARM_CATEGORY_DANGEROUS_CONTENT",
          "probability": "NEGLIGIBLE"
        }
      ]
    }
  ],
  "usageMetadata": {
    "promptTokenCount": 7,
    "candidatesTokenCount": 205,
    "totalTokenCount": 212
  }
}
```


### Do I need any license for Message Verification.

Absolutely not! In fact, the JSON Web Message can be shared openly with anyone. For example, here's a link to a .NET Fiddle that uses the same verifyMessageIntegrity code in the browser to decode AI-generated message content. This feature of HexaEight Signature Validation demonstrates how our technology can future-proof communication between machines, users, and AI, ensuring a safe and secure communication environment

### Try it on .NET Fiddle:
[Run the code on .NET Fiddle](https://dotnetfiddle.net/bFLXUk)


