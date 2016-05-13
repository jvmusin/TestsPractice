using System;

namespace Tests
{
    public class UserEntity
    {
        public string Login { get; set; }
        public Guid UserId { get; set; }
        public string PasswordHash { get; set; }

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
