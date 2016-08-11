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
    public partial class StartPointFormRevision2 : Form
    {
        SqlConnection c = new SqlConnection("server=(local); Database=Deutsch;Integrated Security=true");

        DataRow rowGetFromExamenator;

        //TextenWiderholer widerholer = new TextenWiderholer();

        public StartPointFormRevision2()
        {
            InitializeComponent();

            MessageBox.Show("Давайте проверим, работает ли мой Git!");
        }

        private void StartPointFormRevision2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'deutschDataSet.WörtePrüfung' table. You can move, or remove it, as needed.
            this.wörtePrüfungTableAdapter.Fill(this.deutschDataSet.WörtePrüfung);

            listView1.View = View.Details;
            listView1.Sorting = System.Windows.Forms.SortOrder.Descending;

            if (c.State != ConnectionState.Open)
                c.Open();

            //SqlCommand comd = new SqlCommand("SELECT idSpiegelArtikel, SpiegelArtikelÜberschrift, SpiegelArtikelÜberschriftURI FROM SpiegelArtikel WHERE (SpiegelArtikelÜberschrift LIKE '%@SAÜ%')", c);
            //comd.Parameters.Add("@SAÜ", SqlDbType.NVarChar).Value = textBox1.Text;
            //SqlDataReader rdr = comd.ExecuteReader();
            SqlCommand SELcmd = new SqlCommand("SELECT idSpiegelArtikel, SpiegelArtikelÜberschrift, SpiegelArtikelÜberschriftURI FROM SpiegelArtikel", c);
            SqlDataReader InitialReader = SELcmd.ExecuteReader();

            int rowsnumber = 0;
            while (InitialReader.Read())
                rowsnumber++;

            //rdr.Close();
            InitialReader.Close();

            //MessageBox.Show(rdr.GetSchemaTable().Rows.Count.ToString());
            //MessageBox.Show(rowsnumber.ToString());

            //rdr.Close();
            //rdr = comd.ExecuteReader();
            //ListViewItem[] items = new ListViewItem[rdr.GetSchemaTable().Rows.Count];
            InitialReader = SELcmd.ExecuteReader();
            ListViewItem[] items = new ListViewItem[rowsnumber];
            int counter = 0;
            while (InitialReader.Read())
            {
                items[counter] = new ListViewItem(InitialReader.GetString(1));

                listView1.Items.Add(items[counter]);

                counter++;
            }

            InitialReader.Close();

            SqlCommand DIScmd = new SqlCommand("SELECT DISTINCT DatumInDB FROM Wörte ORDER BY DatumInDB DESC", c);
            SqlDataReader Reader02 = DIScmd.ExecuteReader();
            while(Reader02.Read())
            {
                listBox2.Items.Add(Reader02.GetDateTime(0));
            }
            Reader02.Close();

            label5.Text = listBox2.Items.Count.ToString();

            TextenWiderholer widerholer = new TextenWiderholer();
            //listBox1.DataSource = widerholer.TextenZuWiderholung();
            dataGridView1.DataSource = widerholer.TextenZuWiderholung();
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

            //TextenUndGroßePhrasenForm textenForm = new TextenUndGroßePhrasenForm();
            //Button b = textenForm.getButtonForEvent;
            //b.Click += B_Click;
            PrepositionenDoppelVerwendung prep = new PrepositionenDoppelVerwendung();
            foreach(string s in prep.PrepositionenArray)
            {
                listBox1.Items.Add(s);
            }
        }

        //private void B_Click(object sender, EventArgs e)
        //{

            //dataGridView1.Refresh();
            //dataGridView1.DataSource = null;
            //TextenWiderholer widerholer = new TextenWiderholer();
            //dataGridView1.DataSource = widerholer.TextenZuWiderholung();
            //dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
        //}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            if (c.State != ConnectionState.Open)
                c.Open();

            //SqlCommand comd = new SqlCommand("SELECT idSpiegelArtikel, SpiegelArtikelÜberschrift, SpiegelArtikelÜberschriftURI FROM SpiegelArtikel WHERE (SpiegelArtikelÜberschrift LIKE '%@SAÜ%')", c);
            SqlCommand comd = new SqlCommand("SELECT idSpiegelArtikel, SpiegelArtikelÜberschrift, SpiegelArtikelÜberschriftURI FROM SpiegelArtikel WHERE (SpiegelArtikelÜberschrift LIKE '%' + @SAÜ + '%')", c);
            comd.Parameters.Add("@SAÜ", SqlDbType.NVarChar).Value = textBox1.Text;
            SqlDataReader rdr = comd.ExecuteReader();

            int rowsnumber = 0;
            while (rdr.Read())
                rowsnumber++;


            rdr.Close();
            rdr = comd.ExecuteReader();
            //ListViewItem[] items = new ListViewItem[rdr.GetSchemaTable().Rows.Count];
            ListViewItem[] items = new ListViewItem[rowsnumber];
            int counter = 0;
            while (rdr.Read())
            {
                items[counter] = new ListViewItem(rdr.GetString(1));

                listView1.Items.Add(items[counter]);

                counter++;
            }

            rdr.Close();
        }

        private void spiegelArtikelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SpiegelArtikelForm spiegel = new SpiegelArtikelForm();
            //spiegel.Show();

            SpiegelArtikelForm_rev02 spiegel = new SpiegelArtikelForm_rev02();
            spiegel.Show();
        }

        private void wörteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WörteForm w = new WörteForm();
            w.Show();
        }

        private void anfangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Examenator exam01 = new Examenator();
            //label1.Text = exam01.getWordToExam();
            rowGetFromExamenator = exam01.getWordToExam();
            //label1.Text = exam01.getWordToExam()[1].ToString();
            label1.Text = rowGetFromExamenator[1].ToString();
            textBox2.Text = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = rowGetFromExamenator[2].ToString();

            if(!String.IsNullOrEmpty(textBox2.Text))
            {
                //deutschDataSet.WörtePrüfung.AddWörtePrüfungRow(DateTime.Today, rowGetFromExamenator[0], textBox2.Text);
                //deutschDataSet.WörtePrüfung.AddWörtePrüfungRow(DateTime.Today, (DeutschDataSet.WörteRow)rowGetFromExamenator[0], textBox2.Text);
                //deutschDataSet.WörtePrüfung.AddWörtePrüfungRow(DateTime.Today, deutschDataSet.Wörte.FindByidWort(Int32.Parse(rowGetFromExamenator[0].ToString())), textBox2.Text);
                //deutschDataSet.WörtePrüfung.AddWörtePrüfungRow(DateTime.Today, deutschDataSet.Wörte.FindByidWort(Int32.Parse(rowGetFromExamenator[0].ToString())), textBox2.Text);

                DataRow a = deutschDataSet.WörtePrüfung.NewRow();
                a["WortPrüfungDatum"] = DateTime.Today;
                a["idWort"] = Int32.Parse(rowGetFromExamenator[0].ToString());
                a["WortMeinVariant"] = textBox2.Text;
                deutschDataSet.WörtePrüfung.Rows.Add(a);

                wörtePrüfungTableAdapter.Update(deutschDataSet.WörtePrüfung);
            }
        }

        private void wörtePrüfungBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.wörtePrüfungBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.deutschDataSet);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "не знаю";

            label2.Text = rowGetFromExamenator[2].ToString();

            if (!String.IsNullOrEmpty(textBox2.Text))
            {
                DataRow a = deutschDataSet.WörtePrüfung.NewRow();
                a["WortPrüfungDatum"] = DateTime.Today;
                a["idWort"] = Int32.Parse(rowGetFromExamenator[0].ToString());
                a["WortMeinVariant"] = textBox2.Text;
                deutschDataSet.WörtePrüfung.Rows.Add(a);

                wörtePrüfungTableAdapter.Update(deutschDataSet.WörtePrüfung);
            }
        }

        private void textenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextenUndGroßePhrasenForm tform = new TextenUndGroßePhrasenForm();
            tform.getButtonForEvent.Click += GetButtonForEvent_Click;
            tform.Show();
        }

        private void GetButtonForEvent_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            TextenWiderholer widerholer = new TextenWiderholer();
            dataGridView1.DataSource = widerholer.TextenZuWiderholung();
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

            //Сначала я подумал, что необходимо как-то использовать деструктор или сборщик мусора, но сначала решил использовать null, для того, чтобы ссылка больше не указывала на объект и 
            //сборщик мусора его автоматически (по идее) должен уничтожить и высвободить память.
            widerholer = null;

            DataGridViewColumnCollection columnCollection = dataGridView1.Columns;
            dataGridView1.Sort(columnCollection[1], ListSortDirection.Descending);
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            listBox3.Items.Clear();

            ListBox lb = sender as ListBox;
            //MessageBox.Show(lb.SelectedItem.ToString());

            SqlCommand cmd03 = new SqlCommand("SELECT idWort, Wort FROM Wörte WHERE DatumInDB = @DatumInDB", c);
            cmd03.Parameters.Add("@DatumInDB", SqlDbType.DateTime).Value = lb.SelectedItem.ToString();
            SqlDataReader reader03 = cmd03.ExecuteReader();
            while(reader03.Read())
            {
                listBox3.Items.Add(reader03.GetString(1));
            }
            reader03.Close();
            label7.Text = listBox3.Items.Count.ToString();
        }
    }
}
