using Microsoft.Data.SqlClient;

const string ConnStr = "Server=FORTRESS;Database=SoftUni;Integrated Security=true;Encrypt=False;";

using (SqlConnection connection = new SqlConnection(ConnStr))
{
    await connection.OpenAsync();

    using (SqlCommand cmd = new SqlCommand("SELECT FirstName, LastName FROM Employees", connection))
    {
        using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                //Console.WriteLine($"Hello my name is {reader["FirstName"]} {reader["LastName"]}");
                Console.WriteLine($"Hello my name is {reader[0]} {reader[1]}");
            }
        }
    }
}