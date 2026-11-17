using Microsoft.AspNetCore.Identity;

namespace Models;
public class Author : IdentityUser<int>
{
    //public int Id { get; set; } // Doesn't need as IdentityUser already has 'Id'
    // public string Name { get; set; } // IdentityUser already has 'UserName'
    // public string Email { get; set; } // IdentityUser already has 'Email'

    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
