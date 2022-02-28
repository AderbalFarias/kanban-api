using System;

namespace Kanban.Domain.Entities
{
    public class User
    {
        public User(Guid id, string login, string password)
        {
            this.Id = id;
            this.Login = login;
            this.Password = password;
        }

        public Guid Id { get; private set; }

        public string Login { get; private set; }

        public string Password { get; private set; }
    }
}
