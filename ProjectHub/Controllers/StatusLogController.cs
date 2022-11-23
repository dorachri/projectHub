using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectHub.Repositories;

namespace ProjectHub.Controllers
{
    public class StatusLogController : Controller
    {
        private readonly IStatusLogRepository _statusLogRepository;
        private readonly IProjectRepository _projectRepository;

        public StatusLogController(
            IStatusLogRepository statusLogRepository,
            IProjectRepository projectRepository)
        {
            _statusLogRepository = statusLogRepository;
            _projectRepository = projectRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public IActionResult GetStudentProjectStatusLogs(int id)
        {
            var project = _projectRepository.GetProjectByStudentId(id);

            return RedirectToAction("StatusLogs", "Project", new { id = project.ProjectId });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _statusLogRepository.DeleteStatusLog(id);
            string returnUrl = Request.Headers["Referer"].ToString();
            return Redirect(returnUrl);
        }
    }
}
