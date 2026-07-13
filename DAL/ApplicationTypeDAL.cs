using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Shared;

namespace DAL
{
    public class ApplicationTypeDAL
    {  

        public static async Task<ApplicationTypeFullOutputDTO> getApplicationTypeByID(int ApplicationTypeID)
        {
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_ApplicationTypes_SelectByApplicationTypeID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new ApplicationTypeFullOutputDTO
                    {
        ApplicationTypeID = (int)reader["ApplicationTypeID"],
        ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"],
        ApplicationFees = (decimal)reader["ApplicationFees"]
                    };
                }
            }
            catch (Exception) { throw; }
            return null;
        }
        public static async Task<List<ApplicationTypeFullOutputDTO>> getAll()
        {
            var list = new List<ApplicationTypeFullOutputDTO>();
            using SqlConnection connection = new SqlConnection(DataSettings.connectionString);
            using SqlCommand command = new SqlCommand("SP_ApplicationTypes_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new ApplicationTypeFullOutputDTO
                    {
        ApplicationTypeID = (int)reader["ApplicationTypeID"],
        ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"],
        ApplicationFees = (decimal)reader["ApplicationFees"]
                    });
                }
            }
            catch (Exception) { throw; }
            return list;
        }  
    }
}