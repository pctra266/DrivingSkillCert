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
        private readonly DrivingSkillCertContext _context;

        public ExamDAO()
        {
            _context = new DrivingSkillCertContext();
        }

        public List<Exam> GetExams()
        {
            try
            {
                return _context.Exams
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
            try
            {
                _context.Exams.Add(exam);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error add exams", ex);

            }
        }

        public void UpdateExam(Exam exam)
        {
            try
            {
                _context.Exams.Update(exam);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error update exams", ex);

            }
        }

        public void DeleteExam(int id)
        {
            try
            {   
                Exam  exam = FindExamById(id);
                _context.Exams.Remove(exam);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error delete exams", ex);

            }
        }

        public Exam FindExamById (int id)
        {
            try
            {
                return _context.Exams.Include(e => e.Course).Where(e => e.ExamId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error update exams", ex);

            }
        }


    }
}
