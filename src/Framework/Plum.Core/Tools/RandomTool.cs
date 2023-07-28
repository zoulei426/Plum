using System;
using System.Text;

namespace Plum.Tools
{
    public class RandomTool
    {
        public static string RandomCode(int length)
        {
            var rd = new Random();
            int currentLength = 0;
            var result = new StringBuilder();

            while (currentLength < length)
            {
                var item = rd.Next(0, 10);
                result.Append(item.ToString());

                currentLength++;
            }

            return result.ToString();
        }
    }
}