namespace CPSC3200PA5
{
    public class dataPlusQuirkyGuard : AGuardData
    {
        private dataPlus _plus;
        private quirkyGuard _quirky;

        public dataPlusQuirkyGuard(int[] seq, bool mode, int k)
        {
            _plus = new dataPlus((uint)k, seq);
            _quirky = new quirkyGuard(seq, mode, k);
        }

        public override int Value(int x)
        {
            return _quirky.Value(x);
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
