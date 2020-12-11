using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShittyTea
{
    public class CowCurrent
    {
        public string fromUsername { get; private set; }
        public string amount { get; private set; }
        public string toUsername { get; private set; }
        private dynamic jsonObj;
        private int fromBalance;
        private int recBalance;
        private int realAmount;
        public CowCurrent(string fromUser, string toUser, string amm)
        {
            this.fromUsername = fromUser;
            this.toUsername = toUser;
            this.amount = amm;
            string json = System.IO.File.ReadAllText(VartStc.pathToStat);
            jsonObj = JsonConvert.DeserializeObject(json);
            if (jsonObj["Credits"][0][fromUsername] == null || jsonObj["Credits"][0][fromUsername] == 0)
            {
                jsonObj["Credits"][0][fromUsername] = 0;
            }
            if (jsonObj["Credits"][0][toUsername] == null)
            {
                jsonObj["Credits"][0][toUsername] = 0;
            }
            fromBalance = jsonObj["Credits"][0][fromUsername];
            recBalance = jsonObj["Credits"][0][toUsername];
            try
            {
                realAmount = Convert.ToInt32(amount);
            }
            catch
            {
                realAmount = 0;
            }
        }
        public async Task<string> Transfer()
        {
            Console.WriteLine(amount);
            if (realAmount <= 0)
            {
                return $"fuck off {fromUsername}.";
            }
            if (fromBalance < realAmount)
            {
                return $"You dont have {realAmount} credits.";
            }
            if (fromBalance >= realAmount)
            {
                int fromBalanceNewTrans = fromBalance - realAmount;
                int recBalanceNewTrans = recBalance + realAmount;
                jsonObj["Credits"][0][fromUsername] = fromBalanceNewTrans;
                jsonObj["Credits"][0][toUsername] = recBalanceNewTrans;
                string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(VartStc.pathToStat, output);
                return $"{fromUsername} sent {realAmount} credits to {toUsername}";
            }
            return "";
        }
    }
}