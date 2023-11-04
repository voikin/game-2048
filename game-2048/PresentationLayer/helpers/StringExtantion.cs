using System.Text;

namespace game_2048.PresentationLayer.helpers;

public static class StringExtension
{
    public static string Repeat(this string a, int b)
    {
        StringBuilder st = new(4);
        for (var i = 0; i < b; i++)
        {
            st.Append(a);
        }

        return st.ToString();
    }
}