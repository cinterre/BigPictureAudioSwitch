using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.Win32;

using AudioSwitcher.AudioApi.CoreAudio;


namespace BigPictureAudioSwitch
{
    class Menu
    {

        ToolStripMenuItem currentItem = null;
        Boolean startup = false;

        public ContextMenuStrip Create()
        {

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;

            CoreAudioController audio = new CoreAudioController();
            IEnumerable<CoreAudioDevice> devices = audio.GetPlaybackDevices();

            Guid deviceId = (Guid)Properties.Settings.Default["DeviceId"];

            item = new ToolStripMenuItem();
            item.Text = "Select device to switch to:";
            menu.Items.Add(item);

            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            foreach (CoreAudioDevice device in devices)
            {
                item = new ToolStripMenuItem();
                item.Text = device.FullName;
                item.Tag = device;

                item.Click += new EventHandler(Device_Click);

                if (device.Id == deviceId)
                {
                    this.currentItem = item;
                    this.currentItem.Checked = true;
                }

                menu.Items.Add(item);
            }

            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            item = new ToolStripMenuItem();
            item.Text = "Launch on startup";
            item.Click += new System.EventHandler(Startup_Click);
            try
            {
                this.startup = (Boolean)Properties.Settings.Default["Startup"];
            }
            catch (System.Configuration.SettingsPropertyNotFoundException)
            {
                this.startup = false;
            }
            
            if (this.startup)
            {
                item.Checked = true;
            }
            menu.Items.Add(item);

            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            menu.Items.Add(item);

            return menu;
        }

        void Device_Click(object sender, EventArgs e)
        {
            if (this.currentItem != null)
            {
                this.currentItem.Checked = false;
            }

            this.currentItem = (ToolStripMenuItem)sender;
            this.currentItem.Checked = true;

            Properties.Settings.Default["DeviceId"] = (this.currentItem.Tag as CoreAudioDevice).Id;
            Properties.Settings.Default.Save();
        }

        void Startup_Click(object sender, EventArgs e)
        {
            if (this.startup)
            {
                this.startup = false;
                Properties.Settings.Default["Startup"] = false;
                Properties.Settings.Default.Save();
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                item.Checked = false;
                try
                {
                    RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    rk.DeleteValue(Application.ProductName);
                }
                catch (System.ArgumentException)
                {

                }
            } 
            
            else
            {
                this.startup = true;
                Properties.Settings.Default["Startup"] = true;
                Properties.Settings.Default.Save();
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                item.Checked = true;
                try
                {
                    RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    rk.SetValue(Application.ProductName, Application.ExecutablePath);
                }
                catch (System.ArgumentException)
                {

                }
            }
            
        }


        void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
