using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPSC3200PA5;

namespace CPSC3200PA5Unittest
{
    [TestClass]
    public class dataExtractorQuirkyGuardTest
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

            dataExtractorQuirkyGuard extractorQuirkyGuard = new dataExtractorQuirkyGuard(array, true, 1);

            Assert.AreEqual(extractorQuirkyGuard.Sum(0), 0);
            Assert.AreEqual(extractorQuirkyGuard.Value(3), 5);
        }

        [TestMethod]
        public void NullArrayInitialization()
        {
            int[] array = new int[] { };
            bool flag = false;

            try
            {
                dataExtractorQuirkyGuard extractorQuirkyGuard = new dataExtractorQuirkyGuard(array, true, 0);
            }
            catch (ArgumentException e)
            {
                flag = true;
            }

            Assert.AreEqual(flag, true);
        }

        [TestMethod]
        public void DuplicateInitialization()
        {
            bool flag = false;
            int[] array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 8 };

            try
            {
                dataExtractorQuirkyGuard extractorQuirkyGuard = new dataExtractorQuirkyGuard(array, true, 3);
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
            int[] array = new int[] { 1, 7 };
            bool flag = false;

            try
            {
                dataExtractorQuirkyGuard extractorQuirkyGuard = new dataExtractorQuirkyGuard(array, true, 0);
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

            dataExtractorQuirkyGuard extractorQuirkyGuard = new dataExtractorQuirkyGuard(array, true, 1);

            int value = extractorQuirkyGuard.Value(6);
            Assert.AreEqual(7, value);

            value = extractorQuirkyGuard.Value(6);
            Assert.AreEqual(11, value);

            value = extractorQuirkyGuard.Value(6);
            Assert.AreEqual(-1, value);
        }

        [TestMethod]
        public void EnoughModeChange()
        {
            int[] array = new int[] { 1, 7, 5, 3, 4, 6, 2, 8, 11 };

            dataExtractorQuirkyGuard extractorQuirkyGuard = new dataExtractorQuirkyGuard(array, true, 2);

            int value = extractorQuirkyGuard.Value(6);
            Assert.AreEqual(7, value);

            value = extractorQuirkyGuard.Value(6);
            Assert.AreEqual(11, value);

            value = extractorQuirkyGuard.Value(6);
            Assert.AreEqual(-1, value); // mode changed 1

            value = extractorQuirkyGuard.Value(6);
            Assert.AreEqual(5, value);

            value = extractorQuirkyGuard.Value(6);
            Assert.AreEqual(3, value);

            value = extractorQuirkyGuard.Value(6);
            Assert.AreEqual(2, value);

            value = extractorQuirkyGuard.Value(6);
            Assert.AreEqual(-1, value); // mode changed 2

            value = extractorQuirkyGuard.Value(6);
            Assert.AreEqual(-2, value);
        }

        [TestMethod]
        public void Any()
        {
            int[] array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            dataExtractorQuirkyGuard extractorQuirkyGuard = new dataExtractorQuirkyGuard(array, true, 3);

            int[] any = extractorQuirkyGuard.Any();

            Assert.AreEqual(2, any.Length);
            Assert.AreEqual(1, any[0]);
            Assert.AreEqual(1, any[1]);

            any = extractorQuirkyGuard.Any();

            Assert.AreEqual(2, any.Length);
            Assert.AreEqual(2, any[0]);
            Assert.AreEqual(1, any[1]);
        }

        [TestMethod]
        public void TargetOnX()
        {
            int[] array = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

            dataExtractorQuirkyGuard extractorQuirkyGuard = new dataExtractorQuirkyGuard(array, true, 3);

            int[] target = extractorQuirkyGuard.Target(3);

            Assert.AreEqual(5, target[0]);
            Assert.AreEqual(15, target[1]);
            Assert.AreEqual(25, target[2]);
        }

        [TestMethod]
        public void TargetOnY()
        {
            int[] arrayX = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataExtractorQuirkyGuard extractorQuirkyGuard = new dataExtractorQuirkyGuard(arrayX, true, 3);

            for (int i = 0; i < 17; i++)
            {
                extractorQuirkyGuard.Target(100);
            }

            int[] target = extractorQuirkyGuard.Target(3);

            Assert.AreEqual(arrayY[0], target[0]);
            Assert.AreEqual(arrayY[2], target[1]);
            Assert.AreEqual(arrayY[4], target[2]);
        }

        [TestMethod]
        public void SumOnX()
        {
            int[] array = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

            dataExtractorQuirkyGuard extractorQuirkyGuard = new dataExtractorQuirkyGuard(array, true, 3);

            int sum = extractorQuirkyGuard.Sum(3);
            int expected = 45;

            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void SumOnY()
        {
            int[] arrayX = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataExtractorQuirkyGuard extractorQuirkyGuard = new dataExtractorQuirkyGuard(arrayX, true, 3);

            for (int i = 0; i < 17; i++)
            {
                extractorQuirkyGuard.Target(100);
            }

            int sum = extractorQuirkyGuard.Sum(3);
            int expected = arrayY[0] + arrayY[2] + arrayY[4];

            Assert.AreEqual(expected, sum);
        }
    }
}
