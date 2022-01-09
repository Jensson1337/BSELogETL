using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BSELogETL
{
    public class Helper
    {
        public static Dictionary<string, string> GetAvailableAttributes()
        {
            return new Dictionary<string, string>()
            {
                { "IpAddress", "IP Address" },
                { "HttpMethod", "HTTP Method" },
                { "HttpLocation", "HTTP Location" },
                { "HttpCode", "HTTP Code" },
                { "RequestedAt", "Requested At" },
                { "PackageSize", "Package Size" },
            };
        }

        public static string GetBaseName(string path)
        {
            int position = path.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            return path.Substring(position);
        }

        public static string RemoveWhitespaces(string value)
        {
            return Regex.Replace(value, @"\s+", "");
        }

        public static bool ValidateIpAddress(string address)
        {
            string pattern = @"(([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\.){3}"
                             + "(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])";

            try
            {
                MatchCollection matches = new Regex(pattern).Matches(address);
                foreach (Match match in matches)
                {
                    if (match.Value == address)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // "its over anakin - i have the high ground!"
            }

            return false;
        }
        
        /**
         * https://stackoverflow.com/questions/2444033/get-dictionary-key-by-value
         */
        public static string KeyByValue(Dictionary<string, string> dict, string val)
        {
            string key = default;
            foreach (KeyValuePair<string, string> pair in dict)
            {
                if (EqualityComparer<string>.Default.Equals(pair.Value, val))
                {
                    key = pair.Key;
                    break;
                }
            }
            return key;
        }
        
        /**
         * https://stackoverflow.com/questions/63055621/how-to-convert-camel-case-to-snake-case-with-two-capitals-next-to-each-other
         */
        public static string ToUnderscoreCase(string str)
            => string.Concat((str ?? string.Empty).Select((x, i) => str != null && i > 0 && char.IsUpper(x) && !char.IsUpper(str[i-1]) ? $"_{x}" : x.ToString())).ToLower();
    }
}