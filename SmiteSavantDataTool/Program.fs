
open System
open Newtonsoft.Json
open SignatureBuilder



let createSessionUrl urlPrefix methodName = 
    let signature = {devid = Config.DEVID; datetime=SignatureBuilder.getDateTime; methodname="createsession"; authkey=Config.AUTHKEY}
    let sessionSignature = buildSignature signature
    let sessionRequestUrl = urlPrefix + methodName + "/" + signature.devid + "/" + sessionSignature + "/" + SignatureBuilder.getDateTime
    sessionRequestUrl


[<EntryPoint>]
let main argv =
    
    let httpClient = Requests.createClient
    
    let urlPrefix =  "https://api.smitegame.com/smiteapi.svc/"

    let sessionUrl = createSessionUrl urlPrefix "createsessionJson"

    let response = Requests.getAsync httpClient sessionUrl |> Async.RunSynchronously
     
    printfn "Resp: %A" response
    
    0