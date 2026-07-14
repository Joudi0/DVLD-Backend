using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class BLLSettings
    {
        public static void Initialize(string connStr)
        {
            DAL.DataSettings.connectionString = connStr;
        }
    }
}
