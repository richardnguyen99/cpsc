using System;

namespace CPSC3200PA5
{
    public class quirkyGuard : guard
    {
        private readonly int MAX_MODE_SWITCH;
        private int modeSwitchCount;

        public quirkyGuard(int[] seq, bool mode, int n) : base(seq, mode)
        {
            this.MAX_MODE_SWITCH = n;
        }

        public override int Value(int x)
        {
            if (this.modeSwitchCount == this.MAX_MODE_SWITCH)
                return -2;

            int result = base.Value(x);
            if (result == -1)
            {
                this.modeSwitchCount++;
                return -1;
            }

            int idx = Array.IndexOf(base.s, result);
            base.s[idx] += base.mode ? result : base.s.Length * 2;

            return result;
        }
    }
}
