using System;

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
