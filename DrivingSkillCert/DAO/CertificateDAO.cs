using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSkillCert.DAO
{
    internal class CertificateDAO
    {
        private readonly DrivingSkillCertContext _context;

        public CertificateDAO()
        {
            _context = new DrivingSkillCertContext();
        }

        // Lấy danh sách tất cả các Certificate
        public List<Certificate> GetCertificates()
        {
            try
            {
                return _context.Certificates.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving certificates", ex);
            }
        }

        // Thêm mới Certificate
        public void AddCertificate(Certificate certificate)
        {
            using (var context = new DrivingSkillCertContext())
            {
                context.Certificates.Add(certificate);
                context.SaveChanges();
            }
        }

        // Cập nhật Certificate
        public void UpdateCertificate(Certificate certificate)
        {
            using (var context = new DrivingSkillCertContext())
            {
                context.Certificates.Update(certificate);
                context.SaveChanges();
            }
        }

        // Xóa Certificate theo CertificateID
        public void DeleteCertificate(int certificateId)
        {
            using (var context = new DrivingSkillCertContext())
            {
                var certificate = context.Certificates.Find(certificateId);
                if (certificate != null)
                {
                    certificate.IsDelete = true;
                    context.SaveChanges();
                }
            }
        }
    }
}
