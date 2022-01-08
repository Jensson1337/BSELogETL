namespace BSELogETL
{
    public class LogEntry
    {
        public string IpAddress { get; set; }
        public string HttpMethod { get; set; }
        public string HttpLocation { get; set; }
        public string HttpCode { get; set; }
        public string RequestedAt { get; set; }
        public string PackageSize { get; set; }

        public LogEntry(string value)
        {
            IpAddress = Helper.RemoveWhitespaces(value.Split('-')[0]);

            RequestedAt = value.Split('[')[1].Split(']')[0];
            
            var request = value.Split('"')[1];
            var splitRequest = request.Split(' ');
            HttpMethod = splitRequest[0];
            HttpLocation = splitRequest[1];

            var splitString = value.Split(' ');
            HttpCode = splitString[splitString.Length - 2];
            PackageSize = splitString[splitString.Length - 1];
        }

        public LogEntry()
        {
            // wow such emptiness
        }
    }
}
