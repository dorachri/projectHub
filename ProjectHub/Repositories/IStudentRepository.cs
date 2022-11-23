using ProjectHub.Models;
using System.Collections.Generic;

namespace ProjectHub.Repositories
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAllStudents();

        Student GetStudentById(int studentId);

        Student GetStudentByUserId(string userId);

        Student GetStudentByCode(string studentCode);

        void AddStudent(Student student);

        void EditStudent(Student student);

        void DeleteStudent(int studentId);

        void DeleteStudent(Student student);
    }
}
