using System;

/// <summary>
///
/// Class invariants - defined types and how to use guard
///
/// guard class will take two arguments, an array of integers and a bool variable
/// to represent its mode. The array, s, is a stable array.
///
/// S array will be sorted after initialization to make it 'stable' for later
/// computation. The class also keeps track of its current mode; when Value()
/// returns -1, which is an error code (see more at Value()), mode switch is
/// toggled.
///
/// A mode is up if the boolean variable is true, or down if otherwise.
///
/// Interface invariants - How to use methods correctly
///
/// - int Value(int x)
///   Computes the smallest prime in s that is larger than x if in up mode, or
///   the largest prime in s that is smaller than x if otherwise.
///   If there is no such value, mode will be flipped.
///
/// </summary>

namespace CPSC3200PA5
{
    public class guard : IGuard
    {
        protected int[] s;
        protected bool mode;

        public guard(int[] seq, bool mode)
        {
            if (seq == null || seq.Length == 0)
                throw new ArgumentException("Passed-in array cannot be empty");

            this.s = new int[seq.Length];
            int[] sortedSeq = MergeSort(seq);

            for (int i = 0; i < seq.Length; i++)
            {
                this.s[i] = sortedSeq[i];
            }

            this.mode = mode;
        }

        // Post-conditions:
        // - Mode might be changed if there is no such value
        public virtual int Value(int x)
        {
            int result = this.mode ? GetSmallestPrimeGreaterThan(x) : GetLargestPrimeLessThan(x);

            if (result == -1) this.mode = !this.mode;

            return result;
        }

        private int GetSmallestPrimeGreaterThan(int x)
        {
            int stopIdx = this.s.Length - 1;
            int smallestPrime = -1;

            for (; (stopIdx >= 0 && this.s[stopIdx] > x); stopIdx--)
            {
                if (IsPrime(this.s[stopIdx])) smallestPrime = this.s[stopIdx];
            }

            return smallestPrime;
        }

        private int GetLargestPrimeLessThan(int x)
        {
            int stopIdx = 0;
            int largestPrime = -1;

            for (; (stopIdx < this.s.Length && this.s[stopIdx] < x); stopIdx++)
            {
                if (IsPrime(this.s[stopIdx])) largestPrime = this.s[stopIdx];
            }

            return largestPrime;
        }

        private bool IsPrime(int x)
        {
            if (x <= 1) return false;

            if (x == 2) return true;

            for (int i = 2; i < (Math.Sqrt(x) + 1); i++)
                if (x % i == 0) return false;

            return true;
        }

        private int[] MergeSort(int[] seq)
        {
            if (seq.Length <= 1) return seq;

            int[] left;
            int[] right;
            int[] result = new int[seq.Length];

            int mid = seq.Length / 2;
            left = new int[mid];

            if (seq.Length % 2 == 0)
                right = new int[mid];
            else
                right = new int[mid + 1];

            for (int i = 0; i < mid; i++)
                left[i] = seq[i];

            for (int x = 0, i = mid; i < seq.Length; i++)
                right[x++] = seq[i];

            left = MergeSort(left);
            right = MergeSort(right);

            result = Merge(left, right);

            return result;
        }

        private int[] Merge(int[] left, int[] right)
        {
            if (left == null || left.Length == 0)
                return right;
            if (right == null || right.Length == 0)
                return left;

            int totalLength = left.Length + right.Length;
            int[] result = new int[totalLength];

            int leftIdx = 0, rightIdx = 0, resultIdx = 0;
            for (; leftIdx < left.Length; resultIdx++)
            {
                if (rightIdx == right.Length)
                {
                    for (; leftIdx < left.Length; leftIdx++)
                    {
                        result[resultIdx++] = left[leftIdx];
                    }
                    break;
                }

                if (left[leftIdx] < right[rightIdx])
                    result[resultIdx] = left[leftIdx++];
                else
                    result[resultIdx] = right[rightIdx++];
            }

            for (; rightIdx < right.Length; rightIdx++)
                result[resultIdx++] = right[rightIdx];

            return result;
        }
    }
}

/// <summary>
///
/// Implementation invariants - for class designer
///
/// The main idea to make the main computational method, Value(), reduce the
/// the computing time for the corresponding prime is to sort the intial array.
/// So, it guarantees the time complexity for Value() in worst case is linear.
/// It also makes the code complexity of Value() more readable and understandable.
///
/// s: the array for encapsulating the array passed-in by client. After initial,
/// it should be sorted.
///
/// mode: the up or down state of class, mainly used to instruct the Value() method.
///
/// - Value()
///   The main method for this class but it's just wrapper for other methods to
///   actually compute the desired value.
///
/// - GetSmallestPrimeGreaterThan(x)
///   Compute the smallest value in s that is still greater than x. Note that
///   it doesn't return the value if they're equal.
///
/// - GetLargestPrimeLessThan(x)
///   Compute the largest value in s that is still less than x. Note that it
///   doesn't return the value if they're equal
///
/// - IsPrime(x)
///   Check if the specified number is a prime or not. The reason why the loop
///   only counts to square root of x is to reduce the complexity. For example,
///   16 = 4*4 so there is no need to loop over 5-16.
///
/// - MergeSort()
///   Merge sort is used to sort the array passed in by the client. It returns
///   a newly-sorted array rather than modifying the old one. Merge sort is
///   employed to get the best time complexity in worst case.
///
/// - Merge()
///   Standard merging helper function to support MergeSort(). The core idea is
///   that it keeps comparing and merging the right-ordered value to the new list
///   until one of two sorted list is exhaustive. A final coping from the remaining
///   of the inexhaustive one to the returned list is performed at the end.
///
/// </summary>
