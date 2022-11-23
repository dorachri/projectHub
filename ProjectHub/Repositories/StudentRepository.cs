using Microsoft.EntityFrameworkCore;
using ProjectHub.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHub.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _appDbContext;

        public StudentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddStudent(Student student)
        {
            _appDbContext.Students.Add(student);
            _appDbContext.SaveChanges();
        }

        public void DeleteStudent(int studentId)
        {
            Student student = _appDbContext.Students.FirstOrDefault(s => s.StudentId == studentId);
            _appDbContext.Students.Remove(student);
            _appDbContext.SaveChanges();
        }

        public void DeleteStudent(Student student)
        {
            _appDbContext.Students.Remove(student);
            _appDbContext.SaveChanges();
        }

        public void EditStudent(Student student)
        {
            _appDbContext.Students.Update(student);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _appDbContext.Students.Include(s => s.User);
        }

        public Student GetStudentByCode(string studentCode)
        {
            return _appDbContext.Students.FirstOrDefault(s => s.StudentCode == studentCode);
        }

        public Student GetStudentById(int studentId)
        {
            return _appDbContext.Students
                .Include(s => s.User)
                .FirstOrDefault(s => s.StudentId == studentId);
        }

        public Student GetStudentByUserId(string userId)
        {
            return _appDbContext.Students.FirstOrDefault(s => s.UserId == userId);
        }
    }
}
