using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class TestAppointment
    {

        /// <summary>
        /// Adds a new record into the system, automatically embedding internal business rules and states.
        /// </summary>
        public static async Task<int> addTestAppointment(TestAppointmentBriefInputDTO dto)
        {
            var fullDto = new TestAppointmentFullOutputDTO
            {
                TestAppointmentID = dto.TestAppointmentID,
                TestTypeID = dto.TestTypeID,
                LocalDrivingLicenseApplicationID = dto.LocalDrivingLicenseApplicationID,
                AppointmentDate = dto.AppointmentDate,
                PaidFees = dto.PaidFees,
                CreatedByUserID = dto.CreatedByUserID,
                IsLocked = dto.IsLocked,
                RetakeTestApplicationID = dto.RetakeTestApplicationID
            };

            return await TestAppointmentDAL.addTestAppointment(fullDto);
        }

        public static async Task<TestAppointmentFullOutputDTO> getTestAppointmentByID(int TestAppointmentID)
        {
            TestAppointmentFullOutputDTO fullDto = await TestAppointmentDAL.getTestAppointmentByID(TestAppointmentID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.TestTypeID != default)
            {
                fullDto.TestTypeDetails = await TestType.getTestTypeByID((int)fullDto.TestTypeID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.LocalDrivingLicenseApplicationID != default)
            {
                fullDto.LocalDrivingLicenseApplicationDetails = await License.getLicenseByID((int)fullDto.LocalDrivingLicenseApplicationID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.RetakeTestApplicationID != default)
            {
                fullDto.RetakeTestApplicationDetails = await Test.getTestByID((int)fullDto.RetakeTestApplicationID);
            }

            return fullDto;
        }

public static async Task<TestAppointmentBriefOutputDTO> getTestAppointmentBriefOutputByID(int TestAppointmentID)
{
    // Fetch the full flat record from DAL
    var fullDto = await TestAppointmentDAL.getTestAppointmentByID(TestAppointmentID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new TestAppointmentBriefOutputDTO
    {
                TestTypeDetails = fullDto.TestTypeID != default ? await TestType.getTestTypeBriefOutputByID((int)fullDto.TestTypeID) : null,
                LocalDrivingLicenseApplicationDetails = fullDto.LocalDrivingLicenseApplicationID != default ? await License.getLicenseBriefOutputByID((int)fullDto.LocalDrivingLicenseApplicationID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                RetakeTestApplicationDetails = fullDto.RetakeTestApplicationID != default ? await Test.getTestBriefOutputByID((int)fullDto.RetakeTestApplicationID) : null,
                    TestAppointmentID = fullDto.TestAppointmentID,
                    TestTypeID = fullDto.TestTypeID,
                    LocalDrivingLicenseApplicationID = fullDto.LocalDrivingLicenseApplicationID,
                    AppointmentDate = fullDto.AppointmentDate,
                    PaidFees = fullDto.PaidFees,
                    CreatedByUserID = fullDto.CreatedByUserID,
                    IsLocked = fullDto.IsLocked,
                    RetakeTestApplicationID = fullDto.RetakeTestApplicationID
    };
}
        /// <summary>
        /// Safe update for regular users using BriefInputDTO. Preserves system-controlled fields.
        /// </summary>
        public static async Task<bool> updateTestAppointment(TestAppointmentBriefInputDTO dto)
        {
            // 1. Fetch the existing full record to preserve internal data (Roles, Active status, Balance, etc.)
            var existingRecord = await getTestAppointmentByID(dto.TestAppointmentID);
            if (existingRecord == null) return false;

            // 2. Safely overwrite only the client-editable properties                existingRecord.TestAppointmentID = dto.TestAppointmentID;
                existingRecord.TestTypeID = dto.TestTypeID;
                existingRecord.LocalDrivingLicenseApplicationID = dto.LocalDrivingLicenseApplicationID;
                existingRecord.AppointmentDate = dto.AppointmentDate;
                existingRecord.PaidFees = dto.PaidFees;
                existingRecord.CreatedByUserID = dto.CreatedByUserID;
                existingRecord.IsLocked = dto.IsLocked;
                existingRecord.RetakeTestApplicationID = dto.RetakeTestApplicationID;
            
            // 3. Forward the fully preserved record to the DAL layer
            return await TestAppointmentDAL.updateTestAppointment(existingRecord);
        }
        public static async Task<bool> deleteTestAppointment(int TestAppointmentID)
        {
            if (await isTestAppointmentExistByID(TestAppointmentID))
            {
                return await TestAppointmentDAL.deleteTestAppointment(TestAppointmentID);
            }
            return false;
        }
        public static Task<bool> isTestAppointmentExistByID(int TestAppointmentID)
        {
            return TestAppointmentDAL.isTestAppointmentExistByID(TestAppointmentID);
        }

        public static async Task<List<TestAppointmentBriefOutputDTO>> Paging(int rowsPerPage, int pageNumber, string sortColumn, string direction)
        {
            List<TestAppointmentFullOutputDTO> fullList = await TestAppointmentDAL.PagingDAL(rowsPerPage, pageNumber, sortColumn, direction);
            List<TestAppointmentBriefOutputDTO> briefList = new List<TestAppointmentBriefOutputDTO>();
            
            foreach (TestAppointmentFullOutputDTO item in fullList)
            {
                var briefItem = new TestAppointmentBriefOutputDTO
                {                    TestAppointmentID = item.TestAppointmentID,
                    TestTypeID = item.TestTypeID,
                    LocalDrivingLicenseApplicationID = item.LocalDrivingLicenseApplicationID,
                    AppointmentDate = item.AppointmentDate,
                    PaidFees = item.PaidFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsLocked = item.IsLocked,
                    RetakeTestApplicationID = item.RetakeTestApplicationID
                };

                // Populate nested object for brief list item
                if (item.TestTypeID != default)
                {
                    briefItem.TestTypeDetails = await TestType.getTestTypeBriefOutputByID((int)item.TestTypeID);
                }

                // Populate nested object for brief list item
                if (item.LocalDrivingLicenseApplicationID != default)
                {
                    briefItem.LocalDrivingLicenseApplicationDetails = await License.getLicenseBriefOutputByID((int)item.LocalDrivingLicenseApplicationID);
                }

                // Populate nested object for brief list item
                if (item.CreatedByUserID != default)
                {
                    briefItem.CreatedByUserDetails = await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID);
                }

                // Populate nested object for brief list item
                if (item.RetakeTestApplicationID != default)
                {
                    briefItem.RetakeTestApplicationDetails = await Test.getTestBriefOutputByID((int)item.RetakeTestApplicationID);
                }

                briefList.Add(briefItem);
            }
            
            return briefList;
        }
        public static async Task<List<TestAppointmentBriefOutputDTO>> getAllBrief()
        {
            List<TestAppointmentFullOutputDTO> fullList = await TestAppointmentDAL.getAll();
            List<TestAppointmentBriefOutputDTO> briefList = new List<TestAppointmentBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new TestAppointmentBriefOutputDTO
                {
                TestTypeDetails = item.TestTypeID != default ? await TestType.getTestTypeBriefOutputByID((int)item.TestTypeID) : null,
                LocalDrivingLicenseApplicationDetails = item.LocalDrivingLicenseApplicationID != default ? await License.getLicenseBriefOutputByID((int)item.LocalDrivingLicenseApplicationID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                RetakeTestApplicationDetails = item.RetakeTestApplicationID != default ? await Test.getTestBriefOutputByID((int)item.RetakeTestApplicationID) : null,
                    TestAppointmentID = item.TestAppointmentID,
                    TestTypeID = item.TestTypeID,
                    LocalDrivingLicenseApplicationID = item.LocalDrivingLicenseApplicationID,
                    AppointmentDate = item.AppointmentDate,
                    PaidFees = item.PaidFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsLocked = item.IsLocked,
                    RetakeTestApplicationID = item.RetakeTestApplicationID                });
            }
            return briefList;
        }
        public static async Task<List<TestAppointmentFullOutputDTO>> getAllFull()
        {
            List<TestAppointmentFullOutputDTO> fullList = await TestAppointmentDAL.getAll();
            foreach (var item in fullList)
            {
                if (item.TestTypeID != default)
                {
                    item.TestTypeDetails = await TestType.getTestTypeByID((int)item.TestTypeID);
                }

                if (item.LocalDrivingLicenseApplicationID != default)
                {
                    item.LocalDrivingLicenseApplicationDetails = await License.getLicenseByID((int)item.LocalDrivingLicenseApplicationID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }

                if (item.RetakeTestApplicationID != default)
                {
                    item.RetakeTestApplicationDetails = await Test.getTestByID((int)item.RetakeTestApplicationID);
                }
            }
            return fullList;
        }

        public static async Task<TestAppointmentFullOutputDTO> getTestAppointmentByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            TestAppointmentFullOutputDTO fullDto = await TestAppointmentDAL.getTestAppointmentByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.TestTypeID != default)
            {
                fullDto.TestTypeDetails = await TestType.getTestTypeByID((int)fullDto.TestTypeID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.LocalDrivingLicenseApplicationID != default)
            {
                fullDto.LocalDrivingLicenseApplicationDetails = await License.getLicenseByID((int)fullDto.LocalDrivingLicenseApplicationID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.RetakeTestApplicationID != default)
            {
                fullDto.RetakeTestApplicationDetails = await Test.getTestByID((int)fullDto.RetakeTestApplicationID);
            }

            return fullDto;
        }

public static async Task<TestAppointmentBriefOutputDTO> getTestAppointmentBriefOutputByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
{
    // Fetch the full flat record from DAL
    var fullDto = await TestAppointmentDAL.getTestAppointmentByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new TestAppointmentBriefOutputDTO
    {
                TestTypeDetails = fullDto.TestTypeID != default ? await TestType.getTestTypeBriefOutputByID((int)fullDto.TestTypeID) : null,
                LocalDrivingLicenseApplicationDetails = fullDto.LocalDrivingLicenseApplicationID != default ? await License.getLicenseBriefOutputByID((int)fullDto.LocalDrivingLicenseApplicationID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                RetakeTestApplicationDetails = fullDto.RetakeTestApplicationID != default ? await Test.getTestBriefOutputByID((int)fullDto.RetakeTestApplicationID) : null,
                    TestAppointmentID = fullDto.TestAppointmentID,
                    TestTypeID = fullDto.TestTypeID,
                    LocalDrivingLicenseApplicationID = fullDto.LocalDrivingLicenseApplicationID,
                    AppointmentDate = fullDto.AppointmentDate,
                    PaidFees = fullDto.PaidFees,
                    CreatedByUserID = fullDto.CreatedByUserID,
                    IsLocked = fullDto.IsLocked,
                    RetakeTestApplicationID = fullDto.RetakeTestApplicationID
    };
}
        public static async Task<TestAppointmentFullOutputDTO> getTestAppointmentByRetakeTestApplicationID(int? RetakeTestApplicationID)
        {
            TestAppointmentFullOutputDTO fullDto = await TestAppointmentDAL.getTestAppointmentByRetakeTestApplicationID(RetakeTestApplicationID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.TestTypeID != default)
            {
                fullDto.TestTypeDetails = await TestType.getTestTypeByID((int)fullDto.TestTypeID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.LocalDrivingLicenseApplicationID != default)
            {
                fullDto.LocalDrivingLicenseApplicationDetails = await License.getLicenseByID((int)fullDto.LocalDrivingLicenseApplicationID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.RetakeTestApplicationID != default)
            {
                fullDto.RetakeTestApplicationDetails = await Test.getTestByID((int)fullDto.RetakeTestApplicationID);
            }

            return fullDto;
        }

public static async Task<TestAppointmentBriefOutputDTO> getTestAppointmentBriefOutputByRetakeTestApplicationID(int? RetakeTestApplicationID)
{
    // Fetch the full flat record from DAL
    var fullDto = await TestAppointmentDAL.getTestAppointmentByRetakeTestApplicationID(RetakeTestApplicationID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new TestAppointmentBriefOutputDTO
    {
                TestTypeDetails = fullDto.TestTypeID != default ? await TestType.getTestTypeBriefOutputByID((int)fullDto.TestTypeID) : null,
                LocalDrivingLicenseApplicationDetails = fullDto.LocalDrivingLicenseApplicationID != default ? await License.getLicenseBriefOutputByID((int)fullDto.LocalDrivingLicenseApplicationID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                RetakeTestApplicationDetails = fullDto.RetakeTestApplicationID != default ? await Test.getTestBriefOutputByID((int)fullDto.RetakeTestApplicationID) : null,
                    TestAppointmentID = fullDto.TestAppointmentID,
                    TestTypeID = fullDto.TestTypeID,
                    LocalDrivingLicenseApplicationID = fullDto.LocalDrivingLicenseApplicationID,
                    AppointmentDate = fullDto.AppointmentDate,
                    PaidFees = fullDto.PaidFees,
                    CreatedByUserID = fullDto.CreatedByUserID,
                    IsLocked = fullDto.IsLocked,
                    RetakeTestApplicationID = fullDto.RetakeTestApplicationID
    };
}        public static Task<bool> isTestAppointmentExistByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            return TestAppointmentDAL.isTestAppointmentExistByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
        }
        public static Task<bool> isTestAppointmentExistByRetakeTestApplicationID(int? RetakeTestApplicationID)
        {
            return TestAppointmentDAL.isTestAppointmentExistByRetakeTestApplicationID(RetakeTestApplicationID);
        }
        public static async Task<List<TestAppointmentBriefOutputDTO>> getAllBriefByTestTypeID(int TestTypeID)
        {
            List<TestAppointmentFullOutputDTO> fullList = await TestAppointmentDAL.getAllByTestTypeID(TestTypeID);
            List<TestAppointmentBriefOutputDTO> briefList = new List<TestAppointmentBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new TestAppointmentBriefOutputDTO
                {
                TestTypeDetails = item.TestTypeID != default ? await TestType.getTestTypeBriefOutputByID((int)item.TestTypeID) : null,
                LocalDrivingLicenseApplicationDetails = item.LocalDrivingLicenseApplicationID != default ? await License.getLicenseBriefOutputByID((int)item.LocalDrivingLicenseApplicationID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                RetakeTestApplicationDetails = item.RetakeTestApplicationID != default ? await Test.getTestBriefOutputByID((int)item.RetakeTestApplicationID) : null,
                    TestAppointmentID = item.TestAppointmentID,
                    TestTypeID = item.TestTypeID,
                    LocalDrivingLicenseApplicationID = item.LocalDrivingLicenseApplicationID,
                    AppointmentDate = item.AppointmentDate,
                    PaidFees = item.PaidFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsLocked = item.IsLocked,
                    RetakeTestApplicationID = item.RetakeTestApplicationID                });
            }
            return briefList;
        }
        public static async Task<List<TestAppointmentFullOutputDTO>> getAllFullByTestTypeID(int TestTypeID)
        {
            List<TestAppointmentFullOutputDTO> fullList = await TestAppointmentDAL.getAllByTestTypeID(TestTypeID);
            foreach (var item in fullList)
            {
                if (item.TestTypeID != default)
                {
                    item.TestTypeDetails = await TestType.getTestTypeByID((int)item.TestTypeID);
                }

                if (item.LocalDrivingLicenseApplicationID != default)
                {
                    item.LocalDrivingLicenseApplicationDetails = await License.getLicenseByID((int)item.LocalDrivingLicenseApplicationID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }

                if (item.RetakeTestApplicationID != default)
                {
                    item.RetakeTestApplicationDetails = await Test.getTestByID((int)item.RetakeTestApplicationID);
                }
            }
            return fullList;
        }
        public static async Task<List<TestAppointmentBriefOutputDTO>> getAllBriefByCreatedByUserID(int CreatedByUserID)
        {
            List<TestAppointmentFullOutputDTO> fullList = await TestAppointmentDAL.getAllByCreatedByUserID(CreatedByUserID);
            List<TestAppointmentBriefOutputDTO> briefList = new List<TestAppointmentBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new TestAppointmentBriefOutputDTO
                {
                TestTypeDetails = item.TestTypeID != default ? await TestType.getTestTypeBriefOutputByID((int)item.TestTypeID) : null,
                LocalDrivingLicenseApplicationDetails = item.LocalDrivingLicenseApplicationID != default ? await License.getLicenseBriefOutputByID((int)item.LocalDrivingLicenseApplicationID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                RetakeTestApplicationDetails = item.RetakeTestApplicationID != default ? await Test.getTestBriefOutputByID((int)item.RetakeTestApplicationID) : null,
                    TestAppointmentID = item.TestAppointmentID,
                    TestTypeID = item.TestTypeID,
                    LocalDrivingLicenseApplicationID = item.LocalDrivingLicenseApplicationID,
                    AppointmentDate = item.AppointmentDate,
                    PaidFees = item.PaidFees,
                    CreatedByUserID = item.CreatedByUserID,
                    IsLocked = item.IsLocked,
                    RetakeTestApplicationID = item.RetakeTestApplicationID                });
            }
            return briefList;
        }
        public static async Task<List<TestAppointmentFullOutputDTO>> getAllFullByCreatedByUserID(int CreatedByUserID)
        {
            List<TestAppointmentFullOutputDTO> fullList = await TestAppointmentDAL.getAllByCreatedByUserID(CreatedByUserID);
            foreach (var item in fullList)
            {
                if (item.TestTypeID != default)
                {
                    item.TestTypeDetails = await TestType.getTestTypeByID((int)item.TestTypeID);
                }

                if (item.LocalDrivingLicenseApplicationID != default)
                {
                    item.LocalDrivingLicenseApplicationDetails = await License.getLicenseByID((int)item.LocalDrivingLicenseApplicationID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }

                if (item.RetakeTestApplicationID != default)
                {
                    item.RetakeTestApplicationDetails = await Test.getTestByID((int)item.RetakeTestApplicationID);
                }
            }
            return fullList;
        }

    }
}
