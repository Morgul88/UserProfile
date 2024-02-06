using ConsoleApp.MenuService;
using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{

    services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Projects\UserProfile\Infrastructure\Data\local_db.mdf;Integrated Security=True;Connect Timeout=30"));

    services.AddScoped<AdressRepository>();
    services.AddScoped<CommentRepository>();
    services.AddScoped<RoleRepository>();
    services.AddScoped<UserProfileRepository>();
    services.AddScoped<UserRepository>();
    services.AddScoped<MenuService>();
    services.AddScoped<UserProfileService>();

}).Build();

builder.Start();


Console.Clear();


var userProfileService = builder.Services.GetRequiredService<UserProfileService>();
var menuService = builder.Services.GetRequiredService<MenuService>();



menuService.ShowMainMenu();
Console.ReadLine();









//var userResult = userProfileService.CreateUserProfile(new UserProfile
//{

//    FirstNAme = "Adam",
//    LastNAme = "Starander",
//    Gender = "Male",
//    RoleType = "Admin",
//    AdressName = "Molndal",
//    City = "Goteborg",
//    StreetName = "Bergavägen",
//    PostalCode = "43512",
//    Country = "Sweden",
//    PhoneNumber = "0733205434",
//    Email = "Adam@domain.com",
//    Password = "12345",
//    Comment = "Hejsan",

//});

//Console.Clear();
////menuService.ShowMenu();
//if(userResult)
//Console.WriteLine("Lyckaders skapa User");
//Console.ReadKey();

//var updateResult = userProfileService.UpdateRoleProfile(1, "Private");

//Console.Clear();
//if(updateResult)
//    Console.WriteLine("Uppdatering lyckas");
//if (!updateResult)
//    Console.WriteLine("Uppdatering misslyckades");


//var allUsers = userProfileService.GetAllUserProfile();

//Console.WriteLine(allUsers);
//Console.ReadLine();