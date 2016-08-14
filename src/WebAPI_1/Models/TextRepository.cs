using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using System.Net.Http;
using Microsoft.CodeAnalysis;


namespace WebApi.Models
{
    public class TextRepository : ITextRepository
    {
        private readonly string _connectionString;
        private static readonly ConcurrentDictionary<string, Text> Texts = new ConcurrentDictionary<string, Text>();

        public TextRepository()
        {
            //ConnectionString = new ConfigurationBuilder().Build().GetConnectionString("LocalConnection");
            _connectionString = @"data source=(localdb)\MSSQLLocalDB;Initial Catalog=APIDebugDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
        }

        public async Task<dynamic> Create(Text newText)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    // Convert NewText to sql.
                    sqlConnection.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Key", newText.Key); // set up parameters.
                    param.Add("@Author", newText.Author); // set up parameters.
                    param.Add("@PubDate", newText.PubDate); // set up parameters.
                    param.Add("@Title", newText.Title); // set up parameters.
                    dynamic response = await sqlConnection.QueryAsync<Text>("CreateProcedure", param,
                        commandType: CommandType.StoredProcedure);  // should return a single row result of the new created recored.
                    return response.Count == 1 ? response : HttpStatusCode.BadRequest;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return null;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        // use procedure.
        public async Task<dynamic> GetAll()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    dynamic rows = await sqlConnection.QueryAsync<dynamic>("GetAllProcedure");
                    return rows ?? HttpStatusCode.NotFound;   // returns List<Ditionary<Text>>.
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return HttpStatusCode.InternalServerError;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public async Task<dynamic> GetByKey(string key)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@key", key); // set up parameters.
                    dynamic rows =
                        await
                            sqlConnection.QueryAsync<Text>("GetProcedure", param,
                                commandType: CommandType.StoredProcedure);
                    return rows.Count == 0 ? HttpStatusCode.NotFound : rows;   // returns a List of Result.
                    // example: [{"Key":"2","Title":"Title 2","PubDate":"2016-06-23T11:11:46.353","Author":"Author 1"}]

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return HttpStatusCode.InternalServerError;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public Text Remove(string key)
        {
            Text text;
            Texts.TryGetValue(key, out text);
            Texts.TryRemove(key, out text);
            return text;
        }

        public async Task<dynamic> Update(Text text)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    // Convert NewText to sql.
                    sqlConnection.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Key", text.Key); // set up parameters.
                    param.Add("@Author", text.Author); // set up parameters.
                    param.Add("@PubDate", text.PubDate); // set up parameters.
                    param.Add("@Title", text.Title); // set up parameters.
                    dynamic response = await sqlConnection.QueryAsync<Text>("UpdateProcedure", param,
                        commandType: CommandType.StoredProcedure);
                    //await
                    //    sqlConnection.ExecuteAsync("CreateProcedure", NewText, commandType: CommandType.StoredProcedure);


                    Console.WriteLine("Create Response is:" + response);
                    return await response.Count != 1 ? HttpStatusCode.BadRequest: HttpStatusCode.NoContent;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return  HttpStatusCode.InternalServerError;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
    }
}
