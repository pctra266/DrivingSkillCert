using System;
using System.Collections.Generic;
using System.Linq;
using Model.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSkillCert.DAO
{
    internal class RegistrationDAO
    {
        private readonly DrivingSkillCertContext _context;

        public RegistrationDAO()
        {
            _context = new DrivingSkillCertContext();
        }

        // Lấy danh sách tất cả các Registration
        public List<Registration> GetRegistrations()
        {
            try
            {
                return _context.Registrations.Include(r=>r.User).Include(r=>r.Course).Where(r=>r.IsDelete==false).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving registrations", ex);
            }
        }

        // Thêm mới Registration
        public void AddRegistration(Registration registration)
        {
            using (var context = new DrivingSkillCertContext())
            {
                context.Registrations.Add(registration);
                context.SaveChanges();
            }
        }

        // Cập nhật Registration
        public void UpdateRegistration(Registration registration)
        {
            using (var context = new DrivingSkillCertContext())
            {
                context.Registrations.Update(registration);
                context.SaveChanges();
            }
        }

        public List<Registration> GetRegistrationsOfTeacher(int teacherId)
        {
            using (var context = new DrivingSkillCertContext())
            {
                return context.Registrations.Include(r => r.Course).Include(r=>r.User)
                    .Where(c => c.Course.Teacher.UserId == teacherId && c.IsDelete == false && c.Course.IsDelete == false)
                    .ToList();      
            }
        }

        // Xóa Registration theo RegistrationID
        public void DeleteRegistration(int registrationId)
        {
            using (var context = new DrivingSkillCertContext())
            {
                var registration = context.Registrations.Find(registrationId);
                if (registration != null)
                {
                    registration.IsDelete = true;
                    context.Registrations.Update(registration);
                    context.SaveChanges();
                }
            }
        }
    }
}
