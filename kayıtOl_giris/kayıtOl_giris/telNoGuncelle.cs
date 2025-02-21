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
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace kayıtOl_giris
{
    public partial class telNoGuncelle : Form
    {
        public telNoGuncelle()
        {
            InitializeComponent();
        }
        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size=4096;user id=MuhammetTRZ_SQLLogin_1;pwd=9hbeo7oosu;data source=Muhammet.mssql.somee.com;persist security info=False;initial catalog=Muhammet;TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);
        int kod;
        int DogrulamaKod;
        int kalanSüre = 120;
        object kullanıcıİsmi;
        object kullanıcıMaili;
        int hesabımDropDownMenu = 2;
        int HesapYönetimiDropDownMenu = 2;
        long tcTekMi;

        void bilgilendirmeMaili()
        {
            // Kullanıcıdan alınan bilgileri değişkenlere atıyoruz
            string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
            string toEmail = kullanıcıMaili.ToString(); // Alıcı e-posta adresi
            string subject = "📱 Gebze Belediyesi Kütüphanesi Telefon Numaranızı Güncelleyin"; // Konu   
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
                                        <p>Hesabınıza kayıtlı telefon numarası başarıyla güncellenmiştir.</p>
                                        <p class=""önemli"">📌 Yeni Telefon Numaranız: {textBox3.Text}</p>
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
        void mailYolla(string x)
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();
                string isimAlanSqlKodu = "select isim from try_to_login where tc=@tc";//mailin içinde kullanacağımız için kişinin ismini aldık 
                using(SqlCommand komut = new SqlCommand(isimAlanSqlKodu,connect))
                {
                    komut.Parameters.AddWithValue("@tc", textBox1.Text);
                    kullanıcıİsmi = komut.ExecuteScalar();

                    // Kullanıcıdan alınan bilgileri değişkenlere atıyoruz
                    string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
                    string toEmail = x; // Alıcı e-posta adresi
                    string subject = "📱 Gebze Belediyesi Kütüphanesi Telefon Numaranızı Güncelleyin"; // Konu   
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
                                                <p>Kütüphane hesabınız için telefon numaranızı güncellemek istediğinizi belirttiniz. Bu işlemi tamamlamak için aşağıdaki doğrulama kodunu ilgili alana girmeniz gerekmektedir:</p>
                                                <p class=""Önemli"">🔢 Doğrulama Kodunuz: {DogrulamaKod}</p>
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
        private void telNoGuncelle_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = 11;
            textBox2.MaxLength = 4;
            textBox3.MaxLength = 11;
            
            textBox6.MaxLength = 6;
            Random rnd = new Random();
            DogrulamaKod = rnd.Next(11111,99999);//doğrulama kodu ıluşturuldu

            kod = rnd.Next(1111,9999);//form üzerinde aynısı ilgili kutuya yazılması gereken kod oluşturuld
            label3.Text= kod.ToString();

            Dictionary<TextBox, Panel> metinKutularıPanelleri = new Dictionary<TextBox, Panel>
            {
                {textBox1,panel6 },
                {textBox3,panel1 },
                {textBox2,panel4 },
                {textBox6,panel7 },
            };
            foreach (var item in metinKutularıPanelleri)
            {
                var metinKutusu = item.Key;
                var panel = item.Value;

                metinKutusu.MouseEnter += (s, arg) => panel.BackColor = Color.FromArgb(80, 200, 120);
                metinKutusu.MouseLeave += (s, args) => panel.BackColor = Color.White;
            }
            Button[] butonlar = { button8,button18, button19, button1, button2, button4, button5 };

            foreach (var button in butonlar)
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {//girilen değer sayı değilse ve silme tuşu değilse yazımı engellenir
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^0\d{10}$";
            if(Regex.IsMatch(textBox3.Text, pattern))
            {//metin kutusuna her bir değer girildiğinde,metin regex yani bizim oluşturduğumuz patterna(düzene)uyuyor mu
                pictureBox1.Visible = true;
                pictureBox6.Visible = false;
            }
            else
            {
                pictureBox1.Visible = false;
                pictureBox6.Visible = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {//girilen değer sayı değilse ve silme tuşu değilse yazımı engellenir
                e.Handled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^(\d{4})$";
            if (Regex.IsMatch(textBox2.Text, pattern) && textBox2.Text==kod.ToString())
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

        private void button2_Click(object sender, EventArgs e)
        {

            PictureBox[] uyarıFotoları = { pictureBox4, pictureBox3, pictureBox6 };
            if (uyarıFotoları.Any(uyarılar => uyarılar.Visible))
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

            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                connect.Open();
                string mailAlanSqlKodu = "select mail from try_to_login where tc=@tc";//mail göndereceğimiz kişinin maili alındı
                using(SqlCommand komut = new SqlCommand(mailAlanSqlKodu, connect))
                {
                    komut.Parameters.AddWithValue("@tc", textBox1.Text);
                    kullanıcıMaili= komut.ExecuteScalar();//mail başka fonksiyonlarda kullanılacaı için globalde tutuluyor
                    //neden ? kullandım
                    //sonuc null ise, ToString() çağırmaz ve null döner.
                    //sonuc null değilse, ToString() çağırır.
                    if (!string.IsNullOrEmpty(kullanıcıMaili?.ToString()))    
                    {
                        timer1.Start();
                        mailYolla(kullanıcıMaili.ToString());
                        label7.Visible = true;
                        label9.Visible = true;
                        label10.Visible = true;
                        textBox6.Visible = true;
                        button12.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("böyle bir TC'ye sahip kimse bulunamadı");
                        return;
                    }

                }
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
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
            TextBox[] metinKutuları = { textBox1, textBox2, textBox3 ,textBox6};
            if (metinKutuları.Any(metinler => string.IsNullOrEmpty(metinler.Text)))
            {//metin kutularını foreach döngüsü gibi kontrol ederek 1 tane bile metin kutusunun boş olup olmadığına bakar
                MessageBox.Show("tüm bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(textBox6.Text == DogrulamaKod.ToString())//girilen değer doğrulama koduna eşit mi
            {
                using(SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string telefonNoGuncelleyenKod = "update try_to_login set telefonNumara =@yeniTelNo where tc=@tc";
                    using (SqlCommand komut2 = new SqlCommand(telefonNoGuncelleyenKod, connect))//tc'ye bağlı kişinin numarası güncellendi
                    {
                        komut2.Parameters.AddWithValue("@yeniTelNo", textBox3.Text);
                        komut2.Parameters.AddWithValue("@tc", textBox1.Text);
                        komut2.ExecuteNonQuery();
                        bilgilendirmeMaili();
                        MessageBox.Show("telefon numaranız güncellenmiştir\nBu sayfa otomatik olarak kapanacaktır","Başarılı",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1 form1 = new Form1();
                        form1.Show();
                        this.Hide();
                    }
                }
            }
            else
            {
                MessageBox.Show("hatalı doğrulama kodu","Uyarı",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            kalanSüre--;
            label10.Text = kalanSüre.ToString();
            if(kalanSüre == 0)
            {
                timer1.Stop();
                MessageBox.Show("Doğrulama Kodunuzu zamanında giremediniz","Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Error);
                DogrulamaKod = (-1234567);
                label7.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                textBox6.Visible = false;
                button12.Visible = false;
                kalanSüre = 120;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
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

        private void button15_Click_1(object sender, EventArgs e)
        {
            if(hesabımDropDownMenu % 2  == 0)
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

        private void button19_Click(object sender, EventArgs e)
        {
            girisYap girisForm = new girisYap();
            girisForm.Show();
            this.Hide();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            kayıtcs kayıtForm = new kayıtcs();
            kayıtForm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sifreDegistir sifreDegistirForm = new sifreDegistir();
            sifreDegistirForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TcGuncelleme TcGuncelleForm = new TcGuncelleme();
            TcGuncelleForm.Show();
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

        private void button6_Click(object sender, EventArgs e)
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
