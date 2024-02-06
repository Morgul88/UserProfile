using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;


public class RoleEntity
{


    public int Id { get; set; }


    public string RoleType { get; set; } = null!;


    public virtual ICollection<UserProfileEntity> UserProfile { get; set; } = new List<UserProfileEntity>(); // Många till en relation. Userprofile kan ha en roll.
}

