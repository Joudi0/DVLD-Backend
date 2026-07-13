using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class Driver
    {

        /// <summary>
        /// Adds a new record into the system, automatically embedding internal business rules and states.
        /// </summary>
        public static async Task<int> addDriver(DriverBriefInputDTO dto)
        {
            var fullDto = new DriverFullOutputDTO
            {
                DriverID = dto.DriverID,
                PersonID = dto.PersonID,
                CreatedByUserID = dto.CreatedByUserID,
                CreatedDate = dto.CreatedDate
            };

            return await DriverDAL.addDriver(fullDto);
        }

        public static async Task<DriverFullOutputDTO> getDriverByID(int DriverID)
        {
            DriverFullOutputDTO fullDto = await DriverDAL.getDriverByID(DriverID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.PersonID != default)
            {
                fullDto.PersonDetails = await Person.getPersonByID((int)fullDto.PersonID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            return fullDto;
        }

public static async Task<DriverBriefOutputDTO> getDriverBriefOutputByID(int DriverID)
{
    // Fetch the full flat record from DAL
    var fullDto = await DriverDAL.getDriverByID(DriverID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new DriverBriefOutputDTO
    {
                PersonDetails = fullDto.PersonID != default ? await Person.getPersonBriefOutputByID((int)fullDto.PersonID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                    DriverID = fullDto.DriverID,
                    PersonID = fullDto.PersonID,
                    CreatedByUserID = fullDto.CreatedByUserID,
                    CreatedDate = fullDto.CreatedDate
    };
}
        /// <summary>
        /// Safe update for regular users using BriefInputDTO. Preserves system-controlled fields.
        /// </summary>
        public static async Task<bool> updateDriver(DriverBriefInputDTO dto)
        {
            // 1. Fetch the existing full record to preserve internal data (Roles, Active status, Balance, etc.)
            var existingRecord = await getDriverByID(dto.DriverID);
            if (existingRecord == null) return false;

            // 2. Safely overwrite only the client-editable properties                existingRecord.DriverID = dto.DriverID;
                existingRecord.PersonID = dto.PersonID;
                existingRecord.CreatedByUserID = dto.CreatedByUserID;
                existingRecord.CreatedDate = dto.CreatedDate;
            
            // 3. Forward the fully preserved record to the DAL layer
            return await DriverDAL.updateDriver(existingRecord);
        }
        public static async Task<bool> deleteDriver(int DriverID)
        {
            if (await isDriverExistByID(DriverID))
            {
                return await DriverDAL.deleteDriver(DriverID);
            }
            return false;
        }
        public static Task<bool> isDriverExistByID(int DriverID)
        {
            return DriverDAL.isDriverExistByID(DriverID);
        }

        public static async Task<List<DriverBriefOutputDTO>> Paging(int rowsPerPage, int pageNumber, string sortColumn, string direction)
        {
            List<DriverFullOutputDTO> fullList = await DriverDAL.PagingDAL(rowsPerPage, pageNumber, sortColumn, direction);
            List<DriverBriefOutputDTO> briefList = new List<DriverBriefOutputDTO>();
            
            foreach (DriverFullOutputDTO item in fullList)
            {
                var briefItem = new DriverBriefOutputDTO
                {                    DriverID = item.DriverID,
                    PersonID = item.PersonID,
                    CreatedByUserID = item.CreatedByUserID,
                    CreatedDate = item.CreatedDate
                };

                // Populate nested object for brief list item
                if (item.PersonID != default)
                {
                    briefItem.PersonDetails = await Person.getPersonBriefOutputByID((int)item.PersonID);
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
        public static async Task<List<DriverBriefOutputDTO>> getAllBrief()
        {
            List<DriverFullOutputDTO> fullList = await DriverDAL.getAll();
            List<DriverBriefOutputDTO> briefList = new List<DriverBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new DriverBriefOutputDTO
                {
                PersonDetails = item.PersonID != default ? await Person.getPersonBriefOutputByID((int)item.PersonID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    DriverID = item.DriverID,
                    PersonID = item.PersonID,
                    CreatedByUserID = item.CreatedByUserID,
                    CreatedDate = item.CreatedDate                });
            }
            return briefList;
        }
        public static async Task<List<DriverFullOutputDTO>> getAllFull()
        {
            List<DriverFullOutputDTO> fullList = await DriverDAL.getAll();
            foreach (var item in fullList)
            {
                if (item.PersonID != default)
                {
                    item.PersonDetails = await Person.getPersonByID((int)item.PersonID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }
            }
            return fullList;
        }

        public static async Task<DriverFullOutputDTO> getDriverByPersonID(int PersonID)
        {
            DriverFullOutputDTO fullDto = await DriverDAL.getDriverByPersonID(PersonID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.PersonID != default)
            {
                fullDto.PersonDetails = await Person.getPersonByID((int)fullDto.PersonID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            return fullDto;
        }

public static async Task<DriverBriefOutputDTO> getDriverBriefOutputByPersonID(int PersonID)
{
    // Fetch the full flat record from DAL
    var fullDto = await DriverDAL.getDriverByPersonID(PersonID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new DriverBriefOutputDTO
    {
                PersonDetails = fullDto.PersonID != default ? await Person.getPersonBriefOutputByID((int)fullDto.PersonID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                    DriverID = fullDto.DriverID,
                    PersonID = fullDto.PersonID,
                    CreatedByUserID = fullDto.CreatedByUserID,
                    CreatedDate = fullDto.CreatedDate
    };
}        public static Task<bool> isDriverExistByPersonID(int PersonID)
        {
            return DriverDAL.isDriverExistByPersonID(PersonID);
        }
        public static async Task<List<DriverBriefOutputDTO>> getAllBriefByCreatedByUserID(int CreatedByUserID)
        {
            List<DriverFullOutputDTO> fullList = await DriverDAL.getAllByCreatedByUserID(CreatedByUserID);
            List<DriverBriefOutputDTO> briefList = new List<DriverBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new DriverBriefOutputDTO
                {
                PersonDetails = item.PersonID != default ? await Person.getPersonBriefOutputByID((int)item.PersonID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    DriverID = item.DriverID,
                    PersonID = item.PersonID,
                    CreatedByUserID = item.CreatedByUserID,
                    CreatedDate = item.CreatedDate                });
            }
            return briefList;
        }
        public static async Task<List<DriverFullOutputDTO>> getAllFullByCreatedByUserID(int CreatedByUserID)
        {
            List<DriverFullOutputDTO> fullList = await DriverDAL.getAllByCreatedByUserID(CreatedByUserID);
            foreach (var item in fullList)
            {
                if (item.PersonID != default)
                {
                    item.PersonDetails = await Person.getPersonByID((int)item.PersonID);
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
