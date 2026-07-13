using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class ApplicationLogDAL
    {  

        public static async Task<ApplicationLogFullOutputDTO> getApplicationLogByID(int LogID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_ApplicationLog_SelectByLogID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LogID", LogID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new ApplicationLogFullOutputDTO
                    {
        LogID = (int)reader["LogID"],
        ApplicationID = (reader["ApplicationID"] == DBNull.Value) ? null : (int?)reader["ApplicationID"],
        UserID = (reader["UserID"] == DBNull.Value) ? null : (int?)reader["UserID"],
        InsertedDate = (reader["InsertedDate"] == DBNull.Value) ? null : (DateTime?)reader["InsertedDate"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> updateApplicationLog(ApplicationLogFullOutputDTO dto)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_ApplicationLog_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"LogID", SqlDbType.Int).Value = dto.LogID;
		    command.Parameters.Add(@"ApplicationID", SqlDbType.Int).Value = (dto.ApplicationID == null) ? DBNull.Value : (object)dto.ApplicationID;
		    command.Parameters.Add(@"UserID", SqlDbType.Int).Value = (dto.UserID == null) ? DBNull.Value : (object)dto.UserID;
		    command.Parameters.Add(@"InsertedDate", SqlDbType.DateTime).Value = (dto.InsertedDate == null) ? DBNull.Value : (object)dto.InsertedDate;
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> deleteApplicationLog(int LogID)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_ApplicationLog_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LogID", LogID);
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<int> addApplicationLog(ApplicationLogFullOutputDTO dto)
        {
            int ApplicationLogID = -1;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_ApplicationLog_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"ApplicationID", SqlDbType.Int).Value = (dto.ApplicationID == null) ? DBNull.Value : (object)dto.ApplicationID;
		    command.Parameters.Add(@"UserID", SqlDbType.Int).Value = (dto.UserID == null) ? DBNull.Value : (object)dto.UserID;
		    command.Parameters.Add(@"InsertedDate", SqlDbType.DateTime).Value = (dto.InsertedDate == null) ? DBNull.Value : (object)dto.InsertedDate;
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ApplicationLogID = insertedID;
                }
            }
            catch (Exception) { throw; }
            return ApplicationLogID;
        }
        public static async Task<bool> isApplicationLogExistByID(int LogID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_ApplicationLog_IsExistByLogID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LogID", LogID);
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
        public static async Task<List<ApplicationLogFullOutputDTO>> PagingDAL(int RowsPerPage, int PageNumber, string SortColumn, string Direction)
        {
            var list = new List<ApplicationLogFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_ApplicationLog_Paging", connection);
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
                    list.Add(new ApplicationLogFullOutputDTO
                    {
        LogID = (int)reader["LogID"],
        ApplicationID = (reader["ApplicationID"] == DBNull.Value) ? null : (int?)reader["ApplicationID"],
        UserID = (reader["UserID"] == DBNull.Value) ? null : (int?)reader["UserID"],
        InsertedDate = (reader["InsertedDate"] == DBNull.Value) ? null : (DateTime?)reader["InsertedDate"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<ApplicationLogFullOutputDTO>> getAll()
        {
            var list = new List<ApplicationLogFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_ApplicationLog_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new ApplicationLogFullOutputDTO
                    {
        LogID = (int)reader["LogID"],
        ApplicationID = (reader["ApplicationID"] == DBNull.Value) ? null : (int?)reader["ApplicationID"],
        UserID = (reader["UserID"] == DBNull.Value) ? null : (int?)reader["UserID"],
        InsertedDate = (reader["InsertedDate"] == DBNull.Value) ? null : (DateTime?)reader["InsertedDate"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}