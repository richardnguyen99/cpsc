using System;
using System.Text;

/// <summary>
///
/// Class invariants - defined types and how to use dataExtractor
///
/// dataHalf behaves as dataExtractor with additions.Initialized array must hold
/// even values. An activation mode is maintained to determine if dataHalf object
/// is activated or not. A invalid bound is set to keep track of activation.
///
/// A deactivated object will raise exceptions when client is trying to make more
/// requests. Therefore, it's client's responsibility to not to exceed this bound.
///
/// Interface invariants - How to use methods correctly
///
/// - Constructor:
///   Constructs a dataHalf with boundaries for two arrays, along with a
///   bound for number of invalid requests before going to deactivation.
///
/// - InitializeData(int[] array):
///   This method behaves as dataExtractor::InitializeData(array) with additions;
///   passed-in array must hold even values.
///
/// - Any():
///   Request method. This method behaves as dataExtractor::Any() with additions;
///   it returns the same a cartesian product item upon two continuous Any().
///
/// - Target(uint z):
///   Request method. This method behaves as dataExtractor::Target(z).
///
/// - Sum(uint z):
///   Request method. This method behaves as dataExtractor::Sum(z).
///
/// </summary>
namespace CPSC3200PA5
{
    public class dataHalf : dataExtractor
    {
        private bool activated = true;
        private readonly uint MAX_INVALID_REQUEST;
        public dataHalf(uint max, int[] array) : base(array)
        {
            this.MAX_INVALID_REQUEST = max;
            this.InitializeData(array);
        }
        /// <summary>
        ///
        /// Precondition:
        /// - Array must have no duplicates, not enough values or no odd value.
        ///
        /// </summary>
        protected override void InitializeData(int[] array)
        {
            if (ContainsOdd(array))
            {
                throw new ArgumentException("Passed-in array cannot contain odd values", nameof(array));
            }
            this.activated = true;
        }
        /// <summary>
        ///
        /// Preconditions:
        /// Data must be initialized already
        ///
        /// </summary>
        public override int[] Any()
        {
            if (!this.activated)
            {
                throw new Exception("Object is deactivated");
            }
            uint evenRequest = this.requestCounter / 2;
            int[] array = this.ComputeCartesianItem(evenRequest);
            this.requestCounter++;
            return array;
        }
        /// <summary>
        ///
        /// Preconditions:
        /// Data must be initialized already
        ///
        /// Postconditions:
        /// Mode might be set to false
        /// Activation state might be set to deactivated
        ///
        /// </summary>
        public override int[] Target(uint z)
        {
            if (!this.activated)
            {
                throw new Exception("Object is deactivated");
            }
            int[] target = base.Target(z);
            MonitorActivation();
            return target;
        }
        /// <summary>
        ///
        /// Preconditions:
        /// Data must be initialized already
        ///
        /// Postconditions:
        /// Mode might be set to false
        /// Activation state might be set to deactivated
        ///
        /// </summary>
        public override int Sum(uint z)
        {
            if (!this.activated)
            {
                throw new Exception("Object is deactivated");
            }
            int sum = base.Sum(z);
            MonitorActivation();
            return sum;
        }
        private bool ContainsOdd(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] % 2 == 1)
                    return true;
            }
            return false;
        }
        private void MonitorActivation()
        {
            this.activated = this.invalidRequestCounter < this.MAX_INVALID_REQUEST;
        }
    }
}
/// <summary>
///
/// Implementation invariants - for class designer
///
/// activated: a state to keep track of the activation of dataHalf. If dataHalf
/// has gone to deactivated, every request function will raise an exception.
///
/// MAX_INVALID_REQUEST: a maximum amount of invalid requests that a client
/// can make before dataHalf goes to deactivated.
///
/// - Constructor
///   Client will specify the lower boundaries for x and y, and an upper bound
///   for invalid requests.
///
/// - Any()
///   The indexing is same as dataExtractor::Any(). To maintain the same item
///   on every two calls, requestCounter is divided by 2.
///
/// - MonitorActivation()
///   Whenever the maximum amount of invalid requests is reached, the activated
///   state is set to deactivated. This method should be used after the logic
///   is handled so that only the next call after exceeding raises exceptions.
///
/// For more details, please check dataExtractor documentation
///
/// </summary>
