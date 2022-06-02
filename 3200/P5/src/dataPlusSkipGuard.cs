namespace CPSC3200PA5
{
    public class dataPlusSkipGuard : AGuardData
    {
        private dataPlus _plus;
        private skipGuard _skip;

        public dataPlusSkipGuard(int[] seq, bool mode, int kth)
        {
            _plus = new dataPlus((uint)kth, seq);
            _skip = new skipGuard(seq, mode, kth);
        }

        public override int Value(int x)
        {
            return _skip.Value(x);
        }

        public override int[] Any()
        {
            return _plus.Any();
        }

        public override int[] Target(uint z)
        {
            return _plus.Target(z);
        }

        public override int Sum(uint z)
        {
            return _plus.Sum(z);
        }
    }
}
