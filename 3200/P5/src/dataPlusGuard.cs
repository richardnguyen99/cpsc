namespace CPSC3200PA5
{
    public class dataPlusGuard : AGuardData
    {
        private dataPlus _plus;
        private guard _guard;

        public dataPlusGuard(int[] seq, bool mode, int kth)
        {
            _plus = new dataPlus((uint)kth, seq);
            _guard = new guard(seq, mode);
        }

        public override int Value(int x)
        {
            return _guard.Value(x);
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
