using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace DnDCompanionLauncher
{
    public partial class Form1 : Form
    {
        string pathToLocal = @"C:\Program Files (x86)\Procrastn8 Games\D&D Companion Tool\GhDiceRoller.exe";
        string pathToServerRefrence = @"https://github.com/procrastn8-games/D-D-Companion-Tool/blob/master/FileRefrenceChecker/bin/Debug/FileRefrenceChecker.exe?raw=true";
        string pathToServerRelease = @"https://github.com/procrastn8-games/D-D-Companion-Tool/blob/master/GhDiceRoller/bin/Release/GhDiceRoller.exe?raw=true";

        public Form1()
        {
            InitializeComponent();
            this.Show();
            this.Refresh();
            Application.DoEvents();
            Version current = GetFileVersion(pathToLocal);
            Version server = GitFileVersion(pathToServerRefrence);

            if (current < server)
            {
                Download(pathToServerRelease);
            }

            ProcessStartInfo pi = new ProcessStartInfo();
            pi.FileName = pathToLocal;
            Process p = new Process();
            p.StartInfo = pi;
            p.Start();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        
          
        }

        private void Download(string pathToServerRelease)
        {
            using (var client = new WebClient())
            {
                string refrence = Path.GetTempPath() + Path.GetRandomFileName()+"__.exe";
                client.DownloadFile(pathToServerRelease, refrence);
                File.Delete(pathToLocal);
                File.Copy(refrence, pathToLocal, true);
            }
        }

        private Version GitFileVersion(string pathLocal)
        {
            string refrence = Path.GetTempPath() + Path.GetFileNameWithoutExtension(pathLocal) + "_REF.pdb";
            using (var client = new WebClient())
            {
                client.DownloadFile(pathToServerRefrence, refrence);
            }
            var v = new Version(0, 0, 0);
            var fv = FileVersionInfo.GetVersionInfo(refrence);
            v = new Version(fv.FileMajorPart, fv.FileMajorPart, fv.FileBuildPart);

            return v;
        }

        private Version GetFileVersion(string pathToLocal)
        {
            try
            {
            Directory.CreateDirectory(Path.GetFullPath(pathToLocal));

            }
            catch (Exception)
            {
            }
            var v = new Version(0,0,0);
            if (File.Exists(pathToLocal))
            {
                var fv = FileVersionInfo.GetVersionInfo(pathToLocal);
                v = new Version(fv.FileMajorPart, fv.FileMajorPart, fv.FileBuildPart);
            }

            return v;
        }
    }
}
