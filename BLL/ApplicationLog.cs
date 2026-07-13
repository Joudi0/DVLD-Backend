using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class ApplicationLog
    {

        public static async Task<ApplicationLogFullOutputDTO> getApplicationLogByID(int LogID)
        {
            ApplicationLogFullOutputDTO fullDto = await ApplicationLogDAL.getApplicationLogByID(LogID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.ApplicationID != default)
            {
                fullDto.ApplicationDetails = await Application.getApplicationByID((int)fullDto.ApplicationID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.UserID != default)
            {
                fullDto.UserDetails = await clsUser.getUserByID((int)fullDto.UserID);
            }

            return fullDto;
        }

public static async Task<ApplicationLogBriefOutputDTO> getApplicationLogBriefOutputByID(int LogID)
{
    // Fetch the full flat record from DAL
    var fullDto = await ApplicationLogDAL.getApplicationLogByID(LogID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new ApplicationLogBriefOutputDTO
    {
                ApplicationDetails = fullDto.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)fullDto.ApplicationID) : null,
                UserDetails = fullDto.UserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.UserID) : null,
                    LogID = fullDto.LogID,
                    ApplicationID = fullDto.ApplicationID,
                    UserID = fullDto.UserID,
                    InsertedDate = fullDto.InsertedDate
    };
}
        /// <summary>
        /// Safe update for regular users using BriefInputDTO. Preserves system-controlled fields.
        /// </summary>
        public static async Task<bool> updateApplicationLog(ApplicationLogBriefInputDTO dto)
        {
            // 1. Fetch the existing full record to preserve internal data (Roles, Active status, Balance, etc.)
            var existingRecord = await getApplicationLogByID(dto.LogID);
            if (existingRecord == null) return false;

            // 2. Safely overwrite only the client-editable properties                existingRecord.LogID = dto.LogID;
                existingRecord.ApplicationID = dto.ApplicationID;
                existingRecord.UserID = dto.UserID;
                existingRecord.InsertedDate = dto.InsertedDate;
            
            // 3. Forward the fully preserved record to the DAL layer
            return await ApplicationLogDAL.updateApplicationLog(existingRecord);
        }
        public static async Task<bool> deleteApplicationLog(int LogID)
        {
            if (await isApplicationLogExistByID(LogID))
            {
                return await ApplicationLogDAL.deleteApplicationLog(LogID);
            }
            return false;
        }

        /// <summary>
        /// Adds a new record into the system, automatically embedding internal business rules and states.
        /// </summary>
        public static async Task<int> addApplicationLog(ApplicationLogBriefInputDTO dto)
        {
            var fullDto = new ApplicationLogFullOutputDTO
            {
                LogID = dto.LogID,
                ApplicationID = dto.ApplicationID,
                UserID = dto.UserID,
                InsertedDate = dto.InsertedDate
            };

            return await ApplicationLogDAL.addApplicationLog(fullDto);
        }
        public static Task<bool> isApplicationLogExistByID(int LogID)
        {
            return ApplicationLogDAL.isApplicationLogExistByID(LogID);
        }

        public static async Task<List<ApplicationLogBriefOutputDTO>> Paging(int rowsPerPage, int pageNumber, string sortColumn, string direction)
        {
            List<ApplicationLogFullOutputDTO> fullList = await ApplicationLogDAL.PagingDAL(rowsPerPage, pageNumber, sortColumn, direction);
            List<ApplicationLogBriefOutputDTO> briefList = new List<ApplicationLogBriefOutputDTO>();
            
            foreach (ApplicationLogFullOutputDTO item in fullList)
            {
                var briefItem = new ApplicationLogBriefOutputDTO
                {                    LogID = item.LogID,
                    ApplicationID = item.ApplicationID,
                    UserID = item.UserID,
                    InsertedDate = item.InsertedDate
                };

                // Populate nested object for brief list item
                if (item.ApplicationID != default)
                {
                    briefItem.ApplicationDetails = await Application.getApplicationBriefOutputByID((int)item.ApplicationID);
                }

                // Populate nested object for brief list item
                if (item.UserID != default)
                {
                    briefItem.UserDetails = await clsUser.getUserBriefOutputByID((int)item.UserID);
                }

                briefList.Add(briefItem);
            }
            
            return briefList;
        }
        public static async Task<List<ApplicationLogBriefOutputDTO>> getAllBrief()
        {
            List<ApplicationLogFullOutputDTO> fullList = await ApplicationLogDAL.getAll();
            List<ApplicationLogBriefOutputDTO> briefList = new List<ApplicationLogBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new ApplicationLogBriefOutputDTO
                {
                ApplicationDetails = item.ApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ApplicationID) : null,
                UserDetails = item.UserID != default ? await clsUser.getUserBriefOutputByID((int)item.UserID) : null,
                    LogID = item.LogID,
                    ApplicationID = item.ApplicationID,
                    UserID = item.UserID,
                    InsertedDate = item.InsertedDate                });
            }
            return briefList;
        }
        public static async Task<List<ApplicationLogFullOutputDTO>> getAllFull()
        {
            List<ApplicationLogFullOutputDTO> fullList = await ApplicationLogDAL.getAll();
            foreach (var item in fullList)
            {
                if (item.ApplicationID != default)
                {
                    item.ApplicationDetails = await Application.getApplicationByID((int)item.ApplicationID);
                }

                if (item.UserID != default)
                {
                    item.UserDetails = await clsUser.getUserByID((int)item.UserID);
                }
            }
            return fullList;
        }

    }
}
