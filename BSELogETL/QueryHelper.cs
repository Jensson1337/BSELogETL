using System;
using System.Windows.Forms;

namespace BSELogETL
{
    public static class QueryHelper
    {
        public static void AppendWhereIpAddresses(ref string whereString, ListBox.ObjectCollection items)
        {
            if (items.Count == 0)
            {
                return;
            }

            var begin = "WHERE";
            if (whereString.Length > 0)
            {
                begin = "AND";
            }

            whereString += begin + " ip_address IN (";

            bool isFirst = true;
            foreach (var item in items)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    whereString += ", ";
                }

                whereString += "\"" + item + "\"";
            }

            whereString += ") ";
        }

        public static void AppendWhereInDateRange(ref string whereString, DateTime beginAt, DateTime endAt)
        {
            var begin = "WHERE";
            if (whereString.Length > 0)
            {
                begin = "AND";
            }

            whereString += begin + " requested_at > \"" + beginAt.ToString("yyyy-MM-dd HH:mm:ss")
                           + "\" AND requested_at < \"" + endAt.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
        }
    }
}