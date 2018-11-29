using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOReportApplication.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TOReportApplication.Logic.Tests
{
    public class ShiftCalendarManagerTests
    {
        private ShiftCalendarManager shiftCalendarManager = new ShiftCalendarManager();

        [TestMethod()]
        [TestCase("10:00:00","11:00:00")]
        [TestCase("12:00:00", "14:59:59")]
        public void FirstShiftTest(string fromTime,string toTime)
        {
            var fromTimeAsTimeSpan = TimeSpan.Parse(fromTime);
            var toTimeAsTimeSpan = TimeSpan.Parse(toTime);
            var shift = shiftCalendarManager.GetShift(fromTimeAsTimeSpan, toTimeAsTimeSpan);
            Assert.AreEqual(ShiftInfoEnum.FirstShift,shift);
        }


        [TestMethod()]
        [TestCase("15:00:00", "16:00:00")]
        [TestCase("19:00:00", "22:59:59")]
        public void SecondShiftTest(string fromTime, string toTime)
        {
            var fromTimeAsTimeSpan = TimeSpan.Parse(fromTime);
            var toTimeAsTimeSpan = TimeSpan.Parse(toTime);
            var shift = shiftCalendarManager.GetShift(fromTimeAsTimeSpan, toTimeAsTimeSpan);
            Assert.AreEqual(ShiftInfoEnum.SecondShift, shift);
        }

        [TestMethod()]
        [TestCase("00:00:00", "01:00:00")]
        [TestCase("00:12:00", "6:59:59")]
        public void ThirdShiftTest(string fromTime, string toTime)
        {
            var fromTimeAsTimeSpan = TimeSpan.Parse(fromTime);
            var toTimeAsTimeSpan = TimeSpan.Parse(toTime);
            var shift = shiftCalendarManager.GetShift(fromTimeAsTimeSpan, toTimeAsTimeSpan);
            Assert.AreEqual(ShiftInfoEnum.ThirdShift, shift);
        }

        [TestMethod()]
        [TestCase("14:00:00", "16:00:00")]
        [TestCase("14:59:59", "15:00:00")]
        public void FirstToSecondShiftTest(string fromTime, string toTime)
        {
            var fromTimeAsTimeSpan = TimeSpan.Parse(fromTime);
            var toTimeAsTimeSpan = TimeSpan.Parse(toTime);
            var shift = shiftCalendarManager.GetShift(fromTimeAsTimeSpan, toTimeAsTimeSpan);
            Assert.AreEqual(ShiftInfoEnum.FirstToSecondShitf, shift);
        }

        [TestMethod()]
        [TestCase("22:00:00", "00:05:00")]
        [TestCase("22:59:59", "00:00:00")]
        [TestCase("22:00:00", "23:30:00")]
        public void SecondToThirdShiftTest(string fromTime, string toTime)
        {
            var fromTimeAsTimeSpan = TimeSpan.Parse(fromTime);
            var toTimeAsTimeSpan = TimeSpan.Parse(toTime);
            var shift = shiftCalendarManager.GetShift(fromTimeAsTimeSpan, toTimeAsTimeSpan);
            Assert.AreEqual(ShiftInfoEnum.SecondToThirdShift, shift);
        }

        [TestMethod()]
        [TestCase("00:30:00", "07:05:00")]
        [TestCase("23:30:00", "07:05:00")]
        public void ThirdToFirstShiftTest(string fromTime, string toTime)
        {
            var fromTimeAsTimeSpan = TimeSpan.Parse(fromTime);
            var toTimeAsTimeSpan = TimeSpan.Parse(toTime);
            var shift = shiftCalendarManager.GetShift(fromTimeAsTimeSpan, toTimeAsTimeSpan);
            Assert.AreEqual(ShiftInfoEnum.ThirdToFirstShift, shift);
        }
    }
}