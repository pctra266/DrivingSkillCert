using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSkillCert.DAO
{
    class QuestionDAO
    {
        public List<Question> GetQuestionsByBankId(int bankId)
        {
            using var context = new DrivingSkillCertContext();
            return context.Questions
                .Where(q => q.BankId == bankId)
                .ToList();
        }

        public void CreateQuestion(Question question)
        {
            using var context = new DrivingSkillCertContext();
            context.Questions.Add(question);
            context.SaveChanges();
        }

        public void UpdateQuestion(Question question)
        {
            using var context = new DrivingSkillCertContext();

            var existingQuestion = context.Questions.Find(question.QuestionId);
            if (existingQuestion != null)
            {
                existingQuestion.Question1 = question.Question1;
                context.SaveChanges();
            }
        }

        public void DeleteQuestion(int questionId)
        {
            using var context = new DrivingSkillCertContext();

            var question = context.Questions.Find(questionId);
            if (question != null)
            {
                context.Questions.Remove(question);
                context.SaveChanges();
            }
        }
        public List<Question> GetQuestionsByCourseId(int courseId)
        {
            using var context = new DrivingSkillCertContext();

            return context.Questions
                .Where(q => q.Bank.CourseId == courseId)
                .Include(q => q.Bank)
                .ToList();
        }
    }
}
