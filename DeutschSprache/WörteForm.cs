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
using System.Diagnostics;

namespace DeutschSprache
{
    public partial class WörteForm : Form
    {
        SqlConnection connection = new SqlConnection("server=(local);Database=Deutsch;Integrated security = true");

        public WörteForm()
        {
            InitializeComponent();
            //listView1.View = View.Details;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Dispose();
        }

        private void wörteBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.wörteBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.deutschDataSet);

        }

        private void WörteForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'deutschDataSet.Hefte' table. You can move, or remove it, as needed.
            this.hefteTableAdapter.Fill(this.deutschDataSet.Hefte);
            // TODO: This line of code loads data into the 'deutschDataSet.Sätze' table. You can move, or remove it, as needed.
            this.sätzeTableAdapter.Fill(this.deutschDataSet.Sätze);
            // TODO: This line of code loads data into the 'deutschDataSet.Wortarte' table. You can move, or remove it, as needed.
            this.wortarteTableAdapter.Fill(this.deutschDataSet.Wortarte);
            // TODO: This line of code loads data into the 'deutschDataSet.Artikel' table. You can move, or remove it, as needed.
            this.artikelTableAdapter.Fill(this.deutschDataSet.Artikel);
            // TODO: This line of code loads data into the 'deutschDataSet.Wörte' table. You can move, or remove it, as needed.
            this.wörteTableAdapter.Fill(this.deutschDataSet.Wörte);

            wörteBindingSource.CurrentItemChanged += WörteBindingSource_CurrentItemChanged;
            wörteBindingSource.PositionChanged += WörteBindingSource_PositionChanged;
            Random random = new Random();
            wörteBindingSource.Position = random.Next(0, wörteBindingSource.Count);

            comboBox1.DataSource = deutschDataSet.Artikel;
            comboBox1.ValueMember = "idArtikel";
            comboBox1.DisplayMember = "Artikel";
            comboBox1.SelectedValueChanged += ComboBox1_SelectedValueChanged;
            if (!String.IsNullOrEmpty(idArtikelTextBox.Text))
                comboBox1.SelectedValue = Int32.Parse(idArtikelTextBox.Text);
            if (String.IsNullOrEmpty(idArtikelTextBox.Text))
                comboBox1.Text = "";

            comboBox2.DataSource = deutschDataSet.Wortarte;
            comboBox2.ValueMember = "idWortart";
            comboBox2.DisplayMember = "Wortart";
            comboBox2.SelectedValueChanged += ComboBox2_SelectedValueChanged;
            if (!String.IsNullOrEmpty(idWortartTextBox.Text))
                comboBox2.SelectedValue = Int32.Parse(idWortartTextBox.Text);

            comboBox3.DataSource = deutschDataSet.Hefte;
            comboBox3.ValueMember = "idHeft";
            comboBox3.DisplayMember = "HeftName";
            comboBox3.SelectedValueChanged += ComboBox3_SelectedValueChanged;
            if (!String.IsNullOrEmpty(idHeftTextBox.Text))
                comboBox3.SelectedValue = Int32.Parse(idHeftTextBox.Text);
            if (String.IsNullOrEmpty(idHeftTextBox.Text))
                comboBox3.Text = "";

            listView1.View = View.Details;
            //listView1.View = View.List;
            listView1.MultiSelect = false;
            listView1.FullRowSelect = true;
            listView1.HideSelection = false;
            //foreach (ColumnHeader ch in listView1.Columns)
            //    listView1.AutoResizeColumn(ch.Index, ColumnHeaderAutoResizeStyle.HeaderSize);
            listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            //listView1.Focus();

            if (connection.State != ConnectionState.Open)
                connection.Open();
            SqlCommand c1 = new SqlCommand("SELECT Wort, Übersetzungen FROM Wörte", connection);
            //SqlDataAdapter adapter = new SqlDataAdapter(c1);

            SqlDataReader r1 = c1.ExecuteReader();
            int i = 0;
            ListViewItem[] itemsOfList = new ListViewItem[deutschDataSet.Wörte.Count];
            while (r1.Read())
            {
                //listView1.Items.Add(r1.GetString(0));
                //listView1.Items[i].SubItems.Add(r1.GetString(1));
                //i++;
                itemsOfList[i] = new ListViewItem(r1.GetString(0));
                itemsOfList[i].SubItems.Add(r1.GetString(1));

                listView1.Items.Add(itemsOfList[i]);

                i++;
            }
            r1.Close();

            listView1.MouseDoubleClick += ListView1_MouseDoubleClick;

            listView2.View = View.List;
            //SqlCommand c2 = new SqlCommand("SELECT Satz FROM Sätze JOIN [Wort-Sätze] WHERE [Wort-Sätze].idWort = @idWort", connection); - это не правильно
            SqlCommand c2 = new SqlCommand("SELECT Sätze.Satz FROM Sätze JOIN (Wörte JOIN [Wort-Sätze] ON [Wort-Sätze].idWort = @idWort) ON [Wort-Sätze].idSatz = Sätze.idSatz", connection);
            c2.Parameters.Add("@idWort", SqlDbType.Int).Value = Int32.Parse(idWortTextBox.Text);
            SqlDataReader r2 = c2.ExecuteReader();

            int a = 0;
            while(r2.Read())
            {
                a++;
            }
            int j = 0;
            //ListViewItem[] lw2 = new ListViewItem[deutschDataSet.Sätze.Count];
            //ListViewItem[] lw2 = new ListViewItem[deutschDataSet.Sätze.Count + 7];
            ListViewItem[] lw2 = new ListViewItem[a];
            while (r2.Read())
            {
                lw2[j] = new ListViewItem(r2.GetString(0));
                //lw2[j].SubItems.Add
                listView2.Items.Add(lw2[j]);

                j++;
            }
            r2.Close();
        }

        private void ComboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            idHeftTextBox.Text = comboBox3.SelectedValue.ToString();

            //if (!String.IsNullOrEmpty(idHeftTextBox.Text))
            //    comboBox3.SelectedValue = Int32.Parse(idHeftTextBox.Text);
            //if (String.IsNullOrEmpty(idHeftTextBox.Text))
            //    comboBox3.Text = "";
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //wörteBindingSource.Position = wörteBindingSource.Find(listView1.Columns[0].Name, listView1.Items[listView1.SelectedIndices[0]]);
            //wörteBindingSource.Position = wörteBindingSource.Find("Text", listView1.SelectedItems[0]);
            //wörteBindingSource.Position = wörteBindingSource.Find("Text", listView1.Items[listView1.SelectedIndices[0]]);
            wörteBindingSource.Position = wörteBindingSource.Find("Wort", listView1.SelectedItems[0].Text);
        }

        private void WörteBindingSource_PositionChanged(object sender, EventArgs e)
        {
            //comboBox3.SelectedValueChanged -= ComboBox3_SelectedValueChanged;
            //comboBox3.Text = null;

            if (String.IsNullOrEmpty(idArtikelTextBox.Text))
                comboBox1.Text = "";

            listView2.Items.Clear();

            if (Int32.Parse(idWortTextBox.Text) > 0)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                listView2.View = View.Details;
                SqlCommand c2 = new SqlCommand("SELECT Sätze.Satz, Sätze.Satzübersetzung FROM Sätze JOIN (Wörte JOIN [Wort-Sätze] ON [Wort-Sätze].idWort = Wörte.idWort) ON [Wort-Sätze].idSatz = Sätze.idSatz WHERE Wörte.idWort = @idWort", connection);
                c2.Parameters.Add("@idWort", SqlDbType.Int).Value = Int32.Parse(idWortTextBox.Text);
                SqlDataReader r2 = c2.ExecuteReader();
                int j = 0;
                ListViewItem[] lw2 = new ListViewItem[deutschDataSet.Sätze.Count];
                while (r2.Read())
                {
                    lw2[j] = new ListViewItem(r2.GetString(0));
                    lw2[j].SubItems.Add(r2.GetString(1));
                    listView2.Items.Add(lw2[j]);

                    j++;
                }
                r2.Close();
            }

            if (!String.IsNullOrEmpty(idArtikelTextBox.Text))
                comboBox1.SelectedValue = Int32.Parse(idArtikelTextBox.Text);
            if (!String.IsNullOrEmpty(idWortartTextBox.Text))
                comboBox2.SelectedValue = Int32.Parse(idWortartTextBox.Text);
            //if (!String.IsNullOrEmpty(idArtikelTextBox.Text))
            //    comboBox3.SelectedValue = Int32.Parse(idHeftTextBox.Text);
            //comboBox3.SelectedValueChanged += ComboBox3_SelectedValueChanged;
        }

        private void WörteBindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            //if(!String.IsNullOrEmpty(idArtikelTextBox.Text))
            //    comboBox1.SelectedValue = Int32.Parse(idArtikelTextBox.Text);
            //if(!String.IsNullOrEmpty(idWortartTextBox.Text))
            //    comboBox2.SelectedValue = Int32.Parse(idWortartTextBox.Text);

            //listView2.Items.Clear();

            ////if(!wörteBindingSource.AddingNew)
            //if (Int32.Parse(idWortTextBox.Text) > 0)
            //{
            //    if (connection.State != ConnectionState.Open)
            //        connection.Open();
            //    //listView2.View = View.List;
            //    listView2.View = View.Details;
            //    //SqlCommand c2 = new SqlCommand("SELECT Satz FROM Sätze JOIN [Wort-Sätze] WHERE [Wort-Sätze].idWort = @idWort", connection); - это не правильно
            //    //SqlCommand c2 = new SqlCommand("SELECT Sätze.Satz FROM Sätze JOIN (Wörte JOIN [Wort-Sätze] ON [Wort-Sätze].idWort = @idWort) ON [Wort-Sätze].idSatz = Sätze.idSatz", connection);
            //    SqlCommand c2 = new SqlCommand("SELECT Sätze.Satz, Sätze.Satzübersetzung FROM Sätze JOIN (Wörte JOIN [Wort-Sätze] ON [Wort-Sätze].idWort = Wörte.idWort) ON [Wort-Sätze].idSatz = Sätze.idSatz WHERE Wörte.idWort = @idWort", connection);
            //    c2.Parameters.Add("@idWort", SqlDbType.Int).Value = Int32.Parse(idWortTextBox.Text);
            //    SqlDataReader r2 = c2.ExecuteReader();
            //    int j = 0;
            //    ListViewItem[] lw2 = new ListViewItem[deutschDataSet.Sätze.Count];
            //    //ListViewItem[] lw2 = new ListViewItem[]
            //    while (r2.Read())
            //    {
            //        lw2[j] = new ListViewItem(r2.GetString(0));
            //        lw2[j].SubItems.Add(r2.GetString(1));
            //        //lw2[j].SubItems.Add
            //        listView2.Items.Add(lw2[j]);

            //        j++;
            //    }
            //    r2.Close();
            //}
            
        }

        private void ComboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            idWortartTextBox.Text = comboBox2.SelectedValue.ToString();
        }

        private void ComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            idArtikelTextBox.Text = comboBox1.SelectedValue.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wörteBindingSource.AddNew();

            datumInDBDateTimePicker.Text = null;
            datumInDBDateTimePicker.Value = DateTime.Today;

            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            if (connection.State != ConnectionState.Open)
                connection.Open();
            SqlCommand command = new SqlCommand("SELECT Wort FROM Wörte WHERE (Wort LIKE + '%' + @wort + '%')", connection);
            command.Parameters.Add("@wort", SqlDbType.NVarChar).Value = textBox1.Text;
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                listView1.Items.Add(reader.GetString(0));

            reader.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            SqlCommand insComm = new SqlCommand("INSERT INTO Sätze (Satz, Satzübersetzung, SatzInDB) VALUES (@Satz, @Satzübersetzung, GETDATE())", connection);
            insComm.Parameters.Add("@Satz", SqlDbType.NVarChar).Value = SatzInsertTextBox.Text;
            insComm.Parameters.Add("@Satzübersetzung", SqlDbType.NVarChar).Value = SatzübersetzungTextBox.Text;
            insComm.ExecuteNonQuery();

            SqlCommand cmdWörteSätze = new SqlCommand("INSERT INTO [Wort-Sätze] (idWort, idSatz) VALUES (@idWort, @idSatz)", connection);
            cmdWörteSätze.Parameters.Add("@idWort", SqlDbType.Int).Value = Int32.Parse(idWortTextBox.Text);

            //SqlCommand cmd2 = new SqlCommand("SELECT idSatz FROM Sätze WHERE idSatz = SCOPE_IDENTITY()", connection);
            SqlCommand cmd2 = new SqlCommand("WITH bottom AS (SELECT TOP 1 idSatz FROM Sätze ORDER BY idSatz DESC) SELECT * FROM bottom", connection);

            //cmdWörteSätze.Parameters.Add("@idSatz", SqlDbType.BigInt).Value = 6;
            cmdWörteSätze.Parameters.Add("@idSatz", SqlDbType.BigInt).Value = cmd2.ExecuteScalar();
            cmdWörteSätze.ExecuteNonQuery();

            sätzeTableAdapter.Fill(deutschDataSet.Sätze);

            SatzInsertTextBox.Text = "";
            SatzübersetzungTextBox.Text = "";

            listView2.Items.Clear();

            if (Int32.Parse(idWortTextBox.Text) > 0)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                listView2.View = View.Details;
                SqlCommand c2 = new SqlCommand("SELECT Sätze.Satz, Sätze.Satzübersetzung FROM Sätze JOIN (Wörte JOIN [Wort-Sätze] ON [Wort-Sätze].idWort = Wörte.idWort) ON [Wort-Sätze].idSatz = Sätze.idSatz WHERE Wörte.idWort = @idWort", connection);
                c2.Parameters.Add("@idWort", SqlDbType.Int).Value = Int32.Parse(idWortTextBox.Text);
                SqlDataReader r2 = c2.ExecuteReader();
                int j = 0;
                ListViewItem[] lw2 = new ListViewItem[deutschDataSet.Sätze.Count];
                while (r2.Read())
                {
                    lw2[j] = new ListViewItem(r2.GetString(0));
                    lw2[j].SubItems.Add(r2.GetString(1));
                    listView2.Items.Add(lw2[j]);

                    j++;
                }
                r2.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("https://slovari.yandex.ru/" + wortTextBox.Text + "/%D0%BF%D0%B5%D1%80%D0%B5%D0%B2%D0%BE%D0%B4/");
        }
    }
}
