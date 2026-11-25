using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;
using Bison.Infrastructure.Repositories;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Bison.Web.Pages;

[Authorize] // Only authenticated users can access this page
public class AboutMeModel : PageModel
{
    private readonly BisonDBContext _context;
    private readonly IObservationService _service;

    public Author? CurrentUser { get; set; }
    public List<Observation> MyObservations { get; set; } = new();
    public List<Comment> MyComments { get; set; } = new();
    public List<Proposal> MyProposals { get; set; } = new();

    public AboutMeModel(BisonDBContext context, IObservationService service)
    {
        _context = context;
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userName = User.Identity?.Name;
        if (string.IsNullOrEmpty(userName))
        {
            return Redirect("/");
        }

        // Get current user with all their data
        CurrentUser = await _context.Authors
            .FirstOrDefaultAsync(a => a.UserName == userName);

        if (CurrentUser == null)
        {
            return Redirect("/");
        }

        // Get user's observations
        MyObservations = await _context.Observations
            .Include(o => o.Taxon)
            .Where(o => o.Author.Id == CurrentUser.Id)
            .OrderByDescending(o => o.TimeStamp)
            .ToListAsync();

        // Get user's comments
        MyComments = await _context.Comments
            .Include(c => c.Observation)
            .Where(c => c.Author.Id == CurrentUser.Id)
            .OrderByDescending(c => c.TimeStamp)
            .ToListAsync();

        // Get user's proposals
        MyProposals = await _context.Proposals
            .Include(p => p.Observation)
            .Where(p => p.Author.Id == CurrentUser.Id)
            .OrderByDescending(p => p.TimeStamp)
            .ToListAsync();

        return Page();
    }
}
