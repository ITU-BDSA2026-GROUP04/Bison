
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Bison.Razor.Pages;

public class FilteredObservationsModel : PageModel
{
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public FilteredObservationsModel(IObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet(int id, int page = 1)
    {
        Observations = _service.GetFilteredObservations(id, page);
        return Page();
    }
}
