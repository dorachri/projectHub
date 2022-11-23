using ProjectHub.Models;
using ProjectHub.Repositories;
using ProjectHub.ViewModels;
using System;

namespace ProjectHub.Helpers
{
    public static class ControllerHelper
    {
        public static ProjectViewModel CreateProjectViewModel(Project project)
        {
            var projectViewModel = new ProjectViewModel
            {
                ProjectId = project.ProjectId,
                ProjectCode = project.ProjectCode,
                Title = project.Title,
                Description = project.Description,
                Notes = project.Notes,
                PublicationDate = project.PublicationDate,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ProjectUrl = project.ProjectUrl,
                Status = project.Status,
                Student = project.Student,
                Professor = project.Professor,
                Company = project.Company
            };

            return projectViewModel;
        }

        public static StudentViewModel CreateStudentViewModel(Student student)
        {
            var studentViewModel = new StudentViewModel
            {
                StudentId = student.StudentId,
                StudentCode = student.StudentCode,
                Institution = student.Institution,
                Department = student.Department,
                School = student.School,
                Degree = student.Degree,
                User = student.User
            };

            return studentViewModel;
        }

        public static ProfessorViewModel CreateProfessorViewModel(Professor professor)
        {
            var professorViewModel = new ProfessorViewModel
            {
                ProfessorId = professor.ProfessorId,
                Institution = professor.Institution,
                Department = professor.Department,
                School = professor.School,
                Rank = professor.Rank,
                User = professor.User
            };

            return professorViewModel;
        }

        public static CompanyViewModel CreateCompanyViewModel(Company company)
        {
            var companyViewModel = new CompanyViewModel
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                Type = company.Type,
                Industry = company.Industry,
                Specialties = company.Specialties,
                Website = company.Website,
                User = company.User
            };

            return companyViewModel;
        }

        public static string CreateProjectCode(IProjectRepository projectRepository)
        {
            string projectCode = string.Empty;
            bool projectCodeExists = true;

            while (projectCodeExists)
            {
                Random rnd = new Random();
                int number = rnd.Next(1, 1000);
                string numberStr = number.ToString();
                numberStr = numberStr.PadLeft(4, '0');
                projectCode = DateTime.Now.ToString("ddMMyyyy") + numberStr;
                projectCodeExists = projectRepository.ProjectCodeExists(projectCode);
            }

            return projectCode;
        }
    }
}
