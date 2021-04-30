using System;
using System.Threading.Tasks;
using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using Serilog;

namespace WebCrawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Demo starting up!");

            await DemoSimpleCrawler();
            await DemoSinglePageRequest();
        }

        private static async Task DemoSimpleCrawler()
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 25, //Crawl 25 pages
                MinCrawlDelayPerDomainMilliSeconds = 3000 // Wait 3 seconds between requests
            };

            var crawler = new PoliteWebCrawler(config);
            var siteToCrawl = "http://wiprodigital.com";

            crawler.PageCrawlCompleted += PageCrawlCompleted;

            var crawlResult = await crawler.CrawlAsync(new Uri(siteToCrawl));

        }

        private static async Task DemoSinglePageRequest()
        {
            var PageRequester = new PageRequester(new CrawlConfiguration(), new WebContentExtractor());

            var crawledPage = await PageRequester.MakeRequestAsync(new Uri("http://google.com"));
            Log.Logger.Information("{result}", new
            {
                url = crawledPage.Uri,
                status = Convert.ToInt32(crawledPage.HttpResponseMessage.StatusCode)
            });
        }

        private static void PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            var httpStatus = e.CrawledPage.HttpResponseMessage.StatusCode;
            var rawPageText = e.CrawledPage.Content.Text; 
        }
    }
}
