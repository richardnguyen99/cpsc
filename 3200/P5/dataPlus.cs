using System;
/// <summary>
///
/// Class invariants - defined types and how to use dataExtractor
///
/// dataPlus behaves as dataExtractor with additions. y is initially concatenated
/// with a, where a is not the same number across all objects. A frequency, k, is
/// initialized so that n, where n is a multiplication of k, steps is concatenated
/// to the end of y.
///
/// Interface invariants - How to use methods correctly
///
/// - Constructor:
///   Constructs a dataPlus with boundaries for two arrays, along with a
///   frequence, k, to keep track when to concatenate n steps to the end of y,
///   where n is multiplcation of k.
///
/// - InitializeData(int[] array):
///   This method behaves as dataExtractor::InitializeData(array) with additions;
///   y is initially concatenated with a, where a is not the same number across
///   all objects
///
/// - Any():
///   Request method. This method behaves as dataExtractor::Any().
///
/// - Target(uint z):
///   Request method. This method behaves as dataExtractor::Any() with additions.
///   It returns z odd values from array x concatenated with z even values from
///   array y.
///
/// - Sum(uint z):
///   Request method. This method behaves as dataExtractor::Sum(z).
///
/// </summary>
namespace CPSC3200PA5
{
    public class dataPlus : dataExtractor
    {
        private readonly uint REQUEST_FREQ;
        public dataPlus(uint x, uint y, uint kth) : base(x, y)
        {
            this.REQUEST_FREQ = kth;
        }
        /// <summary>
        ///
        /// Precondition:
        /// - Array must have no duplicates or not enough values
        ///
        /// </summary>
        public override void InitializeData(int[] array)
        {
            int[] srcArray = new int[array.Length];
            array.CopyTo(srcArray, 0);
            base.InitializeData(srcArray);
            int[] exclusiveArray = new int[srcArray.Length];
            srcArray.CopyTo(exclusiveArray, 0);
            int counter = srcArray.Length;
            for (int i = 0; i < srcArray.Length; i++)
            {
                for (int j = 0; j < base.y.Length; j++)
                {
                    if (srcArray[i] == base.y[j])
                    {
                        srcArray[i] = int.MaxValue;
                        counter--;
                    }
                }
            }
            int[] shrinkedArray = new int[counter];
            for (int i = 0, j = 0; i < exclusiveArray.Length && j < counter; i++)
            {
                if (exclusiveArray[i] != int.MaxValue)
                {
                    shrinkedArray[j++] = exclusiveArray[i];
                }
            }
            if (counter < srcArray.Length)
            {
                int[] tempY = base.y;
                base.y = new int[tempY.Length + 1];
                tempY.CopyTo(base.y, 0);
                base.y[tempY.Length] = shrinkedArray[0];
            }
        }
        /// <summary>
        ///
        /// Preconditions:
        /// Data must be initialized already
        ///
        /// </summary>
        public override int[] Any()
        {
            int[] any = base.Any();
            MonitorConcat();
            return any;
        }
        /// <summary>
        ///
        /// Preconditions:
        /// Data must be initialized already
        ///
        /// Postconditions:
        /// Mode might be set to false
        ///
        /// </summary>
        public override int Sum(uint z)
        {
            int sum = base.Sum(z);
            MonitorConcat();
            return sum;
        }
        /// <summary>
        ///
        /// Preconditions:
        /// Data must be initialized already
        ///
        /// Postconditions:
        /// Mode might be set to false
        ///
        /// </summary>
        public override int[] Target(uint z)
        {
            MonitorRequest(z);
            int[] oddX = this.SelectFrom(base.x, z, false);
            int[] evenY = this.SelectFrom(base.y, z, true);
            int[] target = new int[oddX.Length + evenY.Length];
            oddX.CopyTo(target, 0);
            evenY.CopyTo(target, oddX.Length);
            MonitorConcat();
            return target;
        }
        private void MonitorConcat()
        {
            if (base.requestCounter % this.REQUEST_FREQ == 0)
            {
                int[] tempY = base.y;
                base.y = new int[tempY.Length + 1];
                tempY.CopyTo(base.y, 0);
                base.y[tempY.Length] = (int)base.requestCounter;
            }
        }
        private int[] SelectFrom(int[] srcArray, uint count, bool even)
        {
            int evenFactor = even ? 0 : 1;
            //return srcArray.Where(e => e % 2 == evenOddFactor).Take((int)count).ToArray();
            int counter = srcArray.Length;
            int[] matchedArray = new int[srcArray.Length];
            srcArray.CopyTo(matchedArray, 0);
            for (int i = 0; i < srcArray.Length; i++)
            {
                if (srcArray[i] % 2 != evenFactor)
                {
                    matchedArray[i] = int.MaxValue;
                    counter--;
                }
            }
            counter = counter < count ? counter : (int)count;
            int[] shrinkedArray = new int[counter];
            for (int i = 0, j = 0; i < matchedArray.Length && j < counter; i++)
            {
                if (matchedArray[i] != int.MaxValue)
                {
                    shrinkedArray[j++] = matchedArray[i];
                }
            }
            return shrinkedArray;
        }
    }
}
/// <summary>
///
/// Implementation invariants - for class designer
///
/// REQUEST_FREQ: a frequency that for every a multiplication of it, the current
/// request is concatenated to the end of y.
///
/// - Constructor
///   Client will specify the lower boundaries for x and y, and a frequency
///   for concatenation.
///
/// - InitializeData()
///   This method behaves as dataExtractor::InitializeData() with additions.
///   After initializing from parent, it checks the first value that is from
///   x and not shared with y. Then, a new array with the concatenated value
///   is set back to y.
///
/// - Take(uint z)
///   This method will compute the odd values from array X then compute the even
///   values from array Y. Then it will concate two above arrays. Thus, the
///   maximum amount of values in this concatenated array should be 2*z.
///
/// - MonitorConcat()
///   Whenever n request, where n is a multiplication of REQUEST_FREQ, n will be
///   added to the end of y. The concatenation should be handled after the logic.
///
/// For more details, please check dataExtractor documentation
///
/// </summary>
