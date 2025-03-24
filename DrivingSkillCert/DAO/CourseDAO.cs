using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSkillCert.DAO
{
    internal class CourseDAO
    {
      

        public List<Course> GetCourses()
        {
            using var context = new DrivingSkillCertContext();
            try
            {
                return context.Courses.Include(t => t.Teacher).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving courses", ex);
            }
        }

        public void CreateCourse(Course course)
        {
            using var context = new DrivingSkillCertContext();

            try
            {
                context.Courses.Add(course);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating course", ex);
            }
        }

        public void UpdateCourse(Course course)
        {
            using var context = new DrivingSkillCertContext();

            try
            {
                context.Courses.Update(course);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating course", ex);
            }
        }

        public void DeleteCourse(int courseId)
        {
            using var context = new DrivingSkillCertContext();

            try
            {
                var course = context.Courses.Find(courseId);
                if (course != null)
                {
                    course.IsDelete = true;
                    context.SaveChanges();
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
            using var context = new DrivingSkillCertContext();

            try
            {
                return context.Courses
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
