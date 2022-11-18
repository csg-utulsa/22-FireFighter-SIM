using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stringext : MonoBehaviour
{
    // for making a number a required string length
    public static string NumberPad(int number, int digits, bool add_to_front = true) {
        string str = number.ToString();
        while (str.Length < digits) {
            if (add_to_front) {
                str = "0" + str;
            }
            else {
                str = str + "0";
            }
        }
        return str;
    }

    public static bool StringIsSpecialChar(string str, bool blankIsSpecial = false) {
        if (blankIsSpecial) {
            return !(char.IsLetterOrDigit(char.Parse(str)));
        } else {
            return !(char.IsLetterOrDigit(char.Parse(str)) || str == " ");
        }
        //return !(char.IsLetterOrDigit(char.Parse(str)) || str == " ");
        //return (str == " " || str == "." || str == "!" || str == "?" || str == "," || str == ";" || str == "~" || "");
    }
}
