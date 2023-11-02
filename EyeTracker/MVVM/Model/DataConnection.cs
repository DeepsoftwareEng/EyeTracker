using Microsoft.Data.SqlClient;

namespace EyeTracker.MVVM.Model
{
    class DataConnection
    {
        private SqlConnection dc = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = EyeTracker; Integrated Security=true; TrustServerCertificate=True");
        public SqlConnection GetConnection()
        {
            return this.dc;
        }
    }
}
