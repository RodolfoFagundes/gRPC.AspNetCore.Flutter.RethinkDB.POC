namespace GrpcServer.RethinkDB
{
    public class RethinkDbOptions
    {
        public string HostnameOrIp { get; set; }

        public int? DriverPort { get; set; }

        public int? Timeout { get; set; }
    }
}
