using System;
using System.Windows.Forms;
using System.Collections.Generic;

using AudioSwitcher.AudioApi.CoreAudio;


namespace BigPictureAudioSwitch
{
    class Menu
    {

        ToolStripMenuItem currentItem = null;

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

        void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
