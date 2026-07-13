using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class License
    {

        /// <summary>
        /// Administrative add: Accepts FullInputDTO to allow setting all fields (Role, Status, etc.).
        /// </summary>
        public static async Task<int> addAdminLicense(LicenseFullInputDTO dto)
        {
            var fullDto = new LicenseFullOutputDTO
            {
                LicenseID = dto.LicenseID,
                ApplicationID = dto.ApplicationID,
                DriverID = dto.DriverID,
                LicenseClass = dto.LicenseClass,
                IssueDate = dto.IssueDate,
                ExpirationDate = dto.ExpirationDate,
                Notes = dto.Notes,
                PaidFees = dto.PaidFees,
                IsActive = dto.IsActive,
                IssueReason = dto.IssueReason,
                CreatedByUserID = dto.CreatedByUserID
            };

            return await LicenseDAL.addLicense(fullDto);
        }

        /// <summary>
        /// Administrative full update using FullInputDTO. Allows modification of all columns.
        /// </summary>
        public static async Task<bool> updateLicense(LicenseFullInputDTO dto)
        {
            var fullDto = new LicenseFullOutputDTO
            {                LicenseID = dto.LicenseID,
                ApplicationID = dto.ApplicationID,
                DriverID = dto.DriverID,
                LicenseClass = dto.LicenseClass,
                IssueDate = dto.IssueDate,
                ExpirationDate = dto.ExpirationDate,
                Notes = dto.Notes,
                PaidFees = dto.PaidFees,
                IsActive = dto.IsActive,
                IssueReason = dto.IssueReason,
                CreatedByUserID = dto.CreatedByUserID
            };

            return await LicenseDAL.updateLicense(fullDto);
        }
        /// <summary>
        /// Adds a new record into the system, automatically embedding internal business rules and states.
        /// </summary>
        public static async Task<int> addLicense(LicenseBriefInputDTO dto)
        {
            var fullDto = new LicenseFullOutputDTO
            {
                LicenseID = dto.LicenseID,
                ApplicationID = dto.ApplicationID,
                DriverID = dto.DriverID,
                LicenseClass = dto.LicenseClass,
                IssueDate = dto.IssueDate,
                ExpirationDate = dto.ExpirationDate,
                Notes = dto.Notes,
                PaidFees = dto.PaidFees,
                IssueReason = dto.IssueReason,
                CreatedByUserID = dto.CreatedByUserID
            };
            fullDto.IsActive = true;

            return await LicenseDAL.addLicense(fullDto);
        }

        public static async Task<LicenseFullOutputDTO> getLicenseByID(int LicenseID)
        {
            LicenseFullOutputDTO fullDto = await LicenseDAL.getLicenseByID(LicenseID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.ApplicationID != default)
            {
                fullDto.ApplicationDetails = await Application.getApplicationByID((int)fullDto.ApplicationID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.DriverID != default)
            {
                fullDto.DriverDetails = await Driver.getDriverByID((int)fullDto.DriverID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            return fullDto;
        }

public static async Task<LicenseBriefOutputDTO> getLicenseBriefOutputByID(int LicenseID)
{
    // Fetch the full flat record from DAL
    var fullDto = await LicenseDAL.getLicenseByID(LicenseID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new LicenseBriefOutputDTO
    {
                ApplicationDetails = fullDto.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)fullDto.ApplicationID) : null,
                DriverDetails = fullDto.DriverID != default ? await Driver.getDriverBriefOutputByID((int)fullDto.DriverID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                    LicenseID = fullDto.LicenseID,
                    ApplicationID = fullDto.ApplicationID,
                    DriverID = fullDto.DriverID,
                    LicenseClass = fullDto.LicenseClass,
                    IssueDate = fullDto.IssueDate,
                    ExpirationDate = fullDto.ExpirationDate,
                    Notes = fullDto.Notes,
                    PaidFees = fullDto.PaidFees,
                    IsActive = fullDto.IsActive,
                    IssueReason = fullDto.IssueReason,
                    CreatedByUserID = fullDto.CreatedByUserID
    };
}
        /// <summary>
        /// Safe update for regular users using BriefInputDTO. Preserves system-controlled fields.
        /// </summary>
        public static async Task<bool> updateLicense(LicenseBriefInputDTO dto)
        {
            // 1. Fetch the existing full record to preserve internal data (Roles, Active status, Balance, etc.)
            var existingRecord = await getLicenseByID(dto.LicenseID);
            if (existingRecord == null) return false;

            // 2. Safely overwrite only the client-editable properties                existingRecord.LicenseID = dto.LicenseID;
                existingRecord.ApplicationID = dto.ApplicationID;
                existingRecord.DriverID = dto.DriverID;
                existingRecord.LicenseClass = dto.LicenseClass;
                existingRecord.IssueDate = dto.IssueDate;
                existingRecord.ExpirationDate = dto.ExpirationDate;
                existingRecord.Notes = dto.Notes;
                existingRecord.PaidFees = dto.PaidFees;
                existingRecord.IssueReason = dto.IssueReason;
                existingRecord.CreatedByUserID = dto.CreatedByUserID;
            
            // 3. Forward the fully preserved record to the DAL layer
            return await LicenseDAL.updateLicense(existingRecord);
        }
        public static async Task<bool> deleteLicense(int LicenseID)
        {
            if (await isLicenseExistByID(LicenseID))
            {
                return await LicenseDAL.deleteLicense(LicenseID);
            }
            return false;
        }
        public static Task<bool> isLicenseExistByID(int LicenseID)
        {
            return LicenseDAL.isLicenseExistByID(LicenseID);
        }

        public static async Task<List<LicenseBriefOutputDTO>> Paging(int rowsPerPage, int pageNumber, string sortColumn, string direction)
        {
            List<LicenseFullOutputDTO> fullList = await LicenseDAL.PagingDAL(rowsPerPage, pageNumber, sortColumn, direction);
            List<LicenseBriefOutputDTO> briefList = new List<LicenseBriefOutputDTO>();
            
            foreach (LicenseFullOutputDTO item in fullList)
            {
                var briefItem = new LicenseBriefOutputDTO
                {                    LicenseID = item.LicenseID,
                    ApplicationID = item.ApplicationID,
                    DriverID = item.DriverID,
                    LicenseClass = item.LicenseClass,
                    IssueDate = item.IssueDate,
                    ExpirationDate = item.ExpirationDate,
                    Notes = item.Notes,
                    PaidFees = item.PaidFees,
                    IsActive = item.IsActive,
                    IssueReason = item.IssueReason,
                    CreatedByUserID = item.CreatedByUserID
                };

                // Populate nested object for brief list item
                if (item.ApplicationID != default)
                {
                    briefItem.ApplicationDetails = await Application.getApplicationBriefOutputByID((int)item.ApplicationID);
                }

                // Populate nested object for brief list item
                if (item.DriverID != default)
                {
                    briefItem.DriverDetails = await Driver.getDriverBriefOutputByID((int)item.DriverID);
                }

                // Populate nested object for brief list item
                if (item.CreatedByUserID != default)
                {
                    briefItem.CreatedByUserDetails = await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID);
                }

                briefList.Add(briefItem);
            }
            
            return briefList;
        }
        public static async Task<List<LicenseBriefOutputDTO>> getAllBrief()
        {
            List<LicenseFullOutputDTO> fullList = await LicenseDAL.getAll();
            List<LicenseBriefOutputDTO> briefList = new List<LicenseBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new LicenseBriefOutputDTO
                {
                ApplicationDetails = item.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ApplicationID) : null,
                DriverDetails = item.DriverID != default ? await Driver.getDriverBriefOutputByID((int)item.DriverID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    LicenseID = item.LicenseID,
                    ApplicationID = item.ApplicationID,
                    DriverID = item.DriverID,
                    LicenseClass = item.LicenseClass,
                    IssueDate = item.IssueDate,
                    ExpirationDate = item.ExpirationDate,
                    Notes = item.Notes,
                    PaidFees = item.PaidFees,
                    IsActive = item.IsActive,
                    IssueReason = item.IssueReason,
                    CreatedByUserID = item.CreatedByUserID                });
            }
            return briefList;
        }
        public static async Task<List<LicenseFullOutputDTO>> getAllFull()
        {
            List<LicenseFullOutputDTO> fullList = await LicenseDAL.getAll();
            foreach (var item in fullList)
            {
                if (item.ApplicationID != default)
                {
                    item.ApplicationDetails = await Application.getApplicationByID((int)item.ApplicationID);
                }

                if (item.DriverID != default)
                {
                    item.DriverDetails = await Driver.getDriverByID((int)item.DriverID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }
            }
            return fullList;
        }

        public static async Task<LicenseFullOutputDTO> getLicenseByApplicationID(int ApplicationID)
        {
            LicenseFullOutputDTO fullDto = await LicenseDAL.getLicenseByApplicationID(ApplicationID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.ApplicationID != default)
            {
                fullDto.ApplicationDetails = await Application.getApplicationByID((int)fullDto.ApplicationID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.DriverID != default)
            {
                fullDto.DriverDetails = await Driver.getDriverByID((int)fullDto.DriverID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            return fullDto;
        }

public static async Task<LicenseBriefOutputDTO> getLicenseBriefOutputByApplicationID(int ApplicationID)
{
    // Fetch the full flat record from DAL
    var fullDto = await LicenseDAL.getLicenseByApplicationID(ApplicationID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new LicenseBriefOutputDTO
    {
                ApplicationDetails = fullDto.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)fullDto.ApplicationID) : null,
                DriverDetails = fullDto.DriverID != default ? await Driver.getDriverBriefOutputByID((int)fullDto.DriverID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                    LicenseID = fullDto.LicenseID,
                    ApplicationID = fullDto.ApplicationID,
                    DriverID = fullDto.DriverID,
                    LicenseClass = fullDto.LicenseClass,
                    IssueDate = fullDto.IssueDate,
                    ExpirationDate = fullDto.ExpirationDate,
                    Notes = fullDto.Notes,
                    PaidFees = fullDto.PaidFees,
                    IsActive = fullDto.IsActive,
                    IssueReason = fullDto.IssueReason,
                    CreatedByUserID = fullDto.CreatedByUserID
    };
}        public static Task<bool> isLicenseExistByApplicationID(int ApplicationID)
        {
            return LicenseDAL.isLicenseExistByApplicationID(ApplicationID);
        }
        public static async Task<List<LicenseBriefOutputDTO>> getAllBriefByDriverID(int DriverID)
        {
            List<LicenseFullOutputDTO> fullList = await LicenseDAL.getAllByDriverID(DriverID);
            List<LicenseBriefOutputDTO> briefList = new List<LicenseBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new LicenseBriefOutputDTO
                {
                ApplicationDetails = item.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ApplicationID) : null,
                DriverDetails = item.DriverID != default ? await Driver.getDriverBriefOutputByID((int)item.DriverID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    LicenseID = item.LicenseID,
                    ApplicationID = item.ApplicationID,
                    DriverID = item.DriverID,
                    LicenseClass = item.LicenseClass,
                    IssueDate = item.IssueDate,
                    ExpirationDate = item.ExpirationDate,
                    Notes = item.Notes,
                    PaidFees = item.PaidFees,
                    IsActive = item.IsActive,
                    IssueReason = item.IssueReason,
                    CreatedByUserID = item.CreatedByUserID                });
            }
            return briefList;
        }
        public static async Task<List<LicenseFullOutputDTO>> getAllFullByDriverID(int DriverID)
        {
            List<LicenseFullOutputDTO> fullList = await LicenseDAL.getAllByDriverID(DriverID);
            foreach (var item in fullList)
            {
                if (item.ApplicationID != default)
                {
                    item.ApplicationDetails = await Application.getApplicationByID((int)item.ApplicationID);
                }

                if (item.DriverID != default)
                {
                    item.DriverDetails = await Driver.getDriverByID((int)item.DriverID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }
            }
            return fullList;
        }
        public static async Task<List<LicenseBriefOutputDTO>> getAllBriefByCreatedByUserID(int CreatedByUserID)
        {
            List<LicenseFullOutputDTO> fullList = await LicenseDAL.getAllByCreatedByUserID(CreatedByUserID);
            List<LicenseBriefOutputDTO> briefList = new List<LicenseBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new LicenseBriefOutputDTO
                {
                ApplicationDetails = item.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ApplicationID) : null,
                DriverDetails = item.DriverID != default ? await Driver.getDriverBriefOutputByID((int)item.DriverID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    LicenseID = item.LicenseID,
                    ApplicationID = item.ApplicationID,
                    DriverID = item.DriverID,
                    LicenseClass = item.LicenseClass,
                    IssueDate = item.IssueDate,
                    ExpirationDate = item.ExpirationDate,
                    Notes = item.Notes,
                    PaidFees = item.PaidFees,
                    IsActive = item.IsActive,
                    IssueReason = item.IssueReason,
                    CreatedByUserID = item.CreatedByUserID                });
            }
            return briefList;
        }
        public static async Task<List<LicenseFullOutputDTO>> getAllFullByCreatedByUserID(int CreatedByUserID)
        {
            List<LicenseFullOutputDTO> fullList = await LicenseDAL.getAllByCreatedByUserID(CreatedByUserID);
            foreach (var item in fullList)
            {
                if (item.ApplicationID != default)
                {
                    item.ApplicationDetails = await Application.getApplicationByID((int)item.ApplicationID);
                }

                if (item.DriverID != default)
                {
                    item.DriverDetails = await Driver.getDriverByID((int)item.DriverID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }
            }
            return fullList;
        }

    }
}
