using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Tests
{
    public class UserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<string, UserEntity> users 
            = new ConcurrentDictionary<string, UserEntity>();

        public UserEntity Find(string login)
        {
            UserEntity entity;
            return !users.TryGetValue(login, out entity)
                ? null
                : entity;
        }

        public void Create(UserEntity userEntity)
        {
            if (!users.TryAdd(userEntity.Login, userEntity))
                throw new Exception($"User with login '{userEntity.Login}' is already created");
        }

        public void CreateAll(IEnumerable<UserEntity> users)
        {
            foreach (var user in users)
                Create(user);
        }
    }
}