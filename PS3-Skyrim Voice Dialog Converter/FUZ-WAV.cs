﻿using System;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PS3_Skyrim_Voice_Dialog_Converter
{
    public partial class FUZ_WAV : Form
    {
        private List<string> filePaths = new List<string>();

        public FUZ_WAV()
        {
            InitializeComponent();
        }

        private void FUZ_WAV_Load(object sender, EventArgs e)
        {
            /// thse are what you see when you hover over the buttons
            toolTip1.SetToolTip(this.btadd, "This is to add them one by one");
            toolTip1.SetToolTip(this.btadddir, "This is for adding a dirtory");
            toolTip1.SetToolTip(this.btClear, "this will purge the list");
            toolTip1.SetToolTip(this.credit, "This will open the credit");
            toolTip1.SetToolTip(this.btstart, "this will start the ");
            /// this will check to see if the MSEnc is present
            if (File.Exists("data\\MSEnc.exe"))
            {

            }
            else
            {
                MessageBox.Show("MSEnc.exe is missing please put it in the data folder");
            }
            /// this will check to see if the ENCVAG.DLL is present
            if (File.Exists("Data\\ENCVAG.DLL"))
            {

            }
            else
            {
                MessageBox.Show("ENCVAG.DLL is missing please put it in the data folder");
            }
            /// this will check to see if xWMAEncode.exe is present
            if (File.Exists("Data\\xWMAEncode.exe"))
            {

            }
            else
            {
                MessageBox.Show("xWMAEncode.exe is missing please put it in the data folder");
            }
            /// this will check to see if "fuz_extractor.exe" is present
            if (File.Exists("Data\\fuz_extractor.exe"))
            {

            }
            else
            {
                MessageBox.Show("fuz_extractor.exe is missing please put it in the data folder");
            }
            /// this is going to show the Mesagebox that give the warning if anything is missing it might now work like its suppost to
            Directory.CreateDirectory("fuz");
        }

        private void btadd_Click(object sender, EventArgs e)
        {
            /// this is going to add files via the add file button to the listbox
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = true;
            OFD.ShowDialog();
            filePaths.AddRange(OFD.FileNames);
            for (int i = 0; i < filePaths.Count; i++)
            {
                lboxFiles.Items.Add(OFD.SafeFileNames[i]);
            }
            /// this is going to tell the system that label is the count of the listbox and the " To be converted "
            /// IE, "5 To be converted "
            label1.Text = lboxFiles.Items.Count + " To be converted ";
        }

        private void btadddir_Click(object sender, EventArgs e)
        {
            /// this is going to add all files via the add dir to the listbox
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.Description = "Select the Sound Folder";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(FBD.SelectedPath, "*.fuz", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (!filePaths.Contains(file))
                    {
                        lboxFiles.Items.Add(Path.GetFileName(file));
                        filePaths.Add(file);
                        label1.Text = lboxFiles.Items.Count + " To be converted ";
                    }
                }
            }
            /// this is going to tell the system that label is the count of the listbox and the " To be converted "
            /// IE, "5 To be converted "
            label1.Text = lboxFiles.Items.Count + " To be converted ";
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            /// this is going to clear the list box
            lboxFiles.Items.Clear();
            /// this is going to refresh the listbox
            lboxFiles.Refresh();
            /// this is going to clear the list of file that where in the listbox
            filePaths.Clear();
            /// this is going to make the progress bar know how many files there are so it can move the bar acordingly
            progressBar1.Maximum = lboxFiles.Items.Count;
            /// this is going to tell the system that label is the count of the listbox and the " To be converted "
            /// IE, "5 To be converted "
            label1.Text = lboxFiles.Items.Count + " To be converted ";
        }

        private void lboxFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string text in array)
            {
                if ((Path.GetExtension(text) == ".fuz" || Path.GetExtension(text) == ".FUZ") && !filePaths.Contains(text))
                {
                    lboxFiles.Items.Add(Path.GetFileName(text));
                    filePaths.Add(text);
                    label1.Text = lboxFiles.Items.Count + " To be converted ";
                }
                if (Directory.Exists(text))
                {
                    string[] files = Directory.GetFiles(text, "*.fuz", SearchOption.AllDirectories);
                    foreach (string text2 in files)
                    {
                        if (!filePaths.Contains(text2))
                        {
                            lboxFiles.Items.Add(Path.GetFileName(text2));
                            filePaths.Add(text2);
                            label1.Text = lboxFiles.Items.Count + " To be converted ";
                        }
                    }
                }
            }
        }

        private void lboxFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void credit_Click(object sender, EventArgs e)
        {
            /// this is going to open the credits window
            if (Application.OpenForms["Credits"] == null)
            {
                Credits form = new Credits();
                form.Show();
            }
        }

        private string getFileName(string path)
        {
            path = path.Replace("\\", ",");
            string[] pathSplit = path.Split(',');
            return pathSplit[pathSplit.Length - 1];
        }

        private void btstart_Click(object sender, EventArgs e)
        {
            /// this is going to create the dirtory fuz
            Directory.CreateDirectory("fuz");
            /// this is going to pass the dirtory info
            DirectoryInfo info = new DirectoryInfo(Application.StartupPath + "\\fuz\\");
            FileInfo[] files = info.GetFiles();
            foreach (FileInfo file in files)
            {
                /// this is going to delete all existing files in the fuz folder
                file.Delete();
            }
            for (int i = 0; i < filePaths.Count; i++)
            {
                /// This is going to check if the listbox has any files with the extension fuz
                if (filePaths[i].Contains(".fuz"))
                {
                    /// The fuz stucture goes as follows
                    /// FUZ---> is like a archive but not quite the same so it goes FUZ --> XWM + lip.
                    /// the way to convert it is to extract the fuz and convert the xwm to wav than to ATRAC3(AT3) than repack it
                    /// This is going to extract the fuz file
                    Process process1 = new Process();
                    process1.StartInfo.FileName = "cmd.exe";
                    process1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process1.StartInfo.Arguments = "/c Data\\fuz_extractor.exe -e \"" + filePaths[i];
                    process1.Start();
                    process1.WaitForExit();
                    /// This its going to convert the fuz file from xwm to wav
                    File.Move(filePaths[i].ToString().Replace(".fuz", ".xwm"), filePaths[i].ToString().Replace(".fuz", ".wav"));
                    /// Than its going to convert the fuz file from wav to AT9
                    Process process3 = new Process();
                    process3.StartInfo.FileName = "Data\\MSEnc.exe";
                    process3.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process3.StartInfo.Arguments = "-in \"" + filePaths[i].ToString().Replace(".fuz", ".wav") + "\" -out \"" + filePaths[i].ToString().Replace(".fuz", ".xwm") + "\" -atrac3" + "\" -skipresample" + "\" -x";
                    process3.Start();
                    process3.WaitForExit();
                    /// Than its going to repack the lip and "xwm"(ATRAC3/AT3) back into a fuz format
                    Process process4 = new Process();
                    process4.StartInfo.FileName = "cmd.exe";
                    process4.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process4.StartInfo.Arguments = "/c Data\\fuz_extractor.exe -c \"" + filePaths[i].ToString().Replace(".fuz", ".xwm") + "\" \"" + filePaths[i].ToString().Replace(".fuz", ".lip") + "\" \"" + filePaths[i].ToString().Replace(".fuz", ".fuz") + "\"";
                    process4.Start();
                    process4.WaitForExit();
                    /// This is clean up.
                    /// this is going to delete the extracted lip/xwm after they are extracted than converted
                    File.Delete(filePaths[i].ToString().Replace(".fuz", ".xwm"));
                    File.Delete(filePaths[i].ToString().Replace(".fuz", ".lip"));
                    File.Delete(filePaths[i].ToString().Replace(".fuz", ".wav"));
                    /// this is going to make the progress bar know how many files there are so it can move the bar acordingly
                    progressBar1.Maximum = lboxFiles.Items.Count;
                    /// This is going to make it so you can see the bar move
                    System.GC.Collect();
                    /// this is going to make it move
                    progressBar1.Value++;
                }

                /// this checks to see if they are all done converting
                if (progressBar1.Value == progressBar1.Maximum)
                {
                    /// this is going to show the messege box
                    MessageBox.Show("Your Voice Dialouge is converted!", "Finished!");
                    filePaths.Clear();
                    lboxFiles.Items.Clear();
                    label1.Text = "Press convert to start";
                    progressBar1.Value = 0;
                }
            }
        }
    }
}
