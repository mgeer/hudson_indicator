namespace X3.Spider
{
    using System;
    using System.IO;
    using System.Text;

    public class HtmlConverter
    {
        private readonly HtmlCharsetMatcher matcher = new HtmlCharsetMatcher();

        private static bool CharsetIsSpecifiedInHtml(Encoding encoding)
        {
            return (encoding != null);
        }

        public string Convert(Stream stream, string backupEncoding)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] bytes = GetBytes(stream);
            string html = ConvertToString(bytes, encoding);
            Encoding encoding2 = this.matcher.Match(html);
            if (CharsetIsSpecifiedInHtml(encoding2))
            {
                if (!this.EncodingEquals(encoding2, encoding))
                {
                    return ConvertToString(bytes, encoding2);
                }
                return html;
            }
            if (!string.IsNullOrEmpty(backupEncoding) && !this.EncodingEquals(EncodingUtil.TryGetEncoding(backupEncoding), encoding))
            {
                return ConvertToString(bytes, Encoding.GetEncoding(backupEncoding));
            }
            return html;
        }

        private static string ConvertToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        private bool EncodingEquals(Encoding charset, Encoding defaultEncoding)
        {
            return (charset == defaultEncoding);
        }

        private static byte[] GetBytes(Stream stream)
        {
            byte[] buffer2;
            byte[] buffer = new byte[0x8000];
            using (MemoryStream stream2 = new MemoryStream())
            {
                int num;
            Label_0011:
                num = stream.Read(buffer, 0, buffer.Length);
                if (num <= 0)
                {
                    buffer2 = stream2.ToArray();
                }
                else
                {
                    stream2.Write(buffer, 0, num);
                    goto Label_0011;
                }
            }
            return buffer2;
        }
    }
}

