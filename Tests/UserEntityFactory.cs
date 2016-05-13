using System;

namespace Tests
{
    public class UserEntityFactory : IUserEntityFactory
    {
        public UserEntity Create(string login, Guid userId, string passwordHash)
        {
            return new UserEntity(login, userId, passwordHash);
        }
    }
}
