using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LanguagesChartTableImg
{
    public partial class Form1 : Form
    {
        BindingSource _bs = new BindingSource();
        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            _bs.DataSource = new List<Languages>();
            dataGridView1.DataSource = _bs;
            _bs.Add(new Languages());
            bindingNavigator1.BindingSource = _bs;
            propertyGrid1.DataBindings.Add("SelectedObject", _bs, "");
            pictureBox1.DataBindings.Add("ImageLocation", _bs, "Image", true);
            chart1.DataSource = from g in _bs.DataSource as List<Languages>
                                group g by g.StrType
                                into gr
                                select new { Type = gr.Key, Avg = gr.Average(g => g.SpeakersAmount) };
            chart1.Series[0].XValueMember = "Type";
            chart1.Series[0].YValueMembers = "Avg";
            chart1.Legends.Clear();
            chart1.Titles.Add("Языки мира по группам");
            _bs.CurrentChanged += (o, e) => chart1_Click();
        }
        
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            var opf = new OpenFileDialog();
            opf.InitialDirectory = Environment.CurrentDirectory;
            opf.Filter = "Картинка в формате jpg|*.jpg|Картинка в формате jpeg|*.jpeg|Картинка в формате png|*.png";
            if (opf.ShowDialog() != DialogResult.OK) return;
            ((Languages)_bs.Current).Image = opf.FileName;
            _bs.ResetBindings(false);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.InitialDirectory = System.Environment.CurrentDirectory;
            dialog.Filter = "Файл в формате bin|*.bin|Файл в формате xml|*.xml";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var stream = new FileStream(dialog.FileName, FileMode.Create);
                switch (dialog.FilterIndex)
                {
                    case 1:
                    {
                        var fmt = new BinaryFormatter();
                        fmt.Serialize(stream, _bs.DataSource);
                    }
                        break;
                    case 2:
                    {
                        var xmlSer = new XmlSerializer(typeof(List<Languages>));
                        xmlSer.Serialize(stream, _bs.DataSource);
                    }
                        break;
                }
                stream.Close();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = "Файл в формате bin|*.bin|Файл в формате xml|*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var stream = new FileStream(ofd.FileName, FileMode.Open);
                switch (ofd.FilterIndex)
                {
                    case 1:
                    {
                        var fmt = new BinaryFormatter();
                        _bs.DataSource = (List<Languages>)fmt.Deserialize(stream);
                    }
                        break;
                    case 2:
                    {
                        var xmlSer = new XmlSerializer(typeof(List<Languages>));
                        _bs.DataSource = (List<Languages>)xmlSer.Deserialize(stream);
                    }
                        break;
                }
                stream.Close();
            }
        }

        private void chart1_Click(object sender = null, EventArgs e = null)
        {
            chart1.DataSource = from g in _bs.DataSource as List<Languages>
                group g by g.StrType
                into gr
                select new { Type = gr.Key, Avg = gr.Average(g => g.SpeakersAmount) };
            chart1.Series[0].XValueMember = "Type";
            chart1.Series[0].YValueMembers = "Avg";
            chart1.DataBind();
            chart1.Refresh();
            chart1.ResetBindings();
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Value.Text) && ToDo.SelectedItem != null && Stolb.SelectedItem != null)
            {
                var arr = new List<int>();
                var value = uint.Parse(Value.Text.ToString());
                var stolb = Stolb.SelectedItem.ToString();
                var toDo = ToDo.SelectedItem.ToString();
                dataGridView1.ClearSelection();
                var dictionary = new Dictionary<string, int>();
                dictionary.Add(">", 1);
                dictionary.Add(">=", 2);
                dictionary.Add("<", 3);
                dictionary.Add("<=", 4);
                var operation = 0;
                foreach (var key in dictionary.Where(key => key.Key == toDo))
                {
                    operation = key.Value;
                }
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    if (col.HeaderText != stolb) continue;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[col.Index].Value != null)
                        {
                            switch (operation)
                            {
                                case 1:
                                {
                                    if (row.Cells[col.Index].Value != null &&
                                        uint.Parse(row.Cells[col.Index].Value.ToString()) > value)
                                    {
                                        row.Visible = true;
                                    }
                                    else
                                    {
                                        dataGridView1.CurrentCell = null;
                                        row.Visible = false;
                                    }
                                }
                                    break;
                                case 2:
                                {
                                    if (row.Cells[col.Index].Value != null &&
                                        uint.Parse(row.Cells[col.Index].Value.ToString()) >= value)
                                    {
                                        row.Visible = true;
                                    }
                                    else
                                    {
                                        dataGridView1.CurrentCell = null;
                                        row.Visible = false;
                                    }
                                }
                                    break;
                                case 3:
                                {
                                    if (row.Cells[col.Index].Value != null &&
                                        uint.Parse(row.Cells[col.Index].Value.ToString()) < value)
                                    {
                                        row.Visible = true;
                                    }
                                    else
                                    {
                                        dataGridView1.CurrentCell = null;
                                        row.Visible = false;
                                    }
                                }
                                    break;
                                case 4:
                                {
                                    if (row.Cells[col.Index].Value != null &&
                                        uint.Parse(row.Cells[col.Index].Value.ToString()) <= value)
                                    {
                                        row.Visible = true;
                                    }
                                    else
                                    {
                                        dataGridView1.CurrentCell = null;
                                        row.Visible = false;
                                    }
                                }
                                    break;
                            }
                        }
                    }

                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                    row.Visible = true;
            }
        }
    }
}