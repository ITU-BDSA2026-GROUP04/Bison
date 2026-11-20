using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;

namespace Bison.Web.Pages;

public class PublicModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; } = new(); //added the new() 

    //Bind properties to connect HTML form inputs with PageModel 
    [BindProperty]
    public string Text {get; set; } = string.Empty;

    [BindProperty]
    public string? ImageURL{get; set; }
    
    [BindProperty]
    public int TaxonId{get; set;} = 1;


    public PublicModel(IObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet([FromQuery] int page = 1)
    {
        Observations = _service.GetObservations(page);
        return Page();
    }
 
    //New; cause noting was working once clicked share -> might be because this was missing
    public ActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            Observations = _service.GetObservations(1); //Reloading the timeline
            return Page();
        }

        var authorName = User.Identity?.Name; //This is what the user writes

        if (string.IsNullOrEmpty(authorName)) //Extra precaution
        {
            return RedirectToPage(); 
        }

        _service.createObservation(Text, authorName, TaxonId, ImageURL);
        return RedirectToPage();

    }


}
