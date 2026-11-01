using Dafny;
using Microsoft.AspNetCore.SignalR;

namespace Models;
public class Post
{
    public int PostId { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; }

    public int TaxonId {get; set;}
    public Taxon Taxon{get;set;}
}
