using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class InternationalLicense
    {

        /// <summary>
        /// Administrative add: Accepts FullInputDTO to allow setting all fields (Role, Status, etc.).
        /// </summary>
        public static async Task<int> addAdminInternationalLicense(InternationalLicenseFullInputDTO dto)
        {
            var fullDto = new InternationalLicenseFullOutputDTO
            {
                InternationalLicenseID = dto.InternationalLicenseID,
                ApplicationID = dto.ApplicationID,
                DriverID = dto.DriverID,
                IssuedUsingLocalLicenseID = dto.IssuedUsingLocalLicenseID,
                IssueDate = dto.IssueDate,
                ExpirationDate = dto.ExpirationDate,
                IsActive = dto.IsActive,
                CreatedByUserID = dto.CreatedByUserID
            };

            return await InternationalLicenseDAL.addInternationalLicense(fullDto);
        }

        /// <summary>
        /// Administrative full update using FullInputDTO. Allows modification of all columns.
        /// </summary>
        public static async Task<bool> updateInternationalLicense(InternationalLicenseFullInputDTO dto)
        {
            var fullDto = new InternationalLicenseFullOutputDTO
            {                InternationalLicenseID = dto.InternationalLicenseID,
                ApplicationID = dto.ApplicationID,
                DriverID = dto.DriverID,
                IssuedUsingLocalLicenseID = dto.IssuedUsingLocalLicenseID,
                IssueDate = dto.IssueDate,
                ExpirationDate = dto.ExpirationDate,
                IsActive = dto.IsActive,
                CreatedByUserID = dto.CreatedByUserID
            };

            return await InternationalLicenseDAL.updateInternationalLicense(fullDto);
        }
        /// <summary>
        /// Adds a new record into the system, automatically embedding internal business rules and states.
        /// </summary>
        public static async Task<int> addInternationalLicense(InternationalLicenseBriefInputDTO dto)
        {
            var fullDto = new InternationalLicenseFullOutputDTO
            {
                InternationalLicenseID = dto.InternationalLicenseID,
                ApplicationID = dto.ApplicationID,
                DriverID = dto.DriverID,
                IssuedUsingLocalLicenseID = dto.IssuedUsingLocalLicenseID,
                IssueDate = dto.IssueDate,
                ExpirationDate = dto.ExpirationDate,
                CreatedByUserID = dto.CreatedByUserID
            };
            fullDto.IsActive = true;

            return await InternationalLicenseDAL.addInternationalLicense(fullDto);
        }

        public static async Task<InternationalLicenseFullOutputDTO> getInternationalLicenseByID(int InternationalLicenseID)
        {
            InternationalLicenseFullOutputDTO fullDto = await InternationalLicenseDAL.getInternationalLicenseByID(InternationalLicenseID);
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
            if (fullDto.IssuedUsingLocalLicenseID != default)
            {
                fullDto.IssuedUsingLocalLicenseDetails = await License.getLicenseByID((int)fullDto.IssuedUsingLocalLicenseID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            return fullDto;
        }

public static async Task<InternationalLicenseBriefOutputDTO> getInternationalLicenseBriefOutputByID(int InternationalLicenseID)
{
    // Fetch the full flat record from DAL
    var fullDto = await InternationalLicenseDAL.getInternationalLicenseByID(InternationalLicenseID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new InternationalLicenseBriefOutputDTO
    {
                ApplicationDetails = fullDto.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)fullDto.ApplicationID) : null,
                DriverDetails = fullDto.DriverID != default ? await Driver.getDriverBriefOutputByID((int)fullDto.DriverID) : null,
                IssuedUsingLocalLicenseDetails = fullDto.IssuedUsingLocalLicenseID != default ? await License.getLicenseBriefOutputByID((int)fullDto.IssuedUsingLocalLicenseID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                    InternationalLicenseID = fullDto.InternationalLicenseID,
                    ApplicationID = fullDto.ApplicationID,
                    DriverID = fullDto.DriverID,
                    IssuedUsingLocalLicenseID = fullDto.IssuedUsingLocalLicenseID,
                    IssueDate = fullDto.IssueDate,
                    ExpirationDate = fullDto.ExpirationDate,
                    IsActive = fullDto.IsActive,
                    CreatedByUserID = fullDto.CreatedByUserID
    };
}
        /// <summary>
        /// Safe update for regular users using BriefInputDTO. Preserves system-controlled fields.
        /// </summary>
        public static async Task<bool> updateInternationalLicense(InternationalLicenseBriefInputDTO dto)
        {
            // 1. Fetch the existing full record to preserve internal data (Roles, Active status, Balance, etc.)
            var existingRecord = await getInternationalLicenseByID(dto.InternationalLicenseID);
            if (existingRecord == null) return false;

            // 2. Safely overwrite only the client-editable properties                existingRecord.InternationalLicenseID = dto.InternationalLicenseID;
                existingRecord.ApplicationID = dto.ApplicationID;
                existingRecord.DriverID = dto.DriverID;
                existingRecord.IssuedUsingLocalLicenseID = dto.IssuedUsingLocalLicenseID;
                existingRecord.IssueDate = dto.IssueDate;
                existingRecord.ExpirationDate = dto.ExpirationDate;
                existingRecord.CreatedByUserID = dto.CreatedByUserID;
            
            // 3. Forward the fully preserved record to the DAL layer
            return await InternationalLicenseDAL.updateInternationalLicense(existingRecord);
        }
        public static async Task<bool> deleteInternationalLicense(int InternationalLicenseID)
        {
            if (await isInternationalLicenseExistByID(InternationalLicenseID))
            {
                return await InternationalLicenseDAL.deleteInternationalLicense(InternationalLicenseID);
            }
            return false;
        }
        public static Task<bool> isInternationalLicenseExistByID(int InternationalLicenseID)
        {
            return InternationalLicenseDAL.isInternationalLicenseExistByID(InternationalLicenseID);
        }

        public static async Task<List<InternationalLicenseBriefOutputDTO>> Paging(int rowsPerPage, int pageNumber, string sortColumn, string direction)
        {
            List<InternationalLicenseFullOutputDTO> fullList = await InternationalLicenseDAL.PagingDAL(rowsPerPage, pageNumber, sortColumn, direction);
            List<InternationalLicenseBriefOutputDTO> briefList = new List<InternationalLicenseBriefOutputDTO>();
            
            foreach (InternationalLicenseFullOutputDTO item in fullList)
            {
                var briefItem = new InternationalLicenseBriefOutputDTO
                {                    InternationalLicenseID = item.InternationalLicenseID,
                    ApplicationID = item.ApplicationID,
                    DriverID = item.DriverID,
                    IssuedUsingLocalLicenseID = item.IssuedUsingLocalLicenseID,
                    IssueDate = item.IssueDate,
                    ExpirationDate = item.ExpirationDate,
                    IsActive = item.IsActive,
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
                if (item.IssuedUsingLocalLicenseID != default)
                {
                    briefItem.IssuedUsingLocalLicenseDetails = await License.getLicenseBriefOutputByID((int)item.IssuedUsingLocalLicenseID);
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
        public static async Task<List<InternationalLicenseBriefOutputDTO>> getAllBrief()
        {
            List<InternationalLicenseFullOutputDTO> fullList = await InternationalLicenseDAL.getAll();
            List<InternationalLicenseBriefOutputDTO> briefList = new List<InternationalLicenseBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new InternationalLicenseBriefOutputDTO
                {
                ApplicationDetails = item.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ApplicationID) : null,
                DriverDetails = item.DriverID != default ? await Driver.getDriverBriefOutputByID((int)item.DriverID) : null,
                IssuedUsingLocalLicenseDetails = item.IssuedUsingLocalLicenseID != default ? await License.getLicenseBriefOutputByID((int)item.IssuedUsingLocalLicenseID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    InternationalLicenseID = item.InternationalLicenseID,
                    ApplicationID = item.ApplicationID,
                    DriverID = item.DriverID,
                    IssuedUsingLocalLicenseID = item.IssuedUsingLocalLicenseID,
                    IssueDate = item.IssueDate,
                    ExpirationDate = item.ExpirationDate,
                    IsActive = item.IsActive,
                    CreatedByUserID = item.CreatedByUserID                });
            }
            return briefList;
        }
        public static async Task<List<InternationalLicenseFullOutputDTO>> getAllFull()
        {
            List<InternationalLicenseFullOutputDTO> fullList = await InternationalLicenseDAL.getAll();
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

                if (item.IssuedUsingLocalLicenseID != default)
                {
                    item.IssuedUsingLocalLicenseDetails = await License.getLicenseByID((int)item.IssuedUsingLocalLicenseID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }
            }
            return fullList;
        }

        public static async Task<InternationalLicenseFullOutputDTO> getInternationalLicenseByApplicationID(int ApplicationID)
        {
            InternationalLicenseFullOutputDTO fullDto = await InternationalLicenseDAL.getInternationalLicenseByApplicationID(ApplicationID);
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
            if (fullDto.IssuedUsingLocalLicenseID != default)
            {
                fullDto.IssuedUsingLocalLicenseDetails = await License.getLicenseByID((int)fullDto.IssuedUsingLocalLicenseID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            return fullDto;
        }

public static async Task<InternationalLicenseBriefOutputDTO> getInternationalLicenseBriefOutputByApplicationID(int ApplicationID)
{
    // Fetch the full flat record from DAL
    var fullDto = await InternationalLicenseDAL.getInternationalLicenseByApplicationID(ApplicationID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new InternationalLicenseBriefOutputDTO
    {
                ApplicationDetails = fullDto.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)fullDto.ApplicationID) : null,
                DriverDetails = fullDto.DriverID != default ? await Driver.getDriverBriefOutputByID((int)fullDto.DriverID) : null,
                IssuedUsingLocalLicenseDetails = fullDto.IssuedUsingLocalLicenseID != default ? await License.getLicenseBriefOutputByID((int)fullDto.IssuedUsingLocalLicenseID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                    InternationalLicenseID = fullDto.InternationalLicenseID,
                    ApplicationID = fullDto.ApplicationID,
                    DriverID = fullDto.DriverID,
                    IssuedUsingLocalLicenseID = fullDto.IssuedUsingLocalLicenseID,
                    IssueDate = fullDto.IssueDate,
                    ExpirationDate = fullDto.ExpirationDate,
                    IsActive = fullDto.IsActive,
                    CreatedByUserID = fullDto.CreatedByUserID
    };
}
        public static async Task<InternationalLicenseFullOutputDTO> getInternationalLicenseByIssuedUsingLocalLicenseID(int IssuedUsingLocalLicenseID)
        {
            InternationalLicenseFullOutputDTO fullDto = await InternationalLicenseDAL.getInternationalLicenseByIssuedUsingLocalLicenseID(IssuedUsingLocalLicenseID);
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
            if (fullDto.IssuedUsingLocalLicenseID != default)
            {
                fullDto.IssuedUsingLocalLicenseDetails = await License.getLicenseByID((int)fullDto.IssuedUsingLocalLicenseID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            return fullDto;
        }

public static async Task<InternationalLicenseBriefOutputDTO> getInternationalLicenseBriefOutputByIssuedUsingLocalLicenseID(int IssuedUsingLocalLicenseID)
{
    // Fetch the full flat record from DAL
    var fullDto = await InternationalLicenseDAL.getInternationalLicenseByIssuedUsingLocalLicenseID(IssuedUsingLocalLicenseID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new InternationalLicenseBriefOutputDTO
    {
                ApplicationDetails = fullDto.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)fullDto.ApplicationID) : null,
                DriverDetails = fullDto.DriverID != default ? await Driver.getDriverBriefOutputByID((int)fullDto.DriverID) : null,
                IssuedUsingLocalLicenseDetails = fullDto.IssuedUsingLocalLicenseID != default ? await License.getLicenseBriefOutputByID((int)fullDto.IssuedUsingLocalLicenseID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                    InternationalLicenseID = fullDto.InternationalLicenseID,
                    ApplicationID = fullDto.ApplicationID,
                    DriverID = fullDto.DriverID,
                    IssuedUsingLocalLicenseID = fullDto.IssuedUsingLocalLicenseID,
                    IssueDate = fullDto.IssueDate,
                    ExpirationDate = fullDto.ExpirationDate,
                    IsActive = fullDto.IsActive,
                    CreatedByUserID = fullDto.CreatedByUserID
    };
}        public static Task<bool> isInternationalLicenseExistByApplicationID(int ApplicationID)
        {
            return InternationalLicenseDAL.isInternationalLicenseExistByApplicationID(ApplicationID);
        }
        public static Task<bool> isInternationalLicenseExistByIssuedUsingLocalLicenseID(int IssuedUsingLocalLicenseID)
        {
            return InternationalLicenseDAL.isInternationalLicenseExistByIssuedUsingLocalLicenseID(IssuedUsingLocalLicenseID);
        }
        public static async Task<List<InternationalLicenseBriefOutputDTO>> getAllBriefByDriverID(int DriverID)
        {
            List<InternationalLicenseFullOutputDTO> fullList = await InternationalLicenseDAL.getAllByDriverID(DriverID);
            List<InternationalLicenseBriefOutputDTO> briefList = new List<InternationalLicenseBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new InternationalLicenseBriefOutputDTO
                {
                ApplicationDetails = item.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ApplicationID) : null,
                DriverDetails = item.DriverID != default ? await Driver.getDriverBriefOutputByID((int)item.DriverID) : null,
                IssuedUsingLocalLicenseDetails = item.IssuedUsingLocalLicenseID != default ? await License.getLicenseBriefOutputByID((int)item.IssuedUsingLocalLicenseID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    InternationalLicenseID = item.InternationalLicenseID,
                    ApplicationID = item.ApplicationID,
                    DriverID = item.DriverID,
                    IssuedUsingLocalLicenseID = item.IssuedUsingLocalLicenseID,
                    IssueDate = item.IssueDate,
                    ExpirationDate = item.ExpirationDate,
                    IsActive = item.IsActive,
                    CreatedByUserID = item.CreatedByUserID                });
            }
            return briefList;
        }
        public static async Task<List<InternationalLicenseFullOutputDTO>> getAllFullByDriverID(int DriverID)
        {
            List<InternationalLicenseFullOutputDTO> fullList = await InternationalLicenseDAL.getAllByDriverID(DriverID);
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

                if (item.IssuedUsingLocalLicenseID != default)
                {
                    item.IssuedUsingLocalLicenseDetails = await License.getLicenseByID((int)item.IssuedUsingLocalLicenseID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }
            }
            return fullList;
        }
        public static async Task<List<InternationalLicenseBriefOutputDTO>> getAllBriefByCreatedByUserID(int CreatedByUserID)
        {
            List<InternationalLicenseFullOutputDTO> fullList = await InternationalLicenseDAL.getAllByCreatedByUserID(CreatedByUserID);
            List<InternationalLicenseBriefOutputDTO> briefList = new List<InternationalLicenseBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new InternationalLicenseBriefOutputDTO
                {
                ApplicationDetails = item.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ApplicationID) : null,
                DriverDetails = item.DriverID != default ? await Driver.getDriverBriefOutputByID((int)item.DriverID) : null,
                IssuedUsingLocalLicenseDetails = item.IssuedUsingLocalLicenseID != default ? await License.getLicenseBriefOutputByID((int)item.IssuedUsingLocalLicenseID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    InternationalLicenseID = item.InternationalLicenseID,
                    ApplicationID = item.ApplicationID,
                    DriverID = item.DriverID,
                    IssuedUsingLocalLicenseID = item.IssuedUsingLocalLicenseID,
                    IssueDate = item.IssueDate,
                    ExpirationDate = item.ExpirationDate,
                    IsActive = item.IsActive,
                    CreatedByUserID = item.CreatedByUserID                });
            }
            return briefList;
        }
        public static async Task<List<InternationalLicenseFullOutputDTO>> getAllFullByCreatedByUserID(int CreatedByUserID)
        {
            List<InternationalLicenseFullOutputDTO> fullList = await InternationalLicenseDAL.getAllByCreatedByUserID(CreatedByUserID);
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

                if (item.IssuedUsingLocalLicenseID != default)
                {
                    item.IssuedUsingLocalLicenseDetails = await License.getLicenseByID((int)item.IssuedUsingLocalLicenseID);
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
