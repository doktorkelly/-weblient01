using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebClient
{
    public class WebDAOFake : IWebDAO
    {
        public Task<EventResponse> GetDataAsync(string url)
        {
            //Tuple<string, string> urlTupel = Tuple.Create<string, string>(url, "fake response");
            //return Task.Run<Tuple<string, string>>(() => urlTupel);
            Console.WriteLine("get data:    "
                + "url: " + url + ", "
                + "thread id: " + Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(4000);
            EventResponse tupel = EventResponse.Create(url, "fake content");
            return Task.Run<EventResponse>(() => tupel);
        }

        public Task GetDataAsync(string url, Action<string, string> processAction)
        {
            return Task.Run(() => { processAction.Invoke(url, "fake response"); });
        }
    }
}
