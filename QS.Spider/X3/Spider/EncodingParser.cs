namespace X3.Spider
{
    using System;
    using System.Text;

    public class EncodingParser
    {
        private readonly IWebResponse response;

        public EncodingParser(IWebResponse response)
        {
            this.response = response;
        }

        public Encoding GetEncoding()
        {
            string characterSet = this.response.CharacterSet;
            if (string.IsNullOrEmpty(characterSet))
            {
                return Encoding.Default;
            }
            return GetEncoding(characterSet);
        }

        private static Encoding GetEncoding(string contentEncoding)
        {
            return Encoding.GetEncoding(contentEncoding);
        }
    }
}

