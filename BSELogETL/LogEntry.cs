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
    }
}
