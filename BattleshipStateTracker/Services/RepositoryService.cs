using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Threading.Tasks;
using BattleshipStateTracker.Models;
using BattleshipStateTracker.Interfaces;
using System.Collections.Generic;

namespace BattleshipStateTracker.Services
{
	public class RepositoryService: IRepositoryService
	{
		// This const is the name of the environment variable that the serverless.template will use to set
		// the name of the DynamoDB table used to store LPs.
		const string TablenameEnvironmentVariableLookup = "BattleshipBoard";

		public const string ID_QUERY_STRING_NAME = "Id";
		IDynamoDBContext DdbContext { get; set; }

		/// <summary>
		/// Constructor used for testing passing in a preconfigured DynamoDB client.
		/// </summary>
		/// <param name="ddbClient"></param>

		public RepositoryService ()
		{
			// Check to see if a table name was passed in through environment variables and if so 
			// add the table mapping.
			var tableName = System.Environment.GetEnvironmentVariable(TablenameEnvironmentVariableLookup);
			if (!string.IsNullOrEmpty(tableName))
			{
				AWSConfigsDynamoDB.Context.TypeMappings[typeof(BattleshipBoard)] = new Amazon.Util.TypeMapping(typeof(BattleshipBoard), tableName);
			}

			var config = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
			this.DdbContext = new DynamoDBContext(new AmazonDynamoDBClient(), config);
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
			this.DdbContext = new DynamoDBContext(ddbClient, config);
		}

		public async Task CreateBoard(string name, List<ShipPosition> board)
		{
            BattleshipBoard battleshipBoard = new BattleshipBoard
            {
				Id = name,
                Board = board
            };

			await DdbContext.SaveAsync(battleshipBoard);
		}

		public async Task<IEnumerable<BattleshipBoard>> GetBoards(string boardName)
		{
			var conditions = new List<ScanCondition>
			{
				new ScanCondition("Id", ScanOperator.Equal, boardName?.Trim())
			};
			var search = DdbContext.ScanAsync<BattleshipBoard>(conditions);
            return await search.GetRemainingAsync();
        }

		public async Task AddShip(BattleshipBoard board)
		{
			await DdbContext.SaveAsync<BattleshipBoard>(board);
		}
	}
}
