using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProjectHub.Areas.Identity.Data;
using ProjectHub.Filters;
using ProjectHub.Helpers;
using ProjectHub.Hubs;
using ProjectHub.Models;
using ProjectHub.Repositories;
using ProjectHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHub.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IStatusLogRepository _statusLogRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfessorController(
            IProfessorRepository professorRepository,
            IProjectRepository projectRepository,
            IStatusLogRepository statusLogRepository,
            INotificationRepository notificationRepository,
            IHubContext<NotificationHub> notificationHub,
            UserManager<ApplicationUser> userManager)
        {
            _professorRepository = professorRepository;
            _projectRepository = projectRepository;
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

        [AdminAndAnonymousFilter]
        public IActionResult Create()
        {
            return View(new ProfessorViewModel());
        }

        [HttpPost]
        [AdminAndAnonymousFilter]
        public IActionResult Create(ProfessorViewModel professorViewModel)
        {
            var professor = new Professor
            {
                Institution = professorViewModel.Institution,
                Department = professorViewModel.Department,
                School = professorViewModel.School,
                Rank = professorViewModel.Rank
            };

            if (ModelState.IsValid)
                _professorRepository.AddProfessor(professor);

            return RedirectToPage("/Account/Register", new { area = "Identity", id = professor.ProfessorId, role = "Professor", isRedirect = true });
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var professor = _professorRepository.GetProfessorById(id);
            if (professor == null)
                return NotFound();

            return View(professor);
        }

        [Authorize(Roles = "Professor")]
        public IActionResult Edit()
        {
            var professor = _professorRepository.GetProfessorByUserId(_userManager.GetUserId(User));
            if (professor == null)
                return NotFound();

            var professorViewModel = ControllerHelper.CreateProfessorViewModel(professor);

            return View(professorViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Professor")]
        public IActionResult Edit(ProfessorViewModel professorViewModel)
        {
            var professor = _professorRepository.GetProfessorByUserId(_userManager.GetUserId(User));

            professor.Rank = professorViewModel.Rank;

            if (ModelState.IsValid)
                _professorRepository.EditProfessor(professor);
            
            return View(professorViewModel);
        }

        [Authorize(Roles = "Professor")]
        public IActionResult Projects(int id)
        {
            var professor = _professorRepository.GetProfessorByUserId(_userManager.GetUserId(User));
            var projects = _projectRepository.GetAllUserProjects(professor.ProfessorId, "Professor");

            var projectViewModelList = new List<ProjectViewModel>();
            foreach (var project in projects)
            {
                var projectViewModel = ControllerHelper.CreateProjectViewModel(project);
                projectViewModelList.Add(projectViewModel);
            }

            if (id >= 2 && id <= 5)
                return View(projectViewModelList.Where(p => p.Status == id).ToList());
            else
                return View(projectViewModelList);
        }

        [Authorize(Roles = "Professor")]
        public IActionResult EditProject(int id)
        {
            var project = _projectRepository.GetProjectById(id);
            if (project == null)
                return NotFound();

            var professor = _professorRepository.GetProfessorByUserId(_userManager.GetUserId(User));

            if (professor.ProfessorId == project.ProfessorId && project.Status < 5)
            {
                var projectViewModel = ControllerHelper.CreateProjectViewModel(project);
                return View(projectViewModel);
            }
            else
            {
                TempData["Message"] = "You are not allowed to edit a project after it has been completed!";
                string returnUrl = Request.Headers["Referer"].ToString();
                return Redirect(returnUrl);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Professor")]
        public async System.Threading.Tasks.Task<IActionResult> EditProject(ProjectViewModel projectViewModel)
        {
            var project = _projectRepository.GetProjectByCode(projectViewModel.ProjectCode);
            if (project == null)
                return NotFound();

            var previousStatus = project.Status;

            project.Notes = projectViewModel.Notes;
            project.EndDate = projectViewModel.EndDate;
            project.Status = projectViewModel.Status;

            if (project.Status == 5)
                project.EndDate = DateTime.Now.Date;

            if (ModelState.IsValid)
            {
                _projectRepository.EditProject(project);

                if (project.Status != previousStatus)
                {
                    var statusLog = new StatusLog
                    {
                        ProjectId = project.ProjectId,
                        ProjectCode = project.ProjectCode,
                        OldStatus = previousStatus,
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
                }
            }

            return View(projectViewModel);
        }

        [Authorize(Roles = "Professor")]
        public IActionResult DeleteProject(int id)
        {
            var project = _projectRepository.GetProjectById(id);
            if (project == null)
                return NotFound();

            if (project.Status < 5)
            {
                project.ProfessorId = null;
                project.StudentId = null;
                project.StartDate = DateTime.MinValue;
                project.EndDate = DateTime.MinValue;
                project.Status = 1;

                if (ModelState.IsValid)
                {
                    _projectRepository.EditProject(project);
                    _notificationRepository.DeleteAllProjectsNotifications(project.ProjectId);
                    _statusLogRepository.DeleteAllProjectsStatusLogs(project.ProjectId);
                }
            }
            else
                TempData["Message"] = "You are not allowed to delete a project after it has been completed!";

            string returnUrl = Request.Headers["Referer"].ToString();
            return Redirect(returnUrl);
        }

        [Authorize(Roles = "Professor")]
        public IActionResult Dashboard()
        {
            var professor = _professorRepository.GetProfessorByUserId(_userManager.GetUserId(User));
            var projects = _projectRepository.GetAllUserProjects(professor.ProfessorId, "Professor")
                .Where(p => p.StartDate.Year == DateTime.Now.Year || p.EndDate >= DateTime.Now);

            return View(projects);
        }
    }
}
