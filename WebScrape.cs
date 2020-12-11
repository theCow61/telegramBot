using HtmlAgilityPack;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShittyTea
{
    class WebScrape
    {
        private HtmlWeb web = new HtmlWeb();
        private HtmlDocument doc;
        private string CTFurl = @"https://ctftime.org/event/list/upcoming";
        private string NewsApiUrl = @"https://weathrman.ai/";

        public string CTFupcoming()
        {
            int count = 0;
            string scraped = "";
            doc = web.Load(CTFurl);
            foreach (var item in doc.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr"))
            {
                scraped += item.InnerText;
                count++;
                if (count >= 5)
                {
                    break;
                }
            }
            return Convert.ToString(scraped.Replace("\n\n\n", "\n"));
        }
        public async Task<string> Top5NewsupcomingAsync()
        {
            int count = 0;
            string scraped = "";
            doc = await web.LoadFromWebAsync(NewsApiUrl);
            for(int i = 1; i < 3; ++i)
            {
                foreach (var item in doc.DocumentNode.SelectNodes("/html/body/div/div[2]/table/tbody/tr["+i+"]"))
                {
                    scraped += item.InnerText;
                    count++;
                    if(count >= 5)
                    {
                        break;
                    }
                }
            }
            // Thread.Sleep(5000);
            return Convert.ToString(scraped.Replace("\n\n\n", "\n"));
        }
    }
}
