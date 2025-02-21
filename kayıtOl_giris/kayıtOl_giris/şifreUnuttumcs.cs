using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace kayıtOl_giris
{
    public partial class şifreUnuttumcs : Form
    {
        public şifreUnuttumcs()
        {
            InitializeComponent();
        }
        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size=4096;user id=MuhammetTRZ_SQLLogin_1;pwd=9hbeo7oosu;data source=Muhammet.mssql.somee.com;persist security info=False;initial catalog=Muhammet;TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);
        int kod;
        int sifre;
        int kalanSüre = 120;
        int dogrulamaKodu;
        int hesabımDropDownMenu = 2;
        int HesapYönetimiDropDownMenu = 2;
        long tcTekMi;
        void yeniSifreYolla(int sifre,string isim)
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();
                string epostaBulanSqlKodu = "select mail from try_to_login where tc=@tc";
                using (SqlCommand komut = new SqlCommand(epostaBulanSqlKodu, connect))
                {
                    komut.Parameters.AddWithValue("@tc", textBox1.Text);
                    object sonuc = komut.ExecuteScalar();

                    if (sonuc != null)
                    {
                        string mailHesap = sonuc.ToString();

                        string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
                        string toEmail = mailHesap; // Alıcı e-posta adresi
                        string subject = "Gebze Belediyesi Kütüphanesi] Yeni Şifreniz"; // Konu
                        string body = $@"<html>
                                    <head>
                                       <style>
                                            .kutu{{
                                                max-width: 600px;
                                                margin: auto;
                                                padding: 20px;
                                                border: 1px solid #ddd;
                                                border-radius: 20px;
                                                background-color: #f9f9f9;
                                            }}
                                            .baslık{{
                                                font-size: 20px;
                                                font-weight: bold;
                                                color: #0073e6;
                                            }}
                                            .önemli{{
                                                font-size: 20px;
                                                font-weight: bold;
                                                color: red;
                                            }}
                                            .listeBaslık{{
                                                color: orange;
                                            }}
                                        </style>
                                    </head>
                                       
                                    <body>
                                        <div class='kutu'>
                                            <p class='baslık'>Merhaba <b>{isim}</b></p>
                                            <p>Şifre değişikliği talebiniz başarıyla tamamlandı. Sisteme giriş yapabilmeniz için yeni şifreniz aşağıdadır:</p>
                                            <p class='önemli'>🔐 Yeni Şifreniz: <b>{sifre}</b></p>
                                            <p class='listeBaslık'>⚠ Önemli:</p>
                                            <ul>
                                                <li>Güvenliğiniz için lütfen giriş yaptıktan sonra şifrenizi hemen değiştirin.</li>
                                                <li>Şifrenizi kimseyle paylaşmayın ve güçlü bir şifre belirleyin.</li>
                                                <li>Eğer bu işlemi siz yapmadıysanız, hemen bizimle iletişime geçin.</li>
                                            </ul>
                                            <p><b>İyi günler dileriz</b></p>
                                            <p>📚 Gebze Belediyesi Kütüphanesi</p>
                                            <p>✉️ [Destek E-posta Adresi]</p>
                                        </div>  
                                    </body>
                                    </html>"; // Mesaj metni

                        // SMTP ayarları
                        string smtpServer = "smtp.gmail.com"; // Gmail için SMTP server
                        int smtpPort = 587; // Gmail SMTP portu
                        string smtpUsername = "muhammetaliterzi04@gmail.com"; // Gmail kullanıcı adı (e-posta)
                        string smtpPassword = "alnb wlap earj aiao"; // Gmail şifresi

                        try
                        {
                            // SmtpClient ile e-posta gönderimi
                            SmtpClient smtp = new SmtpClient(smtpServer)
                            {
                                Port = smtpPort,
                                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                                EnableSsl = true

                            };

                            MailMessage mail = new MailMessage
                            {
                                From = new MailAddress(fromEmail),
                                Subject = subject,
                                Body = body,
                                IsBodyHtml = true
                            };
                            mail.To.Add(toEmail);

                            // Maili gönder
                            smtp.Send(mail);

                            // Gönderim başarılı mesajı
                            MessageBox.Show("E-posta başarıyla gönderildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            // Hata mesajı
                            MessageBox.Show("E-posta gönderilemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        public string SifreHasle(string x)
        {
            return BCrypt.Net.BCrypt.HashPassword(x);
        }
        void dogrulamaKoduGonder(int x,string isim)
        {

            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();
                string epostaBulanSqlKodu = "select mail from try_to_login where tc=@tc";//kime mail yollayacağız?
                using (SqlCommand komut = new SqlCommand(epostaBulanSqlKodu, connect))
                {
                    komut.Parameters.AddWithValue("@tc", textBox1.Text);
                    object sonuc = komut.ExecuteScalar();//kişinin maili 'sonuc'içinde

                    if (sonuc != null)
                    {
                        string mailHesap = sonuc.ToString();

                        string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
                        string toEmail = mailHesap; // Alıcı e-posta adresi
                        string subject = "🔐 [Gebze Belediyesi Kütüphanesi] Şifre Değiştirme Doğrulama Kodu"; // Konu
                        string body = $@"<html lang=""en"">
                                            <head>
                                                <style>
                                                    .kutu{{
                                                        max-width: 600px;
                                                        margin: auto;
                                                        padding: 20px;
                                                        border: 1px solid #ddd;
                                                        border-radius: 20px;
                                                        background-color: #f9f9f9;
                                                        font-family: Arial, Helvetica, sans-serif;
                                                    }}
                                                    .baslık{{
                                                        font-size: 20px;
                                                        font-weight: bold;
                                                        color: #0073e6;
                                                    }}
                                                    .Önemli{{
                                                        font-size: 20px;
                                                        font-weight: bold;
                                                        color: red;
                                                    }}
                                                    .listeBaslık{{
                                                        color: orange;
                                                    }}
                                                </style>
                                            </head>
                                            <body>
                                                <div class='kutu'>
                                                    <p class='baslık'>Merhaba {isim}</p>
                                                    <p>Şifre değiştirme talebinde bulunduğunuz için bu e-postayı alıyorsunuz. Şifrenizi güvenli bir şekilde değiştirebilmeniz için doğrulama kodunuz aşağıdadır</p>
                                                    <p class='Önemli'>📌 Doğrulama Kodunuz: <b>{dogrulamaKodu}</b></p>
                                                    <p>Lütfen bu kodu şifre değiştirme ekranına girerek işlemi tamamlayın.</p>
                                                    <p class='listeBaslık'><b>⚠ Önemli:</b></p>
                                                    <ul>
                                                        <li>Eğer bu isteği siz yapmadıysanız, hesabınızın güvenliğini sağlamak için hemen bizimle iletişime geçin.</li>
                                                        <li>Kodun geçerlilik süresi <b>2</b> dakika ile sınırlıdır. Süre dolarsa yeni bir kod talep edebilirsiniz.</li>
                                                    </ul>
                                                    <p><b><i>İyi günler dileriz</i></b></p>
                                                    <p>📚 Gebze Belediyesi Kütüphanesi</p>
                                                    <p>✉️ [Destek E-posta Adresi]</p>
                                                </div>
                                            </body>
                                            </html>";//mail içeriği


                        // SMTP ayarları
                        string smtpServer = "smtp.gmail.com"; // Gmail için SMTP server
                        int smtpPort = 587; // Gmail SMTP portu
                        string smtpUsername = "muhammetaliterzi04@gmail.com"; // Gmail kullanıcı adı (e-posta)
                        string smtpPassword = "alnb wlap earj aiao"; // Gmail şifresi

                        try
                        {
                            // SmtpClient ile e-posta gönderimi
                            SmtpClient smtp = new SmtpClient(smtpServer)
                            {
                                Port = smtpPort,
                                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                                EnableSsl = true

                            };

                            MailMessage mail = new MailMessage
                            {
                                From = new MailAddress(fromEmail),
                                Subject = subject,
                                Body = body,
                                IsBodyHtml = true
                                
                            };
                            mail.To.Add(toEmail);

                            // Maili gönder
                            smtp.Send(mail);

                            // Gönderim başarılı mesajı
                            MessageBox.Show("E-posta başarıyla gönderildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            // Hata mesajı
                            MessageBox.Show("E-posta gönderilemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }

                    



                }


            }    
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox[] metinKutuları = { textBox1, textBox2 };

                if(metinKutuları.Any(metinler => string.IsNullOrEmpty(metinler.Text)) || textBox2.Text!=kod.ToString())
                {
                    MessageBox.Show("tüm bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }//bir tane bile metin kutusun boş olup olmadığını foreach döngüsü gibi içlerinde gezerek kontrol eder

                Random rnd = new Random();
                sifre = rnd.Next(10000,99999);// sonradan mail içinde kullnacağımız şifre oluşturuldu
                dogrulamaKodu = rnd.Next(100000,999999);// sonradan mail içinde kullnacağımız doğrulama kodu oluşturuldu

                //4 6 7 
                label4.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                textBox6.Visible = true;
                button13.Visible = true;
                panel8.Visible = true;
                timer1.Start();//geri sayım başladı


                using(SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string isimAlanSqlKodu = "select isim from try_to_login where tc=@tc";
                    using (SqlCommand komut = new SqlCommand(isimAlanSqlKodu, connect))
                    {
                        komut.Parameters.AddWithValue("@tc",textBox1.Text);
                        object kullanıcıİsim= komut.ExecuteScalar();
                        //mailin içinde kullanıcının ismini kullanacığımız için sql sorgusu ile bunu alıp değişken içine koyduk ve
                        dogrulamaKoduGonder(dogrulamaKodu,kullanıcıİsim.ToString()); //parametre ile ilgili fonksiyona yolladık
                    }
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("hata"+hata.Message);
            }
            
        }

        private void şifreUnuttumcs_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            kod=rnd.Next(1111,9999);//form da bulunan işleme devam etmek için aynısının yazılması gereken kod
            label3.Text = kod.ToString();
            textBox1.MaxLength = 11;
            textBox2.MaxLength = 4;

            Dictionary<TextBox, Panel> metinKutusuPanelleri = new Dictionary<TextBox, Panel>
            {
                {textBox1,panel6 },
                {textBox2,panel7},
            };
            foreach(var item in metinKutusuPanelleri)
            {
                var metinKutusu = item.Key;
                var panel = item.Value;

                metinKutusu.MouseEnter += (s, arg) => panel.BackColor = Color.FromArgb(80, 200, 120);
                metinKutusu.MouseLeave += (s, args) => panel.BackColor = Color.White;
            }

            Button[] butonlar = {button19,button20,button21,button23,button24};
            foreach (var button in butonlar)
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            kayıtcs kayıtForm = new kayıtcs();
            kayıtForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            girisYap girisForm = new girisYap();
            girisForm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form1 menüForm = new Form1();
            menüForm.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();//uygulamayı kapatır.
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)//girilen değer sayı değilse ve silme tuşu değilse yazımı engeller
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)//girilen değer sayı değilse ve silme tuşu değilse yazımı engeller
            {
                e.Handled = true;
            }
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
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

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

        private void button13_Click(object sender, EventArgs e)
        {

            TextBox[] metinKutuları = { textBox1, textBox2 ,textBox6};

            if (metinKutuları.Any(metinler => string.IsNullOrEmpty(metinler.Text)) || textBox2.Text != kod.ToString())
            {
                MessageBox.Show("tüm bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }//bir tane bile metin kutusun boş olup olmadığını foreach döngüsü gibi içlerinde gezerek kontrol eder
            if (textBox6.Text != dogrulamaKodu.ToString())
            {//mailde yollanan doğrulama kodu doğru mu girilmiş diye bakar
                MessageBox.Show("Hatalı doğrulama kodu girdiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using(SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();

                string isimAlanSqlKodu = "select isim from try_to_login where tc=@tc";
                using(SqlCommand komut2 = new SqlCommand(isimAlanSqlKodu, connect))//mailde kullanacağımız kişinin ismi tc'sine göre alınmış
                {
                    komut2.Parameters.AddWithValue("@tc", textBox1.Text);
                    object sonuc= komut2.ExecuteScalar();//ilk satırın ilk sutününu alır / şuan isim sonuc içinde bekliyor 

                    string sifreSıfırlamaSqlKodu = "update try_to_login set sifre=@yeniSifre where tc=@tc";//tc'sine göre kişinin şifresi güncelleniyor

                    using (SqlCommand komut = new SqlCommand(sifreSıfırlamaSqlKodu, connect))
                    {
                        string haslenmisSifre = SifreHasle(sifre.ToString());//sistem tarafından verilen 5 basamaklı şifre hashlendi
                        komut.Parameters.AddWithValue("@yeniSifre", haslenmisSifre); 
                        komut.Parameters.AddWithValue("@tc", textBox1.Text);

                        komut.ExecuteNonQuery();
                        timer1.Stop();
                        yeniSifreYolla(sifre,sonuc.ToString());

                        MessageBox.Show("şifre sıfırlama işleminiz tamamlanmıştır\nbu sayfa otomatik kapanacaktır..", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1 form1 = new Form1();
                        form1.Show();

                    }
                }

                
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            kalanSüre--;
            label4.Text = kalanSüre.ToString();

            if(kalanSüre == 0)
            {
                label4.Text = "süreniz bitmiştir";
                MessageBox.Show("Süre bittiği için artık işlem yapamazsınız", "Süre Bitti", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dogrulamaKodu = (-1234567);
                kalanSüre = 120;
                label4.Visible  = false;
                label6.Visible  = false;
                label7.Visible  = false;
                textBox6.Visible = false;
                panel8.Visible = false;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Form1 menüForm = new Form1();
            menüForm.Show();
            this.Hide();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if(hesabımDropDownMenu % 2 == 0)
            {
                panel4.Visible = true;
                hesabımDropDownMenu++;
            }
            else
            {
                panel4.Visible = false;
                hesabımDropDownMenu++;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if(HesapYönetimiDropDownMenu % 2 == 0)
            {
                panel5.Visible = true;
                HesapYönetimiDropDownMenu ++;
            }
            else
            {
                panel5.Visible = false;
                HesapYönetimiDropDownMenu++;
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            girisYap girisForm = new girisYap();
            girisForm.Show();
            this.Close();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            girisYap girisForm = new girisYap();
            girisForm.Show();
            this.Close();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            sifreDegistir sifreDegistirform = new sifreDegistir();
            sifreDegistirform.Show();
            this.Close();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            telNoGuncelle TNGForm = new telNoGuncelle();
            TNGForm.Show();
            this.Close();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            TcGuncelleme TcGuncelleForm = new TcGuncelleme();
            TcGuncelleForm.Show();
            this.Close();
        }

        private void button22_Click(object sender, EventArgs e)
        {

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

        private void button3_Click_1(object sender, EventArgs e)
        {
            Kitap_islemleri kitapİslemleriForm = new Kitap_islemleri();
            kitapİslemleriForm.Show();
            this.Close();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            hesapSilme hesapSilmeForm = new hesapSilme();
            hesapSilmeForm.Show();
            this.Close();
        }
    }
}
