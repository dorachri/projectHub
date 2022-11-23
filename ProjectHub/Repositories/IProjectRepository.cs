using ProjectHub.Models;
using System.Collections.Generic;

namespace ProjectHub.Repositories
{
    public interface IProjectRepository
    {
        Project GetProjectByStudentId(int studentId);

        IEnumerable<Project> GetAllProjects();

        IEnumerable<Project> GetAllUserProjects(int id, string role);

        IEnumerable<Project> SearchByProjectInfo(string projectCode, string title, int searchRange);

        IEnumerable<Project> SearchByCompanyInfo(string companyName, string industry, int searchRange);

        Project GetProjectById(int projectId);

        Project GetProjectByCode(string projectCode);

        bool ProjectCodeExists(string projectCode);

        void AddProject(Project project);

        void EditProject(Project project);

        void DeleteProject(Project project);
    }
}
