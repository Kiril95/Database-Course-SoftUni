using Microsoft.Data.SqlClient;

// With the new version 4.0, the 'Encrypt' property's default value was set from False to True

//const string ConnStr = "Server=FORTRESS;Database=SoftUni;User Id=perko;Password=123456;Encrypt=False;";
//const string ConnStr = "Server=FORTRESS;Database=SoftUni;User Id=perko;Password=123456;TrustServerCertificate=True;";
const string ConnStr = "Server=FORTRESS;Database=SoftUni;Integrated Security=true;TrustServerCertificate=True;";
SqlConnection connection = new SqlConnection(ConnStr);

using (connection)
{
    connection.Open();
    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Employees", connection);

    using (cmd)
    {
        int result = (int)cmd.ExecuteScalar();
        Console.WriteLine(result);
    }
}