﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kayıtOl_giris
{
    public partial class TcGuncelleme : Form
    {
        public TcGuncelleme()
        {
            InitializeComponent();
        }
        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size=4096;user id=MuhammetTRZ_SQLLogin_1;pwd=9hbeo7oosu;data source=Muhammet.mssql.somee.com;persist security info=False;initial catalog=Muhammet;TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);

        int hesabımDropDownMenu = 2;
        int HesapYönetimiDropDownMenu = 2;
        long tcTekMi;
        int kod;
        int kalanSüre = 120;
        int DogrulamaKodu;
        object kullanıcıMaili;
        object kullanıcıİsmi;

        void bilgilendirmeMaili()
        {
            
                // Kullanıcıdan alınan bilgileri değişkenlere atıyoruz
                string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
                string toEmail = kullanıcıMaili.ToString(); // Alıcı e-posta adresi
                string subject = "📱 Gebze Belediyesi Kütüphanesi Tc Kimlik Numaranızı Güncelleyin"; // Konu   
                string body = $@"<html>
                                <head>
                                        <style>
                                             .kutu{{
                                            max-width: 600px;
                                            margin: auto;
                                            padding: 20px;
                                            font-family: Arial, Helvetica, sans-serif;
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
                                        ul li{{
                                            padding: 5px;
                                        }}
                                        </style>
                                </head>
                                <body>
                                    <div class=""kutu"">
                                        <p class=""baslık"">Merhaba {kullanıcıİsmi},</p>
                                        <p>Hesabınıza kayıtlı tc kimlik numarası başarıyla güncellenmiştir.</p>
                                        <p class=""önemli"">📌 Yeni Tc Kimlik Numaranız: {textBox3.Text}</p>
                                        <p class=""listeBaslık"">⚠ Önemli Hatırlatma:</p>
                                        <ul>
                                            <li>Eğer bu değişikliği siz yapmadıysanız, hemen bizimle iletişime geçin.</li>
                                            <li>Hesap güvenliğinizi sağlamak için şifrenizi değiştirmeyi düşünebilirsiniz.</li>
                                        </ul>
                                        <p>Herhangi bir sorunuz olursa destek ekibimize ulaşabilirsiniz.</p>
                                        <p>📚 Gebze Belediyesi Kütüphanesi</p>
                                        <p>✉️ Destek muhammetaliterzi04@gmail.com</p>
                                    </div>
                                </body>
                                </html>";

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
                    MessageBox.Show("Doğrulama kodu E-postanıza yollanmıştır!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Hata mesajı
                    MessageBox.Show("E-posta gönderilemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
        }
        void dogrulamaMailiGonder()
        {
            string isimAlanSqlKodu = "select isim from try_to_login where tc=@tc";
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();
                using(SqlCommand komut = new SqlCommand(isimAlanSqlKodu, connect))
                {
                    komut.Parameters.AddWithValue("@tc", textBox1.Text);
                    kullanıcıİsmi = komut.ExecuteScalar();

                    // Kullanıcıdan alınan bilgileri değişkenlere atıyoruz
                    string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
                    string toEmail = kullanıcıMaili.ToString(); // Alıcı e-posta adresi
                    string subject = "📱 Gebze Belediyesi Kütüphanesi Tc Kimlik Numaranızı Güncelleyin"; // Konu   
                    string body = $@"<html>
                                        <head>
                                            <style>
                                                .kutu{{
                                                    max-width: 600px;
                                                    margin: auto;
                                                    padding: 20px;
                                                    font-family: Arial, Helvetica, sans-serif;
                                                    border: 1px solid #ddd;
                                                    border-radius: 20px;
                                                    background-color: #f9f9f9;
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
                                                ul li{{
                                                    padding: 5px;
                                                }}
                                            </style>
                                        </head>
                                        <body>
                                            <div class=""kutu"">
                                                <p class=""baslık"">Merhaba {kullanıcıİsmi},</p>
                                                <p>Kütüphane hesabınız için tc kimlik numaranızı güncellemek istediğinizi belirttiniz. Bu işlemi tamamlamak için aşağıdaki doğrulama kodunu ilgili alana girmeniz gerekmektedir:</p>
                                                <p class=""Önemli"">🔢 Doğrulama Kodunuz: {DogrulamaKodu}</p>
                                                <p class=""listeBaslık"">⚠ Önemli Hatırlatmalar:</p>
                                                <ul>
                                                    <li>Bu kod yalnızca <b>2<b/> dakika boyunca geçerlidir.</li>
                                                    <li>Eğer telefon numarası değişikliği talebinde bulunmadıysanız, lütfen hemen bizimle iletişime geçin.</li>
                                                </ul>
                                                <p>Herhangi bir sorun yaşarsanız destek ekibimize ulaşabilirsiniz.</p>
                                                <p>📚 Gebze Belediyesi Kütüphanesi</p>
                                                <p>✉️ Destek muhammetaliterzi04@gmail.com</p>
                                            </div>
                                        </body>
                                        </html>";

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
                        MessageBox.Show("Doğrulama kodu E-postanıza yollanmıştır!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Hata mesajı
                        MessageBox.Show("E-posta gönderilemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            //panel 10
            if(hesabımDropDownMenu % 2 == 0)
            {
                panel10.Visible = true;
                hesabımDropDownMenu++;
            }
            else
            {
                panel10.Visible=false;
                hesabımDropDownMenu++;
            }
        }

        private void TcGuncelleme_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = 11;
            textBox2.MaxLength = 4;
            textBox3.MaxLength = 11;
            textBox6.MaxLength = 5;

            Random rnd = new Random();
            kod = rnd.Next(1000,9999);
            DogrulamaKodu = rnd.Next(11111, 99999);//doğrulama kodu ıluşturuldu
            label3.Text= kod.ToString();


            Button[] butonlar = { button18, button19, button3, button4, button5 ,button8};

            foreach (var button in butonlar)//bu kod sayesinde her bir butonun mouse enter yada leave özelliğine kod yazmaya gerek kalmaaycak
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }

            Dictionary<TextBox, Panel> metinKutularıPanelleri = new Dictionary<TextBox, Panel>
            {
                {textBox1,panel6 },
                {textBox3,panel1},
                {textBox2,panel4 },
                {textBox6,panel7},
            };
            foreach (var item in metinKutularıPanelleri)
            {
                var metinKutsu = item.Key;
                var panel = item.Value;
                metinKutsu.MouseEnter += (s, args) => panel.BackColor = Color.FromArgb(80, 200, 120);
                metinKutsu.MouseLeave += (s, args) => panel.BackColor = Color.White;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            //panel 5
            if (HesapYönetimiDropDownMenu % 2 == 0)
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

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            PictureBox[] hataFotoları = { pictureBox3, pictureBox4, pictureBox6, };
            if(hataFotoları.Any(uyarılar => uyarılar.Visible))
            {//resim kutularını foreach döngüsü gibi kontrol ederek 1 tane bile resim kutusunun görünür olup olmadığına bakar
                MessageBox.Show("Hatalı alanları düzeltmelisiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TextBox[] metinKutuları = { textBox1, textBox2, textBox3 };
            if (metinKutuları.Any(metinler => string.IsNullOrEmpty(metinler.Text)))
            {//metin kutularını foreach döngüsü gibi kontrol ederek 1 tane bile metin kutusunun boş olup olmadığına bakar
                MessageBox.Show("tüm bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //7 9 10 
           

            using(SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();
                string mailAlanSqlKodu = "select mail from try_to_login where tc=@tc";
                using (SqlCommand komut = new SqlCommand(mailAlanSqlKodu, connect))
                {
                    komut.Parameters.AddWithValue("@tc", textBox1.Text);
                     kullanıcıMaili= komut.ExecuteScalar();
                    dogrulamaMailiGonder();
                    //doğrulama kodu maili yollama fonksiyonu burada olacak(parametre=mail)
                    textBox6.Visible = true;
                    label7.Visible = true;
                    label9.Visible = true;
                    label10.Visible = true;
                    panel7.Visible = true;
                    button12.Visible = true;
                    timer1.Start();
                }
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^([1-9]\d{10})$";//başlangıç 0 değil,sonra 10 rakam
            if (long.TryParse(textBox3.Text, out long tc))
            {
                tcTekMi = (tc % 2 == 0) ? 0 : 1; // Çiftse 0, tekse 1
                if (Regex.IsMatch(textBox3.Text, pattern) && tcTekMi == 0 && textBox1.Text != textBox3.Text)
                {
                    pictureBox1.Visible = true;
                    pictureBox6.Visible = false;
                }
                else
                {
                    pictureBox1.Visible = false;
                    pictureBox6.Visible = true;
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^(\d{4})$";
            if (Regex.IsMatch(textBox2.Text, pattern) && textBox2.Text == kod.ToString())
            {//metin kutusuna her bir değer girildiğinde,metin regex yani bizim oluşturduğumuz patterna(düzene)uyuyor mu
                pictureBox2.Visible = true;
                pictureBox4.Visible = false;
            }
            else
            {
                pictureBox2.Visible = false;
                pictureBox4.Visible = true;
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            kalanSüre--;
            label10.Text = kalanSüre.ToString();
            if (kalanSüre == 0)
            {
                timer1.Stop();
                MessageBox.Show("Doğrulama Kodunuzu zamanında giremediniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DogrulamaKodu = (-1234567);
                textBox6.Visible = false;
                label10.Visible = false;
                label9.Visible = false;
                label7.Visible = false;
                button7.Visible = false;
                panel7.Visible = false;
                button12.Visible = false;
                kalanSüre = 120;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            PictureBox[] uyarıFotoları = { pictureBox4, pictureBox3, pictureBox6 };
            if (uyarıFotoları.Any(uyarılar => uyarılar.Visible))
            {//resim kutularını foreach döngüsü gibi kontrol ederek 1 tane bile resim kutusunun görünür olup olmadığına bakar
                MessageBox.Show("Hatalı alanları düzeltmelisiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TextBox[] metinKutuları = { textBox1, textBox2, textBox3, textBox6 };
            if (metinKutuları.Any(metinler => string.IsNullOrEmpty(metinler.Text)))
            {//metin kutularını foreach döngüsü gibi kontrol ederek 1 tane bile metin kutusunun boş olup olmadığına bakar
                MessageBox.Show("tüm bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox6.Text == DogrulamaKodu.ToString())//girilen değer doğrulama koduna eşit mi
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string telefonNoGuncelleyenKod = "update try_to_login set tc =@yeniTcNo where tc=@tc";
                    using (SqlCommand komut2 = new SqlCommand(telefonNoGuncelleyenKod, connect))//tc'ye bağlı kişinin numarası güncellendi
                    {
                        komut2.Parameters.AddWithValue("@yeniTcNo", textBox3.Text);
                        komut2.Parameters.AddWithValue("@tc", textBox1.Text);
                        komut2.ExecuteNonQuery();
                        bilgilendirmeMaili();
                        MessageBox.Show("Tc kimlik numaranız güncellenmiştir\nBu sayfa otomatik olarak kapanacaktır", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1 form1 = new Form1();
                        form1.Show();
                        this.Hide();
                    }
                }
            }
            else
            {
                MessageBox.Show("hatalı doğrulama kodu", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            sifreDegistir sifreDegistirform = new sifreDegistir();
            sifreDegistirform.Show();
            this.Close();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            girisYap girisForm = new girisYap();
            girisForm.Show();
            this.Close();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            girisYap girisForm = new girisYap();
            girisForm.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            telNoGuncelle TNGForm = new telNoGuncelle();
            TNGForm.Show();
            this.Close();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            hesapSilme hesapSilmeForm = new hesapSilme();
            hesapSilmeForm.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Kitap_islemleri kitapİslemleriForm = new Kitap_islemleri();
            kitapİslemleriForm.Show();
            this.Close();
        }
    }
}
