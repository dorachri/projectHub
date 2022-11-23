using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ProjectHub.Areas.Identity.Data;
using ProjectHub.Repositories;

namespace ProjectHub.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly ICompanyRepository _companyRepository;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,
            IStudentRepository studentRepository,
            IProfessorRepository professorRepository,
            ICompanyRepository companyRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _studentRepository = studentRepository;
            _professorRepository = professorRepository;
            _companyRepository = companyRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public int Id { get; set; }

        public string Role { get; set; }

        public bool IsRedirect { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Fist name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(int id, string role, bool isRedirect, string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Id = id;
            Role = role;
            IsRedirect = isRedirect;
        }

        public async Task<IActionResult> OnPostAsync(int id, string role, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Username, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName, Inactive = true, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, Input.Password);
                var exists = await _roleManager.RoleExistsAsync(role);

                if (result.Succeeded)
                {
                    switch (role)
                    {
                        case "Student":
                            var student = _studentRepository.GetStudentById(id);
                            student.UserId = user.Id;
                            break;
                        case "Professor":
                            var professor = _professorRepository.GetProfessorById(id);
                            professor.UserId = user.Id;
                            break;
                        case "Company":
                            var company = _companyRepository.GetCompanyById(id);
                            company.UserId = user.Id;
                            break;
                    }

                    if (!exists)
                    {
                        var newRole = new IdentityRole
                        {
                            Name = role
                        };
                        await _roleManager.CreateAsync(newRole);
                    }

                    var addToRole = await _userManager.AddToRoleAsync(user, role);

                    _logger.LogInformation("User created a new account with password.");
                    
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    switch (role)
                    {
                        case "Student":
                            _studentRepository.DeleteStudent(id);
                            break;
                        case "Professor":
                            _professorRepository.DeleteProfessor(id);
                            break;
                        case "Company":
                            _companyRepository.DeleteCompany(id);
                            break;
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            IsRedirect = true;
            return Page();
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated && !context.HttpContext.User.IsInRole("Admin"))
            {
                context.Result = new RedirectToActionResult("Index", "Home", "");
            }
        }

        public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            if (!IsRedirect)
            {
                context.Result = new RedirectToActionResult("Index", "Home", "");
            }
        }
    }
}
