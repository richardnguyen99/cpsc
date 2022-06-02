namespace CPSC3200PA5
{
    public class dataExtractorGuard : AGuardData
    {
        private dataExtractor _extractor;
        private guard _guard;

        public dataExtractorGuard(int[] seq, bool mode)
        {
            _guard = new guard(seq, mode);
            _extractor = new dataExtractor((uint)seq.Length, (uint)seq.Length, seq);
        }

        public override int[] Any()
        {
            return _extractor.Any();
        }

        public override int[] Target(uint z)
        {
            return _extractor.Target(z);
        }

        public override int Sum(uint z)
        {
            return _extractor.Sum(z);
        }

        public override int Value(int x)
        {
            return _guard.Value(x);
        }
    }
}
