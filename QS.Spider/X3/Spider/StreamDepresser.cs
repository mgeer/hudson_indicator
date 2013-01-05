namespace X3.Spider
{
    using System;
    using System.IO;
    using System.IO.Compression;

    public class StreamDepresser
    {
        private readonly IWebResponse response;

        public StreamDepresser(IWebResponse response)
        {
            this.response = response;
        }

        public Stream Depress()
        {
            Stream responseStream = this.response.ResponseStream;
            if (!"gzip".Equals(this.response.ContentEncoding, StringComparison.OrdinalIgnoreCase))
            {
                return responseStream;
            }
            return new GZipStream(responseStream, CompressionMode.Decompress);
        }
    }
}

