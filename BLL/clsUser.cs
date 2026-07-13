using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class clsUser
    {

        /// <summary>
        /// Administrative add: Accepts FullInputDTO to allow setting all fields (Role, Status, etc.).
        /// </summary>
        public static async Task<int> addAdminUser(UserFullInputDTO dto)
        {
            var fullDto = new UserFullOutputDTO
            {
                UserID = dto.UserID,
                PersonID = dto.PersonID,
                UserName = dto.UserName,
                IsActive = dto.IsActive,
                UserRole = dto.UserRole
            };

            // Business Logic: Generate cryptography properties for Admin-driven user creation
            if (!string.IsNullOrEmpty(dto.Password))
            {
                string salt = Shared.clsSecurityHelper.GenerateSalt();
                fullDto.PasswordSalt = salt;
                fullDto.PasswordHash = Shared.clsSecurityHelper.ComputeHash(dto.Password, salt);
            }
            return await UserDAL.addUser(fullDto);
        }

        /// <summary>
        /// Administrative full update using FullInputDTO. Allows modification of all columns.
        /// </summary>
        public static async Task<bool> updateUser(UserFullInputDTO dto)
        {
            var fullDto = new UserFullOutputDTO
            {                UserID = dto.UserID,
                PersonID = dto.PersonID,
                UserName = dto.UserName,
                IsActive = dto.IsActive,
                UserRole = dto.UserRole
            };

            // Business Logic: Admin-driven password overwrite
            if (!string.IsNullOrEmpty(dto.Password))
            {
                string salt = Shared.clsSecurityHelper.GenerateSalt();
                fullDto.PasswordSalt = salt;
                fullDto.PasswordHash = Shared.clsSecurityHelper.ComputeHash(dto.Password, salt);
            }
            else
            {
                var existingUser = await getUserByID(dto.UserID);
                if(existingUser != null)
                {
                    fullDto.PasswordHash = existingUser.PasswordHash;
                    fullDto.PasswordSalt = existingUser.PasswordSalt;
                }
            }
            return await UserDAL.updateUser(fullDto);
        }
        public static async Task<AuthDTO> checkLogin(string Username, string Password)
        {
            // 1. Retrieve cryptographic security data from the database
            AuthDTO authData = await UserDAL.getHashAndSalt(Username);

            if (authData == null) return null;

            // 2. Compute the hash using the provided password and the retrieved salt
            string generatedHash = clsSecurityHelper.ComputeHash(Password, authData.PasswordSalt);
            
            // 3. Verify if the computed hash matches the stored password hash
            if (generatedHash == authData.PasswordHash) 
            {
                return authData; // Return the AuthDTO object if login is successful to generate the token
            }
            else
            {
                return null; 
            }
        }    public static async Task<int> RegisterUser(Shared.RegisterRequestDTO registerDto)
    {
        if (registerDto == null || string.IsNullOrEmpty(registerDto.Password))
            throw new ArgumentException("Password is required for registration.");

        string salt = clsSecurityHelper.GenerateSalt();
        string hash = clsSecurityHelper.ComputeHash(registerDto.Password, salt);

        var fullDto = new UserFullOutputDTO
        {
            UserID = -1,
            PersonID = registerDto.PersonID,
            UserName = registerDto.UserName,
            PasswordHash = hash,
            IsActive = true,
            PasswordSalt = salt,
        };

        return await UserDAL.addUser(fullDto);
    }
        /// <summary>
        /// Adds a new record into the system, automatically embedding internal business rules and states.
        /// </summary>
        public static async Task<int> addUser(UserBriefInputDTO dto)
        {
            var fullDto = new UserFullOutputDTO
            {
                UserID = dto.UserID,
                PersonID = dto.PersonID,
                UserName = dto.UserName
            };
            fullDto.IsActive = true;

            // Business Logic: Generate cryptography properties securely behind the scenes
            if (!string.IsNullOrEmpty(dto.Password))
            {
                string salt = Shared.clsSecurityHelper.GenerateSalt();
                fullDto.PasswordSalt = salt;
                fullDto.PasswordHash = Shared.clsSecurityHelper.ComputeHash(dto.Password, salt);
            }
            else
            {
                return -1;
            }
            return await UserDAL.addUser(fullDto);
        }

        public static async Task<UserFullOutputDTO> getUserByID(int UserID)
        {
            UserFullOutputDTO fullDto = await UserDAL.getUserByID(UserID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.PersonID != default)
            {
                fullDto.PersonDetails = await Person.getPersonByID((int)fullDto.PersonID);
            }

            return fullDto;
        }

public static async Task<UserBriefOutputDTO> getUserBriefOutputByID(int UserID)
{
    // Fetch the full flat record from DAL
    var fullDto = await UserDAL.getUserByID(UserID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new UserBriefOutputDTO
    {
                PersonDetails = fullDto.PersonID != default ? await Person.getPersonBriefOutputByID((int)fullDto.PersonID) : null,
                    UserID = fullDto.UserID,
                    PersonID = fullDto.PersonID,
                    UserName = fullDto.UserName,
                    IsActive = fullDto.IsActive,
                    UserRole = fullDto.UserRole
    };
}
        public static async Task<UserFullOutputDTO> getUserByUserName(string UserName)
        {
            UserFullOutputDTO fullDto = await UserDAL.getUserByUserName(UserName);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.PersonID != default)
            {
                fullDto.PersonDetails = await Person.getPersonByID((int)fullDto.PersonID);
            }

            return fullDto;
        }

public static async Task<UserBriefOutputDTO> getUserBriefOutputByUserName(string UserName)
{
    // Fetch the full flat record from DAL
    var fullDto = await UserDAL.getUserByUserName(UserName);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new UserBriefOutputDTO
    {
                PersonDetails = fullDto.PersonID != default ? await Person.getPersonBriefOutputByID((int)fullDto.PersonID) : null,
                    UserID = fullDto.UserID,
                    PersonID = fullDto.PersonID,
                    UserName = fullDto.UserName,
                    IsActive = fullDto.IsActive,
                    UserRole = fullDto.UserRole
    };
}
        /// <summary>
        /// Safe update for regular users using BriefInputDTO. Preserves system-controlled fields.
        /// </summary>
        public static async Task<bool> updateUser(UserBriefInputDTO dto)
        {
            // 1. Fetch the existing full record to preserve internal data (Roles, Active status, Balance, etc.)
            var existingRecord = await getUserByID(dto.UserID);
            if (existingRecord == null) return false;

            // 2. Safely overwrite only the client-editable properties                existingRecord.UserID = dto.UserID;
                existingRecord.PersonID = dto.PersonID;
                existingRecord.UserName = dto.UserName;
            // Business Logic: If a new password is provided by the user, re-hash it.
            if (!string.IsNullOrEmpty(dto.Password))
            {
                string salt = Shared.clsSecurityHelper.GenerateSalt();
                existingRecord.PasswordSalt = salt;
                existingRecord.PasswordHash = Shared.clsSecurityHelper.ComputeHash(dto.Password, salt);
            }
            
            // 3. Forward the fully preserved record to the DAL layer
            return await UserDAL.updateUser(existingRecord);
        }
        public static async Task<bool> deleteUser(int UserID)
        {
            if (await isUserExistByID(UserID))
            {
                return await UserDAL.deleteUser(UserID);
            }
            return false;
        }
        public static Task<bool> isUserExistByID(int UserID)
        {
            return UserDAL.isUserExistByID(UserID);
        }

        public static async Task<List<UserBriefOutputDTO>> Paging(int rowsPerPage, int pageNumber, string sortColumn, string direction)
        {
            List<UserFullOutputDTO> fullList = await UserDAL.PagingDAL(rowsPerPage, pageNumber, sortColumn, direction);
            List<UserBriefOutputDTO> briefList = new List<UserBriefOutputDTO>();
            
            foreach (UserFullOutputDTO item in fullList)
            {
                var briefItem = new UserBriefOutputDTO
                {                    UserID = item.UserID,
                    PersonID = item.PersonID,
                    UserName = item.UserName,
                    IsActive = item.IsActive,
                    UserRole = item.UserRole
                };

                // Populate nested object for brief list item
                if (item.PersonID != default)
                {
                    briefItem.PersonDetails = await Person.getPersonBriefOutputByID((int)item.PersonID);
                }

                briefList.Add(briefItem);
            }
            
            return briefList;
        }
        public static async Task<List<UserBriefOutputDTO>> getAllBrief()
        {
            List<UserFullOutputDTO> fullList = await UserDAL.getAll();
            List<UserBriefOutputDTO> briefList = new List<UserBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new UserBriefOutputDTO
                {
                PersonDetails = item.PersonID != default ? await Person.getPersonBriefOutputByID((int)item.PersonID) : null,
                    UserID = item.UserID,
                    PersonID = item.PersonID,
                    UserName = item.UserName,
                    IsActive = item.IsActive,
                    UserRole = item.UserRole                });
            }
            return briefList;
        }
        public static async Task<List<UserFullOutputDTO>> getAllFull()
        {
            List<UserFullOutputDTO> fullList = await UserDAL.getAll();
            foreach (var item in fullList)
            {
                if (item.PersonID != default)
                {
                    item.PersonDetails = await Person.getPersonByID((int)item.PersonID);
                }
            }
            return fullList;
        }

    }
}
