using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    public class UrlResponseTuple
    {
        public string Url { get; set; }
        public string Response { get; set; }
        public static UrlResponseTuple Create(string url, string response)
        {
            UrlResponseTuple tuple = new UrlResponseTuple();
            tuple.Url = url;
            tuple.Response = response;
            return tuple;
        }
    }

    public interface IWebDAO
    {
        Task<UrlResponseTuple> GetDataAsync(string url);
        Task GetDataAsync(string url, Action<string, string> processAction);
    }
}
