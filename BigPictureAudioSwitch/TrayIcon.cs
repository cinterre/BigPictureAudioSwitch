using System;
using System.Windows.Forms;
using BigPictureAudioSwitch.Properties;

namespace BigPictureAudioSwitch
{
    class TrayIcon : IDisposable
    {
        NotifyIcon icon;

        public TrayIcon()
        {

            icon = new NotifyIcon();
        }

        public void Display()
        {
            icon.Icon = Resources.Icon;
            icon.Text = "BigPicture Audio Switcher";
            icon.Visible = true;
            icon.ContextMenuStrip = new Menu().Create();
        }

        public void Dispose()
        {
            icon.Dispose();
        }

    }
}
