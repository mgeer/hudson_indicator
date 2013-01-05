using System;
using System.Text;

namespace X3.Spider
{
    using System.Net;

    public class Spider : ISpider
    {
        private readonly HtmlConverter htmlConverter = new HtmlConverter();
        private static readonly NewLineEraser NewLineEraser = new NewLineEraser();

        private string GetStringFrom(HttpWebResponse response)
        {
            var adapter = new WebResponseAdapter(response);
            var depresser = new StreamDepresser(adapter);
            using (var stream = depresser.Depress())
            {
                return htmlConverter.Convert(stream, adapter.CharacterSet);
            }
        }

        public string Grab(string url)
        {
            var request = CreateRequest(url);
            return DoGrab(request);
        }

        public string GrapWithCredentials(string url, string userName, string password)
        {
            var request = CreateRequestWithCredentials(url, userName, password);
            return DoGrab(request);
        }

        private string DoGrab(HttpWebRequest request)
        {
            using (var response = (HttpWebResponse) request.GetResponse())
            {
                var stringFrom = GetStringFrom(response);
                return NewLineEraser.Filter(stringFrom);
            }
        }

        private static HttpWebRequest CreateRequest(string url)
        {
            return (HttpWebRequest) WebRequest.Create(url);
        }

        private static HttpWebRequest CreateRequestWithCredentials(string url, string userName, string password)
        {
            var request = CreateRequest(url);
            request.PreAuthenticate = true;
            var credentialText = string.Format("{0}:{1}", userName, password);
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentialText));
            request.Headers.Add("Authorization", "Basic " + credentials);
            return request;
        }
    }
}

