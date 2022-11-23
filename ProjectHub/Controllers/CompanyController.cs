using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectHub.Areas.Identity.Data;
using ProjectHub.Filters;
using ProjectHub.Helpers;
using ProjectHub.Models;
using ProjectHub.Repositories;
using ProjectHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHub.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyController(
            ICompanyRepository companyRepository,
            IProjectRepository projectRepository,
            UserManager<ApplicationUser> userManager)
        {
            _companyRepository = companyRepository;
            _projectRepository = projectRepository;
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
            return View(new CompanyViewModel());
        }

        [HttpPost]
        [AdminAndAnonymousFilter]
        public IActionResult Create(CompanyViewModel companyViewModel)
        {
            var company = new Company
            {
                CompanyName = companyViewModel.CompanyName,
                Type = companyViewModel.Type,
                Industry = companyViewModel.Industry,
                Specialties = companyViewModel.Specialties,
                Website = companyViewModel.Website
            };

            if (ModelState.IsValid)
                _companyRepository.AddCompany(company);

            return RedirectToPage("/Account/Register", new { area = "Identity", id = company.CompanyId, role = "Company", isRedirect = true });
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var company = _companyRepository.GetCompanyById(id);
            if (company == null)
                return NotFound();

            return View(company);
        }

        [Authorize(Roles = "Company")]
        public IActionResult Edit()
        {
            var company = _companyRepository.GetCompanyByUserId(_userManager.GetUserId(User));
            if (company == null)
                return NotFound();

            var companyViewModel = ControllerHelper.CreateCompanyViewModel(company);

            return View(companyViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public IActionResult Edit(CompanyViewModel companyViewModel)
        {
            var company = _companyRepository.GetCompanyByUserId(_userManager.GetUserId(User));

            company.CompanyName = companyViewModel.CompanyName;
            company.Type = companyViewModel.Type;
            company.Specialties = companyViewModel.Specialties;
            company.Website = companyViewModel.Website;

            if (ModelState.IsValid)
                _companyRepository.EditCompany(company);
            
            return View(companyViewModel);
        }

        [Authorize(Roles = "Company")]
        public IActionResult Projects(int id)
        {
            var company = _companyRepository.GetCompanyByUserId(_userManager.GetUserId(User));
            var projects = _projectRepository.GetAllUserProjects(company.CompanyId, "Company");

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

        [Authorize(Roles = "Company")]
        public IActionResult EditProject(int id)
        {
            var project = _projectRepository.GetProjectById(id);
            if (project == null)
                return NotFound();

            var company = _companyRepository.GetCompanyByUserId(_userManager.GetUserId(User));

            if (company.CompanyId == project.CompanyId && project.StudentId == null && project.ProfessorId == null
                    && project.Status == 1 && project.StartDate == DateTime.MinValue)
            {
                var projectViewModel = ControllerHelper.CreateProjectViewModel(project);
                return View(projectViewModel);
            }
            else
            {
                TempData["Message"] = "You are not allowed to edit a project after it has been assigned!";
                string returnUrl = Request.Headers["Referer"].ToString();
                return Redirect(returnUrl);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public IActionResult EditProject(ProjectViewModel projectViewModel)
        {
            var project = _projectRepository.GetProjectByCode(projectViewModel.ProjectCode);
            if (project == null)
                return NotFound();

            project.Title = projectViewModel.Title;
            project.Description = projectViewModel.Description;
            project.Notes = projectViewModel.Notes;

            if (ModelState.IsValid)
                _projectRepository.EditProject(project);

            return View(projectViewModel);
        }

        [Authorize(Roles = "Company")]
        public IActionResult Dashboard()
        {
            var company = _companyRepository.GetCompanyByUserId(_userManager.GetUserId(User));
            var projects = _projectRepository.GetAllUserProjects(company.CompanyId, "Company");

            return View(projects);
        }
    }
}
