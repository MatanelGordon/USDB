using CNC.Models;

namespace CNC.Services
{
    public class UsersStorageService(ILogger<UsersStorageService> logger)
    {
        private readonly Dictionary<string, ToolConnectionString> _registeredUsers = new();

        public bool Exists(string user) => _registeredUsers.ContainsKey(user);

        public ToolConnectionString? GetHostByUser(string user)
        {
            _registeredUsers.TryGetValue(user, out var host);
            return host;
        }

        /// <summary>
        /// Acts like GetHostByUser, except it throws an error if not found.
        /// </summary>
        /// <param name="user">Registered Tool Identity</param>
        /// <returns>Connection string (Host + Port)</returns
        public ToolConnectionString GetRequiredHostByUser(string user)
        {
            var answer = GetHostByUser(user);

            if (answer is null)
            {
                throw new KeyNotFoundException($"Required user {user} not found in storage");
            }

            return answer;
        }

        public bool Register(string user, string host, int port)
        {
            if (host.Contains(':'))
            {
                logger.LogError($"Unsuccessful Registration - {user} - host parameter contains ':' - {host}");
                return false;
            }
            
            if (!_registeredUsers.TryAdd(user, new ToolConnectionString(host, port)))
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
