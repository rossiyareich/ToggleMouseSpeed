namespace ToggleMouseSpeed;

public class AppSettings
{
    public string? SlowIconPath { get; set; }
    public string? NormalIconPath { get; set; }
    public string? FastIconPath { get; set; }
    public int SlowSpeed { get; set; }
    public int NormalSpeed { get; set; }
    public int FastSpeed { get; set; }
    public int DefaultToIndex { get; set; }
    public string? Cycle { get; set; }

    public int GetSpeedAtIndex(int i)
    {
        if (i == 0)
        {
            return SlowSpeed;
        }

        if (i == 1)
        {
            return NormalSpeed;
        }

        if (i == 2)
        {
            return FastSpeed;
        }

        throw new ArgumentOutOfRangeException();
    }

    public string? GetIconAtIndex(int i)
    {
        if (i == 0)
        {
            return SlowIconPath;
        }

        if (i == 1)
        {
            return NormalIconPath;
        }

        if (i == 2)
        {
            return FastIconPath;
        }

        throw new ArgumentOutOfRangeException();
    }
}
