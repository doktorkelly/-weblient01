using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using log4net;

namespace WebClient.Test
{
    [TestFixture]
    class WebClientClassTest
    {
        [Test]
        [Ignore]
        public void DownloadStateful_in_out()
        {
            //Arrange
            IWebDAO webDAO = new WebDAOFake();
            WebClientClass webClient = new WebClientClass(webDAO);
            IEnumerable<string> urls = new List<string>() { "url1", "url2" };

            //Act
            webClient.DownloadStateful(urls);
            IList<string> filesWritten = webClient.FilesWritten;

            //Assert
            Assert.IsNotEmpty(filesWritten);
            Assert.AreEqual("url1.html", filesWritten[0]);
            Assert.AreEqual("url2.html", filesWritten[1]);
        }

        [Test]
        [Ignore]
        public void DownloadUrls_in_out()
        {
            //Arrange
            IWebDAO webDAO = new WebDAOFake();
            WebClientClass webClient = new WebClientClass(webDAO);
            IEnumerable<string> urls = new List<string>() { "url1", "url2", "url3" };

            //Act
            IEnumerable<string> filesWritten = webClient.DownloadUrls(urls);

            //Assert
            Assert.IsNotEmpty(filesWritten);
            Assert.AreEqual("url1.html", filesWritten.ElementAt(0));
            Assert.AreEqual("url2.html", filesWritten.ElementAt(1));
            Assert.AreEqual("url3.html", filesWritten.ElementAt(2));
        }

        [Test]
        [Ignore("ok but slow")]
        public void DownloadUrlwithPLinq_in_out()
        {
            //Arrange
            IWebDAO webDAO = new WebDAOFake();
            WebClientClass webClient = new WebClientClass(webDAO);
            IEnumerable<string> urls = new List<string>() { "url0", "url1", "url2", "url3", "url4", "url5","url6", "url7", "url8", "url9" };
            Console.WriteLine();

            //Act
            IEnumerable<string> filesWritten = webClient.DownloadUrlsWithPLinq(urls, 8);

            //Assert
            Assert.IsNotEmpty(filesWritten);
            Assert.AreEqual("url0.html", filesWritten.ElementAt(0));
            Assert.AreEqual("url1.html", filesWritten.ElementAt(1));
            Assert.AreEqual("url2.html", filesWritten.ElementAt(2));
        }

        [Test]
        //[Ignore("TODO")]
        public void DownloadRanges_in_out()
        {
            //Arrange
            IWebDAO webDAO = new WebDAOFake();
            WebClientClass webClient = new WebClientClass(webDAO);
            IEnumerable<string> channels = new List<string>() { "chan0", "chan1", "chan2", "chan3", "chan4", "chan5", "chan6", "chan7", "chan8", "chan9" };
            IEnumerable<EventRequest> requests = channels
                .Select(url => EventRequest.Create(url, new DateTime(2015, 01, 01), new DateTime(2015, 01, 05)));
            Console.WriteLine();

            //Act
            IEnumerable<string> filesWritten = webClient.DownloadRanges(requests, 8);

            //Assert
            Assert.IsNotEmpty(filesWritten);
            Assert.AreEqual("channel=chan0&from=2015-01-01&to=2015-01-01", filesWritten.ElementAt(0));
            Assert.AreEqual("channel=chan0&from=2015-01-02&to=2015-01-02", filesWritten.ElementAt(1));
            Assert.AreEqual("channel=chan1&from=2015-01-01&to=2015-01-01", filesWritten.ElementAt(5));
        }

        [Test]
        public void ToDayRequests_in_out()
        {
            //Arrange
            EventRequest req = EventRequest.Create("chan01", new DateTime(2015, 01, 01), new DateTime(2015, 01, 05));
            EventRequest reqExp1 = EventRequest.Create("chan01", new DateTime(2015, 01, 01), new DateTime(2015, 01, 01));
            EventRequest reqExp2 = EventRequest.Create("chan01", new DateTime(2015, 01, 02), new DateTime(2015, 01, 02));

            //Act
            IEnumerable<EventRequest> reqList = WebClientClass.ToDayRequests(req);

            //Assert
            Assert.IsNotEmpty(reqList);
            Assert.AreEqual(reqExp1, reqList.ElementAt(0));
            Assert.AreEqual(reqExp2, reqList.ElementAt(1));
        }

    }
}
;