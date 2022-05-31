using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPSC3200PA5;

namespace CPSC3200PA5Unittest
{
    [TestClass]
    public class dataExtractorTest
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
            dataExtractor extractor = new dataExtractor(8, 8, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 });

            Assert.AreEqual(extractor.Sum(0), 0);
        }

        [TestMethod]
        public void DuplicateInitialization()
        {
            bool flag = false;
            try
            {
                dataExtractor extractor = new dataExtractor(8, 8, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 8 });
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
                dataExtractor extractor = new dataExtractor(8, 8, new int[] { 1, 2, 3 });
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
            dataExtractor extractor = new dataExtractor(8, 8, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 });

            int[] any = extractor.Any();

            Assert.AreEqual(2, any.Length);
            Assert.AreEqual(1, any[0]);
            Assert.AreEqual(1, any[1]);

            any = extractor.Any();

            Assert.AreEqual(2, any.Length);
            Assert.AreEqual(2, any[0]);
            Assert.AreEqual(1, any[1]);
        }

        [TestMethod]
        public void TargetOnX()
        {
            int[] array = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

            dataExtractor extractor = new dataExtractor(8, 8, array);

            int[] target = extractor.Target(3);

            Assert.AreEqual(5, target[0]);
            Assert.AreEqual(15, target[1]);
            Assert.AreEqual(25, target[2]);
        }

        [TestMethod]
        public void TargetOnY()
        {
            int[] arrayX = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataExtractor extractor = new dataExtractor(8, 8, arrayX);

            for (int i = 0; i < 17; i++)
            {
                extractor.Target(100);
            }

            int[] target = extractor.Target(3);

            Assert.AreEqual(arrayY[0], target[0]);
            Assert.AreEqual(arrayY[2], target[1]);
            Assert.AreEqual(arrayY[4], target[2]);
        }

        [TestMethod]
        public void SumOnX()
        {
            int[] array = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

            dataExtractor extractor = new dataExtractor(8, 8, array);

            int sum = extractor.Sum(3);
            int expected = 45;

            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void SumOnY()
        {
            int[] arrayX = new int[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataExtractor extractor = new dataExtractor(8, 8, arrayX);

            for (int i = 0; i < 17; i++)
            {
                extractor.Target(100);
            }

            int sum = extractor.Sum(3);
            int expected = arrayY[0] + arrayY[2] + arrayY[4];

            Assert.AreEqual(expected, sum);
        }
    }
}
