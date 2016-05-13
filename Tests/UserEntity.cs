using System;

namespace Tests
{
    public class UserEntity
    {
        public string Login { get; }
        public Guid UserId { get; }
        public string PasswordHash { get; }

        public UserEntity() : this(null, Guid.Empty, null)
        {
        }

        public UserEntity(string login, Guid userId, string passwordHash)
        {
            Login = login;
            UserId = userId;
            PasswordHash = passwordHash;
        }
    }
}
