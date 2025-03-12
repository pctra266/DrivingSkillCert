using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrivingSkillCert.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSkillCert.DAO
{
    internal class ExamDAO
    {

        public List<Exam> GetExams()
        {
            using var context = new DrivingSkillCertContext();

            try
            {
                return context.Exams
                    .Include(e => e.Course.Teacher)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving exams", ex);
            }
        }

        public void AddExam(Exam exam)
        {
            using var context = new DrivingSkillCertContext();

            try
            {
                context.Exams.Add(exam);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error add exams", ex);

            }
        }

        public void UpdateExam(Exam exam)
        {
            using var context = new DrivingSkillCertContext();

            try
            {
                context.Exams.Update(exam);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error update exams", ex);

            }
        }

        public void DeleteExam(int id)
        {
            using var context = new DrivingSkillCertContext();

            try
            {   
                Exam  exam = FindExamById(id);
                context.Exams.Remove(exam);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error delete exams", ex);

            }
        }

        public Exam FindExamById (int id)
        {
                        using var context = new DrivingSkillCertContext();

            try
            {
                return context.Exams.Include(e => e.Course).Where(e => e.ExamId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error update exams", ex);

            }
        }


    }
}
