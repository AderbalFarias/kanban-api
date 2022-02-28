using Kanban.Domain.Entities;

namespace Kanban.Domain.Services
{
    public interface ITokenService
    {
        string Generate(User user);
    }
}