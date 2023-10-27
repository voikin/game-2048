namespace game_2048.PresentationLayer.helpers;

public class HSLColor
{
    // Hue, Saturation, Luminosity
    public double H { get; set; }
    public double S { get; set; }
    public double L { get; set; }

    public HSLColor(double h, double s, double l)
    {
        this.H = h;
        this.S = s;
        this.L = l;
    }

    public ConsoleColor ToConsoleColor()
    {
        if (L < 0.5)
        {
            return ConsoleColor.DarkRed;
        }
        else if (H < 30)
        {
            return ConsoleColor.Red;
        }
        else if (H < 60)
        {
            return ConsoleColor.DarkYellow;
        }
        else
        {
            return ConsoleColor.Yellow;
        }
    }
}