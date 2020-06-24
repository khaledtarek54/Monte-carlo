using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace monte_carlo
{
   
    public partial class Form1 : Form
    {
        List<clist> demand = new List<clist>();
        List<clist> freqency = new List<clist>();
        List<clist> relativefreq = new List<clist>();
        public Form1()
        {

            InitializeComponent();
            
            
        }
        int flag = 0;
        bool stop = false;
        double tot = 0;
        double tot2 = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\user\Downloads\monte-carlo\monte-carlo\monte-carlo\monte-carlo\bin\Debug\data1.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;
            int k = 0;
            for (int i = 2; ; i++)
            {
                for (int j = 1; j <= 2; j++)
                {


                    //write the value to the console
                    if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                    {
                        if (j % 2 == 1)
                        {
                            clist pnn = new clist();
                            pnn.val = Convert.ToDouble(xlRange.Cells[i, j].Value2);
                            tot += pnn.val;
                            pnn.cumulative = tot;
                            
                            demand.Add(pnn);
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[k].Cells[0].Value = pnn.val;
                            
                        }
                        else
                        {
                            clist pnn = new clist();
                            pnn.val = Convert.ToDouble(xlRange.Cells[i, j].Value2);
                            tot2 += pnn.val;
                            pnn.cumulative = tot2;
                            freqency.Add(pnn);
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[k].Cells[1].Value = pnn.val;
                            dataGridView1.Rows[k].Cells[3].Value = pnn.cumulative;
                            
                        }
                    }
                    else
                    {
                        stop = true;
                    }
                    //add useful things here!
                    
                }
                k++;
                if (stop)
                {
                    break;
                }
            }
            tot = 0;
            for (int i = 0; i < freqency.Count; i++)
            {
                clist pnn = new clist();
                pnn.val = freqency[i].val / freqency[freqency.Count-1].cumulative;
                tot += pnn.val;
                pnn.relativecumulative = tot;
                relativefreq.Add(pnn);
                dataGridView1.Rows[i].Cells[2].Value = pnn.val;
                dataGridView1.Rows[i].Cells[4].Value = pnn.relativecumulative;
            }
            for (int i = 0; i < freqency.Count; i++)
            {
                if (i == 0)
                {
                    freqency[i].range = 0;
                    freqency[i].rangeend = freqency[i].cumulative;
                    dataGridView1.Rows[i].Cells[5].Value = freqency[i].range +"-"+freqency[i].rangeend;
                }
                else
                {
                    freqency[i].range = (freqency[i-1].cumulative+1);
                    freqency[i].rangeend = freqency[i].cumulative;
                    dataGridView1.Rows[i].Cells[5].Value = freqency[i].range + "-" + freqency[i].rangeend;
                }
            }
            xlWorkbook.Close(true);
            xlApp.Quit();
        }
        public class clist
        {
            public double val;
            public double cumulative;
            public double relativecumulative;
            public double range;
            public double rangeend;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        int rand;
        private void button1_Click(object sender, EventArgs e)
        {


            Random rr = new Random();
            for (int i = 1; i <= Convert.ToInt32(textBox1.Text); i++)
            {
                double tot = 0;
                double rest = 0;
                dataGridView1.Rows[i-1].Cells[6].Value = i;
                dataGridView1.Rows.Add();
                rand = rr.Next(0, 100);
                for (int j = 0; j < demand.Count; j++)
                {
                    if (rand >= freqency[j].range && rand <= freqency[j].rangeend)
                    {
                        dataGridView1.Rows[i-1].Cells[7].Value = demand[j].val;

                    }
                }

                if (Convert.ToDouble(dataGridView1.Rows[i-1].Cells[7].Value) < Convert.ToDouble(textBox4.Text))
                {
                    dataGridView1.Rows[i-1].Cells[8].Value = Convert.ToDouble(dataGridView1.Rows[i-1].Cells[7].Value) * Convert.ToDouble(textBox3.Text);
                }
                else
                {

                    rest = Convert.ToDouble(dataGridView1.Rows[i-1].Cells[7].Value) - Convert.ToDouble(textBox4.Text);
                    tot = Convert.ToDouble(textBox4.Text) * Convert.ToDouble(textBox3.Text);
                    tot += rest * Convert.ToDouble(textBox5.Text);
                    dataGridView1.Rows[i-1].Cells[8].Value = tot;
                }


            }
            for (int z = 1; z < Convert.ToInt32(textBox2.Text); z++)
            {
                dataGridView1.Columns.Add("weekdemand", "weekdemand");
                dataGridView1.Columns.Add("cost", "cost");
                for (int i = 1; i <= Convert.ToInt32(textBox1.Text); i++)
                {

                    double rest = 0;
                    rand = rr.Next(0, 100);
                    for (int j = 0; j < demand.Count; j++)
                    {
                        if (rand >= freqency[j].range && rand <= freqency[j].rangeend)
                        {
                            dataGridView1.Rows[i-1].Cells[dataGridView1.Columns.Count - 2].Value = demand[j].val;
                        }
                    }


                    if (Convert.ToDouble(dataGridView1.Rows[i-1].Cells[dataGridView1.Columns.Count - 2].Value) < Convert.ToDouble(textBox4.Text))
                    {
                        dataGridView1.Rows[i-1].Cells[dataGridView1.Columns.Count - 1].Value = Convert.ToDouble(dataGridView1.Rows[i-1].Cells[dataGridView1.Columns.Count - 2].Value) * Convert.ToDouble(textBox3.Text);
                    }
                    else
                    {

                        rest = Convert.ToDouble(dataGridView1.Rows[i-1].Cells[dataGridView1.Columns.Count - 2].Value) - Convert.ToDouble(textBox4.Text);
                        tot = Convert.ToDouble(textBox4.Text) * Convert.ToDouble(textBox3.Text);
                        tot += rest * Convert.ToDouble(textBox5.Text);
                        dataGridView1.Rows[i-1].Cells[dataGridView1.Columns.Count - 1].Value = tot;
                    }
                }
            }
        }

     
    }
}
