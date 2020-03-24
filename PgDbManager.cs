using System;
using System.Collections.Generic;
using System.Reflection;
using DbUp;
using Npgsql;
using NpgsqlTypes;

namespace DbUpPostgresPoc
{
    public class PgDbManager
    {
        private const string Host = "localhost";
        private const string User = "postgres";
        private const string DBname = "PocDbUp";
        private const string Password = "postgres";
        private const string Port = "5432";

        private static NpgsqlConnection _conn;
        private static string ConnectionString => $"Server={Host};Username={User};Database={DBname};Port={Port};Password={Password};SSLMode=Prefer";

        private NpgsqlConnection GetConnection()
        {
            if (_conn == null) _conn = new NpgsqlConnection(ConnectionString);

            return _conn; 
        }

        public void Dispose()
        {
            Console.WriteLine("Closing connection to database...");
            
            GetConnection().Dispose();
            
            Console.WriteLine("Connection closed.");
            
        }

        public void Execute(string cmd)
        {
            GetConnection().Open();
            
            using var command = new NpgsqlCommand(cmd, GetConnection());
            command.ExecuteNonQuery();
            Console.WriteLine($"Command {cmd} executed");
            
            GetConnection().Close();
        }
        
        public void Execute(string cmd, IEnumerable<Tuple<string, NpgsqlDbType, object>> @params)
        {
            GetConnection().Open();
            
            using var command = new NpgsqlCommand(cmd, GetConnection());

            foreach (var item in @params)
            {
                command.Parameters.AddWithValue(item.Item1, item.Item2, item.Item3);
            }
            
            command.ExecuteNonQuery();
            Console.WriteLine($"Command {cmd} executed");
            
            GetConnection().Close();
        }

        public void RunMigrations()
        {
            Console.WriteLine();
            Console.WriteLine($"About to run migrations...");
            
            var upgrader = DeployChanges.To
                .PostgresqlDatabase(ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful) Console.WriteLine(result.Error);
            
            Console.WriteLine();
            Console.WriteLine($"Migrations runned successfully!");
        }
    }
}