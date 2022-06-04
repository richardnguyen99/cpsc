using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CPSC3200PA5;

namespace CPSC3200PA5Unittest
{
    [TestClass]
    public class dataPlusQuirkyGuardTest
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
            int[] array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            dataPlusQuirkyGuard plusQuirkyGuard = new dataPlusQuirkyGuard(array, true, 4);

            Assert.AreEqual(plusQuirkyGuard.Sum(0), 0);
            Assert.AreEqual(plusQuirkyGuard.Value(4), 5);
        }

        [TestMethod]
        public void DuplicateInitialization()
        {
            bool flag = false;
            int[] array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 8 };

            try
            {
                dataPlusQuirkyGuard plusQuirkyGuard = new dataPlusQuirkyGuard(array, true, 4);
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
            int[] array = new int[] { 1, 7 };

            try
            {
                dataPlusQuirkyGuard plusQuirkyGuard = new dataPlusQuirkyGuard(array, true, 4);
            }
            catch (ArgumentException e)
            {
                flag = true;
            }

            Assert.AreEqual(flag, true);
        }

        [TestMethod]
        public void Value()
        {
            int[] array = new int[] { 1, 7, 5, 3, 4, 6, 2, 8, 11 };

            dataPlusQuirkyGuard plusQuirkyGuard = new dataPlusQuirkyGuard(array, true, 1);

            int value = plusQuirkyGuard.Value(6);
            Assert.AreEqual(7, value);

            plusQuirkyGuard.Value(15);

            value = plusQuirkyGuard.Value(6);
            Assert.AreEqual(-2, value);
        }

        [TestMethod]
        public void Any()
        {
            int[] arrayX = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            uint minX = 8;
            uint minY = 8;
            uint kth = 4;

            dataPlusQuirkyGuard plusQuirkyGuard = new dataPlusQuirkyGuard(arrayX, true, (int)kth);

            uint totalRequests = minX * (minY + 1);
            uint extraValues = totalRequests / kth;
            totalRequests = minX * (minY + extraValues + 1);

            for (int i = 0; i < totalRequests - 1; i++)
            {
                plusQuirkyGuard.Any();
            }

            int[] any = plusQuirkyGuard.Any();

            Assert.AreEqual(arrayX[minX - 1], any[0]);
            Assert.AreEqual(kth * extraValues, (uint)any[1]);
        }

        public void Target()
        {
            int[] arrayX = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataPlusQuirkyGuard plusQuirkyGuard = new dataPlusQuirkyGuard(arrayX, true, 4);

            int[] target = plusQuirkyGuard.Target(3);

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

            dataPlusQuirkyGuard plusQuirkyGuard = new dataPlusQuirkyGuard(array, true, 4);

            int sum = plusQuirkyGuard.Sum(3);
            int expected = 45;

            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void SumOnY()
        {
            int[] arrayX = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataPlusQuirkyGuard plusQuirkyGuard = new dataPlusQuirkyGuard(arrayX, true, 4);

            for (int i = 0; i < 17; i++)
            {
                plusQuirkyGuard.Target(100);
            }

            int sum = plusQuirkyGuard.Sum(3);
            int expected = arrayY[0] + arrayY[2] + arrayY[4];

            Assert.AreEqual(expected, sum);
        }
    }
}
