using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Collections.Generic;
using Graphing_Calculator_UI;

namespace UnitTests
{
    [TestClass]
    public class RangedPlotterTests
    {
        [TestMethod]
        public void TestGetPointSegments()
        {
            var points = new List<Point>()
            {
                new Point(0, 1),
                new Point(1, 2),
                new Point(2, 10),
                new Point(3, 100),
                new Point(4, 8),
                new Point(5, 4)
            };

            var segments = RangedPlotter.GetPointSegments(points, 0, 5);

            Assert.AreEqual(2, segments.Count);
            Assert.AreEqual(3, segments[0].Count);
            Assert.AreEqual(2, segments[1].Count);
        }
    }
}
