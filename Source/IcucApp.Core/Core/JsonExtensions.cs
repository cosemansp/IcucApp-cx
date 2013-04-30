using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace IcucApp.Core
{
    public static class JsonExtensions
    {
         public static string ToJson(this object source)
         {
             var serializer = new JsonSerializer();
             return serializer.Serialize(source);
         }

         public static T FromJson<T>(this string source)
         {
             var deserializer = new JsonDeserializer();
             var dummyResponse = new RestSharp.RestResponse { Content = source };
             return deserializer.Deserialize<T>(dummyResponse);
         }  
    }
}