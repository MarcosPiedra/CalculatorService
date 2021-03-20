using CalculatorServices.WebAPI.DTOs;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CalculatorServices.WebAPI.Tests.Unit
{
    public class CalculatorServiceTest : CalculatorServiceWebApi
    {
        [Theory]
        [InlineData(10, 2, 5, 0)]
        [InlineData(11, 2, 5, 1)]
        public async Task Should_OperateCorrectly_When_Divide(int dividend, int divisor, int quotient, int remainder)
        {
            var server = await GetServer();
            var client = server.GetTestClient();
            var toSend = new DivRequest() { Dividend = dividend, Divisor = divisor };
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync($"api/v1/calculator/div", content);
            var responseContetAsString = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<DivResponse>(responseContetAsString);

            Assert.True(httpResponse.StatusCode == HttpStatusCode.OK);
            Assert.Equal(quotient, response.Quotient);
            Assert.Equal(remainder, response.Remainder);
        }

        [Theory]
        [InlineData(0, 0)]
        public async Task Should_ReportBadRequest_When_Divide(int dividend, int divisor)
        {
            var server = await GetServer();
            var client = server.GetTestClient();
            var toSend = new DivRequest() { Dividend = dividend, Divisor = divisor };
            var content = new StringContent(JsonConvert.SerializeObject(toSend), Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync($"api/v1/calculator/div", content);

            Assert.True(httpResponse.StatusCode == HttpStatusCode.BadRequest);
        }

        //...
    }
}

