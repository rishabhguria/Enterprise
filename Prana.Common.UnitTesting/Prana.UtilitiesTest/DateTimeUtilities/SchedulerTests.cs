using Prana.BusinessObjects;
using Prana.Utilities.DateTimeUtilities;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.DateTimeUtilities
{
    public class SchedulerTests
    {
        [Fact]
        [Trait("Prana.Utilities", "Scheduler")]
        public void GetInstance_ReturnsSameInstance()
        {
            Scheduler scheduler = new Scheduler();
            // Arrange
            var instance1 = scheduler.GetInstance();
            var instance2 = scheduler.GetInstance();

            // Act & Assert
            Assert.Same(instance1, instance2);
        }

        [Fact]
        [Trait("Prana.Utilities", "Scheduler")]
        public void AddNewTask_AddsTaskToListOfTask()
        {
            // Arrange
            var scheduler = new Scheduler();
            var task = new SchedulerTask(DateTime.Now.AddMinutes(10)); // Task scheduled 10 minutes from now

            // Act
            scheduler.AddNewTask(task);

            // Assert
            // Assert contains(item,itemscollection)
            Assert.Contains(task, scheduler.ClearanceDateTimeList);
        }

        [Fact]
        [Trait("Prana.Utilities", "Scheduler")]
        public void CollapseEventsAtSameTime_ReturnsSingleCollapsedEvents()
        {
            // Arrange
            var scheduler = new Scheduler();
            var task1 = new SchedulerTask(new DateTime(2015, 12, 25));
            var task2 = new SchedulerTask(new DateTime(2015, 12, 25));
            scheduler.AddNewTask(task1);
            scheduler.AddNewTask(task2);

            // Act
            scheduler.CollapseEventsAtSameTime();

            // Assert
            Assert.True(scheduler.ClearanceDateTimeList.Count == 1); 
        }

        [Fact]
        [Trait("Prana.Utilities", "Scheduler")]
        public void CollapseEventsAtSameTime_ReturnsDifferentCollapsedEvents()
        {
            // Arrange
            var scheduler = new Scheduler();
            var task1 = new SchedulerTask(new DateTime(2015, 12, 25));
            var task2 = new SchedulerTask(new DateTime(2015, 12, 26));
            scheduler.AddNewTask(task1);
            scheduler.AddNewTask(task2);

            // Act
            scheduler.CollapseEventsAtSameTime();

            // Assert
            Assert.True(scheduler.ClearanceDateTimeList.Count == 2);
        }
    }

    public class SchedulerTask : Task
    {
        public SchedulerTask(DateTime dateTime)
        {
            ScheduleTime = dateTime;
        }

        public override void AddTasksAtSameTime(ITask scheduledtasksAtSameTime)
        {

        }
    }
}
