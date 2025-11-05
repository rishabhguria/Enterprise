namespace Prana.Global
{
    public struct ServerDetails
    {
        public string IpAddress;
        public int Port;

        public void CreateDetails(string details)
        {
            string[] serverdata = details.Split(':');
            IpAddress = serverdata[0];
            Port = int.Parse(serverdata[1]);
        }
    }
}