using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class DetainedLicense
    {

        /// <summary>
        /// Adds a new record into the system, automatically embedding internal business rules and states.
        /// </summary>
        public static async Task<int> addDetainedLicense(DetainedLicenseBriefInputDTO dto)
        {
            var fullDto = new DetainedLicenseFullOutputDTO
            {
                DetainID = dto.DetainID,
                LicenseID = dto.LicenseID,
                DetainDate = dto.DetainDate,
                FineFees = dto.FineFees,
                CreatedByUserID = dto.CreatedByUserID,
                IsReleased = dto.IsReleased,
                ReleaseDate = dto.ReleaseDate,
                ReleasedByUserID = dto.ReleasedByUserID,
                ReleaseApplicationID = dto.ReleaseApplicationID
            };

            return await DetainedLicenseDAL.addDetainedLicense(fullDto);
        }

        public static async Task<DetainedLicenseFullOutputDTO> getDetainedLicenseByID(int DetainID)
        {
            DetainedLicenseFullOutputDTO fullDto = await DetainedLicenseDAL.getDetainedLicenseByID(DetainID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.LicenseID != default)
            {
                fullDto.LicenseDetails = await License.getLicenseByID((int)fullDto.LicenseID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.ReleasedByUserID != default)
            {
                fullDto.ReleasedByUserDetails = await clsUser.getUserByID((int)fullDto.ReleasedByUserID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.ReleaseApplicationID != default)
            {
                fullDto.ReleaseApplicationDetails = await Application.getApplicationByID((int)fullDto.ReleaseApplicationID);
            }

            return fullDto;
        }

public static async Task<DetainedLicenseBriefOutputDTO> getDetainedLicenseBriefOutputByID(int DetainID)
{
    // Fetch the full flat record from DAL
    var fullDto = await DetainedLicenseDAL.getDetainedLicenseByID(DetainID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new DetainedLicenseBriefOutputDTO
    {
                LicenseDetails = fullDto.LicenseID != default ? await License.getLicenseBriefOutputByID((int)fullDto.LicenseID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                ReleasedByUserDetails = fullDto.ReleasedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.ReleasedByUserID) : null,
                ReleaseApplicationDetails = fullDto.ReleaseApplicationID != default ? await Application.getApplicationBriefOutputByID((int)fullDto.ReleaseApplicationID) : null,
                    DetainID = fullDto.DetainID,
                    LicenseID = fullDto.LicenseID,
                    DetainDate = fullDto.DetainDate,
                    FineFees = fullDto.FineFees,
                    CreatedByUserID = fullDto.CreatedByUserID,
                    IsReleased = fullDto.IsReleased,
                    ReleaseDate = fullDto.ReleaseDate,
                    ReleasedByUserID = fullDto.ReleasedByUserID,
                    ReleaseApplicationID = fullDto.ReleaseApplicationID
    };
}
        /// <summary>
        /// Safe update for regular users using BriefInputDTO. Preserves system-controlled fields.
        /// </summary>
        public static async Task<bool> updateDetainedLicense(DetainedLicenseBriefInputDTO dto)
        {
            // 1. Fetch the existing full record to preserve internal data (Roles, Active status, Balance, etc.)
            var existingRecord = await getDetainedLicenseByID(dto.DetainID);
            if (existingRecord == null) return false;

            // 2. Safely overwrite only the client-editable properties                existingRecord.DetainID = dto.DetainID;
                existingRecord.LicenseID = dto.LicenseID;
                existingRecord.DetainDate = dto.DetainDate;
                existingRecord.FineFees = dto.FineFees;
                existingRecord.CreatedByUserID = dto.CreatedByUserID;
                existingRecord.IsReleased = dto.IsReleased;
                existingRecord.ReleaseDate = dto.ReleaseDate;
                existingRecord.ReleasedByUserID = dto.ReleasedByUserID;
                existingRecord.ReleaseApplicationID = dto.ReleaseApplicationID;
            
            // 3. Forward the fully preserved record to the DAL layer
            return await DetainedLicenseDAL.updateDetainedLicense(existingRecord);
        }
        public static async Task<bool> deleteDetainedLicense(int DetainID)
        {
            if (await isDetainedLicenseExistByID(DetainID))
            {
                return await DetainedLicenseDAL.deleteDetainedLicense(DetainID);
            }
            return false;
        }
        public static Task<bool> isDetainedLicenseExistByID(int DetainID)
        {
            return DetainedLicenseDAL.isDetainedLicenseExistByID(DetainID);
        }

        public static async Task<List<DetainedLicenseBriefOutputDTO>> Paging(int rowsPerPage, int pageNumber, string sortColumn, string direction)
        {
            List<DetainedLicenseFullOutputDTO> fullList = await DetainedLicenseDAL.PagingDAL(rowsPerPage, pageNumber, sortColumn, direction);
            List<DetainedLicenseBriefOutputDTO> briefList = new List<DetainedLicenseBriefOutputDTO>();
            
            foreach (DetainedLicenseFullOutputDTO item in fullList)
            {
                var briefItem = new DetainedLicenseBriefOutputDTO
                {                    DetainID = item.DetainID,
                    LicenseID = item.LicenseID,
                    DetainDate = item.DetainDate,
                    FineFees = item.FineFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsReleased = item.IsReleased,
                    ReleaseDate = item.ReleaseDate,
                    ReleasedByUserID = item.ReleasedByUserID,
                    ReleaseApplicationID = item.ReleaseApplicationID
                };

                // Populate nested object for brief list item
                if (item.LicenseID != default)
                {
                    briefItem.LicenseDetails = await License.getLicenseBriefOutputByID((int)item.LicenseID);
                }

                // Populate nested object for brief list item
                if (item.CreatedByUserID != default)
                {
                    briefItem.CreatedByUserDetails = await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID);
                }

                // Populate nested object for brief list item
                if (item.ReleasedByUserID != default)
                {
                    briefItem.ReleasedByUserDetails = await clsUser.getUserBriefOutputByID((int)item.ReleasedByUserID);
                }

                // Populate nested object for brief list item
                if (item.ReleaseApplicationID != default)
                {
                    briefItem.ReleaseApplicationDetails = await Application.getApplicationBriefOutputByID((int)item.ReleaseApplicationID);
                }

                briefList.Add(briefItem);
            }
            
            return briefList;
        }
        public static async Task<List<DetainedLicenseBriefOutputDTO>> getAllBrief()
        {
            List<DetainedLicenseFullOutputDTO> fullList = await DetainedLicenseDAL.getAll();
            List<DetainedLicenseBriefOutputDTO> briefList = new List<DetainedLicenseBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new DetainedLicenseBriefOutputDTO
                {
                LicenseDetails = item.LicenseID != default ? await License.getLicenseBriefOutputByID((int)item.LicenseID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                ReleasedByUserDetails = item.ReleasedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.ReleasedByUserID) : null,
                ReleaseApplicationDetails = item.ReleaseApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ReleaseApplicationID) : null,
                    DetainID = item.DetainID,
                    LicenseID = item.LicenseID,
                    DetainDate = item.DetainDate,
                    FineFees = item.FineFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsReleased = item.IsReleased,
                    ReleaseDate = item.ReleaseDate,
                    ReleasedByUserID = item.ReleasedByUserID,
                    ReleaseApplicationID = item.ReleaseApplicationID                });
            }
            return briefList;
        }
        public static async Task<List<DetainedLicenseFullOutputDTO>> getAllFull()
        {
            List<DetainedLicenseFullOutputDTO> fullList = await DetainedLicenseDAL.getAll();
            foreach (var item in fullList)
            {
                if (item.LicenseID != default)
                {
                    item.LicenseDetails = await License.getLicenseByID((int)item.LicenseID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }

                if (item.ReleasedByUserID != default)
                {
                    item.ReleasedByUserDetails = await clsUser.getUserByID((int)item.ReleasedByUserID);
                }

                if (item.ReleaseApplicationID != default)
                {
                    item.ReleaseApplicationDetails = await Application.getApplicationByID((int)item.ReleaseApplicationID);
                }
            }
            return fullList;
        }
        public static async Task<List<DetainedLicenseBriefOutputDTO>> getAllBriefByLicenseID(int LicenseID)
        {
            List<DetainedLicenseFullOutputDTO> fullList = await DetainedLicenseDAL.getAllByLicenseID(LicenseID);
            List<DetainedLicenseBriefOutputDTO> briefList = new List<DetainedLicenseBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new DetainedLicenseBriefOutputDTO
                {
                LicenseDetails = item.LicenseID != default ? await License.getLicenseBriefOutputByID((int)item.LicenseID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                ReleasedByUserDetails = item.ReleasedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.ReleasedByUserID) : null,
                ReleaseApplicationDetails = item.ReleaseApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ReleaseApplicationID) : null,
                    DetainID = item.DetainID,
                    LicenseID = item.LicenseID,
                    DetainDate = item.DetainDate,
                    FineFees = item.FineFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsReleased = item.IsReleased,
                    ReleaseDate = item.ReleaseDate,
                    ReleasedByUserID = item.ReleasedByUserID,
                    ReleaseApplicationID = item.ReleaseApplicationID                });
            }
            return briefList;
        }
        public static async Task<List<DetainedLicenseFullOutputDTO>> getAllFullByLicenseID(int LicenseID)
        {
            List<DetainedLicenseFullOutputDTO> fullList = await DetainedLicenseDAL.getAllByLicenseID(LicenseID);
            foreach (var item in fullList)
            {
                if (item.LicenseID != default)
                {
                    item.LicenseDetails = await License.getLicenseByID((int)item.LicenseID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }

                if (item.ReleasedByUserID != default)
                {
                    item.ReleasedByUserDetails = await clsUser.getUserByID((int)item.ReleasedByUserID);
                }

                if (item.ReleaseApplicationID != default)
                {
                    item.ReleaseApplicationDetails = await Application.getApplicationByID((int)item.ReleaseApplicationID);
                }
            }
            return fullList;
        }
        public static async Task<List<DetainedLicenseBriefOutputDTO>> getAllBriefByCreatedByUserID(int CreatedByUserID)
        {
            List<DetainedLicenseFullOutputDTO> fullList = await DetainedLicenseDAL.getAllByCreatedByUserID(CreatedByUserID);
            List<DetainedLicenseBriefOutputDTO> briefList = new List<DetainedLicenseBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new DetainedLicenseBriefOutputDTO
                {
                LicenseDetails = item.LicenseID != default ? await License.getLicenseBriefOutputByID((int)item.LicenseID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                ReleasedByUserDetails = item.ReleasedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.ReleasedByUserID) : null,
                ReleaseApplicationDetails = item.ReleaseApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ReleaseApplicationID) : null,
                    DetainID = item.DetainID,
                    LicenseID = item.LicenseID,
                    DetainDate = item.DetainDate,
                    FineFees = item.FineFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsReleased = item.IsReleased,
                    ReleaseDate = item.ReleaseDate,
                    ReleasedByUserID = item.ReleasedByUserID,
                    ReleaseApplicationID = item.ReleaseApplicationID                });
            }
            return briefList;
        }
        public static async Task<List<DetainedLicenseFullOutputDTO>> getAllFullByCreatedByUserID(int CreatedByUserID)
        {
            List<DetainedLicenseFullOutputDTO> fullList = await DetainedLicenseDAL.getAllByCreatedByUserID(CreatedByUserID);
            foreach (var item in fullList)
            {
                if (item.LicenseID != default)
                {
                    item.LicenseDetails = await License.getLicenseByID((int)item.LicenseID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }

                if (item.ReleasedByUserID != default)
                {
                    item.ReleasedByUserDetails = await clsUser.getUserByID((int)item.ReleasedByUserID);
                }

                if (item.ReleaseApplicationID != default)
                {
                    item.ReleaseApplicationDetails = await Application.getApplicationByID((int)item.ReleaseApplicationID);
                }
            }
            return fullList;
        }
        public static async Task<List<DetainedLicenseBriefOutputDTO>> getAllBriefByReleasedByUserID(int? ReleasedByUserID)
        {
            List<DetainedLicenseFullOutputDTO> fullList = await DetainedLicenseDAL.getAllByReleasedByUserID(ReleasedByUserID);
            List<DetainedLicenseBriefOutputDTO> briefList = new List<DetainedLicenseBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new DetainedLicenseBriefOutputDTO
                {
                LicenseDetails = item.LicenseID != default ? await License.getLicenseBriefOutputByID((int)item.LicenseID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                ReleasedByUserDetails = item.ReleasedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.ReleasedByUserID) : null,
                ReleaseApplicationDetails = item.ReleaseApplicationID != default ? await Application.getApplicationBriefOutputByID((int)item.ReleaseApplicationID) : null,
                    DetainID = item.DetainID,
                    LicenseID = item.LicenseID,
                    DetainDate = item.DetainDate,
                    FineFees = item.FineFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsReleased = item.IsReleased,
                    ReleaseDate = item.ReleaseDate,
                    ReleasedByUserID = item.ReleasedByUserID,
                    ReleaseApplicationID = item.ReleaseApplicationID                });
            }
            return briefList;
        }
        public static async Task<List<DetainedLicenseFullOutputDTO>> getAllFullByReleasedByUserID(int? ReleasedByUserID)
        {
            List<DetainedLicenseFullOutputDTO> fullList = await DetainedLicenseDAL.getAllByReleasedByUserID(ReleasedByUserID);
            foreach (var item in fullList)
            {
                if (item.LicenseID != default)
                {
                    item.LicenseDetails = await License.getLicenseByID((int)item.LicenseID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }

                if (item.ReleasedByUserID != default)
                {
                    item.ReleasedByUserDetails = await clsUser.getUserByID((int)item.ReleasedByUserID);
                }

                if (item.ReleaseApplicationID != default)
                {
                    item.ReleaseApplicationDetails = await Application.getApplicationByID((int)item.ReleaseApplicationID);
                }
            }
            return fullList;
        }

    }
}
