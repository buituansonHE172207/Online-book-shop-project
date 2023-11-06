using BusinessObject;
using Data_Layer.DAO;
using Data_Layer.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Repository.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        public IEnumerable<Role> GetRolesList() => RoleDAO.Instance.GetRoleList();
    }
}
