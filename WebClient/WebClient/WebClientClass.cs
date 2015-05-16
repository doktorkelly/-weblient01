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

        public IEnumerable<string> DownloadUrlsWithPLinq(IEnumerable<Tuple<string,string>> inList, int numThreads = 1) 
        {
            List<string> result = inList
                .AsParallel()
                .WithDegreeOfParallelism(numThreads)
                .Select(paramList => paramList.Item1 + "&" + paramList.Item2)
                .Select(url => WebDao.GetDataAsync(url).Result)
                .Select(resTuple => WriteToFile(resTuple.Url, resTuple.Response))
                .ToList();
                //.GroupBy(paramList => paramList.Item1)
                //.Select(grp => DownloadByItem1(grp.Key, grp.ToList()))
                //.ToList();
            return result;
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
