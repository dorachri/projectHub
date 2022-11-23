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

namespace ProjectHub.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IStatusLogRepository _statusLogRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(
            IStudentRepository studentRepository,
            IProjectRepository projectRepository,
            IStatusLogRepository statusLogRepository,
            INotificationRepository notificationRepository,
            IHubContext<NotificationHub> notificationHub,
            UserManager<ApplicationUser> userManager)
        {
            _studentRepository = studentRepository;
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
            return View(new StudentViewModel());
        }

        [HttpPost]
        [AdminAndAnonymousFilter]
        public IActionResult Create(StudentViewModel studentViewModel)
        {
            var student = new Student
            {
                StudentCode = studentViewModel.StudentCode,
                Institution = studentViewModel.Institution,
                Department = studentViewModel.Department,
                School = studentViewModel.School,
                Degree = studentViewModel.Degree
            };

            if (ModelState.IsValid)
                _studentRepository.AddStudent(student);

            return RedirectToPage("/Account/Register", new { area = "Identity", id = student.StudentId, role = "Student", isRedirect = true });
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [Authorize(Roles = "Student")]
        public IActionResult Edit()
        {
            var student = _studentRepository.GetStudentByUserId(_userManager.GetUserId(User));
            if (student == null)
                return NotFound();

            var studentViewModel = ControllerHelper.CreateStudentViewModel(student);

            return View(studentViewModel);
        }

        [Authorize(Roles = "Student")]
        public IActionResult Project()
        {
            var student = _studentRepository.GetStudentByUserId(_userManager.GetUserId(User));
            var repProject = _projectRepository.GetAllUserProjects(student.StudentId, "Student");

            var projectViewModelList = new List<ProjectViewModel>();
            foreach (var project in repProject)
            {
                var projectViewModel = ControllerHelper.CreateProjectViewModel(project);
                projectViewModelList.Add(projectViewModel);
            }

            return View(projectViewModelList);
        }

        [Authorize(Roles = "Student")]
        public IActionResult EditProject(int id)
        {
            var project = _projectRepository.GetProjectById(id);
            if (project == null)
                return NotFound();

            var student = _studentRepository.GetStudentByUserId(_userManager.GetUserId(User));

            if (student.StudentId == project.StudentId && project.Status < 5)
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
        [Authorize(Roles = "Student")]
        public async System.Threading.Tasks.Task<IActionResult> EditProject(ProjectViewModel projectViewModel)
        {
            var project = _projectRepository.GetProjectByCode(projectViewModel.ProjectCode);
            if (project == null)
                return NotFound();

            var previousStatus = project.Status;

            project.Notes = projectViewModel.Notes;
            project.ProjectUrl = projectViewModel.ProjectUrl;
            project.Status = projectViewModel.Status;

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
                        UserId = project.Student.UserId,
                        EventDate = DateTime.Now
                    };

                    var notificationToProfessor = new Notification
                    {
                        ProjectId = project.ProjectId,
                        ProjectCode = project.ProjectCode,
                        UserId = project.Professor.UserId,
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
                    _notificationRepository.AddNotification(notificationToProfessor);
                    _notificationRepository.AddNotification(notificationToCompany);
                    await _notificationHub.Clients.Users(project.Professor.UserId, project.Company.UserId).SendAsync("ReceiveNotification");
                }
            }

            return View(projectViewModel);
        }
    }
}
