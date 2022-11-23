using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProjectHub.Areas.Identity.Data;
using ProjectHub.Helpers;
using ProjectHub.Hubs;
using ProjectHub.Models;
using ProjectHub.Repositories;
using ProjectHub.ViewModels;
using System;
using System.Linq;

namespace ProjectHub.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IStatusLogRepository _statusLogRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectController(
            IProjectRepository projectRepository,
            IStudentRepository studentRepository,
            IProfessorRepository professorRepository,
            ICompanyRepository companyRepository,
            IStatusLogRepository statusLogRepository,
            INotificationRepository notificationRepository,
            IHubContext<NotificationHub> notificationHub,
            UserManager<ApplicationUser> userManager)
        {
            _projectRepository = projectRepository;
            _studentRepository = studentRepository;
            _professorRepository = professorRepository;
            _companyRepository = companyRepository;
            _statusLogRepository = statusLogRepository;
            _notificationRepository = notificationRepository;
            _notificationHub = notificationHub;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Company,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Company,Admin")]
        public IActionResult Create(ProjectViewModel projectViewModel)
        {
            string projectCode = ControllerHelper.CreateProjectCode(_projectRepository);
            var company = _companyRepository.GetCompanyByUserId(_userManager.GetUserId(User));

            var project = new Project
            {
                ProjectCode = projectCode,
                CompanyId = company.CompanyId,
                Title = projectViewModel.Title,
                Description = projectViewModel.Description,
                Notes = projectViewModel.Notes,
                PublicationDate = DateTime.Now.Date,
                Status = 1
            };

            if (ModelState.IsValid)
            {
                _projectRepository.AddProject(project);

                string returnUrl = Request.Headers["Referer"].ToString();
                return Redirect(returnUrl);
            }

            return View(projectViewModel);
        }

        [Authorize(Roles = "Professor")]
        public IActionResult Assign(int? id = null)
        {
            if (id != null)
            {
                int projectId = (int)id;
                var project = _projectRepository.GetProjectById(projectId);
                AssignViewModel assignViewModel = new AssignViewModel
                {
                    ProjectCode = project.ProjectCode
                };

                return View(assignViewModel);
            }
            else
                return View();
        }

        [HttpPost]
        [Authorize(Roles = "Professor")]
        public async System.Threading.Tasks.Task<IActionResult> Assign(AssignViewModel assignViewModel)
        {
            var professor = _professorRepository.GetProfessorByUserId(_userManager.GetUserId(User));
            var student = _studentRepository.GetStudentByCode(assignViewModel.StudentCode);
            var project = _projectRepository.GetProjectByCode(assignViewModel.ProjectCode);

            if ((project == null && assignViewModel.ProjectCode != null) || (student == null && assignViewModel.StudentCode != null))
                return NotFound();
            else if (project != null && student != null)
            {
                project.ProfessorId = professor.ProfessorId;
                project.StudentId = student.StudentId;
                project.StartDate = DateTime.Now.Date;
                project.Status = 2;

                if (ModelState.IsValid)
                {
                    _projectRepository.EditProject(project);

                    var statusLog = new StatusLog
                    {
                        ProjectId = project.ProjectId,
                        ProjectCode = project.ProjectCode,
                        OldStatus = 1,
                        NewStatus = project.Status,
                        UserId = project.Professor.UserId,
                        EventDate = DateTime.Now
                    };

                    var notificationToStudent = new Notification
                    {
                        ProjectId = project.ProjectId,
                        ProjectCode = project.ProjectCode,
                        UserId = project.Student.UserId,
                        EventDate = DateTime.Now,
                        IsSeen = false
                    };

                    var notificationToCompany = new Notification
                    {
                        ProjectId = project.ProjectId,
                        ProjectCode = project.ProjectCode,
                        UserId = project.Company.UserId,
                        EventDate = DateTime.Now,
                        IsSeen = false
                    };

                    _statusLogRepository.AddStatusLog(statusLog);
                    _notificationRepository.AddNotification(notificationToStudent);
                    _notificationRepository.AddNotification(notificationToCompany);
                    await _notificationHub.Clients.Users(project.Student.UserId, project.Company.UserId).SendAsync("ReceiveNotification");

                    return RedirectToAction("Projects", "Professor");
                }
            }
            
            return View(assignViewModel);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var project = _projectRepository.GetProjectById(id);

            if (project == null)
                return NotFound();

            ProjectViewModel projectViewModel = ControllerHelper.CreateProjectViewModel(project);

            return View(projectViewModel);
        }

        [Authorize]
        public IActionResult Search()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Search(SearchViewModel searchViewModel)
        {
            var results = Enumerable.Empty<Project>();

            if (searchViewModel.DefaultSearch == true)
                results = _projectRepository.SearchByProjectInfo(searchViewModel.ProjectCode, searchViewModel.Title, searchViewModel.SearchRange);
            else
                results = _projectRepository.SearchByCompanyInfo(searchViewModel.CompanyName, searchViewModel.Industry, searchViewModel.SearchRange);

            return View("SearchResults", results);
        }

        [Authorize(Roles = "Company,Admin")]
        public IActionResult Delete(int id)
        {
            var project = _projectRepository.GetProjectById(id);
            if (project == null)
                return NotFound();

            if (project.StudentId == null && project.ProfessorId == null && project.Status == 1 && project.StartDate == DateTime.MinValue)
                _projectRepository.DeleteProject(project);
            else
                TempData["Message"] = "You are not allowed to delete a project after it has been assigned!";

            string returnUrl = Request.Headers["Referer"].ToString();
            return Redirect(returnUrl);
        }

        [Authorize]
        public IActionResult StatusLogs(int id)
        {
            var statusLogs = _statusLogRepository.GetAllProjectStatusLogs(id).OrderByDescending(log => log.EventDate);
            var project = _projectRepository.GetProjectById(id);

            if (project == null)
                return NotFound();

            var statusLogsViewModel = new StatusLogsViewModel
            {
                ProjectCode = project.ProjectCode,
                ProjectId = project.ProjectId,
                StatusLogs = statusLogs.ToList()
            };

            return View(statusLogsViewModel);
        }
    }
}
