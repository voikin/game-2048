namespace game_2048.PresentationLayer.helpers;

public class RGBToHSL
{
    public HSLColor Convert(int r, int g, int b)
    {
        double _r = r / 255.0;
        double _g = g / 255.0;
        double _b = b / 255.0;

        double max = Math.Max(_r, Math.Max(_g, _b));
        double min = Math.Min(_r, Math.Min(_g, _b));
        double h, s, l;
        l = (max + min) / 2.0;

        if (max == min)
        {
            h = s = 0;   // achromatic
        }
        else
        {
            double d = max - min;
            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
            if (max == _r)
            {
                h = (_g - _b) / d + (_g < _b ? 6 : 0);
            }
            else if (max == _g)
            {
                h = (_b - _r) / d + 2;
            }
            else
            {
                h = (_r - _g) / d + 4;
            }
            h /= 6;
        }

        return new HSLColor(h,s,l);
    }
}