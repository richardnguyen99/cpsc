using System;

/// <summary>
///
/// Class invariants - defined types and how to use quirkyGuard
///
/// quirkyGuard shares the same methods as guard but in a very quirky way. It
/// has the same intialization as guard but quirkyGuard can only switch mode
/// in a certain amount, which is specified by the client at intialization.
///
/// Interface invariants - How to use methods correctly
///
/// - int Value(int x):
///   It computes exactly like guard::Value() but after querying the value,
///   it will modify the array in some manner. If in up mode, the value is
///   added by a prime number, if the in down mode, the value is added by a
///   non-prime number.
///
/// </summary>

namespace CPSC3200PA5
{
    public class quirkyGuard : guard
    {
        private readonly int MAX_MODE_SWITCH;
        private int modeSwitchCount;

        public quirkyGuard(int[] seq, bool mode, int n) : base(seq, mode)
        {
            this.MAX_MODE_SWITCH = n;
        }

        /// <summary>
        ///
        /// Pre-conditions:
        /// - modeSwitch time must be less than the specified maximum
        ///
        /// Post-conditions:
        /// - mode might be changed if there is no such value computed.
        /// /summary>
        public override int Value(int x)
        {
            if (this.modeSwitchCount == this.MAX_MODE_SWITCH)
                return -2;

            int result = base.Value(x);
            if (result == -1)
            {
                this.modeSwitchCount++;
                return -1;
            }

            int idx = Array.IndexOf(base.s, result);
            base.s[idx] += base.mode ? result : base.s.Length * 2;

            return result;
        }
    }
}

/// <summary>
///
/// - Value()
///   The concatenation after computing the value depends on the current mode.
///   If in up mode, the value will be appended to itself since if there is a
///   successfuly query, the value will be a prime number. If in down mode, it
///   add the mulplication of the array length and 2. The error code -2 means
///   the maximum amount of mode switching is reached, thus no further query.
///
/// </summary>
