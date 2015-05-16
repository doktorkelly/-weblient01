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
            Assert.AreEqual("url1.html", filesWritten.ElementAt(0));
            Assert.AreEqual("url2.html", filesWritten.ElementAt(1));
            Assert.AreEqual("url3.html", filesWritten.ElementAt(2));
        }

    }
}
;