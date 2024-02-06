using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class UserProfileEntity
{
    
    public int Id { get; set; }

    public string FirstNAme { get; set; } = null!;

    public string LastNAme { get; set; } = null!;

    [StringLength(10)]
    public string? Gender { get; set; } = null!;

    [ForeignKey("RoleEntity")]
    public int RoleId { get; set; }
    public virtual RoleEntity Role { get; set; } = null!;
    
   [ForeignKey("User")]
    public int UserId { get; set; }

    public virtual UserEntity? User { get; set; }

    [ForeignKey("AdressEntity")]
    public int AdressId { get; set; }
    public virtual AdressEntity Adress { get; set; } = null!;

    public virtual ICollection<CommentEntity> Comment { get; set; } = new List<CommentEntity>(); // En till många relation
}
