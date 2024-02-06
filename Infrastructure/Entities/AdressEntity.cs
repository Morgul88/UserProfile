using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class AdressEntity
{
   
    public int Id { get; set; }

  
    public string City { get; set; } = null!;

    
    public string StreetName { get; set; } = null!;

    
    [StringLength(5)]
    public string PostalCode { get; set; } = null!;

    
    public string Country { get; set; } = null!;

    [StringLength(10)]
    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<UserProfileEntity> UserProfile { get; set; } = new List<UserProfileEntity>();

}