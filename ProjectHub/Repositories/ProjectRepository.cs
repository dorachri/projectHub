using Microsoft.EntityFrameworkCore;
using ProjectHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHub.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProjectRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddProject(Project project)
        {
            _appDbContext.Projects.Add(project);
            _appDbContext.SaveChanges();
        }

        public bool ProjectCodeExists(string projectCode)
        {
            return _appDbContext.Projects.Any(p => p.ProjectCode == projectCode);
        }

        public void DeleteProject(Project project)
        {
            _appDbContext.Projects.Remove(project);
            _appDbContext.SaveChanges();
        }

        public void EditProject(Project project)
        {
            _appDbContext.Projects.Update(project);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return _appDbContext.Projects
                .Include(p => p.Company)
                .Include(p => p.Professor.User)
                .Include(p => p.Student);
        }

        public IEnumerable<Project> GetAllUserProjects(int id, string role)
        {
            switch (role)
            {
                case "Student":
                    return _appDbContext.Projects
                        .Where(p => p.StudentId == id)
                        .Include(p => p.Professor.User)
                        .Include(p => p.Company)
                        .Include(p => p.Notifications);
                case "Professor":
                    return _appDbContext.Projects
                        .Where(p => p.ProfessorId == id)
                        .Include(p => p.Student)
                        .Include(p => p.Company)
                        .Include(p => p.Notifications)
                        .OrderBy(p => p.Status);
                case "Company":
                    return _appDbContext.Projects
                        .Where(p => p.CompanyId == id)
                        .Include(p => p.Student)
                        .Include(p => p.Professor.User)
                        .Include(p => p.Notifications)
                        .OrderByDescending(p => p.PublicationDate);
                default:
                    return Enumerable.Empty<Project>();
            }
        }

        public Project GetProjectByCode(string projectCode)
        {
            return _appDbContext.Projects
                .Include(p => p.Company.User)
                .Include(p => p.Professor.User)
                .Include(p => p.Student.User)
                .FirstOrDefault(p => p.ProjectCode == projectCode);
        }

        public Project GetProjectById(int projectId)
        {
            return _appDbContext.Projects
                .Include(p => p.Company)
                .Include(p => p.Professor)
                .Include(p => p.Student)
                .FirstOrDefault(p => p.ProjectId == projectId);
        }

        public IEnumerable<Project> SearchByCompanyInfo(string companyName, string industry, int searchRange)
        {
            if (companyName == null && industry == null && searchRange == 0)
                return _appDbContext.Projects
                    .Where(p => p.Status == 1)
                    .Include(p => p.Company)
                    .OrderByDescending(p => p.PublicationDate);
            else if (companyName == null && industry == null && searchRange > 0)
                return _appDbContext.Projects
                    .Where(p => p.Status == 1)
                    .Where(p => p.PublicationDate >= DateTime.Now.AddMonths(-searchRange))
                    .Include(p => p.Company)
                    .OrderByDescending(p => p.PublicationDate);
            else if (searchRange == 0)
                return _appDbContext.Projects
                    .Where(p => p.Status == 1)
                    .Where(p => p.Company.CompanyName == companyName || p.Company.Industry == industry)
                    .Include(p => p.Company)
                    .OrderByDescending(p => p.PublicationDate);
            else
                return _appDbContext.Projects
                    .Where(p => p.Status == 1)
                    .Where(p => p.Company.CompanyName == companyName || p.Company.Industry == industry)
                    .Where(p => p.PublicationDate >= DateTime.Now.AddMonths(-searchRange))
                    .Include(p => p.Company)
                    .OrderByDescending(p => p.PublicationDate);
        }

        public IEnumerable<Project> SearchByProjectInfo(string projectCode, string title, int searchRange)
        {
            if (projectCode == null && title == null && searchRange == 0)
                return _appDbContext.Projects
                    .Where(p => p.Status == 1)
                    .Include(p => p.Company)
                    .OrderByDescending(p => p.PublicationDate);
            else if (projectCode == null && title == null && searchRange > 0)
                return _appDbContext.Projects
                    .Where(p => p.Status == 1)
                    .Where(p => p.PublicationDate >= DateTime.Now.AddMonths(-searchRange))
                    .Include(p => p.Company)
                    .OrderByDescending(p => p.PublicationDate);
            if (searchRange == 0)
                return _appDbContext.Projects
                    .Where(p => p.Status == 1)
                    .Where(p => p.ProjectCode == projectCode || p.Title == title)
                    .Include(p => p.Company)
                    .OrderByDescending(p => p.PublicationDate);
            else
                return _appDbContext.Projects
                    .Where(p => p.Status == 1)
                    .Where(p => p.ProjectCode == projectCode || p.Title == title)
                    .Where(p => p.PublicationDate >= DateTime.Now.AddMonths(-searchRange))
                    .Include(p => p.Company)
                    .OrderByDescending(p => p.PublicationDate);
        }

        public Project GetProjectByStudentId(int studentId)
        {
            return _appDbContext.Projects.FirstOrDefault(p => p.Student.StudentId == studentId);
        }
    }
}
