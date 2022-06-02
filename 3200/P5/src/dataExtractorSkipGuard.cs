namespace CPSC3200PA5
{
    public class dataExtractorSkipGuard : AGuardData
    {
        private dataExtractor _extractor;
        private skipGuard _skip;
        public dataExtractorSkipGuard(int[] seq, bool mode, int k)
        {
            _extractor = new dataExtractor(seq);
            _skip = new skipGuard(seq, mode, k);
        }

        public override int Value(int x)
        {
            return _skip.Value(x);
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
