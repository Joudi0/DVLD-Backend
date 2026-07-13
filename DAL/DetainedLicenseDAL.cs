using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class DetainedLicenseDAL
    {  

        public static async Task<int> addDetainedLicense(DetainedLicenseFullOutputDTO dto)
        {
            int DetainedLicenseID = -1;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_DetainedLicenses_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"LicenseID", SqlDbType.Int).Value = dto.LicenseID;
		    command.Parameters.Add(@"DetainDate", SqlDbType.DateTime).Value = dto.DetainDate;
		    command.Parameters.Add(@"FineFees", SqlDbType.Decimal).Value = dto.FineFees;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
		    command.Parameters.Add(@"IsReleased", SqlDbType.Bit).Value = dto.IsReleased;
		    command.Parameters.Add(@"ReleaseDate", SqlDbType.DateTime).Value = (dto.ReleaseDate == null) ? DBNull.Value : (object)dto.ReleaseDate;
		    command.Parameters.Add(@"ReleasedByUserID", SqlDbType.Int).Value = (dto.ReleasedByUserID == null) ? DBNull.Value : (object)dto.ReleasedByUserID;
		    command.Parameters.Add(@"ReleaseApplicationID", SqlDbType.Int).Value = (dto.ReleaseApplicationID == null) ? DBNull.Value : (object)dto.ReleaseApplicationID;
            try
            {
                await connection.OpenAsync();
                object result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DetainedLicenseID = insertedID;
                }
            }
            catch (Exception) { throw; }
            return DetainedLicenseID;
        }
        public static async Task<DetainedLicenseFullOutputDTO> getDetainedLicenseByID(int DetainID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_DetainedLicenses_SelectByDetainID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DetainID", DetainID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new DetainedLicenseFullOutputDTO
                    {
        DetainID = (int)reader["DetainID"],
        LicenseID = (int)reader["LicenseID"],
        DetainDate = (DateTime)reader["DetainDate"],
        FineFees = (decimal)reader["FineFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsReleased = (bool)reader["IsReleased"],
        ReleaseDate = (reader["ReleaseDate"] == DBNull.Value) ? null : (DateTime?)reader["ReleaseDate"],
        ReleasedByUserID = (reader["ReleasedByUserID"] == DBNull.Value) ? null : (int?)reader["ReleasedByUserID"],
        ReleaseApplicationID = (reader["ReleaseApplicationID"] == DBNull.Value) ? null : (int?)reader["ReleaseApplicationID"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<bool> updateDetainedLicense(DetainedLicenseFullOutputDTO dto)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_DetainedLicenses_Update", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(@"DetainID", SqlDbType.Int).Value = dto.DetainID;
		    command.Parameters.Add(@"LicenseID", SqlDbType.Int).Value = dto.LicenseID;
		    command.Parameters.Add(@"DetainDate", SqlDbType.DateTime).Value = dto.DetainDate;
		    command.Parameters.Add(@"FineFees", SqlDbType.Decimal).Value = dto.FineFees;
		    command.Parameters.Add(@"CreatedByUserID", SqlDbType.Int).Value = dto.CreatedByUserID;
		    command.Parameters.Add(@"IsReleased", SqlDbType.Bit).Value = dto.IsReleased;
		    command.Parameters.Add(@"ReleaseDate", SqlDbType.DateTime).Value = (dto.ReleaseDate == null) ? DBNull.Value : (object)dto.ReleaseDate;
		    command.Parameters.Add(@"ReleasedByUserID", SqlDbType.Int).Value = (dto.ReleasedByUserID == null) ? DBNull.Value : (object)dto.ReleasedByUserID;
		    command.Parameters.Add(@"ReleaseApplicationID", SqlDbType.Int).Value = (dto.ReleaseApplicationID == null) ? DBNull.Value : (object)dto.ReleaseApplicationID;
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> deleteDetainedLicense(int DetainID)
        {
            int rowsAffected = 0;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_DetainedLicenses_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DetainID", DetainID);
            try
            {
                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            return (rowsAffected > 0);
        }
        public static async Task<bool> isDetainedLicenseExistByID(int DetainID)
        {
            bool isFound = false;
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_DetainedLicenses_IsExistByDetainID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DetainID", DetainID);
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
        public static async Task<List<DetainedLicenseFullOutputDTO>> PagingDAL(int RowsPerPage, int PageNumber, string SortColumn, string Direction)
        {
            var list = new List<DetainedLicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_DetainedLicenses_Paging", connection);
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
                    list.Add(new DetainedLicenseFullOutputDTO
                    {
        DetainID = (int)reader["DetainID"],
        LicenseID = (int)reader["LicenseID"],
        DetainDate = (DateTime)reader["DetainDate"],
        FineFees = (decimal)reader["FineFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsReleased = (bool)reader["IsReleased"],
        ReleaseDate = (reader["ReleaseDate"] == DBNull.Value) ? null : (DateTime?)reader["ReleaseDate"],
        ReleasedByUserID = (reader["ReleasedByUserID"] == DBNull.Value) ? null : (int?)reader["ReleasedByUserID"],
        ReleaseApplicationID = (reader["ReleaseApplicationID"] == DBNull.Value) ? null : (int?)reader["ReleaseApplicationID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<DetainedLicenseFullOutputDTO>> getAll()
        {
            var list = new List<DetainedLicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_DetainedLicenses_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new DetainedLicenseFullOutputDTO
                    {
        DetainID = (int)reader["DetainID"],
        LicenseID = (int)reader["LicenseID"],
        DetainDate = (DateTime)reader["DetainDate"],
        FineFees = (decimal)reader["FineFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsReleased = (bool)reader["IsReleased"],
        ReleaseDate = (reader["ReleaseDate"] == DBNull.Value) ? null : (DateTime?)reader["ReleaseDate"],
        ReleasedByUserID = (reader["ReleasedByUserID"] == DBNull.Value) ? null : (int?)reader["ReleasedByUserID"],
        ReleaseApplicationID = (reader["ReleaseApplicationID"] == DBNull.Value) ? null : (int?)reader["ReleaseApplicationID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<DetainedLicenseFullOutputDTO>> getAllByLicenseID(int LicenseID)
        {
            var list = new List<DetainedLicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_DetainedLicenses_SelectAllByLicenseID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new DetainedLicenseFullOutputDTO
                    {
        DetainID = (int)reader["DetainID"],
        LicenseID = (int)reader["LicenseID"],
        DetainDate = (DateTime)reader["DetainDate"],
        FineFees = (decimal)reader["FineFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsReleased = (bool)reader["IsReleased"],
        ReleaseDate = (reader["ReleaseDate"] == DBNull.Value) ? null : (DateTime?)reader["ReleaseDate"],
        ReleasedByUserID = (reader["ReleasedByUserID"] == DBNull.Value) ? null : (int?)reader["ReleasedByUserID"],
        ReleaseApplicationID = (reader["ReleaseApplicationID"] == DBNull.Value) ? null : (int?)reader["ReleaseApplicationID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<DetainedLicenseFullOutputDTO>> getAllByCreatedByUserID(int CreatedByUserID)
        {
            var list = new List<DetainedLicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_DetainedLicenses_SelectAllByCreatedByUserID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new DetainedLicenseFullOutputDTO
                    {
        DetainID = (int)reader["DetainID"],
        LicenseID = (int)reader["LicenseID"],
        DetainDate = (DateTime)reader["DetainDate"],
        FineFees = (decimal)reader["FineFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsReleased = (bool)reader["IsReleased"],
        ReleaseDate = (reader["ReleaseDate"] == DBNull.Value) ? null : (DateTime?)reader["ReleaseDate"],
        ReleasedByUserID = (reader["ReleasedByUserID"] == DBNull.Value) ? null : (int?)reader["ReleasedByUserID"],
        ReleaseApplicationID = (reader["ReleaseApplicationID"] == DBNull.Value) ? null : (int?)reader["ReleaseApplicationID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }
        public static async Task<List<DetainedLicenseFullOutputDTO>> getAllByReleasedByUserID(int? ReleasedByUserID)
        {
            var list = new List<DetainedLicenseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_DetainedLicenses_SelectAllByReleasedByUserID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new DetainedLicenseFullOutputDTO
                    {
        DetainID = (int)reader["DetainID"],
        LicenseID = (int)reader["LicenseID"],
        DetainDate = (DateTime)reader["DetainDate"],
        FineFees = (decimal)reader["FineFees"],
        CreatedByUserID = (int)reader["CreatedByUserID"],
        IsReleased = (bool)reader["IsReleased"],
        ReleaseDate = (reader["ReleaseDate"] == DBNull.Value) ? null : (DateTime?)reader["ReleaseDate"],
        ReleasedByUserID = (reader["ReleasedByUserID"] == DBNull.Value) ? null : (int?)reader["ReleasedByUserID"],
        ReleaseApplicationID = (reader["ReleaseApplicationID"] == DBNull.Value) ? null : (int?)reader["ReleaseApplicationID"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}