using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;
namespace kayıtOl_giris

{
    public partial class girisYap : Form
    {
        public girisYap()
        {
            InitializeComponent();
        }
        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size=4096;user id=MuhammetTRZ_SQLLogin_1;pwd=9hbeo7oosu;data source=Muhammet.mssql.somee.com;persist security info=False;initial catalog=Muhammet;TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);
        int hesabımDropDownMenu = 2;
        int HesapYönetimiDropDownMenu = 2;
        long tcTekMi;
             



        public static bool SifreDogrula(string girilenSifre, string veriTabanındakiSifre)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(girilenSifre, veriTabanındakiSifre);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                return false;
            }


        }
        private void button1_Click(object sender, EventArgs e)
        {
            //kullanıcı bilgileri eksik mi girmiş kontrol eder.
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Lütfen bilgilerinizi eksiksiz giriniz.");
                return;
            }

            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                
                connect.Open();
                // Kullanıcının TC kimlik numarasına göre veritabanındaki hashlenmiş şifreyi al
                string sifreAlanSqlKod = "SELECT sifre FROM try_to_login WHERE tc = @tc";
                using (SqlCommand komut2 = new SqlCommand(sifreAlanSqlKod, connect))
                {
                    komut2.Parameters.Add("@tc", SqlDbType.BigInt).Value = Convert.ToInt64(textBox1.Text);
                    object sonuc = komut2.ExecuteScalar();//tek bir değer dönmesini bekleyen döngü

                    if (sonuc != null)
                    {
                        string veriTabanındakiHash = sonuc.ToString();

                        // Kullanıcının girdiği şifreyi hash ile doğrula
                        if (SifreDogrula(textBox2.Text,veriTabanındakiHash))
                        {
                            // Şifre doğruysa sadece tc ile giriş yap
                            string girisSqlKod = "SELECT kimlik, tc FROM try_to_login WHERE tc=@tc";
                            using (SqlCommand komut = new SqlCommand(girisSqlKod, connect))
                            {
                                komut.Parameters.AddWithValue("@tc", textBox1.Text);

                                using (SqlDataReader reader = komut.ExecuteReader())
                                {
                                    if (reader.Read())//kullanıcı bilgileri var ise
                                    {
                                        int kimlik = (int)reader["kimlik"];
                                        long tc = (long)reader["tc"];

                                        //// Kullanıcının girdiği TC kimlik numarası ile veritabanındaki eşleşiyor mu kontrol et
                                        if (long.TryParse(textBox1.Text, out long tcFromTextbox))
                                        {
                                            if(tc == tcFromTextbox)
                                            {
                                                MessageBox.Show("Girişiniz yapılmıştır.");
                                                anaSayfa anaSayfaForm = new anaSayfa(kimlik, tc);//anaSayfaForm'a iki değer götür
                                                anaSayfaForm.Show();
                                                this.Hide();
                                            }
                                            else
                                            {
                                                MessageBox.Show("TC kimlik numaranız eşleşmiyor.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Kullanıcı bulunamadı.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Hatalı şifre veya TC girdiniz. Şifre: " + textBox2.Text + ", TC: " + textBox1.Text, "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Böyle bir kullanıcı bulunamadı.");
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            kayıtcs kayıtForm = new kayıtcs();
            kayıtForm.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form1 menüForm = new Form1();
            menüForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Hide();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back){
                e.Handled = true;
            }
        }

        private void girisYap_Load(object sender, EventArgs e)
        {
            Button[] butonlar = { button13, button16, button17, button18, button19 ,button29};
            TextBox[] metinKutuları = { textBox1, textBox2 };

            Dictionary<TextBox, Panel> metinKutusuPanelleri = new Dictionary<TextBox, Panel>
            {//ilgili textboxları ilgili paneller ile eşleştirdik
                {textBox1,panel3},
                {textBox2,panel6 }
            };

            foreach (var button in butonlar)//bu kod sayesinde her bir butonun mouse enter yada leave özelliğine kod yazmaya gerek kalmaaycak
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }

                foreach(var item in metinKutusuPanelleri)
                {
                    var metinKutsu = item.Key;
                    var panel = item.Value;
                    metinKutsu.MouseEnter += (s, args) => panel.BackColor = Color.FromArgb(80, 200, 120);
                    metinKutsu.MouseLeave += (s, args) => panel.BackColor = Color.White;
             }
            textBox1.MaxLength = 11;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^([1-9]\d{10})$";//başlangıç 0 değil,sonra 10 rakam
            if (long.TryParse(textBox1.Text, out long tc))
            {
                tcTekMi = (tc % 2 == 0) ? 0 : 1; // Çiftse 0, tekse 1

                if (Regex.IsMatch(textBox1.Text, pattern) && tcTekMi == 0)
                {
                    pictureBox5.Visible = true;
                    pictureBox3.Visible = false;
                }
                else
                {
                    pictureBox5.Visible = false;
                    pictureBox3.Visible = true;
                }
            }
            else
            {
               //
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^\d{5}";
            string pattern2 = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[-.\/]).{8,}$";
            if (Regex.IsMatch(textBox2.Text, pattern) || Regex.IsMatch(textBox2.Text, pattern2))
            {
                pictureBox2.Visible = true;
                pictureBox4.Visible = false;
            }
            else
            {
                pictureBox2.Visible = false;
                pictureBox4.Visible = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            sifreDegistir sifreDegistirform = new sifreDegistir();
            sifreDegistirform.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            telNoGuncelle TNGForm = new telNoGuncelle();
            TNGForm.Show();
            this.Hide();
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
        }

        private void button13_Click_2(object sender, EventArgs e)
        {
            string veriTabanındakiHash = "$2a$11$8G3Z7LSkV3llUSjTaN7KrOXIGuwMrKwRvx4TtCglY5yuonVEwc.yS";
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();
                string databasedenHaslemisSifreAlanSqlKodu = "select sifre from try_to_login where tc=@tc";
                using (SqlCommand komut = new SqlCommand(databasedenHaslemisSifreAlanSqlKodu, connect))
                {
                    komut.Parameters.AddWithValue("@tc",textBox1.Text);
                    object sonuc = komut.ExecuteScalar();
                    if (sonuc != null)
                    {
                        string haslenmisSifre=sonuc.ToString();
                        if (haslenmisSifre == veriTabanındakiHash)
                        {
                            MessageBox.Show("eşittir");
                        }
                        else
                        {
                            MessageBox.Show("değildir");
                        }
                    }
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if(hesabımDropDownMenu % 2  == 0)
            {
                panel4.Visible = true;
                hesabımDropDownMenu++;
            }
            else
            {
                panel4.Visible=false;
                hesabımDropDownMenu++;
            }
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.ForeColor = Color.FromArgb(80, 200, 120);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.ForeColor = Color.White;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            kayıtcs kayıtForm = new kayıtcs();
            kayıtForm.Show();
            this.Close();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Close();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            sifreDegistir sifreDegistirform = new sifreDegistir();
            sifreDegistirform.Show();
            this.Close();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            telNoGuncelle TNGForm = new telNoGuncelle();
            TNGForm.Show();
            this.Close();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            TcGuncelleme TcGuncelleForm = new TcGuncelleme();
            TcGuncelleForm.Show();
            this.Close();
        }

        private void button13_MouseEnter(object sender, EventArgs e)
        {
            button13.ForeColor = Color.FromArgb(80, 200, 120);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if(HesapYönetimiDropDownMenu % 2 == 0)
            {
                panel5.Visible = true;
                HesapYönetimiDropDownMenu++;

            }
            else
            {
                panel5.Visible = false;
                HesapYönetimiDropDownMenu++;
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            Kitap_islemleri kitapİslemleriForm = new Kitap_islemleri();
            kitapİslemleriForm.Show();
            this.Hide();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            hesapSilme hesapSilmeForm = new hesapSilme();
            hesapSilmeForm.Show();
            this.Close();
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }
    }
}