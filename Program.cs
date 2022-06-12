using System.Text.Json;

namespace ToggleMouseSpeed;

internal static class Program
{
    [STAThread]
    private static async Task Main()
    {
        const string settingsName = "AppSettings.json";

        AppSettings appSettings = null!;
        try
        {
            string json = await File.ReadAllTextAsync(settingsName);
            appSettings = JsonSerializer.Deserialize<AppSettings>(json)!;
            ArgumentNullException.ThrowIfNull(appSettings);
        }
        catch (Exception)
        {
            MessageBox.Show($"Could not load or deserialize {settingsName}", SystemTrayApp.AppName,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(-1);
        }

        SystemTrayApp app = new(appSettings);
        Application.Run();
    }
}
