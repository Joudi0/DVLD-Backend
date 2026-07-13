using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class TestDAL
    {  

        public static async Task<int> addTest(TestFullOutputDTO dto)
        {
            int TestID = -1;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Tests_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"TestAppointmentID", SqlDbType.Int).Value = dto.TestAppointmentID;
		    command.Parameters.Add(@"TestResult", SqlDbType.Bit).Value = dto.TestResult;
		    command.Parameters.Add(@"Notes", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.Notes) ? DBNull.Value : (object)dto.Notes;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            }
            catch (Exception) { throw; }
            return TestID;
        }
        public static async Task<TestFullOutputDTO> getTestByID(int TestID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Tests_SelectByTestID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TestID", TestID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new TestFullOutputDTO
                    {
        TestID = (int)reader["TestID"],
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestResult = (bool)reader["TestResult"],
        Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> updateTest(TestFullOutputDTO dto)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Tests_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"TestID", SqlDbType.Int).Value = dto.TestID;
		    command.Parameters.Add(@"TestAppointmentID", SqlDbType.Int).Value = dto.TestAppointmentID;
		    command.Parameters.Add(@"TestResult", SqlDbType.Bit).Value = dto.TestResult;
		    command.Parameters.Add(@"Notes", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.Notes) ? DBNull.Value : (object)dto.Notes;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> deleteTest(int TestID)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Tests_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TestID", TestID);
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> isTestExistByID(int TestID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Tests_IsExistByTestID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TestID", TestID);
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null)
                {
                    isFound = Convert.ToInt32(result) == 1;
                }
            }
            catch (Exception) { throw; }
            return isFound;
        }
        public static async Task<List<TestFullOutputDTO>> PagingDAL(int RowsPerPage, int PageNumber, string SortColumn, string Direction)
        {
            var list = new List<TestFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Tests_Paging", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@RowsPerPage", RowsPerPage);
            command.Parameters.AddWithValue("@PageNumber", PageNumber);             
            command.Parameters.AddWithValue("@SortColumn", string.IsNullOrEmpty(SortColumn) ? DBNull.Value : SortColumn);
            command.Parameters.AddWithValue("@Direction", string.IsNullOrEmpty(Direction) ? DBNull.Value : Direction);    
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new TestFullOutputDTO
                    {
        TestID = (int)reader["TestID"],
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestResult = (bool)reader["TestResult"],
        Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<TestFullOutputDTO>> getAll()
        {
            var list = new List<TestFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Tests_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new TestFullOutputDTO
                    {
        TestID = (int)reader["TestID"],
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestResult = (bool)reader["TestResult"],
        Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<TestFullOutputDTO> getTestByTestAppointmentID(int TestAppointmentID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Tests_SelectByTestAppointmentID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new TestFullOutputDTO
                    {
        TestID = (int)reader["TestID"],
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestResult = (bool)reader["TestResult"],
        Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> isTestExistByTestAppointmentID(int TestAppointmentID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Tests_IsExistByTestAppointmentID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null)
                {
                    isFound = Convert.ToInt32(result) == 1;
                }
            }
            catch (Exception) { throw; }
            return isFound;
        }
        public static async Task<List<TestFullOutputDTO>> getAllByCreatedByUserID(int CreatedByUserID)
        {
            var list = new List<TestFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Tests_SelectAllByCreatedByUserID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new TestFullOutputDTO
                    {
        TestID = (int)reader["TestID"],
        TestAppointmentID = (int)reader["TestAppointmentID"],
        TestResult = (bool)reader["TestResult"],
        Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
        CreatedByUserID = (int)reader["CreatedByUserID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}