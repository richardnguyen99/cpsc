using System;

/// <summary>
///
/// Class invariants - defined types and how to use dataExtractor
///
/// dataExtractor encapsulates two arrays, x and y, and allows users to call
/// available requests to extract data from them. Array X is responsible for
/// the actual data passed by client while array Y is responsible for
/// generated data.
///
/// Initially, array x will be used mainly in every request. However, array y
/// will be used instead if client has sent enough invalid requests.
///
/// Client must set the boundaries that X and Y can hold values at minimum, then
/// initialize the two arrays by dependency injection (initializeData)
///
/// dataExtractor keeps track of how many request, including invalid one, that
/// client has sent internally. As mentioned above, it will used to determine
/// which array will be used. Once the state is changed, it cannot be reset.
/// Therefore, it's client's responsibility to keep track of their requests.
///
/// Request is only increased when a request function is used.
/// See interface invariants to know which function is a request function.
///
/// Interface invariants - How to use methods correctly
///
/// - Constructor:
///   Constructs a dataExtractor with boundaries for two arrays
///
/// - InitializeData(int[] array):
///   Load data from client, and must be used before any other request function.
///   The array must not contain any duplicate. Otherwise, an exception will be
///   thrown.
///
/// - Any():
///   Request function. Any() will return an item computed from Cartesian product
///   of x and y.
///   Data must be initialized before calling.
///
/// - Target(uint z):
///   Request function. Target(z) will return an array of z items selected from
///   the coresponding array. Returned items are only either odd or even depending
///   on z.
///   An invalid request is increased when z is either less than the boundaries or
///   greater than the length of at least one array.Data must be initialized before
///   calling.
///
/// - Sum(uint z):
///   Request function. Sum(z) will return a sum of z items selected from the
///   coresponding Target(z).
///   An invalid request is increased when z is either less than the boundaries or
///   greater than the length of at least one array.Data must be initialized before
///   calling.
///
/// </summary>
namespace CPSC3200PA5
{
    public class dataExtractor : IData
    {
        private readonly uint MIN_XLENGTH;
        private readonly uint MIN_YLENGTH;
        protected bool mode = true;
        protected int[] x;
        protected int[] y;
        protected uint requestCounter;
        protected uint invalidRequestCounter;

        public dataExtractor(int[] array)
        {
            this.MIN_XLENGTH = (uint)(GetLastOddFrom(array) / array.Length);
            this.MIN_YLENGTH = (uint)(this.MIN_XLENGTH + 2);

            this.InitializeData(array);
        }
        /// <summary>
        ///
        /// Precondition:
        /// - Array must have no duplicates or not enough values
        ///
        /// </summary>
        protected virtual void InitializeData(int[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Passed-in array cannot be empty");

            if (ContainsDuplicates(array))
            {
                throw new ArgumentException("Passed-in array cannot contain duplicates", nameof(array));
            }
            if (array.Length < this.MIN_XLENGTH)
            {
                throw new ArgumentException("Passed-in array does not match with the defined minimum size", nameof(array.Length));
            }
            this.requestCounter = 0;
            this.invalidRequestCounter = 0;
            this.x = new int[array.Length];
            array.CopyTo(this.x, 0);
            this.y = new int[MIN_YLENGTH];
            for (int i = 0; i < MIN_YLENGTH; i++)
            {
                this.y[i] = this.x[0] + (i * 3);
            }
        }
        /// <summary>
        ///
        /// Preconditions:
        /// Data must be initialized already
        ///
        /// </summary>
        public virtual int[] Any()
        {
            int[] array = ComputeCartesianItem(this.requestCounter);
            requestCounter++;
            return array;
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
        public virtual int[] Target(uint z)
        {
            MonitorRequest(z);
            this.requestCounter++;
            MonitorMode();
            return SelectFrom(mode ? this.x : this.y, z);
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
        public virtual int Sum(uint z)
        {
            MonitorRequest(z);
            requestCounter++;
            MonitorMode();
            return SumFrom(mode ? this.x : this.y, z);
        }
        protected int[] ComputeCartesianItem(uint index)
        {
            uint row = (uint)(index / this.x.Length % this.y.Length);
            uint col = (uint)(index % this.x.Length);
            int[] array =
            {
                this.x[col],
                this.y[row]
            };
            return array;
        }
        protected void MonitorRequest(uint z)
        {
            if (z < this.MIN_XLENGTH || z < this.MIN_YLENGTH || z > this.x.Length || z > this.y.Length)
            {
                this.invalidRequestCounter++;
            }
        }
        private int[] SelectFrom(int[] srcArray, uint count)
        {
            uint evenOddFactor = count % 2;
            //return srcArray.Where(e => e % 2 == evenOddFactor).Take((int)count).ToArray();
            int counter = srcArray.Length;
            int[] matchedArray = new int[srcArray.Length];
            srcArray.CopyTo(matchedArray, 0);
            for (int i = 0; i < srcArray.Length; i++)
            {
                if (srcArray[i] % 2 != evenOddFactor)
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
        private int SumFrom(int[] srcArray, uint count)
        {
            // return SelectFrom(srcArray, count).Sum();
            int[] target = this.SelectFrom(srcArray, count);
            int sum = 0;
            for (int i = 0; i < target.Length; i++)
            {
                sum += target[i];
            }
            return sum;
        }
        private bool ContainsDuplicates(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[i] == arr[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private int GetLastOddFrom(int[] array)
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

        private void MonitorMode()
        {
            this.mode = this.invalidRequestCounter < (this.MIN_XLENGTH + this.MIN_YLENGTH);
        }
    }
}
/// <summary>
///
/// Implementation invariants - for class designer
///
/// MIN_XLENGTH, MIN_YLENGTH: boundaries for the minimum amount of values that
/// x and y can hold accordingly
///
/// mode: check flag to determine which array is being used. (true: x, false: y)
/// Initially, mode is set to true until invalid requests are matched. mode will
/// be handled in MonitorMode() in request functions that might cause invalid
///
/// x: actual data initialized by client. Multiple checks are performed upon the
/// passed-in array to check for duplicates and enough range.
///
/// y: generated data based on the first value from x. Each value is increased by
/// 3 from the previous one until it hits MIN_YLENGTH
///
/// requestCounter: record of how many request has been sent by client
///
/// invalidRequestCounter: record of how many invalid has been made by client.
/// MonitorRequest() will handle when it should be increased.
///
/// Both request counters must be handled before any logic while mode should be
/// handled before the return statement in request functions.
///
/// - Constructor
///   Client will specify the lower boundaries for x and y. This allows the
///   simplicity between setting boundaries and actual initializing data.
///
/// - InitializeData(int[] array)
///   Exceptions should be thrown to terminate the program as soon as clients
///   initialize illegal data. Clients can use InitializeData() multiple times
///   so resetting all fields is necessary.
///
/// - Any()
///   Only index to the Caresian product, xXy, is computed to reduce time
///   complexity of such computation. The indexing uses requestCounter to
///   get the corresponding item in the product. Any() doesn't check for
///   invalid request since it always produces the correct item. It's the client
///   who is responsible for initializing data before calling.
///
/// - Target(uint z)
///   Logic is handled by SelectFrom(). Target() checks for invalid requests
///   since an invalid request might be caused by client if z violates the
///   conditions set in MonitorRequest().
///
/// - Sum(uint z)
///   Logic is handled by SelectFrom(). Sum() checks for invalid requests since
///   an invalid request might be caused by client if z violates the conditions
///   set in MonitorRequest()
///
/// - SelectFrom()
///   It use int.MaxValue to indicate positions that are not qualified. Then,
///   a shrinking operation is necessary to get the array with the correctly-
///   selected items without extra space.
///
/// - MonitorRequest()
///   Whenever a request lower than at least one of the boundaries or at least
///   one of the lengths, invalidRequest is increased.
///
/// - MonitorMode()
///   Whenever invalid requests are matched with the sum of both boundaries,
///   mode is set to false for once.
///
/// </summary>
