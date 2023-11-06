using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.DAO
{
    public class RoleDAO
    {

        private static RoleDAO instance = null;
        private static readonly object instanceLock = new object();
        public static RoleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoleDAO();
                    }
                    return instance;
                }
            }
        }
        //-----------------------------------------------------------------------------

        public IEnumerable<Role> GetRoleList()
        {
            var roleList = new List<Role>();
            try
            {

                using var context = new ClothesShoppingContext();

                roleList = context.Roles.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return roleList;

        }

    }
}

