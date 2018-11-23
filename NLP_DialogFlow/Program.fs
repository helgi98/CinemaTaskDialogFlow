open NLP

open System
open System.Collections.Generic
open NLPApi
open MovieDB

let printMoviRateInfo id = 
    let movieInfo = getMovieInfoById id
    printfn "Title: \"%s\" (%s)\nMetacritic: %s, Imdb: %s" movieInfo.Title movieInfo.Year movieInfo.MetacriticScore movieInfo.ImdbScore
    
let printMovieReleaseDate id = 
    let movieInfo = getMovieInfoById id
    printfn "Title: \"%s\" was released in %s" movieInfo.Title movieInfo.Year
    

let printMovieDescription id = 
    let movieInfo = getMovieInfoById id
    printfn "Title: \"%s\" (%s)\n Description: %s" movieInfo.Title movieInfo.Year movieInfo.ShortDescription

let handleGetMovieRateIntent (title: string) = 
    let movieIds = searchMovieIdsByTile(title) |> Seq.toList
    if movieIds |> Seq.isEmpty then
        printfn "No movie with title \"%s\" was found" title
    
    for movieId in movieIds  do
        printMoviRateInfo movieId
    
let handleGetMovieReleaseDateIntent (title: string) = 
    let movieIds = searchMovieIdsByTile(title) |> Seq.toList
    if movieIds |> Seq.isEmpty then
        printfn "No movie with title \"%s\" was found" title
    
    for movieId in movieIds  do
        printMovieReleaseDate movieId
        
let handleGetMovieDescriptionIntent (title: string) = 
    let movieIds = searchMovieIdsByTile(title) |> Seq.toList
    if movieIds |> Seq.isEmpty then
        printfn "No movie with title \"%s\" was found" title
    
    for movieId in movieIds  do 
        printMovieDescription movieId
       
let handleIntent (intent: string, parameters: Dictionary<string, string>) =
    match intent with
    | "GetMovieRate" -> handleGetMovieRateIntent(parameters.["FilmName"])
    | "GetMovieReleaseDate" -> handleGetMovieReleaseDateIntent(parameters.["FilmName"])
    | "GetMovieDescription" -> handleGetMovieDescriptionIntent(parameters.["FilmName"])
    | "GetMoviesByGenre" -> printfn "Intent: GetMoviesByGenre, Genre: %s" parameters.["GenreName"]
    | "GetMovieReview" -> printfn "Intent: GetMovieReview, Movie: %s" parameters.["FilmName"] 
    | "" -> printfn "Unknow intent"
    | unhandledIntent -> printfn "Intent %s is not currently supported" unhandledIntent

[<EntryPoint>]
let main argv =
    let response = processQuery "Description Star wars"
    
    //printfn "%s" response.IntentName
    handleIntent(response.IntentName, response.Parameters)
    
    
    0 
