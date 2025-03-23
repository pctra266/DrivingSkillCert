using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSkillCert.DAO
{
    class AnswerDAO
    {
        public List<Answer> GetAnswersByQuestionId(int questionId)
        {
            using var context = new DrivingSkillCertContext();

            return context.Answers
                .Where(a => a.QuestionId == questionId)
                .ToList();
        }

        public void CreateAnswer(Answer answer)
        {
            using var context = new DrivingSkillCertContext();

            context.Answers.Add(answer);
            context.SaveChanges();
        }

        public void DeleteAnswersByQuestionId(int questionId)
        {
            using var context = new DrivingSkillCertContext();

            var answers = context.Answers.Where(a => a.QuestionId == questionId);
            context.Answers.RemoveRange(answers);
            context.SaveChanges();
        }
    }
}
