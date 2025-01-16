using Microsoft.Data.SqlClient;

namespace WebApplication1
{
    public class Test1
    {
        public string Password = "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";
        
        public void StartHandle(string TableName, string DateTimeColumn) {
            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Testing1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")) {
                connection.Open();
                string deleteCommandText = String.Format(@"DELETE FROM [dbo].[{0}] 
                                                 WHERE [{1}] < '{2}'", TableName, DateTimeColumn,
                                                    DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss"));
                using (SqlCommand deleteCommand = new SqlCommand(deleteCommandText, connection)) {
                    deleteCommand.Parameters.AddWithValue("@RetentionDate", DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss"));
                    deleteCommand.ExecuteNonQuery();
                }
            }
        }
    }

    public class Test2 {
        public string Password = "ghp_pS8K3x72BQEE0S1fuuFgrqNptcXxYG1l8axC";
        public void StartHandle(string TableName, string DateTimeColumn) {
            Console.WriteLine("********** InDirect call StartHandle from program.cs main");
            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Testing1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")) {
                connection.Open();
                string deleteCommandText = String.Format(@"DELETE FROM [dbo].[{0}] 
                                                 WHERE [{1}] < '{2}'", TableName, DateTimeColumn,
                                                    DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss"));
                using (SqlCommand deleteCommand = new SqlCommand(deleteCommandText, connection)) {
                    deleteCommand.Parameters.AddWithValue("@RetentionDate", DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss"));
                    deleteCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
