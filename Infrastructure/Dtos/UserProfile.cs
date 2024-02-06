using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dtos;

public class UserProfile
{
    public int Id { get; set; }

    public string FirstNAme { get; set; } = null!;

    public string LastNAme { get; set; } = null!;

    public string? Gender { get; set; }

    public string RoleType { get; set; } = null!;

    public string AdressName { get; set; } = null!;

    public string City { get; set; } = null!;

    public string StreetName { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Comment { get; set; } = null!;
}
