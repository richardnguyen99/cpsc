namespace CPSC3200PA5
{
    public class dataExtractorQuirkyGuard : AGuardData
    {
        private dataExtractor _extractor;
        private quirkyGuard _quirky;
        public dataExtractorQuirkyGuard(int[] seq, bool mode, int n)
        {
            _extractor = new dataExtractor(seq);
            _quirky = new quirkyGuard(seq, mode, n);
        }

        public override int Value(int x)
        {
            return _quirky.Value(x);
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
    }
}
