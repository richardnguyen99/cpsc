using System;

namespace CPSC3200PA5
{
    public class skipGuard : guard
    {
        private int unstableSkipStep;

        public skipGuard(int[] seq, bool mode, int k) : base(seq, mode)
        {
            this.unstableSkipStep = k;
        }

        public override int Value(int x)
        {
            int result = x;

            for (int i = 0; i < this.unstableSkipStep + 1 && result != -1; i++)
                result = base.Value(result);

            return result;
        }
    }
}
