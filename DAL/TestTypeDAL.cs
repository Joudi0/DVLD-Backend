using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class TestTypeDAL
    {  

        public static async Task<TestTypeFullOutputDTO> getTestTypeByID(int TestTypeID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestTypes_SelectByTestTypeID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new TestTypeFullOutputDTO
                    {
        TestTypeID = (int)reader["TestTypeID"],
        TestTypeTitle = (string)reader["TestTypeTitle"],
        TestTypeDescription = (string)reader["TestTypeDescription"],
        TestTypeFees = (decimal)reader["TestTypeFees"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<List<TestTypeFullOutputDTO>> getAll()
        {
            var list = new List<TestTypeFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_TestTypes_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new TestTypeFullOutputDTO
                    {
        TestTypeID = (int)reader["TestTypeID"],
        TestTypeTitle = (string)reader["TestTypeTitle"],
        TestTypeDescription = (string)reader["TestTypeDescription"],
        TestTypeFees = (decimal)reader["TestTypeFees"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}