using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcceptFramework.Domain.Common;

namespace AcceptFramework.Repository.Common
{
    internal static class UserRepository
    {
        public static IEnumerable<User> GetAll()
        {
            return new RepositoryBase<User>().Select();
        }

        public static void Insert(User record)
        {
            new RepositoryBase<User>().Create(record);
        }

        public static User GetUser(int Id)
        {
            return new RepositoryBase<User>().Select(a => a.Id == Id).FirstOrDefault();
        }

        public static User GetUser(string username)
        {
            return new RepositoryBase<User>().Select(a => a.UserName == username).FirstOrDefault();
        }

        public static User GetUser(string userName, string password)
        {
            return new RepositoryBase<User>().Select(a => a.UserName == userName && a.Password == password && a.ConfirmationCode == null).FirstOrDefault();

        }

        public static User GetUserByRegistrationCode(string confirmationCode)
        {
            return new RepositoryBase<User>().Select(a => a.ConfirmationCode == confirmationCode || a.PasswordRecoveryCode == confirmationCode).FirstOrDefault();

        }

        public static User GetUserByUserName(string userName)
        {
            return new RepositoryBase<User>().Select(a => a.UserName == userName).FirstOrDefault();
        }

        public static User GetUserBySecretKey(string userSecretKey)
        {
            return new RepositoryBase<User>().Select(a => a.SecretKeyCode == userSecretKey).FirstOrDefault();
        }

        public static User UpdateUser(User user)
        {
            return new RepositoryBase<User>().Update(user);
        }

    }
}
