using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CPSC3200PA5;

namespace CPSC3200PA5Unittest
{
    [TestClass]
    public class dataHalfGuardTest
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
        public void NullArrayInitialization()
        {
            int[] array = new int[] { };
            bool flag = false;

            try
            {
                dataHalfGuard halfGuard = new dataHalfGuard(array, true, 0);
            }
            catch (ArgumentException e)
            {
                flag = true;
            }

            Assert.AreEqual(flag, true);
        }

        [TestMethod]
        public void CorrectInitialization()
        {
            int[] array = new int[] { 2, 4, 6, 8, 10, 12, 14, 16 };

            dataHalfGuard halfGuard = new dataHalfGuard(array, true, 3);

            Assert.AreEqual(halfGuard.Sum(0), 0);
            Assert.AreEqual(halfGuard.Value(1), 2);
        }

        [TestMethod]
        public void DuplicateInitialization()
        {
            bool flag = false;
            int[] array = new int[] { 2, 4, 6, 8, 10, 12, 14, 16, 2 };

            try
            {
                dataHalfGuard halfGuard = new dataHalfGuard(array, true, 1);
            }
            catch (ArgumentException e)
            {
                flag = true;
            }

            Assert.AreEqual(flag, true);
        }

        [TestMethod]
        public void ContainOddInitialization()
        {
            bool flag = false;
            int[] array = new int[] { 2, 4, 6, 8, 10, 12, 14, 16, 1 };

            try
            {
                dataHalfGuard half = new dataHalfGuard(array, true, 1);
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
            int[] array = new int[] { 0, 2, 4, 6, 8, 10 };

            dataHalfGuard halfGuard = new dataHalfGuard(array, true, 3);

            int value = halfGuard.Value(1);
            Assert.AreEqual(2, value);


            value = halfGuard.Value(6);
            Assert.AreEqual(-1, value);
        }

        [TestMethod]
        public void Any()
        {
            int[] array = new int[] { 2, 4, 6, 8, 10, 12, 14, 16 };
            dataHalfGuard halfGuard = new dataHalfGuard(array, true, 1);

            int[] any = halfGuard.Any();

            Assert.AreEqual(2, any.Length);
            Assert.AreEqual(2, any[0]);
            Assert.AreEqual(2, any[1]);

            any = halfGuard.Any();

            Assert.AreEqual(2, any.Length);
            Assert.AreEqual(2, any[0]);
            Assert.AreEqual(2, any[1]);
        }

        [TestMethod]
        public void TargetOnX()
        {
            int[] array = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };

            dataHalfGuard halfGuard = new dataHalfGuard(array, true, 1);

            int[] target = halfGuard.Target(4);

            Assert.AreEqual(10, target[0]);
            Assert.AreEqual(20, target[1]);
            Assert.AreEqual(30, target[2]);
            Assert.AreEqual(40, target[3]);
        }

        [TestMethod]
        public void TargetOnY()
        {
            int[] arrayX = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataHalfGuard halfGuard = new dataHalfGuard(arrayX, true, 20);

            for (int i = 0; i < 17; i++)
            {
                halfGuard.Target(100);
            }

            int[] target = halfGuard.Target(3);

            Assert.AreEqual(arrayY[1], target[0]);
            Assert.AreEqual(arrayY[3], target[1]);
            Assert.AreEqual(arrayY[5], target[2]);
        }

        [TestMethod]
        public void SumOnX()
        {
            int[] array = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };

            dataHalfGuard halfGuard = new dataHalfGuard(array, true, 1);

            int sum = halfGuard.Sum(4);
            int expected = 100;

            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void SumOnY()
        {
            int[] arrayX = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataHalfGuard halfGuard = new dataHalfGuard(arrayX, true, 20);

            for (int i = 0; i < 17; i++)
            {
                halfGuard.Target(100);
            }

            int sum = halfGuard.Sum(3);
            int expected = arrayY[1] + arrayY[3] + arrayY[5];

            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void Deactivated()
        {
            int[] arrayX = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };

            dataHalfGuard halfGuard = new dataHalfGuard(arrayX, true, 5);

            for (int i = 0; i < 5; i++)
            {
                halfGuard.Target(100);
            }

            bool flag = false;
            try
            {
                halfGuard.Target(4);
            }
            catch (Exception e)
            {
                flag = true;
            }

            Assert.AreEqual(true, flag);
        }
    }
}
