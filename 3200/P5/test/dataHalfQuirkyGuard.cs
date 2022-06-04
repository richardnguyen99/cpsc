using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CPSC3200PA5;

namespace CPSC3200PA5Unittest
{
    [TestClass]
    public class dataHalfQuirkyGuardTest
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
                dataHalfQuirkyGuard halfQuirkyGuard = new dataHalfQuirkyGuard(array, true, 0);
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

            dataHalfQuirkyGuard halfQuirkyGuard = new dataHalfQuirkyGuard(array, true, 3);

            Assert.AreEqual(halfQuirkyGuard.Sum(0), 0);
            Assert.AreEqual(halfQuirkyGuard.Value(1), 2);
        }

        [TestMethod]
        public void DuplicateInitialization()
        {
            bool flag = false;
            int[] array = new int[] { 2, 4, 6, 8, 10, 12, 14, 16, 2 };

            try
            {
                dataHalfQuirkyGuard halfQuirkyGuard = new dataHalfQuirkyGuard(array, true, 1);
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
                dataHalfQuirkyGuard half = new dataHalfQuirkyGuard(array, true, 1);
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

            dataHalfQuirkyGuard halfQuirkyGuard = new dataHalfQuirkyGuard(array, true, 2);

            int value = halfQuirkyGuard.Value(4);
            Assert.AreEqual(-1, value);


            value = halfQuirkyGuard.Value(6);
            Assert.AreEqual(2, value);

            value = halfQuirkyGuard.Value(6);
            Assert.AreEqual(-1, value);

            value = halfQuirkyGuard.Value(3);
            Assert.AreEqual(-2, value);
        }

        [TestMethod]
        public void Any()
        {
            int[] array = new int[] { 2, 4, 6, 8, 10, 12, 14, 16 };
            dataHalfQuirkyGuard halfQuirkyGuard = new dataHalfQuirkyGuard(array, true, 1);

            int[] any = halfQuirkyGuard.Any();

            Assert.AreEqual(2, any.Length);
            Assert.AreEqual(2, any[0]);
            Assert.AreEqual(2, any[1]);

            any = halfQuirkyGuard.Any();

            Assert.AreEqual(2, any.Length);
            Assert.AreEqual(2, any[0]);
            Assert.AreEqual(2, any[1]);
        }

        [TestMethod]
        public void TargetOnX()
        {
            int[] array = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };

            dataHalfQuirkyGuard halfQuirkyGuard = new dataHalfQuirkyGuard(array, true, 1);

            int[] target = halfQuirkyGuard.Target(4);

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

            dataHalfQuirkyGuard halfQuirkyGuard = new dataHalfQuirkyGuard(arrayX, true, 20);

            for (int i = 0; i < 17; i++)
            {
                halfQuirkyGuard.Target(100);
            }

            int[] target = halfQuirkyGuard.Target(3);

            Assert.AreEqual(arrayY[1], target[0]);
            Assert.AreEqual(arrayY[3], target[1]);
            Assert.AreEqual(arrayY[5], target[2]);
        }

        [TestMethod]
        public void SumOnX()
        {
            int[] array = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };

            dataHalfQuirkyGuard halfQuirkyGuard = new dataHalfQuirkyGuard(array, true, 1);

            int sum = halfQuirkyGuard.Sum(4);
            int expected = 100;

            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void SumOnY()
        {
            int[] arrayX = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };
            int[] arrayY = GenerateMockYArray(8, arrayX[0]);

            dataHalfQuirkyGuard halfQuirkyGuard = new dataHalfQuirkyGuard(arrayX, true, 20);

            for (int i = 0; i < 17; i++)
            {
                halfQuirkyGuard.Target(100);
            }

            int sum = halfQuirkyGuard.Sum(3);
            int expected = arrayY[1] + arrayY[3] + arrayY[5];

            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void Deactivated()
        {
            int[] arrayX = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };

            dataHalfQuirkyGuard halfQuirkyGuard = new dataHalfQuirkyGuard(arrayX, true, 5);

            for (int i = 0; i < 5; i++)
            {
                halfQuirkyGuard.Target(100);
            }

            bool flag = false;
            try
            {
                halfQuirkyGuard.Target(4);
            }
            catch (Exception e)
            {
                flag = true;
            }

            Assert.AreEqual(true, flag);
        }
    }
}
