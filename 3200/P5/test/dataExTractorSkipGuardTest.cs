using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPSC3200PA5;

namespace CPSC3200PA5Unittest
{
    [TestClass]
    public class dataExtractorSkipGuardTest
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

            dataExtractorSkipGuard extractorSkipGuard = new dataExtractorSkipGuard(array, true, 1);

            Assert.AreEqual(extractorSkipGuard.Sum(0), 0);
            Assert.AreEqual(extractorSkipGuard.Value(3), 7);
        }

        [TestMethod]
        public void NullArrayInitialization()
        {
            int[] array = new int[] { };
            bool flag = false;

            try
            {
                dataExtractorSkipGuard extractorSkipGuard = new dataExtractorSkipGuard(array, true, 0);
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
                dataExtractorSkipGuard extractorSkipGuard = new dataExtractorSkipGuard(array, true, 3);
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
                dataExtractorSkipGuard extractorSkipGuard = new dataExtractorSkipGuard(array, true, 0);
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

            dataExtractorSkipGuard extractorSkipGuard = new dataExtractorSkipGuard(array, true, 1);

            int value = extractorSkipGuard.Value(6);
            Assert.AreEqual(11, value);

            extractorSkipGuard.Value(15);

            value = extractorSkipGuard.Value(6);
            Assert.AreEqual(3, value);
        }

        [TestMethod]
        public void Any()
        {
            int[] array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            dataExtractorSkipGuard extractorSkipGuard = new dataExtractorSkipGuard(array, true, 3);

            int[] any = extractorSkipGuard.Any();

            Assert.AreEqual(2, any.Length);
            Assert.AreEqual(1, any[0]);
            Assert.AreEqual(1, any[1]);

            any = extractorSkipGuard.Any();

            Assert.AreEqual(2, any.Length);
            Assert.AreEqual(2, any[0]);
            Assert.AreEqual(1, any[1]);
        }

        [TestMethod]
        public void TargetOnX()
        {
            int[] array = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

            dataExtractorSkipGuard extractorSkipGuard = new dataExtractorSkipGuard(array, true, 3);

            int[] target = extractorSkipGuard.Target(3);

            Assert.AreEqual(5, target[0]);
            Assert.AreEqual(15, target[1]);
            Assert.AreEqual(25, target[2]);
        }

        [TestMethod]
        public void TargetOnY()
        {
            int[] arrayX = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataExtractorSkipGuard extractorSkipGuard = new dataExtractorSkipGuard(arrayX, true, 3);

            for (int i = 0; i < 17; i++)
            {
                extractorSkipGuard.Target(100);
            }

            int[] target = extractorSkipGuard.Target(3);

            Assert.AreEqual(arrayY[0], target[0]);
            Assert.AreEqual(arrayY[2], target[1]);
            Assert.AreEqual(arrayY[4], target[2]);
        }

        [TestMethod]
        public void SumOnX()
        {
            int[] array = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

            dataExtractorSkipGuard extractorSkipGuard = new dataExtractorSkipGuard(array, true, 1);

            int sum = extractorSkipGuard.Sum(3);
            int expected = 45;

            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void SumOnY()
        {
            int[] arrayX = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataExtractorSkipGuard extractorSkipGuard = new dataExtractorSkipGuard(arrayX, true, 1);

            for (int i = 0; i < 17; i++)
            {
                extractorSkipGuard.Target(100);
            }

            int sum = extractorSkipGuard.Sum(3);
            int expected = arrayY[0] + arrayY[2] + arrayY[4];

            Assert.AreEqual(expected, sum);
        }
    }
}
