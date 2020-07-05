using System;
using сomputorV1.Exception;

namespace сomputorV1.Polynomial
{
    public static class StringChecker
    {
        public static void CheckInput(string input)
        {
            var inputs = input.Split('=');
            if (inputs.Length != 2)
                throw new ExceptionStringFormat($"{input}. Error with \'=\' split.");
            foreach (var s in inputs)
            {
                Correct(s);
            }
        }

        private static void Correct(string s)
        {
            var state = States.Start;
            for (var i = 0; i < s.Length; i++)
            {
                var c = s[i];
                state = GetNextState(state, c, out var nextChar);
                if (!nextChar)
                    i--;
                if (state == States.Error)
                {
                    throw new ExceptionStringFormat($"\n{new String(' ', i)}{s}\n");
                }
            }
        }

        private static States GetNextState(States state, char c, out bool nextChar)
        {
            nextChar = true;
            switch (state)
            {
                case States.Start:
                    if (IsPlusMinus(c)) return States.S1;
                    if (IsNumeric(c)) return States.S2;
                    if (IsDot(c)) return States.S3;
                    if (IsX(c)) return States.S6;
                    break;
                case States.Next:
                    if (IsPlusMinus(c)) return States.S1;
                    break;
                case States.S1:
                    if (IsNumeric(c)) return States.S2;
                    if (IsDot(c)) return States.S3;
                    if (IsX(c)) return States.S6;
                    break;
                case States.S2:
                    if (IsNumeric(c)) return States.S2;
                    if (IsDot(c)) return States.S3;
                    if (IsStar(c)) return States.S5;
                    if (IsX(c)) return States.S6;
                    nextChar = false;
                    return States.Next;
                case States.S3:
                    if (IsNumeric(c)) return States.S4;
                    break;
                case States.S4:
                    if (IsNumeric(c)) return States.S4;
                    if (IsStar(c)) return States.S5;
                    if (IsX(c)) return States.S6;
                    nextChar = false;
                    return States.Next;
                case States.S5:
                    if (IsX(c)) return States.S6;
                    break;
                case States.S6:
                    if (IsUp(c)) return States.S7;
                    nextChar = false;
                    return States.Next;
                case States.S7:
                    if (IsNumeric(c)) return States.S7;
                    nextChar = false;
                    return States.Next;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            return States.Error;
        }

        private enum States
        {
            Start,
            S1,
            S2,
            S3,
            S4,
            S5,
            S6,
            S7,
            Next,
            Error
        }

        #region Is...

        private static bool IsPlusMinus(char c)
        {
            return "+-".Contains(c.ToString());
        }

        private static bool IsNumeric(char c)
        {
            return "0123456789".Contains(c.ToString());
        }

        private static bool IsDot(char c)
        {
            return ".".Contains(c.ToString());
        }

        private static bool IsStar(char c)
        {
            return "*".Contains(c.ToString());
        }

        private static bool IsUp(char c)
        {
            return "^".Contains(c.ToString());
        }

        private static bool IsX(char c)
        {
            return "X".Contains(c.ToString());
        }

        #endregion
    }
}