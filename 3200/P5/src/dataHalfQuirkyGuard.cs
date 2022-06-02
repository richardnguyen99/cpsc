namespace CPSC3200PA5
{
    public class dataHalfQuirkyGuard : AGuardData
    {
        private dataHalf _half;
        private quirkyGuard _quirky;

        public dataHalfQuirkyGuard(int[] seq, bool mode, int n)
        {
            _half = new dataHalf((uint)n, seq);
            _quirky = new quirkyGuard(seq, mode, n);
        }

        public override int Value(int x)
        {
            return _quirky.Value(x);
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
