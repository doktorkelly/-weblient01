using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    public class UrlRequestTuple
    {
        public string Channel { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public static UrlRequestTuple Create(string channel, DateTime from, DateTime to)
        {
            UrlRequestTuple tuple = new UrlRequestTuple();
            tuple.Channel = channel;
            tuple.From = from;
            tuple.To = to;
            return tuple;
        }
        public override string ToString()
        {
            return "channel=" + Channel + "&"
                + "from=" + From + "&"
                + "to=" + To;
        }
    }

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
