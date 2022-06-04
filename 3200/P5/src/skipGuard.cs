using System;


/// <summary>
///
/// Class invariants - defined types and how to use skipGuard
///
/// skipGuard shares most characteristics as guard except now, skipGuard has some
/// constraints in the Value(). See more details in Value().
///
/// Interface invariants - How to use methods correctly
///
/// - int Value(int x)
///   It computes like guard::Value() but instead of returning after first call,
///   the function will skip k steps, which is specified in the constructor by
///   the client, as the matching-requirements value to the final one. An error
///   code, -1, is returned if there is no such value.
///   Example:
///   [2,3,4,5,6,7], mode = up
///   k = 0, Value(4) = 5
///   k = 1, Value(4) = 7
///   k = 2, Value(4) = -1
///
/// </summary>
namespace CPSC3200PA5
{
    public class skipGuard : guard
    {
        private int unstableSkipStep;

        public skipGuard(int[] seq, bool mode, int k) : base(seq, mode)
        {
            this.unstableSkipStep = k;
        }

        // post-conditions:
        // - Mode might be changed if there is no such value
        public override int Value(int x)
        {
            int result = x;

            for (int i = 0; i < this.unstableSkipStep + 1 && result != -1; i++)
                result = base.Value(result);

            return result;
        }
    }
}

/// <summary>
///
/// Implementation invariants - for class designer
///
/// There is no special detail in designing but a bit explantion on Value()
///
/// Value() repeatedly computes guard::Value() based on the previous result it
/// has computed until the k step is matched. If the previous result yields a
/// non-error value, the value is a prime, less or greater than depeding on the
/// current mode. If it encounters an error code, the error code is returned
/// immediately.
///
/// </summary>
