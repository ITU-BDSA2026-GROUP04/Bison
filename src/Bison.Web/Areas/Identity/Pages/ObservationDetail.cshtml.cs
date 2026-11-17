using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;

namespace Bison.Web.Pages;

public class ObservationDetailModel : PageModel
{
    private readonly IObservationService _service;
    public ObservationDetailViewModel Observations { get; set; }

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
