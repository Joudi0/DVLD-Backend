using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class UserDAL
    {  

        public static async Task<AuthDTO> getHashAndSalt(string Username)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Users_GetSecurityDataByUsername", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserName", Username);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new AuthDTO
                    {
                        UserID = (reader["UserID"] == DBNull.Value) ? 0 : (int)reader["UserID"],
                        PasswordHash = (reader["PasswordHash"] == DBNull.Value) ? "" : (string)reader["PasswordHash"],
                        PasswordSalt = (reader["PasswordSalt"] == DBNull.Value) ? "" : (string)reader["PasswordSalt"],
                        UserRoleID = (enRoles)((reader["UserRole"] == DBNull.Value) ? 0 : (int)reader["UserRole"])
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<int> addUser(UserFullOutputDTO dto)
        {
            int UserID = -1;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Users_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"PersonID", SqlDbType.Int).Value = dto.PersonID;
		    command.Parameters.Add(@"UserName", SqlDbType.NVarChar).Value = dto.UserName;
		    command.Parameters.Add(@"PasswordHash", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.PasswordHash) ? DBNull.Value : (object)dto.PasswordHash;
		    command.Parameters.Add(@"IsActive", SqlDbType.Bit).Value = dto.IsActive;
		    command.Parameters.Add(@"UserRole", SqlDbType.Int).Value = (dto.UserRole == null) ? DBNull.Value : (object)dto.UserRole;
		    command.Parameters.Add(@"PasswordSalt", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.PasswordSalt) ? DBNull.Value : (object)dto.PasswordSalt;
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }
            }
            catch (Exception) { throw; }
            return UserID;
        }
        public static async Task<UserFullOutputDTO> getUserByID(int UserID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Users_SelectByUserID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new UserFullOutputDTO
                    {
        UserID = (int)reader["UserID"],
        PersonID = (int)reader["PersonID"],
        UserName = (string)reader["UserName"],
        PasswordHash = (reader["PasswordHash"] == DBNull.Value) ? "" : (string)reader["PasswordHash"],
        IsActive = (bool)reader["IsActive"],
        UserRole = (reader["UserRole"] == DBNull.Value) ? null : (int?)reader["UserRole"],
        PasswordSalt = (reader["PasswordSalt"] == DBNull.Value) ? "" : (string)reader["PasswordSalt"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<UserFullOutputDTO> getUserByUserName(string UserName)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Users_SelectByUserName", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserName", UserName);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new UserFullOutputDTO
                    {
        UserID = (int)reader["UserID"],
        PersonID = (int)reader["PersonID"],
        UserName = (string)reader["UserName"],
        PasswordHash = (reader["PasswordHash"] == DBNull.Value) ? "" : (string)reader["PasswordHash"],
        IsActive = (bool)reader["IsActive"],
        UserRole = (reader["UserRole"] == DBNull.Value) ? null : (int?)reader["UserRole"],
        PasswordSalt = (reader["PasswordSalt"] == DBNull.Value) ? "" : (string)reader["PasswordSalt"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> updateUser(UserFullOutputDTO dto)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Users_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"UserID", SqlDbType.Int).Value = dto.UserID;
		    command.Parameters.Add(@"PersonID", SqlDbType.Int).Value = dto.PersonID;
		    command.Parameters.Add(@"UserName", SqlDbType.NVarChar).Value = dto.UserName;
		    command.Parameters.Add(@"PasswordHash", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.PasswordHash) ? DBNull.Value : (object)dto.PasswordHash;
		    command.Parameters.Add(@"IsActive", SqlDbType.Bit).Value = dto.IsActive;
		    command.Parameters.Add(@"UserRole", SqlDbType.Int).Value = (dto.UserRole == null) ? DBNull.Value : (object)dto.UserRole;
		    command.Parameters.Add(@"PasswordSalt", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(dto.PasswordSalt) ? DBNull.Value : (object)dto.PasswordSalt;
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> deleteUser(int UserID)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Users_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> isUserExistByID(int UserID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Users_IsExistByUserID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserID", UserID);
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
        public static async Task<List<UserFullOutputDTO>> PagingDAL(int RowsPerPage, int PageNumber, string SortColumn, string Direction)
        {
            var list = new List<UserFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Users_Paging", connection);
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
                    list.Add(new UserFullOutputDTO
                    {
        UserID = (int)reader["UserID"],
        PersonID = (int)reader["PersonID"],
        UserName = (string)reader["UserName"],
        PasswordHash = (reader["PasswordHash"] == DBNull.Value) ? "" : (string)reader["PasswordHash"],
        IsActive = (bool)reader["IsActive"],
        UserRole = (reader["UserRole"] == DBNull.Value) ? null : (int?)reader["UserRole"],
        PasswordSalt = (reader["PasswordSalt"] == DBNull.Value) ? "" : (string)reader["PasswordSalt"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<UserFullOutputDTO>> getAll()
        {
            var list = new List<UserFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Users_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new UserFullOutputDTO
                    {
        UserID = (int)reader["UserID"],
        PersonID = (int)reader["PersonID"],
        UserName = (string)reader["UserName"],
        PasswordHash = (reader["PasswordHash"] == DBNull.Value) ? "" : (string)reader["PasswordHash"],
        IsActive = (bool)reader["IsActive"],
        UserRole = (reader["UserRole"] == DBNull.Value) ? null : (int?)reader["UserRole"],
        PasswordSalt = (reader["PasswordSalt"] == DBNull.Value) ? "" : (string)reader["PasswordSalt"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}