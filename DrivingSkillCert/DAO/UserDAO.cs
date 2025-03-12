using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrivingSkillCert.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSkillCert.DAO
{
    internal class UserDAO
    {
        private readonly DrivingSkillCertContext _context;

        public UserDAO()
        {
            _context = new DrivingSkillCertContext();
        }
        public List<User> GetUsers()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving users", ex);
            }
        }

        internal List<User> GetUsersByRole(string role)
        {
            try
            {
                return _context.Users.Where(u => u.Role.Equals(role)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving users", ex);
            }
        }
    }
}
