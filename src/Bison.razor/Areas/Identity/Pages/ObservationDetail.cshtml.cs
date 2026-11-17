using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Pages;

public class ObservationDetailModel : PageModel
{
    private readonly IObservationService _service;
    public ObservationDetailViewModel Observations { get; set; }

    //Adding Bind properties: coupling data with the UI
    [BindProperty]
    public string comment {get; set;} = string.Empty; //the comment text

    [BindProperty]
    public string proposal {get; set;} = string.Empty; //the proposal text

    public ObservationDetailModel(IObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet(int id, int page = 1)
    {
        Observations = _service.GetObservationDetailsById(id, page);
        return Page();
    }


}
