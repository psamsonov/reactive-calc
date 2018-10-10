using System;
using FluentAssertions;
using Xunit;

namespace ReactiveCalculator.Tests
{
    public class CalculatorUnitTests
    {

        private MainViewModel viewModel;

        public CalculatorUnitTests()
        {
            viewModel = new MainViewModel();
            
        }

        [Theory]
        [InlineData( new string []{
            "1","2","3"
        }, "123")]
        [InlineData(new string[]{
            "2","3","4"
        }, "234")]
        [InlineData(new string[]{
            "0","0","0"
        }, "0")]

        public void AppendTest(string[] number, string totalNumber)
        {
            foreach (var n in number)
            {
                viewModel.Append.Execute(n);
            }

            viewModel.Number.Should().BeEquivalentTo(totalNumber);
        }

        [Fact]
        public void BackspaceTest()
        {
            viewModel.Number.Should().BeEquivalentTo("0");
            viewModel.Backspace.Execute(null);
            viewModel.Number.Should().BeEquivalentTo("0");
            viewModel.Append.Execute("1");
            viewModel.Number.Should().NotBe("0");
            viewModel.Backspace.Execute(null);
            viewModel.Number.Should().Be("0");
        }


        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("99")]
        [InlineData("0")]
        public void NumberEqualToItselfTest(string number)
        {
            viewModel.Append.Execute(number);
            
            viewModel.Calculate.Execute(null);

            viewModel.Number.Should().Be(number);
        }

        [Theory]
        [InlineData(new string[]{"1", "2","3"}, "6")]
        public void AdditionTest(string[] numbers, string total)
        {
            foreach (var n in numbers)
            {
                viewModel.Append.Execute(n);
                viewModel.Add.Execute(null);
            }

            viewModel.Calculate.Execute(null);

            viewModel.Number.Should().Be(total);
        }

    }
}
