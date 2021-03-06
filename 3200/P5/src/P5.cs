using System;

namespace CPSC3200PA5
{
    class P5
    {
        static void ArrayFormat(int[] array)
        {
            Console.Write("[");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write("" + array[i]);
                if (i < array.Length - 1)
                    Console.Write(", ");
            }
            Console.WriteLine("]");
        }
        static int RandomNumberIn(int lower, int upper)
        {
            Random random = new Random();
            return random.Next(lower, upper);
        }

        static int RandomOddNumberIn(int lower, int upper)
        {
            Random random = new Random();
            int next = 0;

            do
            {
                next = random.Next(lower, upper);
            } while (next % 2 == 0);

            return next;

        }

        static int[] GenerateRandomArray(uint count)
        {
            int[] testArray = new int[count];
            for (int i = 0; i < count; i++)
            {
                testArray[i] = RandomNumberIn((int)count * i, (int)count * (i + 1));
            }
            return testArray;
        }
        static int[] GenerateRandomEvenArray(uint count)
        {
            int[] testArray = new int[count];
            for (int i = 0; i < count; i++)
            {
                testArray[i] = RandomNumberIn((int)count * i, (int)count * (i + 1)) * 2;
            }
            return testArray;
        }
        static int[] GenerateMockYArray(uint count, int initial)
        {
            int[] y = new int[count];
            for (int i = 0; i < count; i++)
            {
                y[i] = initial + (i * 3);
            }
            return y;
        }
        static bool IsPrime(int x)
        {
            if (x <= 1) return false;

            if (x == 2) return true;

            for (int i = 2; i < (Math.Sqrt(x) + 1); i++)
                if (x % i == 0) return false;

            return true;
        }

        static bool BinaryRandom(int num)
        {
            return num % 2 == 0;
        }

        static int GetLastOddFrom(int[] array)
        {
            int returnedVal = array[array.Length - 1];

            for (int i = array.Length - 1; i <= 0; i++)
            {
                returnedVal = array[i];

                if (returnedVal % 2 == 1) return returnedVal;
            }

            returnedVal = array.Length % 2 == 0 ? array.Length + 1 : array.Length;

            return returnedVal;
        }

        static int[] GeneratePrimeArray(uint count)
        {
            int[] tmpPrimes = new int[count];
            int primeCount = 0;

            for (int i = 0; i < count; i++)
            {
                if (IsPrime(i))
                    tmpPrimes[primeCount++] = i;
            }

            int[] primes = new int[primeCount];
            Array.Copy(tmpPrimes, 0, primes, 0, primeCount);

            return primes;
        }

        static void TestHeterogeneousCollections()
        {
            Console.WriteLine();
            Console.WriteLine("Test Heterogeneous Collections: ");
            Console.WriteLine("================================");
            Console.WriteLine();


            int arrayRange = RandomNumberIn(3, 50);
            int[] testPrimeArray = GeneratePrimeArray((uint)arrayRange);
            int arrayLength = testPrimeArray.Length;
            int[] testEvenArray = GenerateRandomEvenArray((uint)arrayLength);
            bool mode = BinaryRandom(arrayLength);

            Console.Write("Prime array: ");
            ArrayFormat(testPrimeArray);
            Console.Write("Even array: ");
            ArrayFormat(testEvenArray);
            Console.WriteLine("Length: " + arrayLength);
            Console.WriteLine("Mode: " + (mode ? "Up" : "Down"));

            AGuardData[] collection = new AGuardData[9];


            collection[0] = new dataExtractorGuard(testPrimeArray, mode);
            collection[1] = new dataExtractorSkipGuard(testPrimeArray, mode, arrayLength);
            collection[2] = new dataExtractorQuirkyGuard(testPrimeArray, mode, arrayLength);

            collection[3] = new dataHalfGuard(testEvenArray, mode, arrayLength);
            collection[4] = new dataHalfSkipGuard(testEvenArray, mode, arrayLength);
            collection[5] = new dataHalfQuirkyGuard(testEvenArray, mode, arrayLength);

            collection[6] = new dataPlusGuard(testPrimeArray, mode, arrayLength);
            collection[7] = new dataPlusSkipGuard(testPrimeArray, mode, arrayLength);
            collection[8] = new dataPlusQuirkyGuard(testPrimeArray, mode, arrayLength);

            Console.WriteLine();

            Console.WriteLine("dataExtractor::Any()");
            Console.WriteLine("=========================");
            for (int i = 0; i < arrayLength; i++)
            {
                int[] any1 = collection[0].Any();
                int[] any2 = collection[1].Any();
                int[] any3 = collection[2].Any();
                int[] any4 = collection[3].Any();
                int[] any5 = collection[4].Any();
                int[] any6 = collection[5].Any();
                int[] any7 = collection[6].Any();
                int[] any8 = collection[7].Any();
                int[] any9 = collection[8].Any();

                Console.WriteLine("dataExtractorGuard::Any()\t-> (" + any1[0] + ", " + any1[1] + ")");
                Console.WriteLine("dataExtractorSkipGuard::Any()\t-> (" + any2[0] + ", " + any2[1] + ")");
                Console.WriteLine("dataExtractorQuirkyGuard::Any()\t-> (" + any3[0] + ", " + any3[1] + ")");
                Console.WriteLine();

                Console.WriteLine("dataHalfGuard::Any()\t\t-> (" + any4[0] + ", " + any4[1] + ")");
                Console.WriteLine("dataHalfSkipGuard::Any()\t-> (" + any5[0] + ", " + any5[1] + ")");
                Console.WriteLine("dataHalfQuirkyGuard::Any()\t-> (" + any6[0] + ", " + any6[1] + ")");
                Console.WriteLine();

                Console.WriteLine("dataPlusGuard::Any()\t\t-> (" + any7[0] + ", " + any7[1] + ")");
                Console.WriteLine("dataPlusSkipGuard::Any()\t-> (" + any8[0] + ", " + any8[1] + ")");
                Console.WriteLine("dataPlusQuirkyGuard::Any()\t-> (" + any9[0] + ", " + any9[1] + ")");
                Console.WriteLine();
            }

            Console.WriteLine("guard::Value(z)");
            Console.WriteLine("===============");
            for (int i = 0; i < arrayLength; i++)
            {
                int testValue = RandomNumberIn(3, 30);

                int value1 = collection[0].Value(testValue);
                int value2 = collection[1].Value(testValue);
                int value3 = collection[2].Value(testValue);
                int value4 = collection[3].Value(testValue);
                int value5 = collection[4].Value(testValue);
                int value6 = collection[5].Value(testValue);
                int value7 = collection[6].Value(testValue);
                int value8 = collection[7].Value(testValue);
                int value9 = collection[8].Value(testValue);

                Console.WriteLine("dataExtractorGuard::Value(" + testValue + ")\t\t= " + value1);
                Console.WriteLine("dataHalfGuard::Value(" + testValue + ")\t\t= " + value4);
                Console.WriteLine("dataPlusGuard::Value(" + testValue + ")\t\t= " + value7);
                Console.WriteLine();

                Console.WriteLine("dataExtractorSkipGuard::Value(" + testValue + ")\t= " + value2);
                Console.WriteLine("dataHalfSkipGuard::Value(" + testValue + ")\t\t= " + value5);
                Console.WriteLine("dataPlusSkipGuard::Value(" + testValue + ")\t\t= " + value8);
                Console.WriteLine();

                Console.WriteLine("dataExtractorQuirkyGuard::Value(" + testValue + ")\t= " + value3);
                Console.WriteLine("dataHalfQuirkyGuard::Value(" + testValue + ")\t\t= " + value6);
                Console.WriteLine("dataPlusQuirkyGuard::Value(" + testValue + ")\t\t= " + value9);
                Console.WriteLine();
            }
        }

        static void TestDataExtractorModeChange()
        {
            Console.WriteLine();
            Console.WriteLine("Test Datta Extractor Mode change: ");
            Console.WriteLine("==================================");
            Console.WriteLine();

            uint rangeX = (uint)RandomNumberIn(8, 16);
            uint rangeY = (uint)RandomNumberIn(8, 16);

            int[] array1 = GenerateRandomArray(rangeX);
            int[] array2 = GenerateMockYArray(rangeX, array1[0]);

            AGuardData[] polyExtractors = new AGuardData[3];

            polyExtractors[0] = new dataExtractorGuard(array1, true);
            polyExtractors[1] = new dataExtractorSkipGuard(array1, true, array1.Length);
            polyExtractors[2] = new dataExtractorSkipGuard(array1, true, array1.Length);

            for (int i = 0; i < 100; i++)
            {
                polyExtractors[0].Target(100);
                polyExtractors[1].Target(100);
                polyExtractors[2].Target(100);
            }
            Console.WriteLine("Triggered Mode Change");

            int[] target1 = polyExtractors[0].Target(2);
            int[] target2 = polyExtractors[1].Target(2);
            int[] target3 = polyExtractors[2].Target(2);



            Console.WriteLine("Mock Y array:");
            int startIdx = array2[0] % 2 == 0 ? 0 : 1;
            Console.WriteLine("(" + array2[startIdx] + ", " + array2[startIdx + 2] + ")");
            Console.WriteLine();
            Console.WriteLine("Extractor1:");
            Console.WriteLine("(" + target1[0] + ", " + target1[1] + ")");
            Console.WriteLine("Extractor2:");
            Console.WriteLine("(" + target2[0] + ", " + target2[1] + ")");
            Console.WriteLine("Extractor3:");
            Console.WriteLine("(" + target3[0] + ", " + target3[1] + ")");
        }

        static void TestGuardModeChange()
        {
            Console.WriteLine();
            Console.WriteLine("Test Guard Mode change: ");
            Console.WriteLine("========================");
            Console.WriteLine();

            AGuardData[] polyGuards = new AGuardData[3];

            int[] testPrimeArray = GeneratePrimeArray((uint)RandomNumberIn(50, 100));

            int kth = RandomNumberIn(1, testPrimeArray.Length);
            polyGuards[0] = new dataExtractorGuard(testPrimeArray, true);
            polyGuards[1] = new dataExtractorSkipGuard(testPrimeArray, true, kth);
            polyGuards[2] = new dataExtractorQuirkyGuard(testPrimeArray, true, testPrimeArray.Length);

            int biggestPrime = testPrimeArray[testPrimeArray.Length - 1];
            int nthBiggestPrime = testPrimeArray[testPrimeArray.Length - kth - 1];
            Console.WriteLine("Biggest prime in testPrimeArray: " + biggestPrime);

            int first = polyGuards[0].Value(biggestPrime);
            Console.WriteLine("guard::Value(" + biggestPrime + ") = \t" + first);

            if (first == -1)
                Console.WriteLine("Mode has changed");

            int second = polyGuards[0].Value(biggestPrime);
            Console.WriteLine("guard::Value(" + biggestPrime + ") = \t" + second);
            Console.WriteLine();

            first = polyGuards[1].Value(biggestPrime);
            Console.WriteLine("Kth: " + kth);
            Console.WriteLine("skipGuard::Value(" + biggestPrime + ") = \t" + first);

            if (first == -1)
                Console.WriteLine("Mode has changed");

            second = polyGuards[1].Value(biggestPrime);
            Console.WriteLine("skipGuard::Value(" + biggestPrime + ") = \t" + second);
            Console.WriteLine();

            first = polyGuards[2].Value(biggestPrime);
            Console.WriteLine("quirkyGuard::Value(" + biggestPrime + ") = \t" + first);

            if (first == -1)
                Console.WriteLine("Mode has changed");

            second = polyGuards[2].Value(biggestPrime);
            Console.WriteLine("quirkyGuard::Value(" + biggestPrime + ") = \t" + second);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("-------------------------------- CPSC 3200 P5--------------------------------");
            Console.WriteLine("---------------------------- By Minh-Hieu Nguyen-----------------------------\n");
            Console.WriteLine();
            TestHeterogeneousCollections();
            TestDataExtractorModeChange();
            TestGuardModeChange();
        }
    }
}
