using Infrastructure.Dtos;
using Infrastructure.Service;
using Microsoft.Identity.Client;

namespace ConsoleApp.MenuService;

public class MenuService
{
    private readonly UserProfileService _userProfileService;

    public MenuService(UserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    
    public void ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Välkommen!");
            Console.WriteLine("------------------");
            Console.WriteLine("Välj alternativ:");
            Console.WriteLine();
            Console.WriteLine("1.Skapa UserProfile");
            Console.WriteLine();
            Console.WriteLine("2.Visa Profil");
            Console.WriteLine();
            Console.WriteLine("3.Visa profiler i Databas");
            Console.WriteLine();
            Console.WriteLine("4.Ta bort profil");
            Console.WriteLine();
            Console.WriteLine("5.Uppdatera Profil");
            Console.WriteLine();
            Console.WriteLine("6.Exit Application");
            Console.WriteLine();


            var answer = Console.ReadLine();

            switch (answer)
            {
                case "1":
                    ShowCreateUser();
                    break;
                case "2":
                    ShowOneMenu();
                    break;
                case "3":
                    ShowAllMenu();
                    break;
                case "4":
                    ShowRemoveMenu();
                    break;
                case "5":
                    ShowUpdateMenu();
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                
            }
            
        }
        
    }

    internal void ShowCreateUser()
    {
        Console.Clear();
        var user = new UserProfile();

        Console.WriteLine("Create New User");
        Console.WriteLine();
        Console.WriteLine("First Name: ");
        user.FirstNAme = Console.ReadLine()!;
        Console.WriteLine();
        while (string.IsNullOrWhiteSpace(user.FirstNAme))
        {
            Console.WriteLine("Invalid input. Please enter a non-empty first name.");
            user.FirstNAme = Console.ReadLine()!;
        }

        Console.WriteLine("Last Name: ");
        user.LastNAme = Console.ReadLine()!;
        Console.WriteLine();
        Console.WriteLine("Gender: ");
        user.Gender = Console.ReadLine()!;
        Console.WriteLine();
        bool isValidRole = false;
        while (!isValidRole)
        {
            Console.WriteLine("Admin eller Private?: ");
            var roleAnswer = Console.ReadLine()!;
            if (roleAnswer == "Admin" || roleAnswer == "Private")
            {
                user.RoleType = roleAnswer;
                isValidRole = true;
            }
            else
            {
                Console.WriteLine("Du måste ange en giltlig roll. Skriv Admin eller Private");
                
            }
        }
        
        

        Console.WriteLine();

        Console.WriteLine("City: ");
        user.City = Console.ReadLine()!;
        Console.WriteLine();
        Console.WriteLine("Comment: ");
        user.Comment = Console.ReadLine()!;
        Console.WriteLine();
        Console.WriteLine("Street Name: ");
        user.StreetName = Console.ReadLine()!;
        Console.WriteLine();
        Console.WriteLine("PostalCode: ");
        user.PostalCode = Console.ReadLine()!;
        Console.WriteLine();
        Console.WriteLine("Country: ");
        user.Country = Console.ReadLine()!;
        Console.WriteLine();
        Console.WriteLine("Email: ");
        user.Email = Console.ReadLine()!;
        Console.WriteLine();
        Console.WriteLine("PhoneNumber: ");
        user.PhoneNumber = Console.ReadLine()!;
        Console.WriteLine();
        Console.WriteLine("Password: ");
        user.Password = Console.ReadLine()!;
        Console.WriteLine();
        Console.WriteLine("Är du säker på att du vill skapa användaren?");
        Console.WriteLine("Skriv:  y/n");
        var answer = Console.ReadLine();

        if(answer == "y")
        {
            var result = _userProfileService.CreateUserProfile(user);

            Console.Clear();

            if (result)
            {
                Console.WriteLine("User was added succesfully.");
                Console.ReadKey();
                ShowMainMenu();
            };



            Console.ReadLine();
        }
        if(answer == "n")
        {
            ShowMainMenu();
        }
        
    }

    public void ShowAllMenu() 
    {
        Console.Clear();
        Console.WriteLine("All user from database");

        var result = _userProfileService.GetAllUserProfile();
        if (result.Any())
        {
            Console.Clear();
            foreach (var res in result)
            {
                Console.WriteLine($"User ID: {res.Id}");
                Console.WriteLine($"First Name: {res.FirstNAme}");
                Console.WriteLine($"Last Name: {res.LastNAme}");
                Console.WriteLine($"Gender: {res.Gender}");
                Console.WriteLine($"City: {res.Adress.City}");
                Console.WriteLine($"Email: {res.User.Email}");
                Console.WriteLine($"Role: {res.Role.RoleType}");
                Console.WriteLine("-----------------------------");
            }
            
        }
        Console.WriteLine();
        Console.ReadKey();
    }

    public void ShowOneMenu()
    {
        Console.Clear();
        Console.WriteLine("Show One User:");
        Console.WriteLine();
        Console.WriteLine("Vilket Id vill du hämta?");
        
        if (int.TryParse(Console.ReadLine(), out int userId))
        {
            var resultByUser = _userProfileService.GetUserProfileById(userId);
            Console.Clear();
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"User ID: {resultByUser.Id}");
            Console.WriteLine($"FirstName: {resultByUser.FirstNAme}");
            Console.WriteLine($"LastName: {resultByUser.LastNAme}");
            Console.WriteLine($"Gender: {resultByUser.Gender}");
            Console.WriteLine($"City: {resultByUser.Adress.City}");
            Console.WriteLine($"Email: {resultByUser.User.Email}");
            Console.WriteLine($"Role: {resultByUser.Role.RoleType}");
            Console.WriteLine("---------------------------------------");
        }
        Console.ReadKey();
        
    }

    public void ShowRemoveMenu()
    {
        Console.WriteLine("Remove User");
        Console.WriteLine();
        Console.WriteLine("Vilket user vill du ta bort? Ange Id:");

        if (int.TryParse(Console.ReadLine(), out int userId))
        {
            var userProfileEntity = _userProfileService.GetUserProfileById(userId);

            if (userProfileEntity != null)
            {
                var result = _userProfileService.RemoveUserProfileById(userId);

                if (result)
                {
                    Console.Clear();
                    Console.WriteLine($"Användaren med ID {userId} har tagits bort.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Misslyckades med att ta bort användaren med ID {userId}.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Användaren med ID {userId} hittades inte.");
            }

            Console.ReadKey();
        }

        
    }

    public void ShowUpdateMenu()
    {
        Console.Clear();
        Console.WriteLine("Update Your Profile");

        Console.WriteLine("Enter the ID you want to update:");

        if (int.TryParse(Console.ReadLine(), out int userId))
        {
            var resultByUser = _userProfileService.GetUserProfileById(userId);

            if (resultByUser != null)
            {
                Console.WriteLine("Update User");
                Console.WriteLine();

               
                Console.WriteLine($"First Name:{resultByUser.FirstNAme}");
                Console.WriteLine("New FirstName:");
                resultByUser.FirstNAme = Console.ReadLine()!;
                Console.WriteLine();

                
                Console.WriteLine($"Last Name:{resultByUser.LastNAme}");
                Console.WriteLine("New Lastname:");
                resultByUser.LastNAme = Console.ReadLine()!;
                Console.WriteLine();


                Console.WriteLine($"City:{resultByUser.Adress.City}");
                Console.WriteLine("New City:");
                resultByUser.Adress.City = Console.ReadLine()!;
                Console.WriteLine();


                Console.WriteLine($"Streetname:{resultByUser.Adress.StreetName}");
                Console.WriteLine("New Streetname:");
                resultByUser.Adress.StreetName = Console.ReadLine()!;
                Console.WriteLine();

               
                Console.WriteLine($"PostalCode:{resultByUser.Adress.PostalCode}");
                Console.WriteLine("New PostalCode:");
                resultByUser.Adress.PostalCode = Console.ReadLine()!;
                Console.WriteLine();

                
                Console.WriteLine($"Country:{resultByUser.Adress.Country}");
                Console.WriteLine("New Country:");
                resultByUser.Adress.Country = Console.ReadLine()!;
                Console.WriteLine();

                Console.WriteLine($"Email:{resultByUser.User.Email}");
                Console.WriteLine("New Email:");
                resultByUser.User.Email = Console.ReadLine()!;
                Console.WriteLine();

                Console.WriteLine($"PhoneNumber:{resultByUser.Adress.PhoneNumber}");
                Console.WriteLine("New PhoneNumber:");
                resultByUser.Adress.PhoneNumber = Console.ReadLine()!;
                Console.WriteLine();

                Console.WriteLine($"Password:{resultByUser.User.Password}");
                Console.WriteLine("New Password:");
                resultByUser.User.Password = Console.ReadLine()!;
                Console.WriteLine();

                var success = _userProfileService.UpdateProfile(resultByUser);

                if (success)
                {
                    Console.Clear();
                    Console.WriteLine("Profile updated successfully.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Failed to update profile.");
                    
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("User not found with the specified ID.");
                
            }
        }

        Console.ReadKey();
        ShowMainMenu();
    }


}
