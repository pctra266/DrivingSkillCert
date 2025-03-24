using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace DrivingSkillCert.DAO
{
    public class BankQuestionDAO
    {


        // Lấy danh sách BankQuestion (bao gồm thông tin Course liên quan)
        public List<BankQuestion> GetBankQuestion()
        {
            using var context = new DrivingSkillCertContext();

            return context.BankQuestions
                .Include(b => b.Course) // Eager loading thông tin Course
                .ToList();
        }

        // Thêm mới BankQuestion
        public void CreateBankQuestion(BankQuestion bank)
        {
            using var context = new DrivingSkillCertContext();

            context.BankQuestions.Add(bank);
            context.SaveChanges();
        }

        // Cập nhật BankQuestion
        public void UpdateBankQuestion(BankQuestion bank)
        {
            using var context = new DrivingSkillCertContext();

            var existingBank = context.BankQuestions.Find(bank.BankId);
            if (existingBank != null)
            {
                existingBank.BankName = bank.BankName;
                existingBank.CourseId = bank.CourseId;
                context.SaveChanges();
            }
        }

        // Xóa BankQuestion (soft delete)
        public void DeleteBankQuestion(int bankID)
        {
            using var context = new DrivingSkillCertContext();

            var bank = context.BankQuestions.Where(e=>e.BankId == bankID).FirstOrDefault(); 
            if (bank != null)
            {
                context.Remove(bank);
                context.SaveChanges();
            }
        }
    }
}