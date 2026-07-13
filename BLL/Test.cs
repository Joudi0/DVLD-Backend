using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class Test
    {

        /// <summary>
        /// Adds a new record into the system, automatically embedding internal business rules and states.
        /// </summary>
        public static async Task<int> addTest(TestBriefInputDTO dto)
        {
            var fullDto = new TestFullOutputDTO
            {
                TestID = dto.TestID,
                TestAppointmentID = dto.TestAppointmentID,
                TestResult = dto.TestResult,
                Notes = dto.Notes,
                CreatedByUserID = dto.CreatedByUserID
            };

            return await TestDAL.addTest(fullDto);
        }

        public static async Task<TestFullOutputDTO> getTestByID(int TestID)
        {
            TestFullOutputDTO fullDto = await TestDAL.getTestByID(TestID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.TestAppointmentID != default)
            {
                fullDto.TestAppointmentDetails = await TestAppointment.getTestAppointmentByID((int)fullDto.TestAppointmentID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            return fullDto;
        }

public static async Task<TestBriefOutputDTO> getTestBriefOutputByID(int TestID)
{
    // Fetch the full flat record from DAL
    var fullDto = await TestDAL.getTestByID(TestID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new TestBriefOutputDTO
    {
                TestAppointmentDetails = fullDto.TestAppointmentID != default ? await TestAppointment.getTestAppointmentBriefOutputByID((int)fullDto.TestAppointmentID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                    TestID = fullDto.TestID,
                    TestAppointmentID = fullDto.TestAppointmentID,
                    TestResult = fullDto.TestResult,
                    Notes = fullDto.Notes,
                    CreatedByUserID = fullDto.CreatedByUserID
    };
}
        /// <summary>
        /// Safe update for regular users using BriefInputDTO. Preserves system-controlled fields.
        /// </summary>
        public static async Task<bool> updateTest(TestBriefInputDTO dto)
        {
            // 1. Fetch the existing full record to preserve internal data (Roles, Active status, Balance, etc.)
            var existingRecord = await getTestByID(dto.TestID);
            if (existingRecord == null) return false;

            // 2. Safely overwrite only the client-editable properties                existingRecord.TestID = dto.TestID;
                existingRecord.TestAppointmentID = dto.TestAppointmentID;
                existingRecord.TestResult = dto.TestResult;
                existingRecord.Notes = dto.Notes;
                existingRecord.CreatedByUserID = dto.CreatedByUserID;
            
            // 3. Forward the fully preserved record to the DAL layer
            return await TestDAL.updateTest(existingRecord);
        }
        public static async Task<bool> deleteTest(int TestID)
        {
            if (await isTestExistByID(TestID))
            {
                return await TestDAL.deleteTest(TestID);
            }
            return false;
        }
        public static Task<bool> isTestExistByID(int TestID)
        {
            return TestDAL.isTestExistByID(TestID);
        }

        public static async Task<List<TestBriefOutputDTO>> Paging(int rowsPerPage, int pageNumber, string sortColumn, string direction)
        {
            List<TestFullOutputDTO> fullList = await TestDAL.PagingDAL(rowsPerPage, pageNumber, sortColumn, direction);
            List<TestBriefOutputDTO> briefList = new List<TestBriefOutputDTO>();
            
            foreach (TestFullOutputDTO item in fullList)
            {
                var briefItem = new TestBriefOutputDTO
                {                    TestID = item.TestID,
                    TestAppointmentID = item.TestAppointmentID,
                    TestResult = item.TestResult,
                    Notes = item.Notes,
                    CreatedByUserID = item.CreatedByUserID
                };

                // Populate nested object for brief list item
                if (item.TestAppointmentID != default)
                {
                    briefItem.TestAppointmentDetails = await TestAppointment.getTestAppointmentBriefOutputByID((int)item.TestAppointmentID);
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
        public static async Task<List<TestBriefOutputDTO>> getAllBrief()
        {
            List<TestFullOutputDTO> fullList = await TestDAL.getAll();
            List<TestBriefOutputDTO> briefList = new List<TestBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new TestBriefOutputDTO
                {
                TestAppointmentDetails = item.TestAppointmentID != default ? await TestAppointment.getTestAppointmentBriefOutputByID((int)item.TestAppointmentID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    TestID = item.TestID,
                    TestAppointmentID = item.TestAppointmentID,
                    TestResult = item.TestResult,
                    Notes = item.Notes,
                    CreatedByUserID = item.CreatedByUserID                });
            }
            return briefList;
        }
        public static async Task<List<TestFullOutputDTO>> getAllFull()
        {
            List<TestFullOutputDTO> fullList = await TestDAL.getAll();
            foreach (var item in fullList)
            {
                if (item.TestAppointmentID != default)
                {
                    item.TestAppointmentDetails = await TestAppointment.getTestAppointmentByID((int)item.TestAppointmentID);
                }

                if (item.CreatedByUserID != default)
                {
                    item.CreatedByUserDetails = await clsUser.getUserByID((int)item.CreatedByUserID);
                }
            }
            return fullList;
        }

        public static async Task<TestFullOutputDTO> getTestByTestAppointmentID(int TestAppointmentID)
        {
            TestFullOutputDTO fullDto = await TestDAL.getTestByTestAppointmentID(TestAppointmentID);
            if (fullDto == null) return null;
            // Directly populate nested object using the specialized Full method
            if (fullDto.TestAppointmentID != default)
            {
                fullDto.TestAppointmentDetails = await TestAppointment.getTestAppointmentByID((int)fullDto.TestAppointmentID);
            }

            // Directly populate nested object using the specialized Full method
            if (fullDto.CreatedByUserID != default)
            {
                fullDto.CreatedByUserDetails = await clsUser.getUserByID((int)fullDto.CreatedByUserID);
            }

            return fullDto;
        }

public static async Task<TestBriefOutputDTO> getTestBriefOutputByTestAppointmentID(int TestAppointmentID)
{
    // Fetch the full flat record from DAL
    var fullDto = await TestDAL.getTestByTestAppointmentID(TestAppointmentID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new TestBriefOutputDTO
    {
                TestAppointmentDetails = fullDto.TestAppointmentID != default ? await TestAppointment.getTestAppointmentBriefOutputByID((int)fullDto.TestAppointmentID) : null,
                CreatedByUserDetails = fullDto.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)fullDto.CreatedByUserID) : null,
                    TestID = fullDto.TestID,
                    TestAppointmentID = fullDto.TestAppointmentID,
                    TestResult = fullDto.TestResult,
                    Notes = fullDto.Notes,
                    CreatedByUserID = fullDto.CreatedByUserID
    };
}        public static Task<bool> isTestExistByTestAppointmentID(int TestAppointmentID)
        {
            return TestDAL.isTestExistByTestAppointmentID(TestAppointmentID);
        }
        public static async Task<List<TestBriefOutputDTO>> getAllBriefByCreatedByUserID(int CreatedByUserID)
        {
            List<TestFullOutputDTO> fullList = await TestDAL.getAllByCreatedByUserID(CreatedByUserID);
            List<TestBriefOutputDTO> briefList = new List<TestBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new TestBriefOutputDTO
                {
                TestAppointmentDetails = item.TestAppointmentID != default ? await TestAppointment.getTestAppointmentBriefOutputByID((int)item.TestAppointmentID) : null,
                CreatedByUserDetails = item.CreatedByUserID != default ? await clsUser.getUserBriefOutputByID((int)item.CreatedByUserID) : null,
                    TestID = item.TestID,
                    TestAppointmentID = item.TestAppointmentID,
                    TestResult = item.TestResult,
                    Notes = item.Notes,
                    CreatedByUserID = item.CreatedByUserID                });
            }
            return briefList;
        }
        public static async Task<List<TestFullOutputDTO>> getAllFullByCreatedByUserID(int CreatedByUserID)
        {
            List<TestFullOutputDTO> fullList = await TestDAL.getAllByCreatedByUserID(CreatedByUserID);
            foreach (var item in fullList)
            {
                if (item.TestAppointmentID != default)
                {
                    item.TestAppointmentDetails = await TestAppointment.getTestAppointmentByID((int)item.TestAppointmentID);
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
