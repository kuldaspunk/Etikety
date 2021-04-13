﻿using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Etikety
{

    public partial class Wweek2 : Form
    {
        private List<LoadDataFromCSV> loadW2; //vytvoreni pole z load data


        public Wweek2()
        {
            InitializeComponent();
        }

        private void Wweek2_Load(object sender, EventArgs e)
        {
            loadW2 = File.ReadAllLines(@"db/wweek2.txt").Skip(1).Select(x => LoadDataFromCSV.GetPrintData(x)).ToList();
            setDay.DataSource = loadW2.Select(x => x.Day).Distinct().ToArray();           
            setDay.SelectedIndex = -1;
        }

        private void setDay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void printbutW1_Click(object sender, EventArgs e)
        {
            printbutW1.Enabled = false;
            PrintEtiket print = new PrintEtiket();
            List<LoadDataFromCSV> paths;
            if (setDay.SelectedIndex == -1)
            {
                MessageBox.Show("Zadejte den");
                
            }
            else
            { 
            string day = setDay.SelectedItem.ToString();          
            var mycheckboxes = Controls.OfType<GroupBox>().SelectMany(groupBox => groupBox.Controls.OfType<CheckBox>());
            var myGroupBoxes = Controls.OfType<GroupBox>().SelectMany(groupBox => groupBox.Controls.OfType<NumericUpDown>());

                foreach (NumericUpDown txt in myGroupBoxes)
                {


                    int copies = (int)txt.Value;
                    if (txt is null)
                    {
                        txt.Value = 0;

                    }
                    if (txt.Value > 0)
                    {

                        var a = mycheckboxes.Where(l => l.Tag == txt.Tag).First();
                        a.Text = txt.Value.ToString();
                        a.Visible = true;
                        a.Checked = true;
                        txt.Value = 0;
                    }
                    string type = txt.Tag.ToString();
                    paths = loadW2.Where(x => (x.Day == day) & (x.Type == type)).ToList();
                    await Task.Run(() => // spusti se asynchronne metoda printing, zkusit casem pouzit thread nebo backg.worker
                    {
                        if (copies > 0) print.Printing(paths, copies);
                    });
                }
                
                //foreach (NumericUpDown txt in myGroupBoxes)
                //{
                //    if (txt.Value > 1)
                //        txt.Enabled =false;


                //}

            }
            printbutW1.Enabled = true;


        }
    }
}
