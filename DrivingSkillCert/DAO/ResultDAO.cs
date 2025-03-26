using System;
using System.Collections.Generic;
using System.Linq;
using Model.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSkillCert.DAO
{
    internal class ResultDAO
    {
        private readonly DrivingSkillCertContext _context;

        public ResultDAO()
        {
            _context = new DrivingSkillCertContext();
        }

        // Lấy danh sách tất cả các Result
        public List<Result> GetResults()
        {
            try
            {
                return _context.Results.Include(r=>r.User).Include(r=>r.Exam.Course).Where(r=>!r.IsDelete.Value).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving results", ex);
            }
        }

        // Thêm mới Result
        public void AddResult(Result result)
        {
            using (var context = new DrivingSkillCertContext())
            {
                context.Results.Add(result);
                context.SaveChanges();
            }
        }

        // Cập nhật Result
        public void UpdateResult(Result result)
        {
            using (var context = new DrivingSkillCertContext())
            {
                context.Results.Update(result);
                context.SaveChanges();
            }
        }

        // Xóa Result theo ResultID
        public void DeleteResult(int resultId)
        {
            using (var context = new DrivingSkillCertContext())
            {
                var result = context.Results.Find(resultId);
                if (result != null)
                {
                    context.Results.Remove(result);
                    context.SaveChanges();
                }
            }
        }

        public static bool UpdateResult(int resultId, int score, bool passStatus)
        {
            try
            {
                using (var context = new DrivingSkillCertContext())
                {
                    var resultToUpdate = context.Results.FirstOrDefault(r => r.ResultId == resultId);
                    if (resultToUpdate != null)
                    {
                        resultToUpdate.Score = score;
                        resultToUpdate.PassStatus = passStatus;

                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating result: {ex.Message}");
            }
            return false;
        }



        public List<Result> GetExamResultOfUser(int userId, Exam exam)
        {
            return _context.Results.Where(r=>!r.IsDelete.Value&&r.UserId==userId&&r.Exam.ExamId==exam.ExamId).ToList();
        }
    }
}
