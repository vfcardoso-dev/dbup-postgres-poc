using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace DbUpPostgresPoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbManager = new PgDbManager();
            
            Console.WriteLine("Starting PgSQL POC!");

            dbManager.Execute($"DROP TABLE IF EXISTS Inventory");
            
            dbManager.Execute($"CREATE TABLE Inventory(Id UUID, Name VARCHAR(50), Quantity INTEGER)");
            
            var items = new List<Tuple<string, NpgsqlDbType, object>>();
            items.Add(new Tuple<string, NpgsqlDbType, object>("i1", NpgsqlDbType.Uuid, Guid.NewGuid()));
            items.Add(new Tuple<string, NpgsqlDbType, object>("n1", NpgsqlDbType.Varchar, "banana"));
            items.Add(new Tuple<string, NpgsqlDbType, object>("q1", NpgsqlDbType.Integer, 150));
            items.Add(new Tuple<string, NpgsqlDbType, object>("i2", NpgsqlDbType.Uuid, Guid.NewGuid()));
            items.Add(new Tuple<string, NpgsqlDbType, object>("n2", NpgsqlDbType.Varchar, "orange"));
            items.Add(new Tuple<string, NpgsqlDbType, object>("q2", NpgsqlDbType.Integer, 75));
            items.Add(new Tuple<string, NpgsqlDbType, object>("i3", NpgsqlDbType.Uuid, Guid.NewGuid()));
            items.Add(new Tuple<string, NpgsqlDbType, object>("n3", NpgsqlDbType.Varchar, "apple"));
            items.Add(new Tuple<string, NpgsqlDbType, object>("q3", NpgsqlDbType.Integer, 12));
            
            dbManager.Execute($"INSERT INTO inventory (Id, Name, Quantity) VALUES (@i1, @n1, @q1), (@i2, @n2, @q2), (@i3, @n3, @q3)", items);
            
            //Disposing connection
            dbManager.Dispose();
            
            // Running migration
            dbManager.RunMigrations();

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }
    }
}
