using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    public class WebDAO : IWebDAO
    {
        public async Task<UrlResponseTuple> GetDataAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9000/");
                string response = await client.GetStringAsync(url);
                return UrlResponseTuple.Create(url, response);
            }
        }

        public async Task GetDataAsync(string url, Action<string, string> processFunc)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9000/");
                string response = await client.GetStringAsync(url);
                processFunc.Invoke(url, response);
            }
        }

        private Task<string> GetFromUrlTaskVA(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9000/");
                Task<string> responseTask = client.GetStringAsync(url)
                    .ContinueWith<string>(t => t.Result);
                //.ContinueWith(act => {
                //    string response = act.Result;
                //    string fileWritten = WriteToFile(url, response);
                //});
                return responseTask;
            }
        }
    }
}
