using UnityEngine;

/// <summary>
/// A script to translate a given wavelength to a color
/// </summary>
public class LaserHelperFunctions
{
    /*
     * https://stackoverflow.com/questions/1472514/convert-light-frequency-to-rgb
     */

    /// <summary>
    /// The function receives a wavelength and returns a corresponding color
    /// </summary>
    /// <param name="wavelength">The wavelength to convert into a color</param>
    /// <returns>The color based on the wavelength</returns>
    public static Color RgbFromWavelength(float wavelength)
    {

        float red, green, blue;
        float factor;

        if (wavelength >= 380 && wavelength < 440)
        {
            red = -(wavelength - 440) / (440 - 380);
            green = 0f;
            blue = 0f;
        }
        else if (wavelength >= 440 && wavelength < 490)
        {
            red = 0f;
            green = (wavelength - 440) / (490 - 440);
            blue = 1f;
        }
        else if (wavelength >= 490 && wavelength < 510)
        {
            red = 0f;
            green = 1f;
            blue = -(wavelength - 510) / (510 - 490);
        }
        else if (wavelength >= 510 && wavelength < 580)
        {
            red = (wavelength - 510) / (580 - 510);
            green = 1f;
            blue = 0f;
        }
        else if (wavelength >= 580 && wavelength < 645)
        {
            red = 1f;
            green = -(wavelength - 645) / (645 - 580);
            blue = 0f;
        }
        else if (wavelength >= 645 && wavelength < 781)
        {
            red = 1f;
            green = 0f;
            blue = 0f;
        }
        else
        {
            red = 0f;
            green = 0f;
            blue = 0f;
        }

        if (wavelength >= 380 && wavelength < 420)
        {
            factor = 0.3f + 0.7f * (wavelength - 380f) / (420f - 380f);
        }
        else if (wavelength >= 420 && wavelength < 701)
        {
            factor = 1f;
        }
        else if (wavelength >= 701 && wavelength < 781)
        {
            factor = 0.3f + 0.7f * (780 - wavelength) / (780 - 700);
        }
        else
        {
            factor = 0f;
        }

        float realRed = red == 0f ? 0f : Mathf.Pow(red * factor, 0.8f);
        float realGreen = green == 0f ? 0f : Mathf.Pow(green * factor, 0.8f);
        float realBlue = blue == 0f ? 0f : Mathf.Pow(blue * factor, 0.8f);

        return new Color(realRed, realGreen, realBlue, 1f);
    }
}
