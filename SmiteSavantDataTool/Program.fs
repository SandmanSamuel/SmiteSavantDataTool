
open System
open System.Security.Cryptography
open System.Text
open System.Net.Http
open FSharp.Data

type signature = {devid : string; methodname : string; authkey : string; datetime : string}

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

let getAsync (client:HttpClient) (url:string) = 
    async {
        let! response = client.GetAsync(url) |> Async.AwaitTask
        response.EnsureSuccessStatusCode () |> ignore
        let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        return content
    }

let buildSignature signature = MD5HashToString (signature.devid + signature.methodname + signature.authkey + signature.datetime) 

[<EntryPoint>]
let main argv =

    let signatureSession = {devid=Config.DEVID; methodname = "createsession"; authkey=Config.AUTHKEY; datetime = getDateTime}

    let sessionSignature = buildSignature signatureSession

    let urlPrefix =  "https://api.smitegame.com/smiteapi.svc/"

    //printfn "%A" signature
    
    let sessionRequestUrl = urlPrefix + "createsessionJson/" + signatureSession.devid + "/" + sessionSignature + "/" + getDateTime
    use httpClient = new System.Net.Http.HttpClient()
    
    let session = getAsync httpClient sessionRequestUrl |> Async.RunSynchronously

    
    let signatureGods = {devid=Config.DEVID; methodname = "getgods"; authkey=Config.AUTHKEY; datetime = getDateTime}

    let json = JsonValue.Load sessionRequestUrl

    let session_id = json.GetProperty("session_id")

    let godRequestUrl = urlPrefix + "getgodsjson/" + signatureGods.devid + "/" + buildSignature signatureGods + "/" + session_id.AsString() + "/"+ getDateTime + "/1"

    let gods = getAsync httpClient godRequestUrl |> Async.RunSynchronously

    printfn "gods: %A" gods

    
    0