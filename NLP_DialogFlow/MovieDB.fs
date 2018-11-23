module NLP.MovieDB

open Hopac
open HttpFs
open HttpFs.Client
open JsonUtils
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System
open System.Linq

let private BASE_URL = "http://www.omdbapi.com/"
let private API_KEY = "c5ec59cb"

type MovieSearchInfo =
    val Title: string
    val Year: string
    val Id: string
    
    new(title: string, year: string, imdbid: string) = {Title = title; Year = year; Id = imdbid}

type MovieInfo = 
    inherit MovieSearchInfo
    
    val ShortDescription: string
    val ImdbScore: string
    val MetacriticScore: string 
    
        
    new(searchInfo: MovieSearchInfo, plot: string, imdbScore: string, metacriticScore: string) =
        {inherit MovieSearchInfo(searchInfo.Title, searchInfo.Year, searchInfo.Id); 
            ShortDescription = plot; ImdbScore = imdbScore; MetacriticScore = metacriticScore}
            
    new(title: string, year: string, imdbid: string, plot: string, imdbScore: string, metacriticScore: string) =
        {inherit MovieSearchInfo(title, year, imdbid); 
            ShortDescription = plot; ImdbScore = imdbScore; MetacriticScore = metacriticScore}
        
   

let extractMovieInfo (movie: JToken) = 
    let title = movie.["Title"].ToString()
    let year = movie.["Year"].ToString()
    let id = movie.["imdbID"].ToString()
    let plot = movie.["Plot"].ToString()
    let imdbScore = movie.["imdbRating"].ToString()
    let metacriticScore = movie.["Metascore"].ToString()

    new MovieInfo(title, year, id, plot, imdbScore, metacriticScore)
    

let searchMovieIdsByTile (title: String) = 
    let respString = Request.createUrl Get BASE_URL
                    |> Request.queryStringItem "apikey" API_KEY
                    |> Request.queryStringItem "s" title
                    |> Request.responseAsString
                    |> run
    
    let resp = JToken.Parse(respString) 
       
    match getChildArray(resp, "Search") with
    | null -> Seq.empty
    | data -> data |> Seq.map (fun movie -> movie.["imdbID"].ToString())
    
    
let getMovieInfoById (id: string) =
    let respString = Request.createUrl Get BASE_URL
                    |> Request.queryStringItem "apikey" API_KEY
                    |> Request.queryStringItem "i" id
                    |> Request.responseAsString
                    |> run
                    
                    
    extractMovieInfo (JToken.Parse(respString))
                    
    
    