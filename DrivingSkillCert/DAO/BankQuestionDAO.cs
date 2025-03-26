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

        // Xóa BankQuestion (hard delete)
        public void DeleteBankQuestion(int bankID)
        {
            using var context = new DrivingSkillCertContext();
            var bank = context.BankQuestions.FirstOrDefault(e => e.BankId == bankID);

            if (bank != null)
            {
                QuestionDAO questionDAO = new QuestionDAO();
                var questionIds = context.Questions
                                        .Where(question => question.BankId == bankID)
                                        .Select(q => q.QuestionId)
                                        .ToList();

                questionIds.ForEach(questionDAO.DeleteQuestion);

                context.Remove(bank);
                context.SaveChanges();
            }
        }

    }
}