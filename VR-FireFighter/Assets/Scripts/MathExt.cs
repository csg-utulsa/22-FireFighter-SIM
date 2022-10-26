using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExt
{
    public static bool IsWholeNum(float fl) {
        return (fl == (int)fl);
    }

    public static float Wrap(float val, float min, float max) {
        if (val < min) {
            return max - (min - val) % (max - min);
        }
        else {
            return min + (val - min) % (max - min);
        }
    }
}
