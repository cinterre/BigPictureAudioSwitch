using System;
using System.Windows.Forms;
using System.Timers;
using System.Runtime.InteropServices;
using AudioSwitcher.AudioApi.CoreAudio;

namespace BigPictureAudioSwitch
{
    static class Program
    {

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string sClass, string sWindow);

        static Boolean audioSwitched = false;
        static CoreAudioDevice startDevice;
        static CoreAudioController audio;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            TrayIcon tray = new TrayIcon();
            tray.Display();

            audio = new CoreAudioController();
            startDevice = audio.GetDefaultDevice(AudioSwitcher.AudioApi.DeviceType.Playback, AudioSwitcher.AudioApi.Role.Multimedia);

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(ProcessTimer);
            timer.Interval = 500;
            timer.Enabled = true;

            Application.Run();
        }

        static void ProcessTimer(object source, ElapsedEventArgs e)
        {
            Guid deviceId = (Guid)Properties.Settings.Default["DeviceId"];
            // Legacy Big Picture Window
            int found = FindWindow("CUIEngineWin32", "Steam");
            //Steamdeck Big picture window
            if (found == 0)
            {
                found = FindWindow("SDL_app", "Steam Big Picture Mode");
            }


            if (found != 0 && !(audioSwitched))
            {
                Console.WriteLine("Big Picture!");
                audioSwitched = true;
                CoreAudioDevice newDevice = audio.GetDevice(deviceId);

                audio.SetDefaultDevice(newDevice);
            }
            else if (found == 0 && audioSwitched) {
                audioSwitched = false;

                audio.SetDefaultDevice(startDevice);
            }
        }
    }
}
