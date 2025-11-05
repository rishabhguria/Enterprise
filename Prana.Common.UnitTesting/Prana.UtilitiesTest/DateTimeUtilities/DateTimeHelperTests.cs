using Prana.Utilities.DateTimeUtilities;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.DateTimeUtilities
{
    public class DateTimeHelperTests
    {
        private readonly ITestOutputHelper outputHelper;
        public DateTimeHelperTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            outputHelper.WriteLine("Test case for DateTimeHelper Started");
        }

        [Fact]
        [Trait("Prana.Utilities", "DateTimeHelper")]
        public void GetYesterdayDate_TodayIsMonday_ReturnsFriday()
        {
            DateTime today = DateTime.Today;


            if (today.DayOfWeek == DayOfWeek.Monday)
            {
                DayOfWeek ExpectedDayOfWeek = today.AddDays(-3).DayOfWeek;

                outputHelper.WriteLine($"Today Date {today.ToString("yyyy-MM-dd")} and {today.DayOfWeek}");
                DateTime ActualDate = DateTimeHelper.GetYesterdayDate();
                DayOfWeek ActualDayOfWeek = ActualDate.DayOfWeek;
                outputHelper.WriteLine($"Yesterday Date {ActualDate.ToString("yyyy-MM-dd")} and {ActualDayOfWeek}");

                outputHelper.WriteLine($"Actual ({ActualDayOfWeek}) === Expected ({ExpectedDayOfWeek})");
                Assert.True(ExpectedDayOfWeek.Equals(ActualDayOfWeek));
            }
            else
            {
                outputHelper.WriteLine($"Today is not Monday so testcase skipped");
                Assert.True(true);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "DateTimeHelper")]
        public void GetYesterdayDate_TodayIsNotMonday_ReturnsPreviousDay()
        {
            DateTime today = DateTime.Today;

            if (today.DayOfWeek != DayOfWeek.Monday)
            {
                DayOfWeek ExpectedDayOfWeek = today.AddDays(-1).DayOfWeek;

                outputHelper.WriteLine($"Today Date {today.ToString("yyyy-MM-dd")} and {today.DayOfWeek}");
                DateTime ActualDate = DateTimeHelper.GetYesterdayDate();
                DayOfWeek ActualDayOfWeek = ActualDate.DayOfWeek;
                outputHelper.WriteLine($"Yesterday Date {ActualDate.ToString("yyyy-MM-dd")} and {ActualDayOfWeek}");

                outputHelper.WriteLine($"Actual ({ActualDayOfWeek}) === Expected ({ExpectedDayOfWeek})");
                Assert.True(ExpectedDayOfWeek.Equals(ActualDayOfWeek));
            }
            else
            {
                outputHelper.WriteLine($"Today is Monday so testcase skipped");
                Assert.True(true);
            }
        }

        [Theory]
        [InlineData(1, DayOfWeek.Sunday, 2024, 3, "03/03/24 00:00:00")]
        [InlineData(2, DayOfWeek.Monday, 2024, 3, "03/11/24 00:00:00")]
        [InlineData(3, DayOfWeek.Tuesday, 2024, 3, "03/19/24 00:00:00")]
        [InlineData(4, DayOfWeek.Wednesday, 2024, 3, "03/27/24 00:00:00")]
        [InlineData(5, DayOfWeek.Sunday, 2024, 3, "03/31/24 00:00:00")]
        [Trait("Prana.Utilities", "DateTimeHelper")]
        public void GetNthWeekDay_PassCorrectValues_ReturnsCorrectDate(int nth, DayOfWeek weekDay, int year, int month, string ExpectedResult)
        {
            string ActualResult = DateTimeHelper.GetNthWeekDay(nth, weekDay, year, month).ToString("MM/dd/yy HH:mm:ss");

            outputHelper.WriteLine($"Actual ({ActualResult}) === Expected ({ExpectedResult})");
            Assert.True(ExpectedResult.Equals(ActualResult));
        }

        [Theory]
        [InlineData(-1, DayOfWeek.Sunday, 2024, 3)]
        [InlineData(0, DayOfWeek.Sunday, 2024, 3)]
        [InlineData(6, DayOfWeek.Friday, 2024, 3)]
        [InlineData(7, DayOfWeek.Saturday, 2024, 3)]
        [InlineData(8, DayOfWeek.Sunday, 2024, 3)]
        [Trait("Prana.Utilities", "DateTimeHelper")]
        public void GetNthWeekDay_PassWrongValues_ReturnsInvalidOperation(int nth, DayOfWeek weekDay, int year, int month)
        {
            // Act
            Action action = () => DateTimeHelper.GetNthWeekDay(nth, weekDay, year, month);

            Assert.Throws<InvalidOperationException>(action);
        }
    }
}
