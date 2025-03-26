using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
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
                    .Where(e=>e.IsDelete==false)
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
                exam.IsDelete = true;
                context.Update(exam);
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
                throw new Exception("Error find exams", ex);

            }
        }

        public List<Exam> GetExamsOfStudent(int studentId)
        {
            using var context = new DrivingSkillCertContext();

            try
            {
                return context.Exams.Where(e => e.Course.Registrations.Any(r => r.UserId == studentId && r.IsDelete != true && "Approved".Equals(r.Status)) && e.IsDelete == false)
                                    .Include(e => e.Course)
                                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error update exams", ex);

            }
        }


    }
}
