using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studentum.Infrastructure.Repository;

namespace BreakAway.Domain.Intranet.Users
{
    public interface IUserRepository : IRepository<User>
    {
       IEnumerable<Role> GetRoleByUserId(int id);
        IQueryable<Role> GetAllRoles();
    }

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IRepositoryProviderBase repositoryProvider) : base(repositoryProvider) { }

        public IEnumerable<Role> GetRoleByUserId(int id)
        {
            return Provider.All<User>().FirstOrDefault(u => u.Id == id).Roles;
        }

        public IQueryable<Role> GetAllRoles()
        {
            return Provider.All<Role>();
        }
    }
}
