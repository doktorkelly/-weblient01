using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebClient
{
    public class WebClientClass
    {
        public IList<string> FilesWritten { get; private set; }
        private IWebDAO WebDao { get; set; }

        public WebClientClass(IWebDAO webDAO = null)
        {
            this.FilesWritten = new List<string>();
            this.WebDao = webDAO ?? new WebDAO();
        }

        public void DownloadStateful(IEnumerable<string> urls)
        {
            Task webTask1 = WebDao.GetDataAsync(urls.ElementAt(0), WriteToFileStateful);
            Task webTask2 = WebDao.GetDataAsync(urls.ElementAt(1), WriteToFileStateful);
            Task.WaitAll(webTask1, webTask2);
        }

        public IEnumerable<string> DownloadUrls(IEnumerable<string> urls)
        {
            IList<Task<UrlResponseTuple>> tasks = urls
                .Select(url => WebDao.GetDataAsync(url))
                .ToList();
            IEnumerable<string> fileNames = Task
                .WhenAll<UrlResponseTuple>(tasks)
                .Result
                .Select(resTuple => WriteToFile(resTuple.Url, resTuple.Response));
            return fileNames;
        }

        public IEnumerable<string> DownloadUrlsWithPLinq(IEnumerable<string> urls, int numThreads = 1)
        {
            List<string> result = urls
                .AsParallel()
                .WithDegreeOfParallelism(numThreads)
                .Select(url => WebDao.GetDataAsync(url).Result)
                .Select(resTuple => WriteToFile(resTuple.Url, resTuple.Response))
                .ToList();
            return result;
        }

        public IEnumerable<string> DownloadRanges(IEnumerable<UrlRequestTuple> inList, int numThreads = 1) 
        {
            List<string> result = inList
                .AsParallel()
                .WithDegreeOfParallelism(numThreads)
                .SelectMany(req => ToDayRequest(req))
                .Select(req => WebDao.GetDataAsync(req.ToString()).Result)
                .Select(resTuple => WriteToFile(resTuple.Url, resTuple.Response))
                .ToList();
                //.GroupBy(paramList => paramList.Item1)
                //.Select(grp => DownloadByItem1(grp.Key, grp.ToList()))
                //.ToList();
            return result;
        }

        private IEnumerable<UrlRequestTuple> ToDayRequest(UrlRequestTuple req)
        {
            IEnumerable<UrlRequestTuple> urlList = new List<UrlRequestTuple>();
            string channel = req.Channel;
            IEnumerable<DateTime> days = RangeToDayList(req.From, req.To);
            IEnumerable<UrlRequestTuple> urls = days
                .Select(day => DayToRange(day))
                .Select(range => UrlRequestTuple.Create(req.Channel, range.Item1, range.Item1));
            return urls;
        }

        private IEnumerable<DateTime> RangeToDayList(DateTime from, DateTime to)
        {
            int numDays = (from - to).Days;
            IEnumerable<DateTime> days = Enumerable.Range(0, numDays)
                .Select(day => from.AddDays(day));
            return days;
        }

        private Tuple<DateTime, DateTime> DayToRange(DateTime day)
        {
            return Tuple.Create<DateTime, DateTime>(day, day);
        }


        private IEnumerable<string> DownloadByItem1(string key, List<Tuple<string, string>> list)
        {
            throw new NotImplementedException();
        }


        private string WriteToFile(string url, string pageResponse)
        {
            //TODO: write pageResponse into file
            Console.WriteLine("WriteToFile: "
                + "url: " + url + ", "
                + "thread id: " + Thread.CurrentThread.ManagedThreadId);
            string fileWritten = url + ".html";
            return fileWritten;
        }

        private void WriteToFileStateful(string url, string pageResponse)
        {
            //TODO: write pageResponse into file
            string fileWritten = url + ".html";
            FilesWritten.Add(fileWritten);
        }
    }
}
