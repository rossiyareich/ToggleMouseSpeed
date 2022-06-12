using Windows.Win32.UI.WindowsAndMessaging;
using static Windows.Win32.PInvoke;

namespace ToggleMouseSpeed;

public static class MouseSpeed
{
    public static unsafe bool SetMouseSpeed(int speed)
    {
        if (speed < 1 || speed > 20)
        {
            throw new ArgumentOutOfRangeException();
        }

        return SystemParametersInfo(
            SYSTEM_PARAMETERS_INFO_ACTION.SPI_SETMOUSESPEED,
            0,
            (void*)speed,
            SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS.SPIF_UPDATEINIFILE |
            SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS.SPIF_SENDCHANGE |
            SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS.SPIF_SENDWININICHANGE
        );
    }

    public static void SetSpeedToIndex(AppSettings appSettings, int i) =>
        SetMouseSpeed(appSettings.GetSpeedAtIndex(i));
}
