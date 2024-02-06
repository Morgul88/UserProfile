using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Service;

public class UserProfileService(UserProfileRepository userProfileRepository, RoleRepository roleRepository, UserRepository userRepository, AdressRepository adressRepository, CommentRepository commentRepository)
{
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserProfileRepository _userProfileRepository = userProfileRepository;
    private readonly UserRepository _userRepository = userRepository;
    private readonly AdressRepository _adressRepository = adressRepository;
    private readonly CommentRepository _commentRepository = commentRepository;

    public bool CreateUserProfile(UserProfile user)
    {
        try
        {
            
            if (!_userProfileRepository.Exists(x => x.Id == user.Id))
            {

                //var adminRole = _roleRepository.GetOne(x => x.RoleType == "Admin");
                //var privateRole = _roleRepository.GetOne(x => x.RoleType == "Private");

                // Skapa rollerna om de inte redan finns
                //adminRole ??= _roleRepository.Create(new RoleEntity { RoleType = "Admin" });
                //privateRole ??= _roleRepository.Create(new RoleEntity { RoleType = "Private" });

                //var roleEntity = _roleRepository.GetOne(x => x.Id == user.Id);
                //roleEntity ??= _roleRepository.Create(new RoleEntity
                //{
                //    RoleType = user.RoleType
                //});

                RoleEntity roleEntity;

                if (user.RoleType == "Admin" || user.RoleType == "Private")
                {
                    roleEntity = _roleRepository.GetOne(x => x.RoleType == user.RoleType);

                    if (roleEntity == null)
                    {
                        // Rollen finns inte, skapa en ny
                        roleEntity = _roleRepository.Create(new RoleEntity
                        {
                            RoleType = user.RoleType
                        });
                    }
                }
                else
                {
                    // Ogiltigt värde för RoleType, hantera detta enligt behov
                    Debug.WriteLine("Ogiltigt värde för RoleType.");
                    return false; // eller gör något annat för att indikera felaktigt värde
                }

                var adressEntity = _adressRepository.GetOne(a => a.Id == user.Id);
                adressEntity ??= _adressRepository.Create(new AdressEntity
                {
                    City = user.City,
                    StreetName = user.StreetName,
                    PostalCode = user.PostalCode,
                    Country = user.Country,
                    PhoneNumber = user.PhoneNumber,
                });

                var userEntity = _userRepository.GetOne(x => x.Id == user.Id);
                userEntity ??= _userRepository.Create(new UserEntity
                {
                    Email = user.Email,
                    Password = user.Password,
                });

                


                if (roleEntity != null && adressEntity != null && userEntity != null)
                {
                    var userProfileEntity = new UserProfileEntity
                    {
                        FirstNAme = user.FirstNAme,
                        LastNAme = user.LastNAme,
                        Gender = user.Gender,
                        AdressId = adressEntity.Id,
                        UserId = userEntity.Id,
                        RoleId = roleEntity.Id
                        
                    };

                    var result = _userProfileRepository.Create(userProfileEntity);
                    if(userProfileEntity != null)
                    {
                        var commentEntity = _commentRepository.GetOne(x => x.Id == user.Id);
                        commentEntity ??= _commentRepository.Create(new CommentEntity
                        {
                            CommentText = user.Comment,
                            UserProfileId = userProfileEntity.Id,
                        });

                        if (commentEntity != null)
                            return true;
                    }
                    
                    
                }
                
                

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("ERROR :: " + ex.Message);
        }
        return false;
    }


    public bool UpdateProfile(UserProfileEntity userprofileEntity)
    {
        try
        {
            var updatedUserProfileEntity = _userProfileRepository.Update(x => x.Id == userprofileEntity.Id, userprofileEntity);
            return updatedUserProfileEntity != null;

        }
        catch (Exception ex)
        {
            Debug.WriteLine("ERROR :: " + ex.Message);
            // Hantera exception på ett lämpligt sätt, beroende på ditt scenarie
            return false;
        }

        return false;
    }
    public bool RemoveUserProfileById(int userId)
    {
        try
        {
            // Hämta UserProfileEntity baserat på userId
            var userProfileEntity = _userProfileRepository.GetOne(x => x.Id == userId);

            if (userProfileEntity != null)
            {
                // Ta bort UserProfileEntity, vilket kan orsaka kaskad borttagning av associerade entiteter
                _userProfileRepository.Delete(x => x.Id == userId);

                // Alternativt kan du explicit ta bort associerade entiteter separat om de inte kaskaderas
                var adressEntity = _adressRepository.GetOne(x => x.Id == userProfileEntity.AdressId);
                _adressRepository.DeleteByEntity(adressEntity);

                var userEntity = _userRepository.GetOne(x => x.Id == userProfileEntity.UserId);
                _userRepository.DeleteByEntity(userEntity);

                userProfileEntity.RoleId = 0!;
                var rolesEntity = _roleRepository.GetOne(x => x.Id == userProfileEntity.RoleId);
                _roleRepository.DeleteByEntity(rolesEntity);

                var commentEntity = _commentRepository.GetOne(x => x.Id == userProfileEntity.Id);
                _commentRepository.DeleteByEntity(commentEntity);

                return true;
            }
            else
            {
                Debug.WriteLine($"Användaren med ID {userId} hittades inte.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("ERROR :: " + ex.Message);
        }

        return false;
    }
    public IEnumerable<UserProfileEntity> GetAllUserProfile()
    {
        try
        {
            var result = _userProfileRepository.GetAll();

            
            return result;
            
        }


        catch (Exception ex)
        {
            Debug.WriteLine("ERROR :: " + ex.Message);
            // Du bör överväga att logga felet istället för att bara skriva ut det till Debug-fönstret.
        }

        return null!;
    }

    public UserProfileEntity GetUserProfileById(int userId)
    {
        try
        {
            
            Expression<Func<UserProfileEntity, bool>> predicate = x => x.Id == userId;

            
            return _userProfileRepository.GetOne(predicate);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("ERROR :: " + ex.Message);
            // Logga eller hantera felet på lämpligt sätt
            throw; // Rethrow exception för att låta det någon högre upp hantera
        }
    }





}
//public bool CreateUser(User user)
//{
//    try
//    {
//        if (!_userRepository.Exists(x => x.Email == user.Email))
//        {

//            var userProfile = _userRepository.GetOne(x => x.Email == user.Email);
//            if(userProfile == null)
//            {
//                var userEntity = new UserEntity
//                {

//                    Email = user.Email,
//                    Password = user.Password,

//                };
//                var result = _userRepository.Create(userEntity);
//                if (result != null)
//                {
//                    return true;
//                }

//            }

//            return false;


//        }

//    }
//    catch (Exception ex)
//    {
//        Debug.WriteLine("ERROR :: " + ex.Message);
//    }
//    return false;
//}
//public bool CreateRoleProfile(RoleDto role)
//{
//    try
//    {
//        if (!_roleRepository.Exists(x => x.Admin == role.Admin))
//        {

//            var roleEntity = _roleRepository.GetOne(x => x.Admin == role.Admin);
//            if (roleEntity == null)
//            {
//                var roleProfile = new RoleEntity
//                {

//                    Admin = role.Admin,
//                    Private = role.Private,

//                };
//                var result = _roleRepository.Create(roleProfile);
//                if (result != null)
//                {
//                    return true;
//                }

//            }

//            return false;


//        }

//    }
//    catch (Exception ex)
//    {
//        Debug.WriteLine("ERROR :: " + ex.Message);
//    }
//    return false;
//}
//public bool CreateAdress(AdressDto adress)
//{
//    try
//    {
//        if (!_userRepository.Exists(x => x.Id == adress.Id))
//        {

//            var adressProfile = _userRepository.GetOne(x => x.Id == adress.Id);
//            if (adressProfile == null)
//            {
//                var adressEntity = new AdressEntity
//                {
//                    City = adress.City,
//                    StreetName = adress.StreetName,
//                    PostalCode = adress.PostalCode,
//                    Country = adress.Country,
//                    PhoneNumber = adress.PhoneNumber


//                };
//                var result = _adressRepository.Create(adressEntity);
//                if (result != null)
//                {
//                    return true;
//                }

//            }

//            return false;


//        }

//    }
//    catch (Exception ex)
//    {
//        Debug.WriteLine("ERROR :: " + ex.Message);
//    }
//    return false;
//}

