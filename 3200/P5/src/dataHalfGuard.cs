namespace CPSC3200PA5
{
    public class dataHalfGuard : AGuardData
    {
        private dataHalf _half;
        private guard _guard;

        public dataHalfGuard(int[] seq, bool mode, int max)
        {
            _half = new dataHalf((uint)max, seq);
            _guard = new guard(seq, mode);
        }

        public override int Value(int x)
        {
            return _guard.Value(x);
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
