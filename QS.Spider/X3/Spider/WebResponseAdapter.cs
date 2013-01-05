namespace X3.Spider
{
    using System;
    using System.IO;
    using System.Net;

    public class WebResponseAdapter : IWebResponse
    {
        private readonly HttpWebResponse response;

        public WebResponseAdapter(HttpWebResponse response)
        {
            this.response = response;
        }

        public string CharacterSet
        {
            get
            {
                return this.response.CharacterSet;
            }
        }

        public string ContentEncoding
        {
            get
            {
                return this.response.ContentEncoding;
            }
        }

        public Stream ResponseStream
        {
            get
            {
                return this.response.GetResponseStream();
            }
        }
    }
}

