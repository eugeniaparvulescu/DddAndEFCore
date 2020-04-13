using System;
using System.IO;
using App.Controllers;
using Microsoft.Extensions.Configuration;

namespace App
{
    public class Program
    {
        private static bool _useConsoleLogger;

        public static void Main()
        {
            //var disenrollResult = Execute(x => x.DisenrollStudent(1L, 2L));
            //var enrollResult = Execute(x => x.EnrollStudent(1L, 2L, Grade.A));
            var result1 = Execute(x => x.CheckStudentFavoriteCourse(1L, 1L));
            //var result2 = Execute(x => x.CheckStudentFavoriteCourse(1L, 2L));

            //var registerResult = Execute(x => x.RegisterStudent("Geni", "email", 1, Grade.A));
            var editResult = Execute(x => x.EditPeronalInformation(3, "Geni", "boss", 2L, "email.3@gmail.com", 2));

            Console.ReadKey();
        }

        public static string Execute(Func<StudentsController, string> func)
        {
            var connectionString = GetConnectionString();
            using (var context = new SchoolDbContext(connectionString, _useConsoleLogger))
            {
                var controller = new StudentsController(context, new StudentRepository(context));
                return func(controller);
            }
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
