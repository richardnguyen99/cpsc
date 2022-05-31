using System;
using CPSC3200PA5;

namespace CPSC3200PA5
{
    class P3
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
        static void TestHeterogeneousCollections()
        {
            Console.WriteLine();
            Console.WriteLine("Test Heterogeneous Collections:");
            dataExtractor[] collection = new dataExtractor[3];
            uint minX = (uint)RandomNumberIn(4, 8);
            uint minY = (uint)RandomNumberIn(4, 8);
            uint maxInvalidReq = (uint)RandomNumberIn(10, 16);
            uint kthReq = (uint)RandomNumberIn(6, 8);
            int[] testArray1 = GenerateRandomArray(8);
            int[] testArray2 = GenerateMockYArray(8, testArray1[1]);
            int[] testEvenArray = GenerateRandomEvenArray(16);
            collection[0] = new dataExtractor(minX, minY, testArray1);
            collection[1] = new dataHalf(minX, minY, maxInvalidReq, testEvenArray);
            collection[2] = new dataPlus(minX, minY, kthReq, testArray1);

            Console.WriteLine("dataExtractor: ==");
            for (int i = 0; i < 8 * minY; i++)
            {
                int[] any = collection[0].Any();
                Console.WriteLine("(" + any[0] + ", " + any[1] + ")");
            }
            Console.WriteLine("");
            Console.WriteLine("dataHalf: ==");
            for (int i = 0; i < 8 * minY * 2; i++)
            {
                int[] any = collection[1].Any();
                Console.WriteLine("(" + any[0] + ", " + any[1] + ")");
            }
            Console.WriteLine("");
            Console.WriteLine("dataPlus: ==");
            uint totalRequests = 8 * (minY + 1);
            uint extraValues = totalRequests / kthReq;
            totalRequests = 8 * (minY + extraValues + 1);
            for (int i = 0; i < totalRequests; i++)
            {
                int[] any = collection[2].Any();
                Console.WriteLine("(" + any[0] + ", " + any[1] + ")");
            }
        }
        static void TestModeChanges()
        {
            {
                Console.WriteLine();
                Console.WriteLine("Test Mode Changes (dataExtractor):");
                uint minX = (uint)RandomNumberIn(8, 16);
                uint minY = (uint)RandomNumberIn(8, 16);
                int[] array1 = GenerateRandomArray(minX);
                int[] array2 = GenerateMockYArray(minY, array1[0]);
                dataExtractor extractor = new dataExtractor(minX, minY, array1);
                //extractor.InitializeData(array1);
                for (int i = 0; i < 33; i++)
                {
                    extractor.Target(100);
                }
                Console.WriteLine("Triggered Mode Change");
                int[] target = extractor.Target((uint)RandomNumberIn(4, 6));
                Console.Write("Array X: ");
                ArrayFormat(array1);
                Console.Write("Array Y: ");
                ArrayFormat(array2);
                Console.WriteLine("Extractor:");
                Console.WriteLine("(" + target[0] + ", " + target[1] + ")");
            }
            {
                Console.WriteLine();
                Console.WriteLine("Test Mode Changes (dataHalf):");
                uint minX = (uint)RandomNumberIn(8, 16);
                uint minY = (uint)RandomNumberIn(8, 16);
                uint maxInvalidRequest = (uint)RandomNumberIn(50, 80);
                int[] array1 = GenerateRandomEvenArray(minX);
                int[] array2 = GenerateMockYArray(minY, array1[0]);
                dataHalf half = new dataHalf(minX, minY, maxInvalidRequest, array1);
                for (int i = 0; i < 33; i++)
                {
                    half.Target(100);
                }
                Console.WriteLine("Triggered Mode Change");
                int[] target = half.Target((uint)RandomNumberIn(4, 6));
                Console.Write("Array X: ");
                ArrayFormat(array1);
                Console.Write("Array Y: ");
                ArrayFormat(array2);
                Console.WriteLine("Half:");
                Console.WriteLine("(" + target[0] + ", " + target[1] + ")");
            }
            {
                Console.WriteLine();
                Console.WriteLine("Test Mode Changes (dataPlus):");
                uint minX = (uint)RandomNumberIn(8, 16);
                uint minY = (uint)RandomNumberIn(8, 16);
                uint kthRequest = (uint)RandomNumberIn(50, 80);
                int[] array1 = GenerateRandomArray(minX);
                int[] array2 = GenerateMockYArray(minY, array1[0]);
                dataPlus plus = new dataPlus(minX, minY, kthRequest, array1);
                int[] target = plus.Target((uint)RandomNumberIn(4, 6));
                Console.Write("Array X: ");
                ArrayFormat(array1);
                Console.Write("Array Y: ");
                ArrayFormat(array2);
                Console.WriteLine("Plus:");
                Console.WriteLine("(" + target[0] + ", " + target[1] + ", " + target[2] + ", " + target[3] + ", " + target[4] + ", " + target[5] + ")");
            }
        }
        static void TestVariousInstantions()
        {
            {
                Console.WriteLine();
                Console.WriteLine("Test Various Instantiation (dataExtractor):");
                uint number = (uint)RandomNumberIn(7, 8);
                uint minX = (uint)RandomNumberIn(8, 16);
                uint minY = (uint)RandomNumberIn(8, 16);
                int[][] array2D = new int[number][];

                for (int i = 0; i < number; i++)
                    array2D[i] = GenerateRandomArray(minX);

                dataExtractor[] extractors = new dataExtractor[number];
                for (int i = 0; i < number; i++)
                    extractors[i] = new dataExtractor(minX, minY, array2D[i]);

                for (int i = 0; i < number; i++)
                {
                    Console.Write("Array X: ");
                    ArrayFormat(array2D[i]);
                    Console.Write("Array Y: ");
                    int[] mockArrayY = GenerateMockYArray(minY, array2D[i][0]);
                    ArrayFormat(mockArrayY);
                    uint sum = (uint)RandomNumberIn(4, 6);
                    Console.WriteLine("Sum(" + sum + "): " + extractors[i].Sum(sum));
                    Console.WriteLine();
                }
            }
            {
                Console.WriteLine();
                Console.WriteLine("Test Various Instantiation (dataHalf):");
                uint number = (uint)RandomNumberIn(7, 8);
                uint maxRequests = (uint)RandomNumberIn(4, 8);
                uint minX = (uint)RandomNumberIn(8, 16);
                uint minY = (uint)RandomNumberIn(8, 16);
                int[][] array2D = new int[number][];

                for (int i = 0; i < number; i++)
                    array2D[i] = GenerateRandomEvenArray(minX);

                dataHalf[] halves = new dataHalf[number];
                for (int i = 0; i < number; i++)
                    halves[i] = new dataHalf(minX, minY, maxRequests, array2D[i]);

                for (int i = 0; i < number; i++)
                {
                    Console.Write("Array X: ");
                    ArrayFormat(array2D[i]);
                    Console.Write("Array Y: ");
                    int[] mockArrayY = GenerateMockYArray(minY, array2D[i][0]);
                    ArrayFormat(mockArrayY);
                    uint sum = (uint)RandomNumberIn(4, 6);
                    Console.WriteLine("Sum(" + sum + "): " + halves[i].Sum(sum));
                    Console.WriteLine();
                }
            }
            {
                Console.WriteLine();
                Console.WriteLine("Test Various Instantiation (dataPlus):");
                uint number = (uint)RandomNumberIn(7, 8);
                uint kthRequest = (uint)RandomNumberIn(4, 8);
                uint minX = (uint)RandomNumberIn(8, 16);
                uint minY = (uint)RandomNumberIn(8, 16);
                int[][] array2D = new int[number][];

                for (int i = 0; i < number; i++)
                    array2D[i] = GenerateRandomArray(minX);

                dataPlus[] plusses = new dataPlus[number];
                for (int i = 0; i < number; i++)
                    plusses[i] = new dataPlus(minX, minY, kthRequest, array2D[i]);

                for (int i = 0; i < number; i++)
                {
                    Console.Write("Array X: ");
                    ArrayFormat(array2D[i]);
                    Console.Write("Array Y: ");
                    int[] mockArrayY = GenerateMockYArray(minY, array2D[i][0]);
                    ArrayFormat(mockArrayY);
                    uint sum = (uint)RandomNumberIn(4, 6);
                    Console.WriteLine("Sum(" + sum + "): " + plusses[i].Sum(sum));
                    Console.WriteLine();
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("-------------------------------- CPSC 3200 P5--------------------------------");
            Console.WriteLine("---------------------------- By Minh-Hieu Nguyen-----------------------------\n");
            Console.WriteLine();
            //TestHeterogeneousCollections();
            //TestModeChanges();
            //TestVariousInstantions();

            guard g = new guard(new int[]{3,5,1,2,-3, 0, 9, 10, 4, 7,8 ,6, 11}, false);
            Console.WriteLine(g.Value(4));
        }
    }
}
