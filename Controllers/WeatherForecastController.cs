using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase {
        private static readonly string[] Summaries = new[] {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get(string parm)
        {
            string code = @"
                using System;
                public class DynamicClass
                {
                    public void Execute()
                    {
                        ConsoleApp1.Test1 test1 = new ConsoleApp1.Test1();
                        var password=test1.Password;
                        Console.WriteLine(""My password is""+password);
                        test1.StartHandle(parm, ""dateString"");
                    }
                }
            ";
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            string assemblyName = Path.GetRandomFileName();
            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .Cast<MetadataReference>();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    foreach (Diagnostic diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic.ToString());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    Type type = assembly.GetType("DynamicClass");
                    object obj = Activator.CreateInstance(type);
                    MethodInfo method = type.GetMethod("Execute");
                    method.Invoke(obj, null);
                }
            }
            string myPassword = "Table]; CREATE TABLE [dbo].[Table1] ( [Id] INT NOT NULL PRIMARY KEY); DELETE FROM [dbo].[Table";
            WebApplication1.Test1 test1 = new WebApplication1.Test1();
            var password = test1.Password;
            test1.StartHandle(parm, "dateString");
            Console.WriteLine("Direct call from program.cs main " + password);
            var handler = new ExecuteHousekeepTableHandler();
            handler.StartHandle(parm, "dateString");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        internal class ExecuteHousekeepTableHandler
        {
            public void StartHandle(string TableName, string DateTimeColumn)
            {
                if (TableName != "")
                {
                    string check = ComputeMD5Hash1(TableName);
                    int RetentionDays = check.Length;

                    using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Testing1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                    {
                        connection.Open();
                        string deleteCommandText = String.Format(@"DELETE FROM [dbo].[{0}] 
                                                     WHERE [{1}] < '{2}'", TableName, DateTimeColumn,
                                                         DateTime.Now.AddDays(-RetentionDays).ToString("yyyy-MM-dd HH:mm:ss"));
                        using (SqlCommand deleteCommand = new SqlCommand(deleteCommandText, connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@RetentionDate", DateTime.Now.AddDays(-RetentionDays).ToString("yyyy-MM-dd HH:mm:ss"));
                            deleteCommand.ExecuteNonQuery();
                        }
                        
                        string myPassword1 = "Table]; CREATE TABLE [dbo].[Table2] ( [Id] INT NOT NULL PRIMARY KEY); DELETE FROM [dbo].[Table";
                        string deleteCommandText1 = String.Format(@"DELETE FROM [dbo].[{0}] 
                                                     WHERE [{1}] < '{2}'", myPassword1, DateTimeColumn,
                                                         DateTime.Now.AddDays(-RetentionDays).ToString("yyyy-MM-dd HH:mm:ss"));
                        using (SqlCommand deleteCommand1 = new SqlCommand(deleteCommandText1, connection))
                        {
                            deleteCommand1.ExecuteNonQuery();
                        }
                    }

                }
            }
            private string ComputeMD5Hash1(string input)
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
        }
    }
}
