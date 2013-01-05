namespace X3.Spider
{
    using System;
    using System.IO;

    public interface IWebResponse
    {
        string CharacterSet { get; }

        string ContentEncoding { get; }

        Stream ResponseStream { get; }
    }
}

