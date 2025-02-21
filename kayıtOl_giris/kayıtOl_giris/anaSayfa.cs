using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Emit;

namespace kayıtOl_giris
{
    public partial class anaSayfa : Form
    {
        private int kullanıcıKimlik;
        private long kullanıcıTc;
        public anaSayfa(int kimlik,long tc)
        {
            InitializeComponent();
            kullanıcıKimlik = kimlik;
            kullanıcıTc = tc;
        }
        public int KitapAlSayaç = 2;
        public int KitapVerSayaç = 2;
        int hesabımDropDownMenu = 2;
        int HesapYönetimiDropDownMenu = 2;

        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size=4096;user id=MuhammetTRZ_SQLLogin_1;pwd=9hbeo7oosu;data source=Muhammet.mssql.somee.com;persist security info=False;initial catalog=Muhammet;TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);

        void guncelle()
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "select eser_adı,yazar,yayın_tarihi,yayınlayan,dil,adet from kitaplar";

                    using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sqlSelectKodu, connect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
            }
        }
        void geriKitapVermeMailiYolla(string kitapName)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string kitapGeriVermeMailAlmaSqlKodu = "select mail,isim from try_to_login where kimlik=@kimlik";
                    using (SqlCommand komut = new SqlCommand(kitapGeriVermeMailAlmaSqlKodu, connect))
                    {
                        komut.Parameters.AddWithValue("@kimlik", kullanıcıKimlik);
                        using (SqlDataReader reader = komut.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime günümüzTarihi = DateTime.Now.Date;
                                string modifiyeGünümüzTarih=günümüzTarihi.ToShortDateString();
                                string mailHesap = reader["mail"].ToString();
                                string kullanıcıİsmi = reader["isim"].ToString();

                                string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
                                string toEmail = mailHesap; // Alıcı e-posta adresi
                                string subject = "[Gebze Belediyesi Kütüphanesi] Kitap Teslim İşlemi Tamamland"; // Konu
                                string body = $@"<html>
                                                  <head>
                                                      <style>
                                                          .kutu{{
                                                              font-family: Arial, Helvetica, sans-serif;
                                                              max-width: 600px;
                                                              margin: auto;
                                                              border: 1px solid #ddd;
                                                              border-radius: 20px;
                                                              background-color: #f9f9f9;
                                                          }}
                                                          .bilgiler{{
                                                              font-size: 20px;
                                                              font-weight: bold;
                                                              color: #0073e6;
                                                          }}
                                                      </style>
                                                  </head>
                                                  <body>
                                                      <div class=""kutu"">
                                                          <p class=""giris"">Merhaba <b>{kullanıcıİsmi}</b></p>
                                                          <p class=""bilgiler"">📚 <b>{kitapName}</b> adlı kitabı başarıyla kütüphanemize iade ettiniz.</p>
                                                          <p class=""bilgiler"">✅ İade Tarihi: <b>{modifiyeGünümüzTarih}</b></p>
                                                          <p>Kütüphanemizi tercih ettiğiniz için teşekkür ederiz. Yeni kitaplar keşfetmek için her zaman bekleriz!</p>
                                                          <p>Eğer başka bir konuda yardıma ihtiyacınız olursa bizimle iletişime geçebilirsiniz.</p>
                                                          <p><b><i>📖 Keyifli okumalar dileriz!</i></b></p>
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("ödünç kitap alma fonksiyonunda hata oluştu " + ex.ToString(), "hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void oduncKitapMailiYolla(string kitapİsmi,string geriVerilecekTarih)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string ödünçMailAlmaSqlKodu = "select mail,isim from try_to_login where kimlik=@kimlik";
                    using (SqlCommand komut = new SqlCommand(ödünçMailAlmaSqlKodu, connect))
                    {
                        komut.Parameters.AddWithValue("@kimlik", kullanıcıKimlik);
                        using (SqlDataReader reader = komut.ExecuteReader())
                        {//reader sayesinde birden çok sutün okunur burada da tek sorguda birden fazla sutün almak daha mantıklı
                            
                            if (reader.Read())//okuyabildiyse
                            {

                                string mailHesap = reader["mail"].ToString();//sutün isimlerinden dönen verileri ilgili değişkenlere atama işlemi
                                string kullanıcıİsmi = reader["isim"].ToString();
                                //MessageBox.Show($"alıcı: {mailHesap}\nkullanıcı ismi: {kullanıcıİsmi}");

                                string fromEmail = "muhammetaliterzi04@gmail.com"; // Gönderenin e-posta adresi
                                string toEmail = mailHesap; // Alıcı e-posta adresi
                                string subject = "Gebze Belediyesi Kütüphanesi Ödünç Aldığınız Kitap Hakkında Bilgilendirme"; // Konu
                                string body = $@"<html>
                                                    <head>
                                                        <style>
                                                            .kutu{{
                                                                max-width: 600px;
                                                                margin: auto;
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
                                                            .bilgiler{{
                                                                font-size: 20px;
                                                                font-weight: bold;
                                                                color: red;
                                                            }}
                                                            .listeBaslık{{
                                                                color: orange;
                                                                margin-left:10px ;
                                                            }}
                                                            ul li{{
                                                                padding: 5px;
                                                            }}
                                                        </style>
                                                    </head>
                                                    <body>
                                                        <div class=""kutu"">
                                                            <p class=""baslık"">Merhaba {kullanıcıİsmi}</p>
                                                            <p>Gebze Belediyesi Kütüphanesi’nden aşağıda belirtilen kitabı ödünç aldınız:</p>
                                                            <p class=""bilgiler"">📖 Kitap Adı: <b>{kitapİsmi}</b></p>
                                                            <p class=""bilgiler"">📅 Geri Getirme Tarihi: <b>{geriVerilecekTarih}</b></p>
                                                            <p class=""listeBaslık"">Hatırlatma:</p>
                                                            <ul>
                                                                <li>✔ Kitabı zamanında iade ederek diğer okuyucuların da faydalanmasını sağlayabilirsiniz.</li>
                                                                <li>✔ Kitabın zarar görmemesi için dikkatli kullanınız.</li>
                                                            </ul>
                                                            <p>İyi okumalar dileriz!</p>
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
            }catch(Exception ex)
            {
                MessageBox.Show("ödünç kitap alma fonksiyonunda hata oluştu " + ex.ToString(), "hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        
        bool kitapGeriVermeKontrol()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();

                    string sqlKitapGeriVermeDogrulamaKodu = "select eserAdı from kitapHareketleri where kullanıcıID=@kimlik";
                    using (SqlCommand komut = new SqlCommand(sqlKitapGeriVermeDogrulamaKodu, connect))//burada tek bir değer almamıza rağmen reader kullanmak biraz hatalı
                    {                                                                   //scalar kullanmak da olurdu
                        komut.Parameters.AddWithValue("@kimlik", kullanıcıKimlik);

                        using (SqlDataReader reader = komut.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string alınmısKitap = reader["eserAdı"].ToString();//" "sutün isminden dönen veri değişkene atandı
                                if (alınmısKitap == textBox2.Text)
                                {
                                    return true;
                                }
                            }

                        }

                    }
                    MessageBox.Show("Daha önce bu kitabı almadığınız için geri verme işlemi yapamazsınız!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;

                }
            }catch (Exception ex)
            {
                MessageBox.Show("hata", ex.Message);
                return false;
            }
        }
        private void anaSayfa_Load(object sender, EventArgs e)
        {
            Dictionary<TextBox, Panel> metinKutusuPanelleri = new Dictionary<TextBox, Panel>()
                {
                    {textBox1,panel4 },
                    {textBox2,panel5},
                };
            foreach (var item in metinKutusuPanelleri)
            {
                var metinKutusu = item.Key;
                var panel = item.Value;

                metinKutusu.MouseEnter += (s, arg) => panel.BackColor = Color.FromArgb(80, 200, 120);
                metinKutusu.MouseLeave += (s, args) => panel.BackColor = Color.White;
            }
            Button[] butonlar = { button8, button9, button13, button10, button16, button17, button18 };
            foreach (var button in butonlar)
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }


            using (SqlConnection connect =new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "select eser_adı,yazar,yayın_tarihi,yayınlayan,dil,adet from kitaplar";
                        //datagridview da gösterilecek sutünlar alındı
                    using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sqlSelectKodu, connect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata meydana geldi "+hata.Message);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (KitapAlSayaç % 2==0)
            {//Basit bir matematik işlemi sayesinde kurtulun ikinci basışta kapanmasını sağlıyor 
                dateTimePicker1.Visible = true;
                textBox1.Visible = true;
                button5.Visible = true;
                label15.Visible = true;
                KitapAlSayaç++;
            }
            else
            {
                dateTimePicker1.Visible = false;
                textBox1.Visible = false;
                button5.Visible = false;
                label15.Visible = false;
                KitapAlSayaç++;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))//database'e bağlan
            {
                try
                {
                    connect.Open();

                    string sqlStokKontrol = "select adet from kitaplar where eser_adı=@eserAdi";
                    using (SqlCommand stokKomut=new SqlCommand(sqlStokKontrol,connect))
                    {
                        stokKomut.Parameters.AddWithValue("@eserAdi", textBox1.Text);
                        object adetObj=stokKomut.ExecuteScalar();//adet sayısı burada sorgu objesi olduğu için alınıyor
                            //execute scalar ilk satır ilk sutün alır
                            
                        

                        if (string.IsNullOrWhiteSpace(textBox1.Text))
                        {//metin kutusu boş bırakılırsa
                            MessageBox.Show("bir kitap ismi giriniz","Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (adetObj == null)
                        {//değer dönmzezse(boş sutün) öyle bir kitap yoktur
                            MessageBox.Show("Bu isimde bir kitap bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        int mevcutAdet=Convert.ToInt32(adetObj);
                        if(mevcutAdet == 0)
                        {//bir satır döndüyse int'e çevir ve 0 ise stokta kalmadığını anla
                            MessageBox.Show("Bu kitap stokta kalmadı!", "Stok Tükenmiş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        
                        
                    }
                    

                        string sqlKitapAlmaKodu = "update kitaplar set adet =adet - 1 where eser_adı= @eserAdi and adet > 0";
                    using (SqlCommand komut = new SqlCommand(sqlKitapAlmaKodu, connect))//stokta var ise kitap alındığında stok sayısından 1 çıkar
                    {
                        DateTime sınırTarih = DateTime.Now.AddMonths(3);
                        if (dateTimePicker1.Value.Date == DateTime.Now.Date || dateTimePicker1.Value.Date < DateTime.Now.Date ||dateTimePicker1.Value.Date>sınırTarih.Date)
                        {
                            MessageBox.Show("Hata meydana geldi\nGünümüz tarihi girilemez\n.Geçmiş bir tarih girilemez\nKitap en fazla 3 ay kiralanabilir","Hatalı Tarih Girimi",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return;
                        }
                        //değer atamaları
                        string kitapİsmi = textBox1.Text;
                        DateTime secilenTarih=dateTimePicker1.Value.Date;
                        string geriGetirmeTarihi=secilenTarih.ToShortDateString();
                        komut.Parameters.AddWithValue("@eserAdi", textBox1.Text);
                        komut.ExecuteNonQuery();
                        guncelle(); 
                        MessageBox.Show("Kitap başarılı şekilde alındı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        oduncKitapMailiYolla(kitapİsmi, geriGetirmeTarihi);//ilgili parametreler atanıyor

                        //MessageBox.Show("kullanıcıya bilgilendirme maili yollandı","Başarılı",MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //insert into kitapHareketleri(kullanıcıID,eserAdı) values('kimlik','textbox')
                        string sqlDeneme = "insert into kitapHareketleri(kullanıcıID,eserAdı,vermesiGerekenTarih,tc) values(@kimlik,@eserAdı,@vermesiGerekenTarih,@tc)";
                        using(SqlCommand komut2 =new SqlCommand(sqlDeneme, connect))    
                        {
                            komut2.Parameters.AddWithValue("@kimlik", kullanıcıKimlik);
                            komut2.Parameters.AddWithValue("@eserAdı", textBox1.Text);
                            komut2.Parameters.AddWithValue("@vermesiGerekenTarih",dateTimePicker1.Value);
                            komut2.Parameters.AddWithValue("@tc", kullanıcıTc);
                          
                            komut2.ExecuteNonQuery();

                            string alınmaSayısıGuncelleyenSqlKodu = "update kitaplar set alınmaSayısı += 1 where eser_adı = @eser_adı";
                            using(SqlCommand komut3 = new SqlCommand(alınmaSayısıGuncelleyenSqlKodu, connect))
                            {
                                komut3.Parameters.AddWithValue("@eser_adı", textBox1.Text);
                                komut3.ExecuteNonQuery();
                            }
                            guncelle();
                            //MessageBox.Show("veritabanına bilgiler kayıt edildi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                        }

                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata "+ hata.Message);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();

                    string sqlStokKontrol = "select adet from kitaplar where eser_adı=@eserAdi";//databaseden adet alındı
                    using (SqlCommand stokKomut = new SqlCommand(sqlStokKontrol, connect))
                    {
                        stokKomut.Parameters.AddWithValue("@eserAdi", textBox2.Text);
                        object adetObj = stokKomut.ExecuteScalar();

                        if (string.IsNullOrWhiteSpace(textBox2.Text))//kutu boş mu bırakıldı
                        {
                            MessageBox.Show("bir kitap ismi giriniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (adetObj == null)
                        {
                            MessageBox.Show("Bu isimde bir kitap bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }


                        if (!kitapGeriVermeKontrol())
                        {
                            return;
                        }
                    }

                    //kitabı geri verme işlemleri
                    string sqlKitapVermeKodu = "update kitapHareketleri set geri_verildi_mi = 'verildi' where kullanıcıID=@kimlik and eserAdı=@eserAdı";

                    using (SqlCommand komut = new SqlCommand(sqlKitapVermeKodu, connect))
                    {
                        komut.Parameters.AddWithValue("@kimlik", kullanıcıKimlik);
                        komut.Parameters.AddWithValue("@eserAdı", textBox2.Text);
                        komut.ExecuteNonQuery();
                        string kitapİsmi = textBox2.Text;
                        guncelle();//tablo tekrar çağırılarak güncelleme işlemi yapılıyor
                        geriKitapVermeMailiYolla(kitapİsmi);

                        string sqlKitapVermeKodu2 = "update kitaplar set adet =adet + 1 where eser_adı= @eserAdi";

                        using (SqlCommand komut2 = new SqlCommand(sqlKitapVermeKodu2, connect))
                        {
                            komut2.Parameters.AddWithValue("@eserAdi", textBox2.Text);
                            komut2.ExecuteNonQuery();
                            guncelle();
                        }


                    }
                    MessageBox.Show("Kitap başarılı şekilde teslim edildi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata "+ hata.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (KitapVerSayaç % 2 == 0)
            {//Basit bir matematik işlemi sayesinde kurtulun ikinci basışta kapanmasını sağlıyor 
                textBox2.Visible = true;
                button6.Visible = true;
                label2.Visible = true;
                KitapVerSayaç++;
            }
            else
            {
                textBox2.Visible = false;
                button6.Visible = false;
                label2.Visible = false;
                KitapVerSayaç++;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();//programı kapatır
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            kullanıcıSorguİslemleri KSİForm = new kullanıcıSorguİslemleri();
            KSİForm.Show();
            this.Hide();
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (KitapAlSayaç % 2 == 0)
            {//Basit bir matematik işlemi sayesinde kurtulun ikinci basışta kapanmasını sağlıyor 
                dateTimePicker1.Visible = true;
                textBox1.Visible = true;
                button5.Visible = true;
                label15.Visible = true;
                panel4.Visible = true;
                KitapAlSayaç++;
            }
            else
            {
                dateTimePicker1.Visible = false;
                textBox1.Visible = false;
                button5.Visible = false;
                label15.Visible = false;
                panel4.Visible = false;
                KitapAlSayaç++;
            }
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
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (KitapVerSayaç % 2 == 0)
            {//Basit bir matematik işlemi sayesinde kurtulun ikinci basışta kapanmasını sağlıyor 
                textBox2.Visible = true;
                button6.Visible = true;
                label2.Visible = true;
                panel5.Visible = true;
                KitapVerSayaç++;
            }
            else
            {
                textBox2.Visible = false;
                button6.Visible = false;
                label2.Visible = false;
                panel5.Visible = false;
                KitapVerSayaç++;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (hesabımDropDownMenu % 2 == 0)
            {
                panel6.Visible = true;
                hesabımDropDownMenu++;
            }
            else
            {
                panel6.Visible = false;
                hesabımDropDownMenu++;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if(HesapYönetimiDropDownMenu % 2 == 0)
            {
                panel7.Visible = true;
                HesapYönetimiDropDownMenu++;
            }
            else
            {
                panel7.Visible = false;
                HesapYönetimiDropDownMenu++;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Kitap_islemleri kitapİslemleriForm = new Kitap_islemleri();
            kitapİslemleriForm.Show();
            this.Close();
        }
    }
}
