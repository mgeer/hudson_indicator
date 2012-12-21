using System.Text;

namespace hudson_indicator_test
{
    public class BytesToText
    {
        public static string Convert(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}