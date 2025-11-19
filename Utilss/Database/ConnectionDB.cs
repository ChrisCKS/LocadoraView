using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Database
{
    public class ConnectionDB
    {
        private static readonly string _connectionString =
            "Data Source=localhost;Initial Catalog=LocadoraBD;User ID=sa;Password=SqlServer@2022;TrustServerCertificate=True";

        public static string GetConnectionString()
        {
            return _connectionString; 
        }
    }
}
