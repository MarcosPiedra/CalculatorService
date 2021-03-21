using CalculatorService.Domain.Models;
using CalculatorService.Domain.Operation;
using CalculatorService.Domain;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using CalculatorService.CrossCutting;

namespace CalculatorServices.WebAPI.Tests.Unit
{
    public class CalculatorServiceTest
    {
        [Theory]
        [InlineData(10, 2, 5, 0)]
        [InlineData(11, 2, 5, 1)]
        public async Task Should_Divide_When_Invoke(int dividend, int divisor, int quotient, int remainder)
        {
            var sut = new DivService();
            var parameters = new DivParams()
            {
                Dividend = dividend,
                Divisor = divisor,
            };

            var result = await sut.Execute(parameters);

            Assert.Equal(quotient, result.Quotient);
            Assert.Equal(remainder, result.Remainder);
        }

        [Theory]
        [InlineData(0, 0)]
        public async Task DivideByZero_Then_WillFail(int dividend, int divisor)
        {
            var sut = new DivService();
            var parameters = new DivParams()
            {
                Dividend = dividend,
                Divisor = divisor,
            };

            await Assert.ThrowsAnyAsync<DivideByZeroException>(() => sut.Execute(parameters));
        }

        [Fact]
        public async Task Should_Tracked_Divide_When_Invoke()
        {
            var spyList = new List<Operation>();
            var stubDb = new Mock<IRepository<Operation>>();
            stubDb.Setup(x => x.Add(It.IsAny<Operation>())).Callback<Operation>(o => spyList.Add(o)).Returns(Task.CompletedTask);
            stubDb.Setup(x => x.Query).Returns(spyList.AsQueryable());

            var timeProviderFake = new Mock<ITimeProvider>();
            timeProviderFake.Setup(x => x.GetNow()).Returns(DateTime.Now);
            var divService = new DivService();
            var divParams = new DivParams() { Dividend = 10, Divisor = 2 };
            var trakedId = 1;

            var sut = new ServiceTracked<DivParams, DivResult>(divService, trakedId, timeProviderFake.Object, stubDb.Object);
            var result = await sut.Execute(divParams);

            stubDb.Verify(x => x.Add(It.IsAny<Operation>()), Times.Once);
            Assert.Single(spyList);
            Assert.Equal(spyList[0].TrackId, trakedId);
            Assert.Equal(spyList[0].Calculation, divService.GetDescription(divParams, result));
            Assert.Equal(spyList[0].OperationType, divService.GetServiceName());
        }

        //...
    }
}

