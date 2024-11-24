using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtils
{
    public static Color Blend(float value, Color firstColor, Color secondColor)
    {
        value = Mathf.Clamp(value, 0f, 1f);
        float finalH, finalS, finalV;
        Color.RGBToHSV(firstColor, out var h1, out var s1, out var v1);
        Color.RGBToHSV(secondColor, out var h2, out var s2, out var v2);
        float posHueDiff = (1f - h1) + h2, negHueDiff = posHueDiff - 1f;
        float hueDiff = (posHueDiff <= 0.5f) ? posHueDiff : negHueDiff;
        finalH = h1 + (hueDiff * value);
        if (finalH >= 1f)
        {
            finalH -= 1f;
        }
        finalS = s1 + (s2 - s1) * value;
        finalV = v1 + (v2 - v1) * value;
        return Color.HSVToRGB(finalH, finalS, finalV);
    }
}
