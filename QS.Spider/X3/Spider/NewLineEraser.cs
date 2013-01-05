namespace X3.Spider
{
    using System;

    public class NewLineEraser
    {
        public string Filter(string text)
        {
            return text.Replace("\n", " ");
        }
    }
}

