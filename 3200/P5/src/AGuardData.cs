namespace CPSC3200PA5
{
    public abstract class AGuardData : IData, IGuard
    {
        public abstract int Value(int x);
        public abstract int[] Any();
        public abstract int[] Target(uint z);
        public abstract int Sum(uint z);
    }
}
