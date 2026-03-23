using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdavnicaObuce_IT34_2024
{
    public class Konekcija
    {
        public SqlConnection kreirajKonekciju()
        {
            SqlConnectionStringBuilder ccnsb = new SqlConnectionStringBuilder
            {
                DataSource = @"DELL-VOSTRO-531\SQLEXPRESS",
                InitialCatalog = "ProdavnicaObuce_IT34/2024",
                IntegratedSecurity = true
            };

            string con = ccnsb.ToString();
            SqlConnection konekcija = new SqlConnection(con);
            return konekcija;
        }
    }
}
