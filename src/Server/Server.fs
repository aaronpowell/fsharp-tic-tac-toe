open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Saturn
open Models

open Giraffe.Serialization

module List =

    let rand = new System.Random()

    let swap (a: _[]) x y =
        let tmp = a.[x]
        a.[x] <- a.[y]
        a.[y] <- tmp

    let randomize (list: _ list) =
        let a = list |> List.toArray
        Array.iteri (fun i _ -> swap a i (rand.Next(i, Array.length a))) a
        a |> Array.toList

let publicPath = Path.GetFullPath "../Client/public"
let port = 8085us

let getInitialBoard() : Task<Board> = task { return Map.empty }

let webApp = router {
    get "/api/init" (fun next ctx ->
        task {
            let! counter = getInitialBoard()
            return! Successful.OK counter next ctx
        })
    post "/api/move" (fun next ctx ->
         task {
             let! model = ctx.BindModelAsync<Model>()

             let totalPositions = (model.Cols |> List.length) * (model.Rows |> List.length)

             match totalPositions with
             | tc when tc = (model.Board |> Map.count) ->
                return! Successful.OK None next ctx
             | _ ->
                let testRow pos =
                    match Map.tryFind pos model.Board with
                    | Some _ -> None
                    | _ -> Some(pos)

                let positions = model.Cols
                                |> List.map (fun col ->
                                    model.Rows
                                        |> List.map (fun row -> col, row)
                                )
                                |> List.collect id
                let pos = positions
                          |> List.map (fun p -> testRow p)
                          |> List.filter (fun p -> Option.isSome p)
                          |> List.randomize
                          |> List.head

                match pos with
                | Some _ -> return! Successful.OK pos next ctx
                | None -> return! Successful.OK None next ctx
         })
}

let configureSerialization (services:IServiceCollection) =
    let fableJsonSettings = Newtonsoft.Json.JsonSerializerSettings()
    fableJsonSettings.Converters.Add(Fable.JsonConverter())
    services.AddSingleton<IJsonSerializer>(NewtonsoftJsonSerializer fableJsonSettings)

let app = application {
    url ("http://0.0.0.0:" + port.ToString() + "/")
    use_router webApp
    memory_cache
    use_static publicPath
    service_config configureSerialization
    use_gzip
}

run app
