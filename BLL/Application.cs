using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class Application
    {

        /// <summary>
        /// Adds a new record into the system, automatically embedding internal business rules and states.
        /// </summary>
        public static async Task<int> addApplication(ApplicationBriefInputDTO dto)
        {
            var fullDto = new ApplicationFullOutputDTO
            {
                ApplicationID = dto.ApplicationID,
                ApplicantPersonID = dto.ApplicantPersonID,
                ApplicationDate = dto.ApplicationDate,
                ApplicationTypeID = dto.ApplicationTypeID,
                ApplicationStatus = dto.ApplicationStatus,
                LastStatusDate = dto.LastStatusDate,
                PaidFees = dto.PaidFees,
                CreatedByUserID = dto.CreatedByUserID,
                IsDeleted = dto.IsDeleted
            };

            return await ApplicationDAL.addApplication(fullDto);
        }

        public static async Task<ApplicationFullOutputDTO> getApplicationByID(int ApplicationID)
        {
            ApplicationFullOutputDTO fullDto = await ApplicationDAL.getApplicationByID(ApplicationID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.ApplicantPersonID != default)
            {
                fullDto.ApplicantPersonDetails = await Person.getPersonByID((int)fullDto.ApplicantPersonID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.ApplicationTypeID != default)
            {
                fullDto.ApplicationTypeDetails = await ApplicationType.getApplicationTypeByID((int)fullDto.ApplicationTypeID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            return fullDto;
        }

public static async Task<ApplicationBriefOutputDTO> getApplicationBriefOutputByID(int ApplicationID)
{
    // Fetch the full flat record from DAL
    var fullDto = await ApplicationDAL.getApplicationByID(ApplicationID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new ApplicationBriefOutputDTO
    {
                ApplicantPersonDetails = fullDto.ApplicantPersonID != default ? await Person.getPersonBriefOutputByID((int)fullDto.ApplicantPersonID) : null,
                ApplicationTypeDetails = fullDto.ApplicationTypeID != default ? await ApplicationType.getApplicationTypeBriefOutputByID((int)fullDto.ApplicationTypeID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                    ApplicationID = fullDto.ApplicationID,
                    ApplicantPersonID = fullDto.ApplicantPersonID,
                    ApplicationDate = fullDto.ApplicationDate,
                    ApplicationTypeID = fullDto.ApplicationTypeID,
                    ApplicationStatus = fullDto.ApplicationStatus,
                    LastStatusDate = fullDto.LastStatusDate,
                    PaidFees = fullDto.PaidFees,
                    CreatedByUserID = fullDto.CreatedByUserID,
                    IsDeleted = fullDto.IsDeleted
    };
}
        /// <summary>
        /// Safe update for regular users using BriefInputDTO. Preserves system-controlled fields.
        /// </summary>
        public static async Task<bool> updateApplication(ApplicationBriefInputDTO dto)
        {
            // 1. Fetch the existing full record to preserve internal data (Roles, Active status, Balance, etc.)
            var existingRecord = await getApplicationByID(dto.ApplicationID);
            if (existingRecord == null) return false;

            // 2. Safely overwrite only the client-editable properties                existingRecord.ApplicationID = dto.ApplicationID;
                existingRecord.ApplicantPersonID = dto.ApplicantPersonID;
                existingRecord.ApplicationDate = dto.ApplicationDate;
                existingRecord.ApplicationTypeID = dto.ApplicationTypeID;
                existingRecord.ApplicationStatus = dto.ApplicationStatus;
                existingRecord.LastStatusDate = dto.LastStatusDate;
                existingRecord.PaidFees = dto.PaidFees;
                existingRecord.CreatedByUserID = dto.CreatedByUserID;
                existingRecord.IsDeleted = dto.IsDeleted;
            
            // 3. Forward the fully preserved record to the DAL layer
            return await ApplicationDAL.updateApplication(existingRecord);
        }
        public static async Task<bool> deleteApplication(int ApplicationID)
        {
            if (await isApplicationExistByID(ApplicationID))
            {
                return await ApplicationDAL.deleteApplication(ApplicationID);
            }
            return false;
        }
        public static Task<bool> isApplicationExistByID(int ApplicationID)
        {
            return ApplicationDAL.isApplicationExistByID(ApplicationID);
        }

        public static async Task<List<ApplicationBriefOutputDTO>> Paging(int rowsPerPage, int pageNumber, string sortColumn, string direction)
        {
            List<ApplicationFullOutputDTO> fullList = await ApplicationDAL.PagingDAL(rowsPerPage, pageNumber, sortColumn, direction);
            List<ApplicationBriefOutputDTO> briefList = new List<ApplicationBriefOutputDTO>();
            
            foreach (ApplicationFullOutputDTO item in fullList)
            {
                var briefItem = new ApplicationBriefOutputDTO
                {                    ApplicationID = item.ApplicationID,
                    ApplicantPersonID = item.ApplicantPersonID,
                    ApplicationDate = item.ApplicationDate,
                    ApplicationTypeID = item.ApplicationTypeID,
                    ApplicationStatus = item.ApplicationStatus,
                    LastStatusDate = item.LastStatusDate,
                    PaidFees = item.PaidFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsDeleted = item.IsDeleted
                };

                // Populate nested object for brief list item
                if (item.ApplicantPersonID != default)
                {
                    briefItem.ApplicantPersonDetails = await Person.getPersonBriefOutputByID((int)item.ApplicantPersonID);
                }

                // Populate nested object for brief list item
                if (item.ApplicationTypeID != default)
                {
                    briefItem.ApplicationTypeDetails = await ApplicationType.getApplicationTypeBriefOutputByID((int)item.ApplicationTypeID);
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
        public static async Task<List<ApplicationBriefOutputDTO>> getAllBrief()
        {
            List<ApplicationFullOutputDTO> fullList = await ApplicationDAL.getAll();
            List<ApplicationBriefOutputDTO> briefList = new List<ApplicationBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new ApplicationBriefOutputDTO
                {
                ApplicantPersonDetails = item.ApplicantPersonID != default ? await Person.getPersonBriefOutputByID((int)item.ApplicantPersonID) : null,
                ApplicationTypeDetails = item.ApplicationTypeID != default ? await ApplicationType.getApplicationTypeBriefOutputByID((int)item.ApplicationTypeID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    ApplicationID = item.ApplicationID,
                    ApplicantPersonID = item.ApplicantPersonID,
                    ApplicationDate = item.ApplicationDate,
                    ApplicationTypeID = item.ApplicationTypeID,
                    ApplicationStatus = item.ApplicationStatus,
                    LastStatusDate = item.LastStatusDate,
                    PaidFees = item.PaidFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsDeleted = item.IsDeleted                });
            }
            return briefList;
        }
        public static async Task<List<ApplicationFullOutputDTO>> getAllFull()
        {
            List<ApplicationFullOutputDTO> fullList = await ApplicationDAL.getAll();
            foreach (var item in fullList)
            {
                if (item.ApplicantPersonID != default)
                {
                    item.ApplicantPersonDetails = await Person.getPersonByID((int)item.ApplicantPersonID);
                }

                if (item.ApplicationTypeID != default)
                {
                    item.ApplicationTypeDetails = await ApplicationType.getApplicationTypeByID((int)item.ApplicationTypeID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }
            }
            return fullList;
        }
        public static async Task<List<ApplicationBriefOutputDTO>> getAllBriefByApplicantPersonID(int ApplicantPersonID)
        {
            List<ApplicationFullOutputDTO> fullList = await ApplicationDAL.getAllByApplicantPersonID(ApplicantPersonID);
            List<ApplicationBriefOutputDTO> briefList = new List<ApplicationBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new ApplicationBriefOutputDTO
                {
                ApplicantPersonDetails = item.ApplicantPersonID != default ? await Person.getPersonBriefOutputByID((int)item.ApplicantPersonID) : null,
                ApplicationTypeDetails = item.ApplicationTypeID != default ? await ApplicationType.getApplicationTypeBriefOutputByID((int)item.ApplicationTypeID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    ApplicationID = item.ApplicationID,
                    ApplicantPersonID = item.ApplicantPersonID,
                    ApplicationDate = item.ApplicationDate,
                    ApplicationTypeID = item.ApplicationTypeID,
                    ApplicationStatus = item.ApplicationStatus,
                    LastStatusDate = item.LastStatusDate,
                    PaidFees = item.PaidFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsDeleted = item.IsDeleted                });
            }
            return briefList;
        }
        public static async Task<List<ApplicationFullOutputDTO>> getAllFullByApplicantPersonID(int ApplicantPersonID)
        {
            List<ApplicationFullOutputDTO> fullList = await ApplicationDAL.getAllByApplicantPersonID(ApplicantPersonID);
            foreach (var item in fullList)
            {
                if (item.ApplicantPersonID != default)
                {
                    item.ApplicantPersonDetails = await Person.getPersonByID((int)item.ApplicantPersonID);
                }

                if (item.ApplicationTypeID != default)
                {
                    item.ApplicationTypeDetails = await ApplicationType.getApplicationTypeByID((int)item.ApplicationTypeID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }
            }
            return fullList;
        }
        public static async Task<List<ApplicationBriefOutputDTO>> getAllBriefByApplicationTypeID(int ApplicationTypeID)
        {
            List<ApplicationFullOutputDTO> fullList = await ApplicationDAL.getAllByApplicationTypeID(ApplicationTypeID);
            List<ApplicationBriefOutputDTO> briefList = new List<ApplicationBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new ApplicationBriefOutputDTO
                {
                ApplicantPersonDetails = item.ApplicantPersonID != default ? await Person.getPersonBriefOutputByID((int)item.ApplicantPersonID) : null,
                ApplicationTypeDetails = item.ApplicationTypeID != default ? await ApplicationType.getApplicationTypeBriefOutputByID((int)item.ApplicationTypeID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    ApplicationID = item.ApplicationID,
                    ApplicantPersonID = item.ApplicantPersonID,
                    ApplicationDate = item.ApplicationDate,
                    ApplicationTypeID = item.ApplicationTypeID,
                    ApplicationStatus = item.ApplicationStatus,
                    LastStatusDate = item.LastStatusDate,
                    PaidFees = item.PaidFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsDeleted = item.IsDeleted                });
            }
            return briefList;
        }
        public static async Task<List<ApplicationFullOutputDTO>> getAllFullByApplicationTypeID(int ApplicationTypeID)
        {
            List<ApplicationFullOutputDTO> fullList = await ApplicationDAL.getAllByApplicationTypeID(ApplicationTypeID);
            foreach (var item in fullList)
            {
                if (item.ApplicantPersonID != default)
                {
                    item.ApplicantPersonDetails = await Person.getPersonByID((int)item.ApplicantPersonID);
                }

                if (item.ApplicationTypeID != default)
                {
                    item.ApplicationTypeDetails = await ApplicationType.getApplicationTypeByID((int)item.ApplicationTypeID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }
            }
            return fullList;
        }
        public static async Task<List<ApplicationBriefOutputDTO>> getAllBriefByCreatedByUserID(int CreatedByUserID)
        {
            List<ApplicationFullOutputDTO> fullList = await ApplicationDAL.getAllByCreatedByUserID(CreatedByUserID);
            List<ApplicationBriefOutputDTO> briefList = new List<ApplicationBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new ApplicationBriefOutputDTO
                {
                ApplicantPersonDetails = item.ApplicantPersonID != default ? await Person.getPersonBriefOutputByID((int)item.ApplicantPersonID) : null,
                ApplicationTypeDetails = item.ApplicationTypeID != default ? await ApplicationType.getApplicationTypeBriefOutputByID((int)item.ApplicationTypeID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    ApplicationID = item.ApplicationID,
                    ApplicantPersonID = item.ApplicantPersonID,
                    ApplicationDate = item.ApplicationDate,
                    ApplicationTypeID = item.ApplicationTypeID,
                    ApplicationStatus = item.ApplicationStatus,
                    LastStatusDate = item.LastStatusDate,
                    PaidFees = item.PaidFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsDeleted = item.IsDeleted                });
            }
            return briefList;
        }
        public static async Task<List<ApplicationFullOutputDTO>> getAllFullByCreatedByUserID(int CreatedByUserID)
        {
            List<ApplicationFullOutputDTO> fullList = await ApplicationDAL.getAllByCreatedByUserID(CreatedByUserID);
            foreach (var item in fullList)
            {
                if (item.ApplicantPersonID != default)
                {
                    item.ApplicantPersonDetails = await Person.getPersonByID((int)item.ApplicantPersonID);
                }

                if (item.ApplicationTypeID != default)
                {
                    item.ApplicationTypeDetails = await ApplicationType.getApplicationTypeByID((int)item.ApplicationTypeID);
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
