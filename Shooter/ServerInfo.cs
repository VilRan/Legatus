using System.Net;

namespace Shooter
{
    public struct ServerInfo
    {
        public IPEndPoint EndPoint;
        public string Name;
        public bool RequiresPassword;

        public ServerInfo(IPEndPoint endPoint, string name, bool requiresPassword)
        {
            EndPoint = endPoint;
            Name = name;
            RequiresPassword = requiresPassword;
        }
    }
}
