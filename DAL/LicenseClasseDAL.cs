using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class LicenseClasseDAL
    {  

        public static async Task<LicenseClasseFullOutputDTO> getLicenseClasseByID(int LicenseClassID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LicenseClasses_SelectByLicenseClassID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new LicenseClasseFullOutputDTO
                    {
        LicenseClassID = (int)reader["LicenseClassID"],
        ClassName = (string)reader["ClassName"],
        ClassDescription = (string)reader["ClassDescription"],
        MinimumAllowedAge = (byte)reader["MinimumAllowedAge"],
        DefaultValidityLength = (byte)reader["DefaultValidityLength"],
        ClassFees = (decimal)reader["ClassFees"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<List<LicenseClasseFullOutputDTO>> getAll()
        {
            var list = new List<LicenseClasseFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_LicenseClasses_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new LicenseClasseFullOutputDTO
                    {
        LicenseClassID = (int)reader["LicenseClassID"],
        ClassName = (string)reader["ClassName"],
        ClassDescription = (string)reader["ClassDescription"],
        MinimumAllowedAge = (byte)reader["MinimumAllowedAge"],
        DefaultValidityLength = (byte)reader["DefaultValidityLength"],
        ClassFees = (decimal)reader["ClassFees"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}