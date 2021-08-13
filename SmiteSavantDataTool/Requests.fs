module Requests

open System.Net.Http

let getAsync (client:HttpClient) (url:string) = 
    async {
        let! response = client.GetAsync(url) |> Async.AwaitTask
        response.EnsureSuccessStatusCode () |> ignore
        let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        return content
    }

let createClient = 
    let httpClient = new HttpClient()
    httpClient
