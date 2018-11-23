module NLPApi

open System
open System.Collections.Generic
open System.Reflection.Metadata
open Hopac
open HttpFs
open HttpFs.Client
open HttpFs.Client
open Newtonsoft.Json.Linq
open JsonUtils


//Used for communicating with agents
let private CLIENT_TOKEN = "4adc566a8281443c8c852a9f45976107"

let private API_BASE_URL = "https://api.dialogflow.com/v1/"

let private DEFAULT_LANG = "en"

//Session id is created by client. Should use new guid for each user
//But for testing purposes we use the same session all the time
let private SESSION_ID = "88881466-112a-4349-a2f1-419177f6400b"

let private ``application/json`` = ContentType.parse("application/json") |> Option.get


type DialogResponse =
    struct
        val IntentName: string
        val Parameters: Dictionary<string, string>
        
        new(intentName: string, parameters: Dictionary<string, string>) = {IntentName = intentName; Parameters = parameters}
    end       


let private deserializeQueryResponse respString =  
    let result = JToken.Parse(respString).["result"]            
       
    let parameters = new Dictionary<String, String>()
    for parameter in getChildObject(result, "parameters").Properties() do
         parameters.Add(parameter.Name, parameter.Value.ToString())
     
    let intentName = result.["metadata"].["intentName"].ToString()
    
    new DialogResponse(intentName, parameters)
        
let processQuery question = 
    let url = API_BASE_URL + "query"
    let respString = Request.createUrl Get url
                    |> Request.queryStringItem "query" question
                    |> Request.queryStringItem "sessionId" SESSION_ID
                    |> Request.queryStringItem "lang" DEFAULT_LANG
                    |> Request.setHeader (RequestHeader.Authorization ("Bearer " + CLIENT_TOKEN))
                    |> Request.setHeader (RequestHeader.ContentType ``application/json``)
                    |> Request.responseAsString
                    |> run  
                   
    
    deserializeQueryResponse respString

