namespace CNC.Services
{
    public class UsersStorageService(ILogger<UsersStorageService> logger)
    {
        private readonly Dictionary<string, HostString> _registeredUsers = new();

        public bool Exists(string user) => _registeredUsers.ContainsKey(user);

        public HostString? GetHostByUser(string user)
        {
            _registeredUsers.TryGetValue(user, out var host);
            return host;
        }

        public bool Register(string user, string host, int port)
        {
            if (!_registeredUsers.TryAdd(user, new HostString(host, port)))
            {
                logger.LogError($"Unsuccessful Registration - {user}");
                return false;
            }

            logger.LogInformation($"Successful Registration - {user}");
            return true;
        }

        public bool Unregister(string user)
        {
            if (!_registeredUsers.ContainsKey(user))
            {
                logger.LogError($"Unsuccessful Unregistration - {user}");
                return false;
            }

            _registeredUsers.Remove(user);
            logger.LogInformation($"Successful Unrgistration - {user}");
            return true;
        }

        public void Clear() => _registeredUsers.Clear();
    }
}
