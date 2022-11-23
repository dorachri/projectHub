using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectHub.Areas.Identity.Data;
using ProjectHub.Helpers;
using ProjectHub.Repositories;
using ProjectHub.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            IStudentRepository studentRepository,
            IProfessorRepository professorRepository,
            ICompanyRepository companyRepository,
            IProjectRepository projectRepository,
            UserManager<ApplicationUser> userManager)
        {
            _studentRepository = studentRepository;
            _professorRepository = professorRepository;
            _companyRepository = companyRepository;
            _projectRepository = projectRepository;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(int id)
        {
            var students = _studentRepository.GetAllStudents().OrderBy(s => s.StudentId);
            
            if (id == 1)
                return View(students.Where(s => s.Degree == "Undergraduate").ToList());
            else if (id == 2)
                return View(students.Where(s => s.Degree == "Postgraduate").ToList());
            else
                return View(students.ToList());
        }

        public IActionResult Professors(int id)
        {
            var professors = _professorRepository.GetAllProfessors().OrderBy(p => p.ProfessorId);

            if (id == 1)
                return View(professors.Where(p => p.Rank == "Professor").ToList());
            else if (id == 2)
                return View(professors.Where(p => p.Rank == "Assistant Professor").ToList());
            else if (id == 3)
                return View(professors.Where(p => p.Rank == "Associate Professor").ToList());
            else
                return View(professors.ToList());
        }

        public IActionResult Companies(int id)
        {
            var companies = _companyRepository.GetAllCompanies().OrderBy(c => c.CompanyId);

            if (id == 1)
                return View(companies.Where(c => c.Type == "Privately Held").ToList());
            else if (id == 2)
                return View(companies.Where(c => c.Type == "Public Company").ToList());
            else if (id == 3)
                return View(companies.Where(c => c.Type == "Partnership").ToList());
            else
                return View(companies.ToList());
        }

        public IActionResult EditStudent(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            if (student == null)
                return NotFound();

            var studentViewModel = ControllerHelper.CreateStudentViewModel(student);

            return View(studentViewModel);
        }

        [HttpPost]
        public IActionResult EditStudent(StudentViewModel studentViewModel)
        {
            var student = _studentRepository.GetStudentById(studentViewModel.StudentId);

            student.StudentCode = studentViewModel.StudentCode;
            student.Institution = studentViewModel.Institution;
            student.Department = studentViewModel.Department;
            student.School = studentViewModel.School;
            student.Degree = studentViewModel.Degree;

            student.User.Inactive = studentViewModel.User.Inactive;
            student.User.FirstName = studentViewModel.User.FirstName;
            student.User.LastName = studentViewModel.User.LastName;
            student.User.UserName = studentViewModel.User.UserName;
            student.User.Email = studentViewModel.User.Email;
            student.User.PhoneNumber = studentViewModel.User.PhoneNumber;

            if (ModelState.IsValid)
                _studentRepository.EditStudent(student);

            return View(studentViewModel);
        }

        public IActionResult EditProfessor(int id)
        {
            var professor = _professorRepository.GetProfessorById(id);
            if (professor == null)
                return NotFound();

            var professorViewModel = ControllerHelper.CreateProfessorViewModel(professor);

            return View(professorViewModel);
        }

        [HttpPost]
        public IActionResult EditProfessor(ProfessorViewModel professorViewModel)
        {
            var professor = _professorRepository.GetProfessorById(professorViewModel.ProfessorId);

            professor.Institution = professorViewModel.Institution;
            professor.Department = professorViewModel.Department;
            professor.School = professorViewModel.School;
            professor.Rank = professorViewModel.Rank;

            professor.User.Inactive = professorViewModel.User.Inactive;
            professor.User.FirstName = professorViewModel.User.FirstName;
            professor.User.LastName = professorViewModel.User.LastName;
            professor.User.UserName = professorViewModel.User.UserName;
            professor.User.Email = professorViewModel.User.Email;
            professor.User.PhoneNumber = professorViewModel.User.PhoneNumber;

            if (ModelState.IsValid)
                _professorRepository.EditProfessor(professor);

            return View(professorViewModel);
        }

        public IActionResult EditCompany(int id)
        {
            var company = _companyRepository.GetCompanyById(id);
            if (company == null)
                return NotFound();

            var companyViewModel = ControllerHelper.CreateCompanyViewModel(company);

            return View(companyViewModel);
        }

        [HttpPost]
        public IActionResult EditCompany(CompanyViewModel companyViewModel)
        {
            var company = _companyRepository.GetCompanyById(companyViewModel.CompanyId);

            company.CompanyName = companyViewModel.CompanyName;
            company.Type = companyViewModel.Type;
            company.Industry = companyViewModel.Industry;
            company.Specialties = companyViewModel.Specialties;
            company.Website = companyViewModel.Website;

            company.User.Inactive = companyViewModel.User.Inactive;
            company.User.FirstName = companyViewModel.User.FirstName;
            company.User.LastName = companyViewModel.User.LastName;
            company.User.UserName = companyViewModel.User.UserName;
            company.User.Email = companyViewModel.User.Email;
            company.User.PhoneNumber = companyViewModel.User.PhoneNumber;

            if (ModelState.IsValid)
                _companyRepository.EditCompany(company);

            return View(companyViewModel);
        }

        public IActionResult DeleteStudent(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            if (student == null)
                return NotFound();

            var studentViewModel = ControllerHelper.CreateStudentViewModel(student);

            return View(studentViewModel);
        }

        [HttpPost, ActionName("DeleteStudent")]
        public async Task<IActionResult> DeleteStudentAsync(StudentViewModel studentViewModel)
        {
            var student = _studentRepository.GetStudentById(studentViewModel.StudentId);
            var project = _projectRepository.GetAllUserProjects(student.StudentId, "Student");

            if (student.User != null)
            {
                if (project == null || !project.Any())
                {
                    var roleRemoveResult = await _userManager.RemoveFromRoleAsync(student.User, "Student");

                    if (roleRemoveResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(student.User);
                        _studentRepository.DeleteStudent(student);
                    }
                }
                else
                    TempData["Message"] = "You are not allowed to delete a user associated with a project!";
            }
            else
                _studentRepository.DeleteStudent(student);
            
            return RedirectToAction("Students");
        }

        public IActionResult DeleteProfessor(int id)
        {
            var professor = _professorRepository.GetProfessorById(id);
            if (professor == null)
                return NotFound();

            var professorViewModel = ControllerHelper.CreateProfessorViewModel(professor);

            return View(professorViewModel);
        }

        [HttpPost, ActionName("DeleteProfessor")]
        public async Task<IActionResult> DeleteProfessorAsync(ProfessorViewModel professorViewModel)
        {
            var professor = _professorRepository.GetProfessorById(professorViewModel.ProfessorId);
            var projects = _projectRepository.GetAllUserProjects(professor.ProfessorId, "Professor");

            if (professor.User != null)
            {
                if (projects == null || !projects.Any())
                {
                    var roleRemoveResult = await _userManager.RemoveFromRoleAsync(professor.User, "Professor");

                    if (roleRemoveResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(professor.User);
                        _professorRepository.DeleteProfessor(professor);
                    }
                }
                else
                    TempData["Message"] = "You are not allowed to delete a user associated with a project!";
            }
            else
                _professorRepository.DeleteProfessor(professor);

            return RedirectToAction("Professors");
        }

        public IActionResult DeleteCompany(int id)
        {
            var company = _companyRepository.GetCompanyById(id);
            if (company == null)
                return NotFound();

            var companyViewModel = ControllerHelper.CreateCompanyViewModel(company);

            return View(companyViewModel);
        }

        [HttpPost, ActionName("DeleteCompany")]
        public async Task<IActionResult> DeleteCompanyAsync(CompanyViewModel companyViewModel)
        {
            var company = _companyRepository.GetCompanyById(companyViewModel.CompanyId);
            var projects = _projectRepository.GetAllUserProjects(company.CompanyId, "Company");

            if (company.User != null)
            {
                if (projects == null || !projects.Any())
                {
                    var roleRemoveResult = await _userManager.RemoveFromRoleAsync(company.User, "Company");

                    if (roleRemoveResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(company.User);
                        _companyRepository.DeleteCompany(company);
                    }
                }
                else
                    TempData["Message"] = "You are not allowed to delete a user associated with a project!";
            }
            else
                _companyRepository.DeleteCompany(company);

            return RedirectToAction("Companies");
        }

        public IActionResult Projects(int id)
        {
            var projects = _projectRepository.GetAllProjects().OrderByDescending(p => p.PublicationDate);
            var projectViewModelList = new List<ProjectViewModel>();

            foreach (var project in projects)
            {
                var projectViewModel = ControllerHelper.CreateProjectViewModel(project);
                projectViewModelList.Add(projectViewModel);
            }

            if (id >= 1 && id <= 5)
                return View(projectViewModelList.Where(p => p.Status == id).ToList());
            else
                return View(projectViewModelList);
        }

        public IActionResult EditProject(int id)
        {
            var project = _projectRepository.GetProjectById(id);
            if (project == null)
                return NotFound();

            var projectViewModel = ControllerHelper.CreateProjectViewModel(project);

            return View(projectViewModel);
        }

        [HttpPost]
        public IActionResult EditProject(ProjectViewModel projectViewModel)
        {
            var project = _projectRepository.GetProjectByCode(projectViewModel.ProjectCode);
            if (project == null)
                return NotFound();

            project.ProjectCode = projectViewModel.ProjectCode;
            project.Title = projectViewModel.Title;
            project.Description = projectViewModel.Description;
            project.Notes = projectViewModel.Notes;
            project.PublicationDate = projectViewModel.PublicationDate;
            project.StartDate = projectViewModel.StartDate;
            project.EndDate = projectViewModel.EndDate;
            project.ProjectUrl = projectViewModel.ProjectUrl;
            project.Status = projectViewModel.Status;

            if (ModelState.IsValid)
                _projectRepository.EditProject(project);

            return View(projectViewModel);
        }

        public IActionResult UserDashboard()
        {
            var userDashboardViewModel = new UserDashboardViewModel
            {
                AllUsers = _userManager.Users.ToList(),
                Students = _studentRepository.GetAllStudents().ToList(),
                Professors = _professorRepository.GetAllProfessors().ToList(),
                Companies = _companyRepository.GetAllCompanies().ToList()
            };

            return View(userDashboardViewModel);
        }

        public IActionResult ProjectDashboard()
        {
            var projects = _projectRepository.GetAllProjects();

            return View(projects);
        }

        public IActionResult Users(int id)
        {
            var users = _userManager.Users.OrderByDescending(u => u.LastName);

            if (id == 1)
                return View(users.Where(u => u.Inactive == false).ToList());
            else if (id == 2)
                return View(users.Where(u => u.Inactive == true).ToList());
            else
                return View(users.ToList());
        }

        public IActionResult DeleteUser(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
                await _userManager.DeleteAsync(user);

            return RedirectToAction("Users");
        }
    }
}