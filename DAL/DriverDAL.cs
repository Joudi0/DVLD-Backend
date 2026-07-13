using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class DriverDAL
    {  

        public static async Task<int> addDriver(DriverFullOutputDTO dto)
        {
            int DriverID = -1;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Drivers_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"PersonID", SqlDbType.Int).Value = dto.PersonID;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
		    command.Parameters.Add(@"CreatedDate", SqlDbType.DateTime).Value = dto.CreatedDate;
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DriverID = insertedID;
                }
            }
            catch (Exception) { throw; }
            return DriverID;
        }
        public static async Task<DriverFullOutputDTO> getDriverByID(int DriverID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Drivers_SelectByDriverID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DriverID", DriverID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new DriverFullOutputDTO
                    {
        DriverID = (int)reader["DriverID"],
        PersonID = (int)reader["PersonID"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        CreatedDate = (DateTime)reader["CreatedDate"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> updateDriver(DriverFullOutputDTO dto)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Drivers_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"DriverID", SqlDbType.Int).Value = dto.DriverID;
		    command.Parameters.Add(@"PersonID", SqlDbType.Int).Value = dto.PersonID;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
		    command.Parameters.Add(@"CreatedDate", SqlDbType.DateTime).Value = dto.CreatedDate;
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> deleteDriver(int DriverID)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Drivers_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DriverID", DriverID);
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> isDriverExistByID(int DriverID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Drivers_IsExistByDriverID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DriverID", DriverID);
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
        public static async Task<List<DriverFullOutputDTO>> PagingDAL(int RowsPerPage, int PageNumber, string SortColumn, string Direction)
        {
            var list = new List<DriverFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Drivers_Paging", connection);
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
                    list.Add(new DriverFullOutputDTO
                    {
        DriverID = (int)reader["DriverID"],
        PersonID = (int)reader["PersonID"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        CreatedDate = (DateTime)reader["CreatedDate"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<DriverFullOutputDTO>> getAll()
        {
            var list = new List<DriverFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Drivers_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new DriverFullOutputDTO
                    {
        DriverID = (int)reader["DriverID"],
        PersonID = (int)reader["PersonID"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        CreatedDate = (DateTime)reader["CreatedDate"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<DriverFullOutputDTO> getDriverByPersonID(int PersonID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Drivers_SelectByPersonID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new DriverFullOutputDTO
                    {
        DriverID = (int)reader["DriverID"],
        PersonID = (int)reader["PersonID"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        CreatedDate = (DateTime)reader["CreatedDate"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> isDriverExistByPersonID(int PersonID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Drivers_IsExistByPersonID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonID", PersonID);
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
        public static async Task<List<DriverFullOutputDTO>> getAllByCreatedByUserID(int CreatedByUserID)
        {
            var list = new List<DriverFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_Drivers_SelectAllByCreatedByUserID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new DriverFullOutputDTO
                    {
        DriverID = (int)reader["DriverID"],
        PersonID = (int)reader["PersonID"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        CreatedDate = (DateTime)reader["CreatedDate"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}