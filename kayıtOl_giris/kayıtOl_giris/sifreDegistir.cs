using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection.Emit;

namespace kayıtOl_giris
{
    public partial class sifreDegistir : Form
    {
        public sifreDegistir()
        {
            InitializeComponent();
    
            textBox1.TextChanged += textBox1_TextChanged;
            textBox2.TextChanged += textBox2_TextChanged;
            textBox3.TextChanged += textBox3_TextChanged;
            textBox4.TextChanged += textBox4_TextChanged;
        }
        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size=4096;user id=MuhammetTRZ_SQLLogin_1;pwd=9hbeo7oosu;data source=Muhammet.mssql.somee.com;persist security info=False;initial catalog=Muhammet;TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);
        int kod;
        int dogurlamaKodu;
        int kalanSüre = 120;
        public int gözSayac = 2;
        object sonuc;
        object mail;
        int hesabımDropDownMenu = 2;
        int HesapYönetimiDropDownMenu = 2;
        long tcTekMi;
        public string sifreHasle(string x)
        {
            return BCrypt.Net.BCrypt.HashPassword(x);
        }
        
        void yeniSifreYolla(string isim)
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();
                string isimAlmaSqlKodu = "select mail from try_to_login where tc=@tc";
                using (SqlCommand komut = new SqlCommand(isimAlmaSqlKodu, connect))
                {
                    komut.Parameters.AddWithValue("@tc", textBox1.Text);
                    mail = komut.ExecuteScalar();
                }
            }

            string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
            string toEmail = mail.ToString(); // Alıcı e-posta adresi
            string subject = "Gebze Belediyesi Kütüphanesi Şifre değiştirme işlemi onaylandı"; // Konu
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
                                .listeBaslık{{
                                    color: orange;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class=""kutu"">
                                <p class=""baslık"">Merhaba <b>{isim}</b></p>
                                <p>Hesabınızın şifresi başarıyla değiştirildi. Eğer bu değişikliği siz yaptıysanız, bu e-postayı göz ardı edebilirsiniz.</p>
                                <p>🛡️ Eğer şifre değişikliğini siz yapmadıysanız, hemen bizimle iletişime geçin!</p>
                                <p class=""listeBaslık"">📌 Önemli Güvenlik İpuçları:</p>
                                <ul>
                                    <li>Şifrenizi kimseyle paylaşmayın.</li>
                                    <li>Hesabınızı güvende tutmak için şifrenizi düzenli olarak güncelleyin.</li>
                                    <li>Eğer tanımadığınız bir cihazdan giriş yapıldığını fark ederseniz, hemen destek ekibimizle iletişime geçin.</li>
                                </ul>
                                <p><b><i>İyi günler dileriz</i></b></p>
                                <p>📚 Gebze Belediyesi Kütüphanesi</p>
                                <p>✉️ [Destek E-posta Adresi]</p>
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
                //MessageBox.Show("Doğrulama kodu E-postanıza yollanmıştır!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Hata mesajı
                MessageBox.Show("E-posta gönderilemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void mailYolla(string x)
        {

            using(SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();
                string isimAlmaSqlKodu = "select isim from try_to_login where tc=@tc";
                using(SqlCommand komut = new SqlCommand(isimAlmaSqlKodu, connect))
                {
                    komut.Parameters.AddWithValue("@tc", textBox1.Text);
                    sonuc = komut.ExecuteScalar();
                }
            }

            // Kullanıcıdan alınan bilgileri değişkenlere atıyoruz
            string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
            string toEmail = x; // Alıcı e-posta adresi
            string subject = "Gebze Belediyesi Kütüphanesi Şifre Değiştirme Doğrulama Kodu"; // Konu
            string body = $@"<html lang=""en"">
                                <head>
                                    <style>
                                        .kutu{{
                                            max-width: 600px;
                                            margin: auto;
                                            border: 1px solid #ddd;
                                            padding: 20px;
                                            background-color: #f9f9f9;
                                            font-family: Arial, Helvetica, sans-serif;
                                        }}
                                        .baslik{{
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
                                        <p class=""baslik"">Merhaba {sonuc}</p>
                                        <p>Şifre değiştirme talebinde bulundunuz. Güvenliğinizi sağlamak amacıyla, işlemi tamamlayabilmeniz için aşağıdaki doğrulama kodunu kullanmanız gerekmektedir:</p>
                                        <p class=""önemli"">✅ Doğrulama Kodunuz: {dogurlamaKodu}</p>
                                        <p>Lütfen bu kodu şifre değiştirme ekranındaki ilgili alana girerek işlemi tamamlayın.</p>
                                        <p class=""listeBaslık"">⚠ Önemli:</p>
                                        <ul>
                                            <li>Bu kod sadece belirli bir süre için geçerlidir ve tek kullanımlıktır.</li>
                                            <li>Eğer bu talebi siz yapmadıysanız, hesabınızın güvenliği için hemen bizimle iletişime geçin.</li>
                                            <li>Kodunuzu kimseyle paylaşmayın.</li>
                                        </ul>
                                        <p><b><i>İyi günler dileriz</i></b></p>
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


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^([1-9]\d{10})$";//başlangıç 0 değil,sonra 10 rakam
            if (long.TryParse(textBox1.Text, out long tc))
            {
                tcTekMi = (tc % 2 == 0) ? 0 : 1; // Çiftse 0, tekse 1
                if (Regex.IsMatch(textBox1.Text, pattern) && tcTekMi == 0)
                {
                    pictureBox5.Visible = true;
                    pictureBox4.Visible = false;
                }
                else
                {
                    pictureBox5.Visible = false;
                    pictureBox4.Visible = true;
                }
            }
        }

        private void sifreDegistir_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = 11;
            //textBox3.MaxLength = 5;
            textBox2.MaxLength = 5;
            textBox6.MaxLength = 6;

            Button[] butonlar = { button19,button20,button22,button23, button24 };

            foreach (var button in butonlar)
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }


            Dictionary<TextBox, Panel> metinKutularıPanelleri = new Dictionary<TextBox, Panel>
            {
                {textBox1,panel6 },
                {textBox3,panel4 },
                {textBox4,panel7 },
                {textBox5,panel5 },
                {textBox2,panel8 },
                {textBox6,panel9 }
            };
            foreach (var item in metinKutularıPanelleri)
            {
                var metinKutusu = item.Key;
                var panel = item.Value;

                metinKutusu.MouseEnter += (s, arg) => panel.BackColor = Color.FromArgb(80, 200, 120);
                metinKutusu.MouseLeave += (s, args) => panel.BackColor = Color.White;
            }




            Random rnd = new Random();
            dogurlamaKodu = rnd.Next(111111,999999);
            kod = rnd.Next(10000,99999);
            label3.Text=kod.ToString();

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string pattern2 = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[-.\/]).{8,}$";
            string pattern = @"^(\d{5})";
            if (Regex.IsMatch(textBox3.Text, pattern) || Regex.IsMatch(textBox3.Text,pattern2))
            {
                pictureBox2.Visible = true;
                pictureBox1.Visible = false;
                
            }
            else
            {
                pictureBox2.Visible = false;
                pictureBox1.Visible = true;
            }
          
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^(\d{5})";
            string pattern2 = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[-._]).{8,}$";
            if (Regex.IsMatch(textBox4.Text,pattern2))  
            {
                pictureBox6.Visible = true;
                pictureBox3.Visible = false;
            }
            else
            {
                pictureBox6.Visible = false;
                pictureBox3.Visible = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                PictureBox[] uyarıFotoları = {pictureBox4,pictureBox1,pictureBox3,pictureBox7,pictureBox9};
                 if(uyarıFotoları.Any(uyarılar => uyarılar.Visible)){
                    MessageBox.Show("Hatalı alanları düzeltmelisiniz","Uyarı",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    }
                TextBox[] metinKutuları = {textBox1,textBox2,textBox3,textBox4,textBox5,textBox6};        
                if(metinKutuları.Any(metinler => string.IsNullOrEmpty(metinler.Text))){
                    MessageBox.Show("tüm bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                if (textBox6.Text == dogurlamaKodu.ToString())
                {
                    string haslenmisSifre=sifreHasle(textBox4.Text);
                    using (SqlConnection connect = new SqlConnection(databaseLink))
                    {
                        connect.Open();
                        string isimAlanSqlKodu = "select isim from try_to_login where tc=@tc";
                        using(SqlCommand komut3 = new SqlCommand(isimAlanSqlKodu, connect))
                        {
                            komut3.Parameters.AddWithValue("@tc", textBox1.Text);
                            object sonuc = komut3.ExecuteScalar();

                            string telefonNoGuncelleyenKod = "update try_to_login set sifre=@sifre where tc=@tc";
                            using (SqlCommand komut2 = new SqlCommand(telefonNoGuncelleyenKod, connect))
                            {
                                komut2.Parameters.AddWithValue("@sifre", haslenmisSifre);
                                komut2.Parameters.AddWithValue("@tc", textBox1.Text);
                                komut2.ExecuteNonQuery();
                                yeniSifreYolla(sonuc.ToString());
                                MessageBox.Show("Şifreniz güncellenmiş ve bilgilendirme maili yollanmıştır. \n Bu sayfa olarak kapanacaktır", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Form1 form1 = new Form1();
                                form1.Show();
                                this.Hide();
                            }
                        }
                        
                    }
                }
                else
                {
                    MessageBox.Show("hatalı doğrulama kodu", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("hata meydana geldi"+ hata.Message);
            }
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^(\d{5})";
            if(Regex.IsMatch(textBox2.Text, pattern) && kod.ToString()==textBox2.Text.ToString())
            {
                pictureBox10.Visible = true;
                pictureBox9.Visible = false;
            }
            else
            {
                pictureBox10.Visible = false;
                pictureBox9.Visible = true;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^(\d{5})";
            string pattern2 = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[-.\/]).{8,}$";
            if (Regex.IsMatch(textBox5.Text, pattern2) && textBox5.Text==textBox4.Text)
            {
                pictureBox8.Visible = true;
                pictureBox7.Visible = false;
            }
            else
            {
                pictureBox8.Visible = false;
                pictureBox7.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Hide();
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

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form1 menüForm = new Form1();
            menüForm.Show();
            this.Hide();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            PictureBox[] uyarıFotoları = { pictureBox4, pictureBox1, pictureBox3, pictureBox7, pictureBox9 };
            if (uyarıFotoları.Any(uyarılar => uyarılar.Visible))
            {
                MessageBox.Show("Hatalı alanları düzeltmelisiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }//bir tane bile resim kutusun görünürlüğünün açık olup olmadığını foreach döngüsü gibi içlerinde gezerek kontrol eder
            TextBox[] metinKutuları = { textBox1, textBox2, textBox3, textBox4, textBox5 };
            if (metinKutuları.Any(metinler => string.IsNullOrEmpty(metinler.Text)))
            {
                MessageBox.Show("tüm bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                //bir tane bile metin kutusun boş olup olmadığını foreach döngüsü gibi içlerinde gezerek kontrol eder
            }

            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();
                string mailAlanSqlKodu = "select mail from try_to_login where tc=@tc";//kime mail yollanacak?
                using (SqlCommand komut = new SqlCommand(mailAlanSqlKodu, connect))
                {
                    komut.Parameters.AddWithValue("@tc", textBox1.Text);
                    object sonuc = komut.ExecuteScalar();//mail bunun içinde

                    //neden ? kullandım
                    //sonuc null ise, ToString() çağırmaz ve null döner.
                    //sonuc null değilse, ToString() çağırır.
                    // ! durum bu ikiside değilse işlemlere devam et
                    if (!string.IsNullOrEmpty(sonuc?.ToString()))
                    {
                        timer1.Start();
                        mailYolla(sonuc.ToString());

                        textBox6.Visible = true;
                        label8.Visible = true;
                        label6.Visible = true;
                        label9.Visible = true;
                        label10.Visible = true;
                        button7.Visible = true;
                        panel9.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("böyle bir TC'ye sahip kimse bulunamadı");
                        return;
                    }

                }
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
                dogurlamaKodu = (-1234567);
                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                textBox6.Visible = false;
                button7.Visible = false;
                panel9.Visible = false;
                kalanSüre = 120;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            telNoGuncelle TNGForm = new telNoGuncelle();
            TNGForm.Show();
            this.Hide();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            if(gözSayac % 2 == 0)//butona her basışta sayı artar ve basit bir işlem kontolü ile sonuca göre işlem açılır veya kapanır
            {
                textBox4.UseSystemPasswordChar = true;
                gözSayac++;
            }
            else
            {
                textBox4.UseSystemPasswordChar = false;
                gözSayac++;
            }
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            label11.Visible = true;
            label12.Visible = true;
            label13.Visible = true;
            label14.Visible = true;
            label15.Visible = true;
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            if(hesabımDropDownMenu % 2 == 0)
            {
                panel10.Visible = true;
                hesabımDropDownMenu++;
            }
            else
            {
                panel10.Visible = false;
                hesabımDropDownMenu++;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if(HesapYönetimiDropDownMenu % 2 == 0)
            {
                panel1.Visible = true;
                HesapYönetimiDropDownMenu++;
            }
            else
            {
                panel1.Visible = false;
                HesapYönetimiDropDownMenu++;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
         
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

        private void button22_Click(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Close();
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
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
    }
}
