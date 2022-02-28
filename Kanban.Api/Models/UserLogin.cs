using Kanban.Domain.Entities;
using System;

namespace Kanban.Api.Models
{
    public class UserLogin
    {
        public string Login { get; set; }

        public string Senha { get; set; }

        internal User MatToEntity() => new User(Guid.Empty, this.Login, this.Senha);
    }
}
