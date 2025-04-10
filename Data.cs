using System.Data.SqlClient;

namespace prjFullStack
{
    public class Cliente
    {
        public string id { get; internal set; }
        public string nome { get; internal set; }
        public string ativo { get; internal set; }
        public DateTime data_de_cadastro { get; internal set; }
    }
    public static class Data
    {
        /*private string _connectionString { get; set; }
        public Data(string connectionString)
        {
            _connectionString = connectionString;
        }*/
        [Obsolete]
        public static List<Cliente> GetClientes(string cnnStr,string id = "")
        {
            var ret = new List<Cliente>();
            
            using (SqlConnection con = new SqlConnection(cnnStr))
            {
                string SQL = "Select id,nome,email,ativo,data_de_cadastro From Clientes";

                using (SqlCommand cmdSQL = new(SQL, con))
                {
                    con.Open();
                    //cmdSQL.Parameters.Add("@User", SqlDbType.NVarChar).Value = txtEmail.Text;
                    //cmdSQL.Parameters.Add("@Pass", SqlDbType.NVarChar).Value = txtPass.Text;
                    SqlDataReader sqlReader = cmdSQL.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        ret.Add(new Cliente() {id = sqlReader.GetString(0), nome = sqlReader.GetString(1), ativo = sqlReader.GetString(2) , data_de_cadastro = (DateTime)sqlReader.GetSqlDateTime(3) });
                    }

                }
            }

            return ret;
        }
        
    }
}
