using Newtonsoft.Json;
using Amazon;
//Instance version
namespace ShittyTea
{
    public class Vart
    {
        public string pathToProj { get; private set; }
        public string pathToWL { get; private set; }
        public string pathToStat { get; private set; }
        public string pathToExp { get; private set; }
        public string token { get; private set; }
        public string bucketName { get; private set; }
        public string AWSandLocalfolderContainer { get; private set; }
        public RegionEndpoint bucketRegion = RegionEndpoint.USEast2;
        public Vart()
        {
            string json = System.IO.File.ReadAllText("config.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            this.pathToProj = jsonObj["Settings"]["PathToProject"];
            this.pathToWL = this.pathToProj + jsonObj["Settings"]["Wordlist"];
            this.pathToStat = this.pathToProj + jsonObj["Settings"]["Stat"];
            this.pathToExp = jsonObj["Settings"]["PathToExploitdb"];
            this.token = jsonObj["Settings"]["Token"];
            this.bucketName = jsonObj["Settings"]["AWSbucketName"];
            this.AWSandLocalfolderContainer = jsonObj["Settings"]["AWSandLocalContainFolder"];
        }
    }
}
