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
               
                _userProfileRepository.Delete(x => x.Id == userId);

                
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
            
            throw; 
        }

    }





}


