namespace CPSC3200PA5
{
    public class dataHalfSkipGuard : AGuardData
    {
        private dataHalf _half;
        private skipGuard _skip;

        public dataHalfSkipGuard(int[] seq, bool mode, int k)
        {
            _half = new dataHalf((uint)k, seq);
            _skip = new skipGuard(seq, mode, k);
        }

        public override int Value(int x)
        {
            return _skip.Value(x);
        }

        public override int[] Any()
        {
            return _half.Any();
        }

        public override int[] Target(uint z)
        {
            return _half.Target(z);
        }

        public override int Sum(uint z)
        {
            return _half.Sum(z);
        }
    }
}
