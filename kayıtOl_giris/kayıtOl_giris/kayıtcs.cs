    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;
using BCrypt.Net;


namespace kayıtOl_giris
{
    public partial class kayıtcs : Form
    {
        public kayıtcs()
        {
            InitializeComponent();
            textBox3.TextChanged += textBox3_TextChanged;
            textBox4.TextChanged += textBox4_TextChanged;
        }
        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size=4096;user id=MuhammetTRZ_SQLLogin_1;pwd=9hbeo7oosu;data source=Muhammet.mssql.somee.com;persist security info=False;initial catalog=Muhammet;TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);
        int sifre;
        int dogrulamaKodu;
        int kalanSüre = 120;
        bool süreBittiMi = false;
        int hesabımDropDownMenu = 2;
        int HesapYönetimiDropDownMenu = 2;
        long tcTekMi;

        public string SifreHasle(string x)
        {
            return BCrypt.Net.BCrypt.HashPassword(x);
        }

        void ePostaDoğrulama(string isim)
        {
            //önce mailde yollanacak olan doğrulama kodu oluşturuluyor

            Random rnd = new Random();
            dogrulamaKodu = rnd.Next(10000, 99999);

            string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
            string toEmail = textBox4.Text; // Alıcı e-posta adresi
            string subject = "📧 [Gebze Belediyesi Kütüphanesi] E-posta Doğrulama Kodunuz"; // Konu
            string body = $@"
                        <html>
                            <head>
                                <style>
                                    body{{
                                        font-family: Arial, Helvetica, sans-serif;
                                        color: #333;
                                    }}
                                    .kutu{{
                                        max-width: 600px;
                                        margin: auto;
                                        padding: 20px;
                                        border: 1px solid #ddd;
                                        border-radius: 10px;
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
                                        color: rgb(251, 11, 11);
                                    }}
                                    ul li{{
                                        margin: 5px;
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class='kutu'>
                                    <p class='baslık'>Merhaba{isim}</p>
                                    <p>Kütüphane sistemimize kayıt işleminizi tamamlamak için aşağıdaki doğrulama kodunu ilgili alana girerek e-posta adresinizi doğrulayabilirsiniz:</p>
                                    <p class='önemli'>🔏 Doğrulama Kodunuz: <b>{dogrulamaKodu}</b></p>
                                    <ul>
                                        <li>Bu kod <b>2 dakika</b>  içinde geçerlidir.</li>
                                        <li>Süresi dolduğunda yeni bir kod talep edebilirsiniz.</li>
                                        <li>Eğer siz bu işlemi başlatmadıysanız, bu e-postayı görmezden gelebilirsiniz.nHerhangi bir sorunuz olursa bizimle iletişime geçebilirsiniz.</li>
                                    </ul>
                                    <p><b><i>İyi günler dileriz</i></b></p>
                                    <p>📚 Gebze Belediyesi Kütüphanesi</p>
                                    <p>✉️ Destek muhammetaliterzi04@gmail.com</p>
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
                MessageBox.Show("Doğrulama kodu E-postanıza yollanmıştır!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Hata mesajı
                MessageBox.Show("E-posta gönderilemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        void sifreYolla()
        {
            //önce mailde yollanacak olan şifre oluşturuluyor
            string kullanıcıİsmi = textBox1.Text;
            Random rnd = new Random();
            sifre = rnd.Next(10000, 99999);

            // Kullanıcıdan alınan bilgileri değişkenlere atıyoruz
            string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
            string toEmail = textBox4.Text; // Alıcı e-posta adresi
            string subject = "🔑 [Gebze Belediyesi Kütüphanesi] Giriş İçin Şifreniz"; // Konu
            string body = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                color: #333;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: auto;
                                padding: 20px;
                                border: 1px solid #ddd;
                                border-radius: 10px;
                                background-color: #f9f9f9;
                            }}
                            .header {{
                                font-size: 20px;
                                font-weight: bold;
                                color: #0073e6;
                            }}
                            .warning {{
                                color: red;
                                font-weight: bold;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <p class='header'>Merhaba {kullanıcıİsmi},</p>
                            <p>Kütüphane sistemimize giriş yapabilmeniz için size özel olarak oluşturulan geçici şifreniz aşağıdadır:</p>
                            <p class='warning'>🔐 Geçici Şifreniz: <b>{sifre}</b></p>
                            <p><b>⚠ Önemli:</b></p>
                            <ul>
                                <li>Şifrenizi kimseyle paylaşmayın.</li>
                                <li>Eğer bu isteği siz yapmadıysanız, hemen bizimle iletişime geçin.</li>
                                <li>Şifreyi değiştirmek için sistemdeki <b>Şifre Değiştir</b> alanını kullanabilirsiniz.</li>
                            </ul>
                            <p>İyi günler dileriz,</p>
                            <p>📚 <b>Gebze Belediyesi Kütüphanesi</b></p>
                            <p>✉️ [Destek E-posta Adresi]</p>
                        </div>
                    </body>
                    </html>
                    ";//mail içeriği

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



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox[] metinKutuları = { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6 };
                if (metinKutuları.Any(metinler => string.IsNullOrEmpty(metinler.Text)) || dateTimePicker1.Value.Date == DateTime.Now.Date)
                {
                    MessageBox.Show("tüm bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }//metin kutularını foreach döngüsü gibi kontrol ederek 1 tane bile boş olup olmadığına bakar
                PictureBox[] uyarıResimleri = { pictureBox3, pictureBox4, pictureBox7 };
                if (uyarıResimleri.Any(rk => rk.Visible))
                {
                    MessageBox.Show("hatalı işlemleri düzeltmeden kayıt olamazsınız.", "uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }//resim kutularını foreach döngüsü gibi kontrol ederek 1 tane bile resim kutusunun görünür olup olmadığına bakar
                if (dateTimePicker1.Value.Date == DateTime.Now.Date || dateTimePicker1.Value.Date > DateTime.Now.Date)
                {
                    MessageBox.Show("Günümüz tarihinden büyük olamazsın amk", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                if (textBox6.Text != dogrulamaKodu.ToString())
                {
                    MessageBox.Show("Hatalı Doğrulama kodu", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    timer1.Stop();
                }


                string isim = textBox1.Text;
                string soyad = textBox2.Text;
                string telNo = textBox3.Text;
                string mail = textBox4.Text;




                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    sifreYolla();
                    string sifreHash = SifreHasle(sifre.ToString());//oluşturulan şifre hashleniyor ve değişkenin içine atılıyor



                    string kayıtSqlKod = "INSERT INTO try_to_login (isim, soyAd, telefonNumara, mail, dogumTarihi, sifre,tc) VALUES(@isim, @soyAd, @telNo, @mail,@dg,@sifre,@tc)";

                    using (SqlCommand kayıtKomut = new SqlCommand(kayıtSqlKod, connect))
                    {
                        kayıtKomut.Parameters.AddWithValue("@isim", textBox1.Text);
                        kayıtKomut.Parameters.AddWithValue("@soyAd", textBox2.Text);
                        kayıtKomut.Parameters.AddWithValue("@telNo", textBox3.Text);
                        kayıtKomut.Parameters.AddWithValue("@mail", textBox4.Text);
                        kayıtKomut.Parameters.AddWithValue("@dg", dateTimePicker1.Value);
                        kayıtKomut.Parameters.AddWithValue("@sifre", sifreHash);
                        kayıtKomut.Parameters.AddWithValue("tc", textBox5.Text);

                        kayıtKomut.ExecuteNonQuery();//nonquery delete,update,insert gibi işlemlerde kullanılır
                    }
                }

                MessageBox.Show("Kayıt işleminiz tamamlanmıştır\nGiriş şifreniz mail üzerinden iletilicekttir", "Başarılı.", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Form1 menüForm = new Form1();
                menüForm.Show();
                this.Hide();

            }
            catch (Exception hata)
            {
                MessageBox.Show("Bir sorun oluştu: " + hata.Message);
            }
        }

        private void kayıtcs_Load(object sender, EventArgs e)
        {
            textBox5.MaxLength = 11;
            textBox3.MaxLength = 11;
            textBox6.MaxLength = 5;
            label10.Text = "kalan süre" + kalanSüre.ToString();

            //17 18 19 21
            Button[] butonlar = { button4,button17, button18, button19, button21, button16 };

            foreach (var button in butonlar)
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }
            Dictionary<TextBox, Panel> metinKutusuPanelleri = new Dictionary<TextBox, Panel>
            {
                { textBox1, panel3 },
                { textBox2, panel4 },
                { textBox3, panel6 },
                { textBox4, panel5 },
                { textBox5, panel7 },
                { textBox6, panel9 },
            };
            foreach(var item in metinKutusuPanelleri)
            {
                var metinKutusu = item.Key;
                var panel = item.Value;

                metinKutusu.MouseEnter += (s, arg) => panel.BackColor = Color.FromArgb(80, 200, 120);
                metinKutusu.MouseLeave += (s, args) => panel.BackColor = Color.White;
            }


           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kullanıcıİsmi = textBox1.Text;
            TextBox[] metinKutuları = { textBox1, textBox2, textBox3, textBox4, textBox5 };
            if (metinKutuları.Any(metinler => string.IsNullOrEmpty(metinler.Text)) || dateTimePicker1.Value.Date == DateTime.Now.Date)
            {
                MessageBox.Show("tüm bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }//metin kutularını foreach döngüsü gibi kontrol ederek 1 tane bile boş olup olmadığına bakar
            PictureBox[] uyarıResimleri = { pictureBox3, pictureBox4, pictureBox7 };
            if (uyarıResimleri.Any(rk => rk.Visible))
            {
                MessageBox.Show("hatalı işlemleri düzeltmeden kayıt olamazsınız.", "uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }//metin kutularını foreach döngüsü gibi kontrol ederek 1 tane bile boş olup olmadığına bakar
            if (dateTimePicker1.Value.Date == DateTime.Now.Date || dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Günümüz tarihinden büyük olamazsın amk", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            ePostaDoğrulama(kullanıcıİsmi);

            label7.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            textBox6.Visible = true;
            button2.Visible = true;
            button1.Visible = true;

            timer1.Start();
            button2.Enabled = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Hide();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)//girilen değer sayı ve silme tuşu değilse değer girişini engeller
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Space)
            {
                e.Handled = true;//girilen değer harf ve silme tuşu değilse değer girişini engeller
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;//girilen değer harf ve silme tuşu değilse değer girişini engeller
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)//girilen değer sayı ve silme tuşu değilse değer girişini engeller
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^(\d{11})";
            if (Regex.IsMatch(textBox3.Text, pattern) && textBox3.Text.StartsWith("0"))
            {//kutuya her bir değer girildiğinde,metin regex yapısına(pattern)'a uyuşuyor mu ve 0 ile mi başlıyor ona bakar
                pictureBox2.Visible = true;
                pictureBox3.Visible = false;
            }
            else
            {
                pictureBox2.Visible = false;
                pictureBox3.Visible = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^.*@[^@]+\.com$";
            if (Regex.IsMatch(textBox4.Text, pattern))
            {//kutuya her bir değer girildiğinde,metin regex yapısına(pattern)'a uyuşuyor mu diye bajar
                pictureBox5.Visible = true;
                pictureBox4.Visible = false;
            }
            else
            {
                pictureBox5.Visible = false;
                pictureBox4.Visible = true;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^(\d{11})";
            if (Regex.IsMatch(textBox5.Text, pattern))
            {//kutuya her bir değer girildiğinde,metin regex yapısına(pattern)'a uyuşuyor mu
                pictureBox6.Visible = true;
                pictureBox7.Visible = false;
            }
            else
            {
                pictureBox6.Visible = false;
                pictureBox7.Visible = true;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            sifreDegistir sifreDegistirform = new sifreDegistir();
            sifreDegistirform.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            kalanSüre--;
            label10.Text = "Kalan süre:" + kalanSüre.ToString();

            if (kalanSüre == 0)
            {
                timer1.Stop();
                label10.Text = "Süre bitmiştir";
                MessageBox.Show("Süre bittiği için artık işlem yapamazsınız", "Süre Bitti", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dogrulamaKodu = (int)-123456;
                kalanSüre = 120;
                label7.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                textBox6.Visible = false;
                button1.Visible = false;
                button2.Enabled = true;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string kullanıcıİsmi = textBox1.Text;
            TextBox[] metinKutuları = { textBox1, textBox2, textBox3, textBox4, textBox5 };
            if (metinKutuları.Any(metinler => string.IsNullOrEmpty(metinler.Text)) || dateTimePicker1.Value.Date == DateTime.Now.Date)
            {
                MessageBox.Show("tüm bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }//metin kutularını foreach döngüsü gibi kontrol ederek 1 tane bile boş olup olmadığına bakar
            PictureBox[] uyarıResimleri = { pictureBox3, pictureBox4, pictureBox7 };
            if (uyarıResimleri.Any(rk => rk.Visible))
            {
                MessageBox.Show("hatalı işlemleri düzeltmeden kayıt olamazsınız.", "uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }//metin kutularını foreach döngüsü gibi kontrol ederek 1 tane bile boş olup olmadığına bakar
            if (dateTimePicker1.Value.Date == DateTime.Now.Date || dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Günümüz tarihinden büyük olamazsın amk", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void button20_Click(object sender, EventArgs e)
        {
            if(HesapYönetimiDropDownMenu % 2 == 0)
            {
                panel11.Visible = true;
                HesapYönetimiDropDownMenu++;
            }
            else
            {
                panel11.Visible = false;
                HesapYönetimiDropDownMenu++;
            }
        }

        private void dateTimePicker1_MouseEnter(object sender, EventArgs e)
        {
            panel8.BackColor = Color.FromArgb(80, 200, 120);
        }

        private void dateTimePicker1_MouseLeave(object sender, EventArgs e)
        {
            panel8.BackColor = Color.Silver;
        }

        private void pictureBox8_Click_1(object sender, EventArgs e)
        {
            Form1 form1Sayfa = new Form1();
            form1Sayfa.Show();
            this.Hide();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Close();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            girisYap girisForm = new girisYap();
            girisForm.Show();
            this.Close();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            sifreDegistir sifreDegistirForm = new sifreDegistir();
            sifreDegistirForm.Show();
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

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {
            string pattern = @"^([1-9]\d{10})$";//başlangıç 0 değil,sonra 10 rakam
            if (long.TryParse(textBox5.Text, out long tc))
            {
                tcTekMi = (tc % 2 == 0) ? 0 : 1; // Çiftse 0, tekse 1
                if (Regex.IsMatch(textBox5.Text, pattern) && tcTekMi == 0)
                {
                    pictureBox6.Visible = true;
                    pictureBox7.Visible = false;
                }
                else
                {
                    pictureBox6.Visible = false;
                    pictureBox7.Visible = true;
                }
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            hesapSilme hesapSilmeForm = new hesapSilme();
            hesapSilmeForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Kitap_islemleri kitapİslemleriForm = new Kitap_islemleri();
            kitapİslemleriForm.Show();
            this.Close();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            kalanSüre--;
            label10.Text = kalanSüre.ToString();

            if (kalanSüre == 0)
            {
                label10.Text = "süreniz bitmiştir";
                MessageBox.Show("Süre bittiği için artık işlem yapamazsınız", "Süre Bitti", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dogrulamaKodu = (-1234567);
                kalanSüre = 120;
                label10.Visible = false;
                label9.Visible = false;
                label7.Visible = false;
                textBox6.Visible = false;
                panel9.Visible = false;
            }
        }

        //ePostaDoğrulama(kullanıcıİsmi);
        //
        //label7.Visible = true;
        //label9.Visible = true;
        //label10.Visible = true;
        //textBox6.Visible = true;
        //button2.Visible = true;
        //button1.Visible = true;
        //
        //timer1.Start();
        //button2.Enabled = false;
    }

   
}
        

        
    

