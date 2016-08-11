using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DeutschSprache
{
    public partial class Wörte : Form
    {
        List<string> wrt = new List<string>();
        List<int> idi = new List<int>();

        //static string connectionString = "server=COO-TECH"+"\\"+ "MSSQLSERVER2012;Database=DeutschWPF-II;Integrated Security=True";
        //static string connectionString = "server=DMITRIY-PC" + "\\" + "DMITRIY" + "\\" + "MSSQL11.MSSQLSERVER;Database=DeutschWPF-II;Integrated Security=True";
        static string connectionString = "server=DMITRIY-PC;Database=DeutschWPF-II;Integrated Security=True"; 
        public string sql = "SELECT idWort, Wort FROM Wörte";
        SqlConnection sqlcon;
        SqlDataAdapter sqlDataAdapter;
        DataTable table = new DataTable();

        public Wörte()
        {
            InitializeComponent();
            sqlcon = new SqlConnection();
            sqlcon.ConnectionString = connectionString;
            sqlcon.Open();
            sqlDataAdapter = new SqlDataAdapter(sql, sqlcon);
            sqlDataAdapter.Fill(table);

            //Texts texts = new Texts();
            //this.Load += texts.listBox1_DoubleClick;
        }

        private void wörteBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.wörteBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this._DeutschWPF_IIDataSet);

            listBox1.Update();
            listBox1.DisplayMember = "Wort";

        }
        public BindingSource wörteBS
        {
            get { return wörteBindingSource; }
        }

        public DateTimePicker DTPicker
        {
            get { return дата_добавления_в_БДDateTimePicker; }
        }
        public TextBox TBox
        {
            get { return wortTextBox; }
        }

        private void Wörte_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "_DeutschWPF_IIDataSet.Wörte". При необходимости она может быть перемещена или удалена.
            this.wörteTableAdapter.Fill(this._DeutschWPF_IIDataSet.Wörte);

            //this.Dock = DockStyle.Fill;
            //this.Location = new Point(100, 00);
            this.WindowState = FormWindowState.Maximized;
            wörteBindingSource.MoveLast(); //Закомментировал, чтобы работал код из формы Texts.
            Texts texts = new Texts();
            //texts.listBox1_DoubleClick()

            //foreach(DataRow dataR in _DeutschWPF_IIDataSet.Wörte.Rows)
            //{
            //    foreach(DataColumn dataC in _DeutschWPF_IIDataSet.Wörte.Columns)
            //    {
            //        wrt.Add(dataR.ToString());
            //    }
                
            //}

            for (int i = 0; i <= _DeutschWPF_IIDataSet.Wörte.Rows.Count - 1;i++ )
            {
                wrt.Add(_DeutschWPF_IIDataSet.Wörte.Rows[i][1].ToString());
            }

            wrt.Sort();

            listBox1.DataSource = wrt;
            listBox1.DisplayMember = "Wort";

            listBox2.MultiColumn = true;
            listBox2.ColumnWidth = 100;
            listBox2.DataSource = table;
            listBox2.DisplayMember ="Wort";

            listBox3.DataSource = _DeutschWPF_IIDataSet.Wörte;
            listBox3.ValueMember = "idWort";
            listBox3.DisplayMember = "Wort";

            //wörteBindingSource.Sort = "Дата добавления в БД";
        }

        private void Wörte_Leave(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //listBox1.Sorted = !listBox1.Sorted;
           for(int i =0; i<=_DeutschWPF_IIDataSet.Wörte.Count-1;i++)
           {
               idi.Add(Convert.ToInt32( _DeutschWPF_IIDataSet.Wörte.Rows[i][0]));
           }

           idi.Sort();
           listBox1.DataSource = idi;
            
        }

        private void übersetzungTextBox_TextChanged(object sender, EventArgs e)
        {
            //this.Validate();
            //this.wörteBindingSource.EndEdit();
            //this.tableAdapterManager.UpdateAll(this._DeutschWPF_IIDataSet);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            //MessageBox.Show(listBox1.SelectedItem.ToString());
            wörteBindingSource.Position = wörteBindingSource.Find("Wort", listBox1.SelectedItem.ToString());
            //wörteBindingSource.Position = wörteBindingSource.Find("idWort", '"' + listBox1.SelectedItem.ToString() + '"');
            //wörteBindingSource.Position = wörteBindingSource.Find("idWort", listBox1.SelectedIndex.ToString());
            //wörteBindingSource.Position = wörteBindingSource.Find("idWort", listBox1.ValueMember);
            //wörteBindingSource.Position = wörteBindingSource.Find("idWort", listBox1.SelectedValue.ToString());
            //wörteBindingSource.Position = wörteBindingSource.Find("idWort", listBox1.SelectedItem);

        }

        private void listBox3_DoubleClick(object sender, EventArgs e)
        {
            //MessageBox.Show((wörteBindingSource.Find("Wort", listBox3.SelectedItem.ToString())).ToString());
            //MessageBox.Show((listBox3.SelectedItem.ToString()).ToString());
            //MessageBox.Show(listBox3.SelectedItem.ToString());
            //MessageBox.Show(listBox3.SelectedIndex.ToString());
            //MessageBox.Show(listBox3.Items[listBox3.SelectedIndex].ToString());
            //MessageBox.Show(listBox3.SelectedValue.ToString());

            wörteBindingSource.Position = wörteBindingSource.Find("idWort", listBox3.SelectedValue);
            //wörteBindingSource.Position = wörteBindingSource.Find("Wort", listBox3.SelectedItem.ToString());
            //wörteBindingSource.Position = wörteBindingSource.Find("Wort", listBox3.SelectedValue);
            //wörteBindingSource.Position = wörteBindingSource.Find("Wort", "Balte");
            //wörteBindingSource.Position = wörteBindingSource.Find("Wort", "Pendel");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            wörteBindingSource.Sort = "Wort";
            //listBox3.DataSource = _DeutschWPF_IIDataSet.Wörte.Rows.sort
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            дата_добавления_в_БДDateTimePicker.Text = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            wörteBindingSource.AddNew();
        }
    }
}
