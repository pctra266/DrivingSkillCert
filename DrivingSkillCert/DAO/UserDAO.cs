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

        public void DeleteUser(int userId)
        {
            using (var context = new DrivingSkillCertContext())
            {
                var user = context.Users.Find(userId);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
            }
        }
        public void AddUser(User user)
        {
            using (var context = new DrivingSkillCertContext())
            {
                try
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating user", ex);
                }
            }
        }

        // Cập nhật User
        public void UpdateUser(User user)
        {
            using (var context = new DrivingSkillCertContext())
            {
                context.Users.Update(user);
                context.SaveChanges();
            }
        }
    }
}
