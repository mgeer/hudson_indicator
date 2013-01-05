namespace X3.Spider
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public class HtmlCharsetMatcher
    {
        private readonly Regex charsetExpression = new Regex("charset\\s*=\\s*\"?(?<charset>.*?)\"", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Encoding Match(string html)
        {
            string str = this.charsetExpression.Match(html).Groups["charset"].Value;
            if (!string.IsNullOrEmpty(str))
            {
                return EncodingUtil.TryGetEncoding(str);
            }
            return null;
        }
    }
}

