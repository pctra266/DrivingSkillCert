using System;
using System.Collections.Generic;
using System.Linq;
using DrivingSkillCert.Models;
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
                return _context.Registrations.ToList();
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

        // Xóa Registration theo RegistrationID
        public void DeleteRegistration(int registrationId)
        {
            using (var context = new DrivingSkillCertContext())
            {
                var registration = context.Registrations.Find(registrationId);
                if (registration != null)
                {
                    context.Registrations.Remove(registration);
                    context.SaveChanges();
                }
            }
        }
    }
}
