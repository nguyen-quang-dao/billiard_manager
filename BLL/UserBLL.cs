using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DAL;
using DTO;

namespace BLL
{
    public class UserBLL
    {
        UserDAL dal = new UserDAL();

        public UserDTO Login(string username, string password)
        {
            return dal.Login(username, password);
        }public bool IsAdmin(UserDTO user)
        {
            return user.Role == 1;
        }
    }
}