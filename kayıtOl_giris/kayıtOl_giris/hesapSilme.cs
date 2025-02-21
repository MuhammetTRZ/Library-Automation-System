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
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;
using System.Reflection.Emit;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace kayıtOl_giris
{
    public partial class hesapSilme : Form
    {
        public hesapSilme()
        {
            InitializeComponent();
        }
        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size=4096;user id=MuhammetTRZ_SQLLogin_1;pwd=9hbeo7oosu;data source=Muhammet.mssql.somee.com;persist security info=False;initial catalog=Muhammet;TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);
        int dogrulamaKodu;
        long tcTekMi;
        string kullanıcıİsmi;
        string kullanıcıMail;
        int kullanıcıKimlik;
        int kalanSüre = 120;
        int hesabımDropDownMenu = 2;
        int HesapYönetimiDropDownMenu = 2;


        void bilgilendirmeMaili()
        {
            string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
            string toEmail = kullanıcıMail; // Alıcı e-posta adresi
            string subject = "📧 Hesap Silme İşlemi Tamamlandı"; // Konu
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
                                    <p class='baslık'>Sayın{kullanıcıİsmi} :( </p>
                                    <p>Talebiniz üzerine Kocaeli Gebze Halk Kütüphanesi tarafından hesabınız başarıyla silinmiştir. Artık hesabınıza giriş yapamaz ve verilerinize erişemezsiniz..</p>
                                     <p class='önemli'>Bilgilendirme: <b>{dogrulamaKodu}</b></p>
                                    <ul>
                                        <li>Hesap silme işlemi geri alınamaz..</li>
                                        <li>Eğer yanlışlıkla bu işlemi gerçekleştirdiğinizi düşünüyorsanız, yeni bir hesap oluşturarak tekrar kayıt olabilirsiniz.</li>
                                        <li>Gizlilik ve güvenlik politikalarımız hakkında daha fazla bilgi almak isterseniz, destek sayfamızı ziyaret edebilirsiniz.</li>
                                    </ul>
                                    <p> Herhangi bir sorunuz olursa, bizimle [destek e-posta adresi] üzerinden iletişime geçebilirsiniz.</p>
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
        void dogrulamaEposta()
        {
            Random rnd = new Random();
            dogrulamaKodu = rnd.Next(10000,99999);
            using(SqlConnection connect =  new SqlConnection(databaseLink))
            {
                connect.Open();
                string isimAlanSqlKodu = "select isim,mail,kimlik from try_to_login where tc=@tc";
                using(SqlCommand komut = new SqlCommand(isimAlanSqlKodu ,connect))
                {
                    komut.Parameters.AddWithValue("@tc",textBox5.Text);

                    using(SqlDataReader reader = komut.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                             kullanıcıİsmi = reader["isim"].ToString();
                             kullanıcıMail = reader["mail"].ToString();
                             //kullanıcıKimlik = reader["kimlik"].ToString();

                            string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
                            string toEmail = kullanıcıMail; // Alıcı e-posta adresi
                            string subject = "📧 Hesap Silme Talebiniz Hakkında Doğrulama İşlemi"; // Konu
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
                                    <p class='baslık'>Merhaba{kullanıcıİsmi}</p>
                                    <p>Kütüphane sistemimizde bulunan hesabınızı silmek istediğinizi belirttiniz.Hesabınızın güvenliği için bu işlemi onaylamadan önce kimliğinizi doğrulamanız gerekmektedir.</p>
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
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    MessageBox.Show("lütfen bir tc giriniz");
                    return;
                }
                if (pictureBox7.Visible == true)
                {
                    MessageBox.Show("hatakı girişleri düzeltmeden devam edemezsiniz");
                    return;
                }
                
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string verilmemisKitapKontrol = "select count(*) from kitapHareketleri  where tc=@tc and geri_verildi_mi='verilmedi'";
                    using (SqlCommand komut = new SqlCommand(verilmemisKitapKontrol, connect))
                    {
                        komut.Parameters.AddWithValue("@tc", textBox5.Text);
                        object sonuc = komut.ExecuteScalar();
                        int etkilenenSatırSayısı = Convert.ToInt32(sonuc);
                        MessageBox.Show($"etkilenen satır sayısı:{etkilenenSatırSayısı}");
                        if (etkilenenSatırSayısı > 0)
                        {
                            MessageBox.Show("geri vermeniz gereken kitap yada kitaplar olduğu için kaydınızı silemezsiniz");
                            return;
                        }
                    }
                }
                textBox6.Visible = true;
                label7.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                panel9.Visible = true;
                button1.Visible = true;
                timer1.Start();
                dogrulamaEposta();
            }catch (Exception ex)
            {
                MessageBox.Show("bir hata meydana geldi"+ex.Message,"Hata",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            
        }

        private void hesapSilme_Load(object sender, EventArgs e)
        {
            //3 4 12 13 16 17 18 
            Button[] butonlar = {  button4, button12, button13, button16,button17,button18 };

            foreach (var button in butonlar)
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }
            textBox5.MaxLength = 11;

            Dictionary<TextBox, Panel> metinKutusuPanelleri = new Dictionary<TextBox, Panel>
            {
                { textBox5, panel7 },
                { textBox6, panel9 },
            };
            foreach (var item in metinKutusuPanelleri)
            {
                var metinKutusu = item.Key;
                var panel = item.Value;

                metinKutusu.MouseEnter += (s, arg) => panel.BackColor = Color.FromArgb(80, 200, 120);
                metinKutusu.MouseLeave += (s, args) => panel.BackColor = Color.White;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox6.Text))
                {
                    MessageBox.Show("boş alan bırakmayınız", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (textBox6.Text != dogrulamaKodu.ToString() )
                {
                    MessageBox.Show("hatalı doğrulama kodu", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string kimlikAlanSqlKodu = "select kimlik from try_to_login ttl join kitapHareketleri kh on ttl.kimlik=kh.kullanıcıID where ttl.tc=@tc";
                    using(SqlCommand komut = new SqlCommand(kimlikAlanSqlKodu, connect))
                    {
                        komut.Parameters.AddWithValue("@tc", textBox5.Text);
                        kullanıcıKimlik = (int)komut.ExecuteScalar();
                    }

                    string kitapHareketGuncelleSql = $"UPDATE kitapHareketleri SET kullanıcıID = NULL WHERE kullanıcıID = {kullanıcıKimlik}";
                    using (SqlCommand komut = new SqlCommand(kitapHareketGuncelleSql, connect))
                    {
                        komut.Parameters.AddWithValue("@KI", Convert.ToInt64(textBox5.Text)); // tc'yi BigInt'e dönüştür
                        komut.ExecuteNonQuery();
                    }

                    string kayıtSilenSqlKodu = "DELETE FROM try_to_login WHERE tc = @tc";
                    using (SqlCommand komut = new SqlCommand(kayıtSilenSqlKodu, connect))
                    {
                        komut.Parameters.AddWithValue("@tc", Convert.ToInt64(textBox5.Text)); // tc'yi BigInt'e dönüştür
                        komut.ExecuteNonQuery();
                    }
                    bilgilendirmeMaili();

                }
            }catch (Exception ex)
            {
                MessageBox.Show("bir hata meydana geldi" + ex.Message,"Hata" ,MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if(char.IsLetter(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
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
            else
            {
                //
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
            this.Close();
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
                textBox6.Visible = false;
                label7.Visible = false;
                panel9.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                textBox6.Visible = false;
                button1.Visible = false;
                
            }
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

        private void button5_Click(object sender, EventArgs e)
        {
            Kitap_islemleri kitapİslemleriForm = new Kitap_islemleri();
            kitapİslemleriForm.Show();
            this.Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            girisYap girisForm = new girisYap();
            girisForm.Show();
            this.Close();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            kayıtcs kayıtForm = new kayıtcs();
            kayıtForm.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Close();
        }

        private void button16_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Lütfen hesap silmeye çalışmayın ve diğer fonksiyonları deneyip veritabanına kayıtlarınızı bırakın"); ;
            MessageBox.Show("Siz hesap silin diye mi yolladım lan ben projeyi");
            MessageBox.Show("Bir şey silme ok?","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }
}
