﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NovetusLauncher.SDK
{
    public partial class DiogenesEditor : Form
    {
        public DiogenesEditor()
        {
            InitializeComponent();
        }

        public static string DiogenesCrypt(string word)
        {
            StringBuilder result = new StringBuilder("");
            byte[] bytes = Encoding.ASCII.GetBytes(word);

            foreach (byte singular in bytes)
            {
                result.Append(Convert.ToChar(0x55 ^ singular));
            }

            return result.ToString();
        }

        void NewToolStripMenuItemClick(object sender, EventArgs e)
        {
            label2.Text = "Not Loaded";
            richTextBox1.Text = "";
        }

        void LoadToolStripMenuItemClick(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "ROBLOX Diogenes filter (diogenes.fnt)|diogenes.fnt";
                ofd.FilterIndex = 1;
                ofd.FileName = "diogenes.fnt";
                ofd.Title = "Load diogenes.fnt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder builder = new StringBuilder();

                    using (StreamReader reader = new StreamReader(ofd.FileName))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();

                            try
                            {
                                line = DiogenesCrypt(line);
                                label2.Text = "v2";
                            }
                            catch(Exception)
                            {
                                label2.Text = "v1";
                                continue;
                            }

                            builder.Append(line + Environment.NewLine);
                        }
                    }

                    richTextBox1.Text = builder.ToString();
                }
            }
        }

        void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "ROBLOX Diogenes filter v2 (diogenes.fnt)|diogenes.fnt|ROBLOX Diogenes filter v1 (diogenes.fnt)|diogenes.fnt";
                sfd.FilterIndex = 1;
                sfd.FileName = "diogenes.fnt";
                sfd.Title = "Save diogenes.fnt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder builder = new StringBuilder();

                    foreach (string s in richTextBox1.Lines)
                    {
                        if (sfd.FilterIndex == 1)
                        {
                            builder.Append(DiogenesCrypt(s) + Environment.NewLine);
                            label2.Text = "v2";
                        }
                        else
                        {
                            builder.Append(s + Environment.NewLine);
                            label2.Text = "v1";
                        }
                    }

                    using (StreamWriter sw = File.CreateText(sfd.FileName))
                    {
                        sw.Write(builder.ToString());
                    }
                }
            }
        }

        void saveAsTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text file (*.txt)|*.txt";
                sfd.FilterIndex = 1;
                sfd.FileName = "diogenes.txt";
                sfd.Title = "Save diogenes.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllLines(sfd.FileName, richTextBox1.Lines);
                }
            }
        }
    }
}