using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;


namespace BattleshipStateTracker.Tests
{
    public class BattleshipStateTrackerControllerTests
    {
        readonly Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" }
        };

        [Fact]
        public async Task TestGet()
        {
            var lambdaFunction = new LambdaEntryPoint();

            var requestStr = File.ReadAllText("./SampleRequests/BattleshipStateTrackerController-Get.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal("[\"This is the Battleship State Tracker\",\"You can call APIs to create a board, add a ship and attack\"]", response.Body);
            Assert.True(response.MultiValueHeaders.ContainsKey("Content-Type"));
            Assert.Equal("application/json; charset=utf-8", response.MultiValueHeaders["Content-Type"][0]);
        }

        [Fact]
        public async Task TestCreateBoard()
        {
            var lambdaFunction = new LambdaEntryPoint();
            var requestStr = File.ReadAllText("./SampleRequests/BattleshipStateTrackerController-CreateBoard.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            request.Headers = headers;
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal("success", response.Body);
            Assert.True(response.MultiValueHeaders.ContainsKey("Content-Type"));
            Assert.Equal("text/plain; charset=utf-8", response.MultiValueHeaders["Content-Type"][0]);
        }

        [Fact]
        public async Task TestAddShip()
        {
            var lambdaFunction = new LambdaEntryPoint();
            var requestStr = File.ReadAllText("./SampleRequests/BattleshipStateTrackerController-AddShip.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            request.Headers = headers;
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal("success", response.Body);
            Assert.True(response.MultiValueHeaders.ContainsKey("Content-Type"));
            Assert.Equal("text/plain; charset=utf-8", response.MultiValueHeaders["Content-Type"][0]);
        }

        [Fact]
        public async Task TestAttackHit()
        {
            var lambdaFunction = new LambdaEntryPoint();
            var requestStr = File.ReadAllText("./SampleRequests/BattleshipStateTrackerController-Attack.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            request.Headers = headers;
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal("Hit", response.Body);
            Assert.True(response.MultiValueHeaders.ContainsKey("Content-Type"));
            Assert.Equal("text/plain; charset=utf-8", response.MultiValueHeaders["Content-Type"][0]);
        }

        [Fact]
        public async Task TestAttackMiss()
        {
            var lambdaFunction = new LambdaEntryPoint();
            var requestStr = File.ReadAllText("./SampleRequests/BattleshipStateTrackerController-Attack.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            request.Body = "{\"boardName\":\"Bimal1\",\"attackPosition\":{\"xPosition\":4,\"yPosition\":7}}";
            request.Headers = headers;
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal("Miss", response.Body);
            Assert.True(response.MultiValueHeaders.ContainsKey("Content-Type"));
            Assert.Equal("text/plain; charset=utf-8", response.MultiValueHeaders["Content-Type"][0]);
        }

        [Fact]
        public async Task TestAttackOutsideBoard()
        {
            var lambdaFunction = new LambdaEntryPoint();
            var requestStr = File.ReadAllText("./SampleRequests/BattleshipStateTrackerController-Attack.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            request.Body = "{\"boardName\":\"Bimal1\",\"attackPosition\":{\"xPosition\":14,\"yPosition\":7}}";
            request.Headers = headers;
            var context = new TestLambdaContext();
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal("Invalid attack position, please try again", response.Body);
            Assert.True(response.MultiValueHeaders.ContainsKey("Content-Type"));
            Assert.Equal("text/plain; charset=utf-8", response.MultiValueHeaders["Content-Type"][0]);
        }
    }
}
