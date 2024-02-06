using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class CommentEntity
{
    
    public int Id { get; set; }

    public string? CommentText { get; set; }

    [ForeignKey("UserProfile")]
    public int UserProfileId { get; set; }
    public virtual UserProfileEntity UserProfile { get; set; } = null!;

}