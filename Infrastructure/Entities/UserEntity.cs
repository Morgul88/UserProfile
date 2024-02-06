using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class UserEntity
{
    
    public int Id { get; set; }

    
    public string Email { get; set; } = null!;

    
    public string Password { get; set; } = null!;

    public virtual UserProfileEntity? UserProfile { get; set; }
}