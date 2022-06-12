using NHotkey;
using NHotkey.WindowsForms;

namespace ToggleMouseSpeed;

public class SystemTrayApp
{
    public const string Version = "v1.0.0";
    public const string AppName = "ToggleMouseSpeed";

    private readonly AppSettings _appSettings;
    private readonly ToolStripLabel _currentSpeedLabel;

    public SystemTrayApp(AppSettings appSettings)
    {
        _appSettings = appSettings;
        _currentSpeedLabel = new ToolStripLabel {ForeColor = Color.Gray};

        CachedIcons = new Dictionary<int, Icon>();
        CachedIcons[0] = new Icon(appSettings.GetIconAtIndex(0)!);
        CachedIcons[1] = new Icon(appSettings.GetIconAtIndex(1)!);
        CachedIcons[2] = new Icon(appSettings.GetIconAtIndex(2)!);

        TrayIcon = new NotifyIcon();
        TrayIcon.Text = AppName;

        SetSpeedToIndex(appSettings.DefaultToIndex);

        Keys keyCombination = Keys.None;
        try
        {
            keyCombination = GetKeysFromString(appSettings.Cycle!);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Exit(-1);
        }

        HotkeyManager.Current.AddOrReplace("Cycle", keyCombination, true, OnCyclePressed);

        MenuStrip = new ContextMenuStrip();
        MenuStrip.Items.Add(new ToolStripLabel($"{AppName} {Version}") {ForeColor = Color.Gray});
        MenuStrip.Items.Add(new ToolStripSeparator());
        MenuStrip.Items.Add(new ToolStripLabel($"Cycle keybind: {appSettings.Cycle}") {ForeColor = Color.Gray});
        MenuStrip.Items.Add(
            new ToolStripLabel(
                $"Speed increments: {appSettings.SlowSpeed}, {appSettings.NormalSpeed}, {appSettings.FastSpeed}")
            {
                ForeColor = Color.Gray
            });
        MenuStrip.Items.Add(
            new ToolStripLabel($"Default speed: {appSettings.GetSpeedAtIndex(appSettings.DefaultToIndex)}")
            {
                ForeColor = Color.Gray
            });
        MenuStrip.Items.Add(_currentSpeedLabel);
        MenuStrip.Items.Add(new ToolStripSeparator());
        MenuStrip.Items.Add(
            new ToolStripButton("Exit", null, (_, _) => Exit(-1)) {DisplayStyle = ToolStripItemDisplayStyle.Text});

        TrayIcon.ContextMenuStrip = MenuStrip;
        TrayIcon.Visible = true;
    }

    public NotifyIcon TrayIcon { get; }
    public ContextMenuStrip MenuStrip { get; }
    public int CurrentSpeedIndex { get; private set; }
    public Dictionary<int, Icon> CachedIcons { get; }

    public void Exit(int exitCode)
    {
        TrayIcon.Visible = false;
        Environment.Exit(exitCode);
    }

    private Keys GetKeysFromString(string str)
    {
        Keys keyCombination = Keys.None;
        bool isFirst = true;
        string[] keyArr = str.Split('+');
        foreach (string key in keyArr)
        {
            if (Enum.TryParse(typeof(Keys), key, true, out object? result))
            {
                if (isFirst)
                {
                    isFirst = false;
                    keyCombination = (Keys)result!;
                    continue;
                }

                keyCombination |= (Keys)result!;
                continue;
            }

            throw new Exception("Key does not exist in Keys");
        }

        return keyCombination;
    }

    private void OnCyclePressed(object? sender, HotkeyEventArgs args)
    {
        if (CurrentSpeedIndex >= 2)
        {
            CurrentSpeedIndex = 0;
        }
        else
        {
            CurrentSpeedIndex++;
        }

        SetSpeedToIndex(CurrentSpeedIndex);
    }

    private void SetSpeedToIndex(int i)
    {
        CurrentSpeedIndex = i;
        MouseSpeed.SetSpeedToIndex(_appSettings, i);
        _currentSpeedLabel.Text = $"Current speed: {_appSettings.GetSpeedAtIndex(i)}";
        TrayIcon.Icon = CachedIcons[i];
    }
}
