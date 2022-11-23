using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectHub.Areas.Identity.Data;
using ProjectHub.Models;
using ProjectHub.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHub.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(
            INotificationRepository notificationRepository,
            IProjectRepository projectRepository,
            IStudentRepository studentRepository,
            IProfessorRepository professorRepository,
            ICompanyRepository companyRepository,
            UserManager<ApplicationUser> userManager)
        {
            _notificationRepository = notificationRepository;
            _projectRepository = projectRepository;
            _studentRepository = studentRepository;
            _professorRepository = professorRepository;
            _companyRepository = companyRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserNotificationsAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var role = await _userManager.GetRolesAsync(user);
            List<Project> userProjects = new List<Project>();

            if (role.FirstOrDefault() == "Student")
            {
                var student = _studentRepository.GetStudentByUserId(userId);
                userProjects = _projectRepository.GetAllUserProjects(student.StudentId, "Student").ToList();
            }
            else if (role.FirstOrDefault() == "Professor")
            {
                var professor = _professorRepository.GetProfessorByUserId(userId);
                userProjects = _projectRepository.GetAllUserProjects(professor.ProfessorId, "Professor").ToList();
            }
            else if (role.FirstOrDefault() == "Company")
            {
                var company = _companyRepository.GetCompanyByUserId(userId);
                userProjects = _projectRepository.GetAllUserProjects(company.CompanyId, "Company").ToList();
            }

            if (userProjects.FirstOrDefault(p => p.ProjectId == id) == null)
                id = 0;

            List<Notification> notifications;

            if (id == 0)
            {
                notifications = _notificationRepository.GetAllUserNotifications(userId).ToList();
                return View(notifications);
            }
            else
            {
                notifications = _notificationRepository.GetAllUserProjectNotifications(userId, id).ToList();
                return View(notifications);
            }
        }

        public IActionResult Delete(int id)
        {
            _notificationRepository.DeleteNotification(id);
            string returnUrl = Request.Headers["Referer"].ToString();
            return Redirect(returnUrl);
        }

        public IActionResult MarkAsRead(int id)
        {
            var notification = _notificationRepository.GetNotificationById(id);
            notification.IsSeen = true;

            if (ModelState.IsValid)
                _notificationRepository.EditNotification(notification);

            string returnUrl = Request.Headers["Referer"].ToString();
            return Redirect(returnUrl);
        }

        public IActionResult MarkAllAsRead()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = _notificationRepository.GetAllUserNotifications(userId);

            if (notifications.Any())
            {
                foreach (var notification in notifications.ToList())
                {
                    if (notification.IsSeen == false)
                    {
                        notification.IsSeen = true;

                        if (ModelState.IsValid)
                            _notificationRepository.EditNotification(notification);
                    }
                }
            }

            string returnUrl = Request.Headers["Referer"].ToString();
            return Redirect(returnUrl);
        }

        public IActionResult UserNotificationsPartial()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = _notificationRepository.GetAllUserNotifications(userId);

            return PartialView("_UserNotificationsPartial", notifications);
        }
    }
}