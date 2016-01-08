using System;

namespace Codeaddicts.Lizard
{
    public static class IntExtensions
    {
        public static bool IsInRange (this int n, int min, int exclusive_max) {
            return n >= min && n < exclusive_max;
        }
    }
}

