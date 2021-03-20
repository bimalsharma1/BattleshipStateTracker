using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Threading.Tasks;
using BattleshipStateTracker.Models;
using BattleshipStateTracker.Interfaces;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BattleshipStateTracker.Services
{
	public class RepositoryService: IRepositoryService
	{
		// This const is the name of the environment variable that the serverless.template will use to set
		// the name of the DynamoDB table used to store LPs.
		const string TABLENAME_ENVIRONMENT_VARIABLE_LOOKUP = "BattleshipBoard";

		public const string ID_QUERY_STRING_NAME = "Id";
		IDynamoDBContext DDBContext { get; set; }

		/// <summary>
		/// Constructor used for testing passing in a preconfigured DynamoDB client.
		/// </summary>
		/// <param name="ddbClient"></param>

		public RepositoryService ()
		{
			// Check to see if a table name was passed in through environment variables and if so 
			// add the table mapping.
			var tableName = System.Environment.GetEnvironmentVariable(TABLENAME_ENVIRONMENT_VARIABLE_LOOKUP);
			if (!string.IsNullOrEmpty(tableName))
			{
				AWSConfigsDynamoDB.Context.TypeMappings[typeof(BattleshipBoard)] = new Amazon.Util.TypeMapping(typeof(BattleshipBoard), tableName);
			}

			var config = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
			this.DDBContext = new DynamoDBContext(new AmazonDynamoDBClient(), config);
		}

		/// <summary>
		/// Constructor used for testing passing in a preconfigured DynamoDB client.
		/// </summary>
		/// <param name="ddbClient"></param>
		/// <param name="tableName"></param>
		public RepositoryService(IAmazonDynamoDB ddbClient, string tableName)
		{
			if (!string.IsNullOrEmpty(tableName))
			{
				AWSConfigsDynamoDB.Context.TypeMappings[typeof(BattleshipBoard)] = new Amazon.Util.TypeMapping(typeof(BattleshipBoard), tableName);
			}

			var config = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
			this.DDBContext = new DynamoDBContext(ddbClient, config);
		}

		public async Task CreateBoard(string name, List<Position> board)
		{
            BattleshipBoard battleshipBoard = new BattleshipBoard
            {
				Id = name,
                Board = board
            };
			Console.WriteLine("Adding to table");
			Console.WriteLine(JsonConvert.SerializeObject(battleshipBoard));
			// var _lps = JsonConvert.DeserializeObject<LP>(lp);
			try
			{
				await DDBContext.SaveAsync(battleshipBoard);
			} catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
				Console.WriteLine("After error message");
            }
		}

		public async Task<IEnumerable<BattleshipBoard>> GetBoards(string boardName)
		{
			var conditions = new List<ScanCondition>
			{
				new ScanCondition("Id", ScanOperator.Equal, boardName?.Trim())
			};
			var search = DDBContext.ScanAsync<BattleshipBoard>(conditions);

            return await search.GetRemainingAsync();
        }

		public async Task AddShip(BattleshipBoard board)
		{
			await DDBContext.SaveAsync<BattleshipBoard>(board);
		}
	}
}
