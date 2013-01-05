namespace X3.Spider
{
    using System;
    using System.Text;

    public class EncodingUtil
    {
        public static Encoding TryGetEncoding(string encoding)
        {
            try
            {
                return Encoding.GetEncoding(encoding);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

