using System.IO;
using System.Text.RegularExpressions;

namespace BSELogETL
{
    public class Helper
    {
        public static string GetBaseName(string path)
        {
            int position = path.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            return path.Substring(position);
        }
        
        public static string RemoveWhitespaces(string value)
        {
            return Regex.Replace(value, @"\s+", "");
        }
    }
}