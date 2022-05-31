using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CPSC3200PA5;

namespace CPSC3200PA5Unittest
{
    [TestClass]
    public class dataPlusTest
    {
        private int RandomNumberIn(int lower, int upper)
        {
            Random random = new Random();
            return random.Next(lower, upper);
        }

        private int[] GenerateRandomArray(uint count)
        {
            int[] testArray = new int[count];

            for (int i = 0; i < count; i++)
            {
                testArray[i] = RandomNumberIn((int)count * i, (int)count * (i + 1));
            }

            return testArray;
        }
        private int[] GenerateRandomEvenArray(uint count)
        {
            int[] testArray = new int[count];

            for (int i = 0; i < count; i++)
            {
                testArray[i] = RandomNumberIn((int)count * i, (int)count * (i + 1)) * 2;
            }

            return testArray;
        }

        private int[] GenerateMockYArray(uint count, int initial)
        {
            int[] y = new int[count];
            for (int i = 0; i < count; i++)
            {
                y[i] = initial + (i * 3);
            }

            return y;
        }
        [TestMethod]
        public void CorrectInitialization()
        {
            dataPlus plus = new dataPlus(8, 8, 4, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 });

            Assert.AreEqual(plus.Sum(0), 0);
        }

        [TestMethod]
        public void DuplicateInitialization()
        {
            bool flag = false;
            try
            {
                dataPlus plus = new dataPlus(8, 8, 4, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 8 });
            }
            catch (ArgumentException e)
            {
                flag = true;
            }

            Assert.AreEqual(flag, true);
        }

        [TestMethod]
        public void NotBigEnoughInitialization()
        {
            bool flag = false;
            try
            {
                dataPlus plus = new dataPlus(8, 8, 4, new int[] { 1, 2, 3 });
            }
            catch (ArgumentException e)
            {
                flag = true;
            }

            Assert.AreEqual(flag, true);
        }

        [TestMethod]
        public void Any()
        {
            int[] arrayX = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            uint minX = 8;
            uint minY = 8;
            uint kth = 4;

            dataPlus plus = new dataPlus(minX, minY, kth, arrayX);

            uint totalRequests = minX * (minY + 1);
            uint extraValues = totalRequests / kth;
            totalRequests = minX * (minY + extraValues + 1);

            for (int i = 0; i < totalRequests - 1; i++)
            {
                plus.Any();
            }

            int[] any = plus.Any();

            Assert.AreEqual(arrayX[minX - 1], any[0]);
            Assert.AreEqual(kth * extraValues, (uint)any[1]);
        }

        [TestMethod]
        public void Target()
        {
            int[] arrayX = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataPlus plus = new dataPlus(8, 8, 4, arrayX);

            int[] target = plus.Target(3);

            Assert.AreEqual(arrayX[0], target[0]);
            Assert.AreEqual(arrayX[2], target[1]);
            Assert.AreEqual(arrayX[4], target[2]);
            Assert.AreEqual(arrayY[1], target[3]);
            Assert.AreEqual(arrayY[3], target[4]);
            Assert.AreEqual(arrayY[5], target[5]);
        }

        [TestMethod]
        public void SumOnX()
        {
            int[] array = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

            dataPlus plus = new dataPlus(8, 8, 4, array);

            int sum = plus.Sum(3);
            int expected = 45;

            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void SumOnY()
        {
            int[] arrayX = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataPlus plus = new dataPlus(8, 8, 4, arrayX);

            for (int i = 0; i < 17; i++)
            {
                plus.Target(100);
            }

            int sum = plus.Sum(3);
            int expected = arrayY[0] + arrayY[2] + arrayY[4];

            Assert.AreEqual(expected, sum);
        }
    }
}
