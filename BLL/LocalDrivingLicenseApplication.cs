using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class LocalDrivingLicenseApplication
    {

        /// <summary>
        /// Adds a new record into the system, automatically embedding internal business rules and states.
        /// </summary>
        public static async Task<int> addLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationBriefInputDTO dto)
        {
            var fullDto = new LocalDrivingLicenseApplicationFullOutputDTO
            {
                LocalDrivingLicenseApplicationID = dto.LocalDrivingLicenseApplicationID,
                ApplicationID = dto.ApplicationID,
                LicenseClassID = dto.LicenseClassID
            };

            return await LocalDrivingLicenseApplicationDAL.addLocalDrivingLicenseApplication(fullDto);
        }

        public static async Task<LocalDrivingLicenseApplicationFullOutputDTO> getLocalDrivingLicenseApplicationByID(int LocalDrivingLicenseApplicationID)
        {
            LocalDrivingLicenseApplicationFullOutputDTO fullDto = await LocalDrivingLicenseApplicationDAL.getLocalDrivingLicenseApplicationByID(LocalDrivingLicenseApplicationID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.ApplicationID != default)
            {
                fullDto.ApplicationDetails = await Application.getApplicationByID((int)fullDto.ApplicationID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.LicenseClassID != default)
            {
                fullDto.LicenseClassDetails = await License.getLicenseByID((int)fullDto.LicenseClassID);
            }

            return fullDto;
        }

public static async Task<LocalDrivingLicenseApplicationBriefOutputDTO> getLocalDrivingLicenseApplicationBriefOutputByID(int LocalDrivingLicenseApplicationID)
{
    // Fetch the full flat record from DAL
    var fullDto = await LocalDrivingLicenseApplicationDAL.getLocalDrivingLicenseApplicationByID(LocalDrivingLicenseApplicationID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new LocalDrivingLicenseApplicationBriefOutputDTO
    {
                ApplicationDetails = fullDto.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)fullDto.ApplicationID) : null,
                LicenseClassDetails = fullDto.LicenseClassID != default ? await License.getLicenseBriefOutputByID((int)fullDto.LicenseClassID) : null,
                    LocalDrivingLicenseApplicationID = fullDto.LocalDrivingLicenseApplicationID,
                    ApplicationID = fullDto.ApplicationID,
                    LicenseClassID = fullDto.LicenseClassID
    };
}
        /// <summary>
        /// Safe update for regular users using BriefInputDTO. Preserves system-controlled fields.
        /// </summary>
        public static async Task<bool> updateLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationBriefInputDTO dto)
        {
            // 1. Fetch the existing full record to preserve internal data (Roles, Active status, Balance, etc.)
            var existingRecord = await getLocalDrivingLicenseApplicationByID(dto.LocalDrivingLicenseApplicationID);
            if (existingRecord == null) return false;

            // 2. Safely overwrite only the client-editable properties                existingRecord.LocalDrivingLicenseApplicationID = dto.LocalDrivingLicenseApplicationID;
                existingRecord.ApplicationID = dto.ApplicationID;
                existingRecord.LicenseClassID = dto.LicenseClassID;
            
            // 3. Forward the fully preserved record to the DAL layer
            return await LocalDrivingLicenseApplicationDAL.updateLocalDrivingLicenseApplication(existingRecord);
        }
        public static async Task<bool> deleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            if (await isLocalDrivingLicenseApplicationExistByID(LocalDrivingLicenseApplicationID))
            {
                return await LocalDrivingLicenseApplicationDAL.deleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
            }
            return false;
        }
        public static Task<bool> isLocalDrivingLicenseApplicationExistByID(int LocalDrivingLicenseApplicationID)
        {
            return LocalDrivingLicenseApplicationDAL.isLocalDrivingLicenseApplicationExistByID(LocalDrivingLicenseApplicationID);
        }

        public static async Task<List<LocalDrivingLicenseApplicationBriefOutputDTO>> Paging(int rowsPerPage, int pageNumber, string sortColumn, string direction)
        {
            List<LocalDrivingLicenseApplicationFullOutputDTO> fullList = await LocalDrivingLicenseApplicationDAL.PagingDAL(rowsPerPage, pageNumber, sortColumn, direction);
            List<LocalDrivingLicenseApplicationBriefOutputDTO> briefList = new List<LocalDrivingLicenseApplicationBriefOutputDTO>();
            
            foreach (LocalDrivingLicenseApplicationFullOutputDTO item in fullList)
            {
                var briefItem = new LocalDrivingLicenseApplicationBriefOutputDTO
                {                    LocalDrivingLicenseApplicationID = item.LocalDrivingLicenseApplicationID,
                    ApplicationID = item.ApplicationID,
                    LicenseClassID = item.LicenseClassID
                };

                // Populate nested object for brief list item
                if (item.ApplicationID != default)
                {
                    briefItem.ApplicationDetails = await Application.getApplicationBriefOutputByID((int)item.ApplicationID);
                }

                // Populate nested object for brief list item
                if (item.LicenseClassID != default)
                {
                    briefItem.LicenseClassDetails = await License.getLicenseBriefOutputByID((int)item.LicenseClassID);
                }

                briefList.Add(briefItem);
            }
            
            return briefList;
        }
        public static async Task<List<LocalDrivingLicenseApplicationBriefOutputDTO>> getAllBrief()
        {
            List<LocalDrivingLicenseApplicationFullOutputDTO> fullList = await LocalDrivingLicenseApplicationDAL.getAll();
            List<LocalDrivingLicenseApplicationBriefOutputDTO> briefList = new List<LocalDrivingLicenseApplicationBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new LocalDrivingLicenseApplicationBriefOutputDTO
                {
                ApplicationDetails = item.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ApplicationID) : null,
                LicenseClassDetails = item.LicenseClassID != default ? await License.getLicenseBriefOutputByID((int)item.LicenseClassID) : null,
                    LocalDrivingLicenseApplicationID = item.LocalDrivingLicenseApplicationID,
                    ApplicationID = item.ApplicationID,
                    LicenseClassID = item.LicenseClassID                });
            }
            return briefList;
        }
        public static async Task<List<LocalDrivingLicenseApplicationFullOutputDTO>> getAllFull()
        {
            List<LocalDrivingLicenseApplicationFullOutputDTO> fullList = await LocalDrivingLicenseApplicationDAL.getAll();
            foreach (var item in fullList)
            {
                if (item.ApplicationID != default)
                {
                    item.ApplicationDetails = await Application.getApplicationByID((int)item.ApplicationID);
                }

                if (item.LicenseClassID != default)
                {
                    item.LicenseClassDetails = await License.getLicenseByID((int)item.LicenseClassID);
                }
            }
            return fullList;
        }

        public static async Task<LocalDrivingLicenseApplicationFullOutputDTO> getLocalDrivingLicenseApplicationByApplicationID(int ApplicationID)
        {
            LocalDrivingLicenseApplicationFullOutputDTO fullDto = await LocalDrivingLicenseApplicationDAL.getLocalDrivingLicenseApplicationByApplicationID(ApplicationID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.ApplicationID != default)
            {
                fullDto.ApplicationDetails = await Application.getApplicationByID((int)fullDto.ApplicationID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.LicenseClassID != default)
            {
                fullDto.LicenseClassDetails = await License.getLicenseByID((int)fullDto.LicenseClassID);
            }

            return fullDto;
        }

public static async Task<LocalDrivingLicenseApplicationBriefOutputDTO> getLocalDrivingLicenseApplicationBriefOutputByApplicationID(int ApplicationID)
{
    // Fetch the full flat record from DAL
    var fullDto = await LocalDrivingLicenseApplicationDAL.getLocalDrivingLicenseApplicationByApplicationID(ApplicationID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new LocalDrivingLicenseApplicationBriefOutputDTO
    {
                ApplicationDetails = fullDto.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)fullDto.ApplicationID) : null,
                LicenseClassDetails = fullDto.LicenseClassID != default ? await License.getLicenseBriefOutputByID((int)fullDto.LicenseClassID) : null,
                    LocalDrivingLicenseApplicationID = fullDto.LocalDrivingLicenseApplicationID,
                    ApplicationID = fullDto.ApplicationID,
                    LicenseClassID = fullDto.LicenseClassID
    };
}        public static Task<bool> isLocalDrivingLicenseApplicationExistByApplicationID(int ApplicationID)
        {
            return LocalDrivingLicenseApplicationDAL.isLocalDrivingLicenseApplicationExistByApplicationID(ApplicationID);
        }
        public static async Task<List<LocalDrivingLicenseApplicationBriefOutputDTO>> getAllBriefByLicenseClassID(int LicenseClassID)
        {
            List<LocalDrivingLicenseApplicationFullOutputDTO> fullList = await LocalDrivingLicenseApplicationDAL.getAllByLicenseClassID(LicenseClassID);
            List<LocalDrivingLicenseApplicationBriefOutputDTO> briefList = new List<LocalDrivingLicenseApplicationBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new LocalDrivingLicenseApplicationBriefOutputDTO
                {
                ApplicationDetails = item.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ApplicationID) : null,
                LicenseClassDetails = item.LicenseClassID != default ? await License.getLicenseBriefOutputByID((int)item.LicenseClassID) : null,
                    LocalDrivingLicenseApplicationID = item.LocalDrivingLicenseApplicationID,
                    ApplicationID = item.ApplicationID,
                    LicenseClassID = item.LicenseClassID                });
            }
            return briefList;
        }
        public static async Task<List<LocalDrivingLicenseApplicationFullOutputDTO>> getAllFullByLicenseClassID(int LicenseClassID)
        {
            List<LocalDrivingLicenseApplicationFullOutputDTO> fullList = await LocalDrivingLicenseApplicationDAL.getAllByLicenseClassID(LicenseClassID);
            foreach (var item in fullList)
            {
                if (item.ApplicationID != default)
                {
                    item.ApplicationDetails = await Application.getApplicationByID((int)item.ApplicationID);
                }

                if (item.LicenseClassID != default)
                {
                    item.LicenseClassDetails = await License.getLicenseByID((int)item.LicenseClassID);
                }
            }
            return fullList;
        }

    }
}
