using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App
{
    public class Program
    {
        public static void Main()
        {
            var connectionString = GetConnectionString();
            bool useConsoleLogger = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            using (var context = new SchoolContext(connectionString, useConsoleLogger))
            {
                Student student = context.Students.Include(x => x.FavoriteCourse).FirstOrDefault();
            }

            Console.ReadKey();
        }


        private static string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration["ConnectionString"];
        }
    }
}
