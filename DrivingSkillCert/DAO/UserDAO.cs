using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
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
                return _context.Users.Where(u => !u.Role.Equals("Admin") ).Where(x=>x.IsDelete==false).ToList();
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
                    user.IsDelete = true;
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
        // hàm để đăng nhập 
        public Model.Models.User getAccountByEmailAndPassword(string email)
        {
            DrivingSkillCertContext context = new DrivingSkillCertContext();
            return context.Users.FirstOrDefault(u => u.Email.Equals(email) );
        }
        // hàm check trùng email khi đăng kí 
        public bool isEmailExist(string email)
        {
            DrivingSkillCertContext context = new DrivingSkillCertContext();
            return context.Users.Any(u => u.Email.Equals(email));

        }
        // hàm check trùng số điện thoại khi đăng kí 
        public bool isPhoneExist(string phone)
        {
            DrivingSkillCertContext context = new DrivingSkillCertContext();
            return context.Users.Any(u => u.Phone.Equals(phone));

        }
        // hàm check trùng email khi update
        public bool isEmailExistnotCurrentUser(string email, int userId)
        {
            DrivingSkillCertContext context = new DrivingSkillCertContext();
            return context.Users.Any(u => u.Email.Equals(email) && u.UserId != userId);
        }
        // hàm check trùng số điện thoại khi update
        public bool isPhoneExistnotCurrentUser(string phone, int userId)
        {
            DrivingSkillCertContext context = new DrivingSkillCertContext();
            return context.Users.Any(u => u.Phone.Equals(phone) && u.UserId != userId);
        }
        //  hàm cập nhật password 
        public static bool upDatePasswordUser(int userId, string newPassword)
        {
            DrivingSkillCertContext context = new DrivingSkillCertContext();
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.Password = newPassword; // Nên hash mật khẩu trước khi lưu vào DB
                context.SaveChanges();
                return true;
            }
            return false;
        }
        // hàm cập nhật profile
        public static bool updateProfile(Model.Models.User user)
        {
            DrivingSkillCertContext context = new DrivingSkillCertContext();
            if (user == null || user.UserId <= 0)
            {
                return false;
            }
            var existingUser = context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.Class = user.Class;
                existingUser.School = user.School;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Certificate> getUserCertificate(int userId)
        {
            DrivingSkillCertContext context = new DrivingSkillCertContext();
            return context.Certificates.Where(c => c.UserId == userId).ToList();
        }

    }
}
