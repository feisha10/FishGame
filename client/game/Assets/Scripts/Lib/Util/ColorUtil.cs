using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public static class ColorEnum
{
    static public Color White = ColorUtil.RGBToColor(0xffffff);
    static public Color Green = ColorUtil.RGBToColor(0x12f08f);
    static public Color SetGreen = ColorUtil.RGBToColor(0x72FF5F);
    static public Color Red = ColorUtil.RGBToColor(0xe61808);
    static public Color Black = ColorUtil.RGBToColor(0x000000);

    static public Color RoleLev = ColorUtil.RGBToColor(0x32191c);
    static public Color Blue1 = ColorUtil.RGBToColor(0x2bbcff);
    static public Color Blue2 = ColorUtil.RGBToColor(0x28354f);
    static public Color Yellow = ColorUtil.RGBToColor(0xfdd122);
    static public Color Purple = ColorUtil.RGBToColor(0xd582f8);
    static public Color Orange = ColorUtil.RGBToColor(0xf6b144);
    static public Color GreenOut = ColorUtil.RGBToColor(0x195e21);
    static public Color PurpleOut = ColorUtil.RGBToColor(0x7a0576);
    static public Color OrangeOut = ColorUtil.RGBToColor(0xa22200);
    static public Color Blue1Out = ColorUtil.RGBToColor(0x183270);

    static public Color Blue2Out = ColorUtil.RGBToColor(0x09007c);  //据点提示语描边
    static public Color Red2Out = ColorUtil.RGBToColor(0x360101);   //据点提示语描边
}

public class ColorUtil {

    /// <summary>
    /// Convert the specified RGB24 integer to Color.
    /// </summary>
    public static Color RGBToColor(int color)
    {
        float inv = 1f / 255f;
        Color c = Color.black;
        c.r = inv * ((color >> 16) & 0xFF);
        c.g = inv * ((color >> 8) & 0xFF);
        c.b = inv * ((color ) & 0xFF);
        return c;
    }

    public static Color ParseColor(string color)
    {
        return ParseColor(color, 0);
    }

    /// <summary>
    /// Parse a RrGgBb color encoded in the string.
    /// </summary>

    static public Color ParseColor(string text, int offset)
    {
        int r = (HexToDecimal(text[offset]) << 4) | HexToDecimal(text[offset + 1]);
        int g = (HexToDecimal(text[offset + 2]) << 4) | HexToDecimal(text[offset + 3]);
        int b = (HexToDecimal(text[offset + 4]) << 4) | HexToDecimal(text[offset + 5]);
        float f = 1f / 255f;
        return new Color(f * r, f * g, f * b);
    }

    /// <summary>
    /// Convert a hexadecimal character to its decimal value.
    /// </summary>
    static public int HexToDecimal(char ch)
    {
        switch (ch)
        {
            case '0': return 0x0;
            case '1': return 0x1;
            case '2': return 0x2;
            case '3': return 0x3;
            case '4': return 0x4;
            case '5': return 0x5;
            case '6': return 0x6;
            case '7': return 0x7;
            case '8': return 0x8;
            case '9': return 0x9;
            case 'a':
            case 'A': return 0xA;
            case 'b':
            case 'B': return 0xB;
            case 'c':
            case 'C': return 0xC;
            case 'd':
            case 'D': return 0xD;
            case 'e':
            case 'E': return 0xE;
            case 'f':
            case 'F': return 0xF;
        }
        return 0xF;
    }

    /// <summary>
    /// Convert a decimal value to its hex representation.
    /// It's coded because num.ToString("X6") syntax doesn't seem to be supported by Unity's Flash. It just silently crashes.
    /// string.Format("{0,6:X}", num).Replace(' ', '0') doesn't work either. It returns the format string, not the formatted value.
    /// </summary>

    [System.Diagnostics.DebuggerHidden]
    [System.Diagnostics.DebuggerStepThrough]
    static public string DecimalToHex(int num)
    {
        num &= 0xFFFFFF;
#if UNITY_FLASH
		StringBuilder sb = new StringBuilder();
		sb.Append(DecimalToHexChar((num >> 20) & 0xF));
		sb.Append(DecimalToHexChar((num >> 16) & 0xF));
		sb.Append(DecimalToHexChar((num >> 12) & 0xF));
		sb.Append(DecimalToHexChar((num >> 8) & 0xF));
		sb.Append(DecimalToHexChar((num >> 4) & 0xF));
		sb.Append(DecimalToHexChar(num & 0xF));
		return sb.ToString();
#else
        return num.ToString("X6");
#endif
    }

    /// <summary>
    /// 转化对应的Color至RGB字符串形式，如“FFFFFF”
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string ColorToHex(Color color)
    {
        int retVal = 0xFFFFFF & (ColorToInt(color) >> 8);
        return DecimalToHex(retVal);
    }

    /// <summary>
    /// Convert the specified color to RGBA32 integer format.
    /// </summary>

    static public int ColorToInt(Color c)
    {
        int retVal = 0;
        retVal |= Mathf.RoundToInt(c.r * 255f) << 24;
        retVal |= Mathf.RoundToInt(c.g * 255f) << 16;
        retVal |= Mathf.RoundToInt(c.b * 255f) << 8;
        retVal |= Mathf.RoundToInt(c.a * 255f);
        return retVal;
    }

    /// <summary>
    /// Convert the specified RGBA32 integer to Color.
    /// </summary>

    static public Color IntToColor(int val)
    {
        float inv = 1f / 255f;
        Color c = Color.black;
        c.r = inv * ((val >> 24) & 0xFF);
        c.g = inv * ((val >> 16) & 0xFF);
        c.b = inv * ((val >> 8) & 0xFF);
        c.a = inv * (val & 0xFF);
        return c;
    }

    public static string GetStringWithColor(string name, Color color)
    {
        //<color=#72FF5FFF>X/Y</color>
        return "<color=#" + ColorToHex(color) + ">" + name + "</color>";
    }

}
