using Microsoft.Data.SqlClient;

const string ConnStr = "Server=FORTRESS;Database=Diablo;Integrated Security=true;Encrypt=False;";

using (SqlConnection connection = new SqlConnection(ConnStr))
{
    await connection.OpenAsync();
    string query = @"SELECT TOP 3 c.Name, s.Strength, s.Defence, s.Mind, s.Speed, s.Luck FROM Characters AS c
                    JOIN [Statistics] AS s ON c.StatisticId = s.Id
                    ORDER BY s.Strength DESC";

    using (SqlCommand cmd = new SqlCommand(query, connection))
    {
        using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                Console.WriteLine($"Class {reader["Name"]} - {reader["Strength"]}");
            }
        }
    }
}