using System;

namespace Tests
{
    public class UserModel
    {
        public Guid UserId { get; }
        public string Login { get; }

        public UserModel() : this(null, Guid.Empty)
        {
        }

        public UserModel(string login, Guid userId)
        {
            UserId = userId;
            Login = login;
        }
    }
}
