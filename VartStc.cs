using Newtonsoft.Json;
using Amazon;
//Static version
namespace ShittyTea
{
    public class VartStc
    {
        private static string json = System.IO.File.ReadAllText("config.json");
        private static dynamic jsonObj = JsonConvert.DeserializeObject(json);
        public static readonly string pathToProj = jsonObj["Settings"]["PathToProject"];
        public static readonly string pathToWL = pathToProj + jsonObj["Settings"]["Wordlist"];
        public static readonly string pathToStat = pathToProj + jsonObj["Settings"]["Stat"];
        public static readonly string pathToExp = jsonObj["Settings"]["PathToExploitdb"];
        public static readonly string token = jsonObj["Settings"]["Token"];
        public static readonly string bucketName = jsonObj["Settings"]["AWSbucketName"];
        public static readonly string AWSandLocalfolderContainer = jsonObj["Settings"]["AWSandLocalContainFolder"];
        public static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast2;
    }
}
