using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Infrastructure.Services;

namespace Bison.Web.Pages;

public class ObservationDetailModel : PageModel
{
    private readonly IObservationService _service;
    public ObservationDetailViewModel Observations { get; set; }

    //Adding Bind properties: coupling data with the UI for flow
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

    //Comment handler:
    public ActionResult PostComment(int id)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) // String.IsNullOrEmpty //Or WhiteSpace?? 
        {
            return RedirectToPage(new {id});
        }
        _service.createComment(id, comment, User.Identity.Name!);
        return RedirectToPage(new {id});
    }


    //Proposal Handler:
    public ActionResult PostProposal(int id)
    {
         if (User.Identity == null || !User.Identity.IsAuthenticated) //String.IsNullOrEmpty //Or WhiteSpace?? 
        {
            return RedirectToPage(new {id});
        }
        _service.createComment(id, proposal, User.Identity.Name!);
        return RedirectToPage(new {id});
    }

}
