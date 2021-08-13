module SignatureBuilder

open System
open System.Security.Cryptography
open System.Text

type Signature = {devid : string; methodname : string; authkey : string; datetime : string}

let md5 = MD5.Create()

let MD5HashToString (input : string) =
    input
    |> Encoding.UTF8.GetBytes
    |> md5.ComputeHash
    |> Seq.map (fun c -> c.ToString("X2"))
    |> Seq.map (fun c -> c.ToLower())
    |> Seq.reduce (+)

let getDateTime = 
    DateTime.UtcNow.ToString("yyyyMMddHHmmss")

let buildSignature signature = 
    MD5HashToString (signature.devid + signature.methodname + signature.authkey + signature.datetime) 




    