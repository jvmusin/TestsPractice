using System;

namespace Tests
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly IUserRepository userRepository;
        private readonly IGuidFactory guidFactory;
        private readonly IUserEntityFactory userEntityFactory;

        public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository,
            IGuidFactory guidFactory, IUserEntityFactory userEntityFactory)
        {
            this.passwordHasher = passwordHasher;
            this.userRepository = userRepository;
            this.guidFactory = guidFactory;
            this.userEntityFactory = userEntityFactory;
        }

        public void Register(string login, string password)
        {
            CheckForNull(login, password);

            var passwordHash = passwordHasher.Hash(password);
            var userId = guidFactory.Create();
            var userEntity = userEntityFactory.Create(login, userId, passwordHash);
            userRepository.Create(userEntity);
        }

        public UserModel Login(string login, string password)
        {
            CheckForNull(login, password);

            var entity = userRepository.Find(login);

            if (entity == null)
                return null;

            var hash = passwordHasher.Hash(password);
            return hash == entity.PasswordHash 
                ? new UserModel(entity.Login, entity.UserId) 
                : null;
        }

        private void CheckForNull(string login, string password)
        {
            if (login == null)
                throw new ArgumentNullException(nameof(login));
            if (password == null)
                throw new ArgumentNullException(nameof(password));
        }
    }
}
