using Microsoft.Data.SqlClient;

const string ConnStr = "Server=FORTRESS;Database=Diablo;Integrated Security=true;Encrypt=False;";

using (SqlConnection connection = new SqlConnection(ConnStr))
{
    await connection.OpenAsync();
    string query = @"SELECT c.Id, c.Name FROM Characters AS c
                    JOIN [Statistics] AS s ON c.StatisticId = s.Id";

    using (SqlCommand cmd = new SqlCommand(query, connection))
    {
        using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                Console.WriteLine($"Id-{reader[0]} -> Class - {reader[1]}");
            }
        }
    }
}