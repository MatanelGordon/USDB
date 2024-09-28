namespace CNC.Services
{
    public class UsersStorageService(ILogger<UsersStorageService> logger)
    {
        private readonly HashSet<string> _registeredUsers = new();

        public bool Exists(string user) => _registeredUsers.Contains(user);

        public bool Register(string user)
        {
            if (_registeredUsers.Contains(user))
            {
                logger.LogError($"Unsuccessful Registration - {user}");
                return false;
            }

            _registeredUsers.Add(user);
            logger.LogInformation($"Successful Registration - {user}");
            return true;
        }

        public bool Unregister(string user)
        {
            if (!_registeredUsers.Contains(user))
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
