using System;
using System.Drawing;
using MediaPortal.Plugins;

internal static class PointerSeekGeometry
{
    private static int checks;
    private static void Expect(Rectangle bounds, int x, int y, bool accepted, double expected)
    {
        double fraction;
        bool actual = PointerSeek.TryGetFraction(bounds, x, y, out fraction);
        if (actual != accepted || (actual && Math.Abs(fraction - expected) > 0.000001))
            throw new Exception("Unexpected pointer seek hit/fraction at " + x + "," + y);
        checks++;
    }

    public static void Main()
    {
        Rectangle standard = new Rectangle(100, 870, 850, 15);
        Expect(standard, 100, 877, true, 0);
        Expect(standard, 525, 877, true, 0.5);
        Expect(standard, 950, 877, true, 1);
        Expect(standard, 99, 877, false, 0);
        Expect(standard, 951, 877, false, 0);
        Expect(standard, 525, 861, false, 0);
        Expect(standard, 525, 894, false, 0);
        Expect(standard, 525, 862, true, 0.5);
        Expect(standard, 525, 893, true, 0.5);
        Expect(new Rectangle(40, 200, 400, 24), 140, 210, true, 0.25);
        Expect(new Rectangle(100, 870, 0, 15), 100, 877, false, 0);
        Expect(new Rectangle(100, 870, 850, 0), 525, 870, false, 0);
        Console.WriteLine("Pointer seek geometry: " + checks + "/" + checks + " passed");
    }
}
