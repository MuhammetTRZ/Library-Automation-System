using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace kayıtOl_giris
{
    public partial class kullanıcıSorguİslemleri : Form
    {
        private int kullanıcıKimlik;
        public kullanıcıSorguİslemleri()
        {
            InitializeComponent();
            
        }

        public int kitapListesiSayac = 2;
        public int KSButton2Sayac = 2;
        public int KSButton3Sayac = 2;
        public int KSButton4Sayac = 2;

        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size=4096;user id=MuhammetTRZ_SQLLogin_1;pwd=9hbeo7oosu;data source=Muhammet.mssql.somee.com;persist security info=False;initial catalog=Muhammet;TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);
        private void button1_Click(object sender, EventArgs e)
        {
            if (KSButton2Sayac % 2 == 0)
            {
                KSButton2Sayac++;
                textBox1.Visible = true;
                button2.Visible = true;
            }
            else
            {
                KSButton2Sayac++;
                textBox1.Visible = false;
                button2.Visible = false;
            }
         
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void kullanıcıSorguİslemleri_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = 11;
            textBox2.MaxLength = 11;
            textBox3.MaxLength = 11;
            textBox4.MaxLength = 11;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Lütffen bir tc kimlik giriniz giriniz");
                return;
            }
            try
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string geciktirenleriGetirenSqlKodu = "select t.isim ,k.eserAdı,k.alısTarihi,k.vermesiGerekenTarih from kitapHareketleri k join try_to_login t on t.kimlik=k.kullanıcıID where k.tc=@tc and geri_verildi_mi='verilmedi'";
                    using (SqlCommand komut = new SqlCommand(geciktirenleriGetirenSqlKodu, connect))
                    {
                        komut.Parameters.AddWithValue("@tc",Convert.ToInt64(textBox1.Text));

                        using (SqlDataAdapter da = new SqlDataAdapter(komut))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if(dt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = dt;
                            }else
                            {
                                MessageBox.Show("TC kimlik numarası sistemde kayıtlı değil .", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                            }

                        }
                            
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hatalı TC veya şifre!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))    
            {
                MessageBox.Show("Lütffen bir tc kimlik giriniz giriniz");
                return;
            }
            try
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    //join ile iki tablodan istenilen veriler alındı 
                    string geciktirenleriGetirenSqlKodu = "select t.isim ,k.eserAdı from kitapHareketleri k join try_to_login t on t.kimlik=k.kullanıcıID where k.tc=@tc";
                    using (SqlCommand komut = new SqlCommand(geciktirenleriGetirenSqlKodu, connect))
                    {
                        komut.Parameters.AddWithValue("@tc", Convert.ToInt64(textBox2.Text));
                        
                            using (SqlDataAdapter da = new SqlDataAdapter(komut))
                            {//ve bu veriler datagridview de gösterildi
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                if(dt.Rows.Count > 0)
                                {
                                       dataGridView1.DataSource = dt;
                                }
                            else
                            {
                                MessageBox.Show("TC kimlik numarası sistemde kayıtlı değil .","Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Warning); ;
                            }
                               
                            }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("hata meydana geldi" + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(kitapListesiSayac % 2 == 0)
            {//Basit bir matematik işlemi sayesinde butonun ikinci basışta kapanması sağlanıyor 
                kitapListesiSayac++;
                textBox2.Visible = true;
                button5.Visible = true;
                
            }
            else
            {
                kitapListesiSayac++;
                textBox2.Visible=false;
                button5.Visible=false;
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox3.Visible = true;
            button7.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Lütffen bir tc kimlik giriniz giriniz");
                return;
            }
            try
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string geciktirenleriGetirenSqlKodu = "select t.isim ,k.eserAdı,k.alısTarihi from kitapHareketleri k join try_to_login t on t.kimlik=k.kullanıcıID where k.tc=@tc and geri_verildi_mi='verilmedi' and datediff(day,cast(GETDATE() as date),vermesiGerekenTarih)<=3";
                    using (SqlCommand komut = new SqlCommand(geciktirenleriGetirenSqlKodu, connect))
                    {//join ile iki tablodan istenilen veriler alındı 
                        komut.Parameters.AddWithValue("@tc", Convert.ToInt64(textBox3.Text));

                        using (SqlDataAdapter da = new SqlDataAdapter(komut))
                        {//ve bu veriler datagridview'DE gösterildi
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if(dt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = dt;
                            }else
                            {
                                MessageBox.Show("TC kimlik numarası sistemde kayıtlı değil .", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("hata meydana geldi" + ex.Message);
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if(KSButton3Sayac % 2 == 0)
            {//Basit bir matematik işlemi sayesinde butonun ikinci basışta kapanması sağlanıyor 
                KSButton3Sayac++;
                textBox3.Visible = true;
                button7.Visible = true;
            }
            else
            {
                KSButton3Sayac++;
                textBox3.Visible = false;
                button7.Visible = false;
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (KSButton4Sayac % 2 ==0)
            {//Basit bir matematik işlemi sayesinde butonun ikinci basışta kapanması sağlanıyor 
                KSButton4Sayac++;
                textBox4.Visible = true;
                button9.Visible = true;
            }
            else
            {
                KSButton4Sayac++;
                textBox4.Visible = false;
                button9.Visible = false;
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Lütffen bir tc kimlik giriniz giriniz");
                return;
            }
            try
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string geciktirenleriGetirenSqlKodu = "select * from try_to_login where tc=@tc";
                    using (SqlCommand komut = new SqlCommand(geciktirenleriGetirenSqlKodu, connect))
                    {
                        komut.Parameters.AddWithValue("@tc", Convert.ToInt64(textBox4.Text));

                        using (SqlDataAdapter da = new SqlDataAdapter(komut))
                        {//try_to_login tablosundan kişinin tüm bilgisi veriliyor
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = dt;
                            }
                            else
                            {
                                MessageBox.Show("TC kimlik numarası sistemde kayıtlı değil .", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("hata meydana geldi" + ex.Message);
            }
        }
    }
}
