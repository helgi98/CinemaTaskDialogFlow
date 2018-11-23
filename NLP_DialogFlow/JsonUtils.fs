module JsonUtils
open Newtonsoft.Json.Linq

let getChildObject (target: JToken, child: string): JObject = (downcast target.[child] : JObject)

let getChildArray (target: JToken, child: string): JArray = (downcast target.[child] : JArray)

let getChildProperty (target: JToken, child: string): JProperty = (downcast target.[child] : JProperty)