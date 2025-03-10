using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrivingSkillCert.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSkillCert.DAO
{
    internal class CourseDAO
    {
        private readonly DrivingSkillCertContext _context;

        public CourseDAO()
        {
            _context =  new DrivingSkillCertContext();
        }

        public List<Course> GetCourses()
        {
            try
            {
                return _context.Courses.Include(t => t.Teacher).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving courses", ex);
            }
        }

        public void CreateCourse(Course course)
        {
            try
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating course", ex);
            }
        }

        public void UpdateCourse(Course course)
        {
            try
            {
                _context.Courses.Update(course);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating course", ex);
            }
        }

        public void DeleteCourse(int courseId)
        {
            try
            {
                var course = _context.Courses.Find(courseId);
                if (course != null)
                {
                    _context.Courses.Remove(course);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting course", ex);
            }
        }

        // Optional: Get single course by ID
        public Course GetCourseById(int courseId)
        {
            try
            {
                return _context.Courses
                    .Include(c => c.Teacher)
                    .FirstOrDefault(c => c.CourseId == courseId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving course", ex);
            }
        }

    }
}
