using DAL;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class TestType
    {

        public static async Task<TestTypeFullOutputDTO> getTestTypeByID(int TestTypeID)
        {
            TestTypeFullOutputDTO fullDto = await TestTypeDAL.getTestTypeByID(TestTypeID);
            if (fullDto == null) return null;
            return fullDto;
        }

public static async Task<TestTypeBriefOutputDTO> getTestTypeBriefOutputByID(int TestTypeID)
{
    // Fetch the full flat record from DAL
    var fullDto = await TestTypeDAL.getTestTypeByID(TestTypeID);
    if (fullDto == null) return null;

    // Map it instantly in memory to BriefOutputDTO and populate its specific brief nested objects
    return new TestTypeBriefOutputDTO
    {
                    TestTypeID = fullDto.TestTypeID,
                    TestTypeTitle = fullDto.TestTypeTitle,
                    TestTypeDescription = fullDto.TestTypeDescription,
                    TestTypeFees = fullDto.TestTypeFees
    };
}        public static async Task<List<TestTypeBriefOutputDTO>> getAllBrief()
        {
            List<TestTypeFullOutputDTO> fullList = await TestTypeDAL.getAll();
            List<TestTypeBriefOutputDTO> briefList = new List<TestTypeBriefOutputDTO>();
            
            if (fullList == null) return briefList;

            foreach (var item in fullList)
            {
                briefList.Add(new TestTypeBriefOutputDTO
                {
                    TestTypeID = item.TestTypeID,
                    TestTypeTitle = item.TestTypeTitle,
                    TestTypeDescription = item.TestTypeDescription,
                    TestTypeFees = item.TestTypeFees                });
            }
            return briefList;
        }
        public static async Task<List<TestTypeFullOutputDTO>> getAllFull()
        {
            List<TestTypeFullOutputDTO> fullList = await TestTypeDAL.getAll();
            return fullList;
        }

    }
}
