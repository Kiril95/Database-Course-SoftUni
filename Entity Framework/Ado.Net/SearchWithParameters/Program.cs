using Microsoft.Data.SqlClient;

const string ConnStr = "Server=FORTRESS;Database=Diablo;Integrated Security=true;Encrypt=False;";

using (SqlConnection connection = new SqlConnection(ConnStr))
{
    await connection.OpenAsync();

    string? name = Console.ReadLine();
    string query = @"SELECT * FROM Characters AS c
                    JOIN [Statistics] AS s ON c.StatisticId = s.Id
                    WHERE c.Name = LOWER(@className)";

    using (SqlCommand cmd = new SqlCommand(query, connection))
    {
        cmd.Parameters.AddWithValue("className", name!.ToLower());

        using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                Console.WriteLine($"Class {reader["Name"].ToString()?.ToUpper()} \n  " +
                    $"Strength - {reader["Strength"]} \n  " +
                    $"Defence - {reader["Defence"]} \n  " +
                    $"Mind - {reader["Mind"]} \n  " +
                    $"Speed - {reader["Speed"]} \n  " +
                    $"Luck - {reader["Luck"]}");
            }
        }
    }
}