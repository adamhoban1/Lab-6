using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Student_Class_Library;
using Student_MVC_App.Data;
using System;
using System.Threading.Tasks;

namespace Student_MVC_App.Pages.Projects
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly StudentsContext _context;
        private readonly UserManager<Appusers> _userManager;

        public CreateModel(StudentsContext context, UserManager<Appusers> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public project Project { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            Project.OwnerId = user.Id;

            _context.projects.Add(Project);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}