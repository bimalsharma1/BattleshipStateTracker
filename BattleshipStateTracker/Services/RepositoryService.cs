using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Threading.Tasks;
using BattleshipStateTracker.Models;
using BattleshipStateTracker.Interfaces;

namespace AWSServerlessReact.Services
{
	public class RepositoryService: IRepositoryService
	{
		// This const is the name of the environment variable that the serverless.template will use to set
		// the name of the DynamoDB table used to store LPs.
		const string TABLENAME_ENVIRONMENT_VARIABLE_LOOKUP = "BattleshipTable";

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

		public async Task CreateBoard(string name, bool[,] board)
		{
            BattleshipBoard battleshipBoard = new BattleshipBoard
            {
                Name = name,
                Board = board
            };
            // var _lps = JsonConvert.DeserializeObject<LP>(lp);
            await DDBContext.SaveAsync<BattleshipBoard>(battleshipBoard);
			// you can add scan conditions, or leave empty
			//	var search = DDBContext.ScanAsync<BattleshipBoard>(conditions);
			//return await search.GetRemainingAsync();
		}

		public async Task AddShip(string name, int[,] position)
		{
			//foreach(BattleshipBoard bs in bss)
			//{
				// var _lps = JsonConvert.DeserializeObject<LP>(lp);
				//await DDBContext.SaveAsync<BattleshipBoard>(position);
			//}
		}
	}
}
