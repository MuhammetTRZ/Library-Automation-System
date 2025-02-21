using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kayıtOl_giris
{
    public partial class Kitap_islemleri : Form
    {
        public Kitap_islemleri()
        {
            InitializeComponent();
        }
        static string databaseLink = "workstation id=Muhammet.mssql.somee.com;packet size=4096;user id=MuhammetTRZ_SQLLogin_1;pwd=9hbeo7oosu;data source=Muhammet.mssql.somee.com;persist security info=False;initial catalog=Muhammet;TrustServerCertificate=True";
        SqlConnection connect = new SqlConnection(databaseLink);
        int islemDropDownMenu=2;
        int hesabımDropDownMenu = 2;
        int HesapYönetimiDropDownMenu = 2;

        void kitapDüzenlemeButonAçıklamları(string sütunİsmi)
        {
            if (!string.IsNullOrWhiteSpace(textBox11.Text))
            {
                MessageBox.Show($"girilen kimlik yada isbn numarasına denk gelen kitabın {sütunİsmi} {textBox11.Text} olarak düzenler");
            }
            else
            {
                MessageBox.Show($"girilen kimlik yada isbn numarasına denk gelen kitabın {sütunİsmi} isimli sütununu günceller");
            }
        }
        void kitapDetayları(string sqlKodu)
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = $"{sqlKodu}";
                    //datagridview da gösterilecek sutünlar alındı
                    using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                    {
                        komut.Parameters.AddWithValue("@isbn",textBox13.Text);
                        komut.Parameters.AddWithValue("@tc",textBox13.Text);
                        komut.Parameters.AddWithValue("@kimlik", textBox13.Text);
                        SqlDataAdapter da = new SqlDataAdapter(komut);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView4.DataSource = dt;
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
            }
        }
        void kitapDuzenle(string guncellenenSutün)
        {
            if (string.IsNullOrWhiteSpace(textBox10.Text) || string.IsNullOrWhiteSpace(textBox11.Text))
            {
                MessageBox.Show("İSBN numarası ve güncellek halini yazmayı unutmayınız.");
                return;
            }
            try
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string eskiEserAdıİsim = $"select {guncellenenSutün} from kitaplar where isbn=@isbn or kimlik=@kimlik";
                    using (SqlCommand komut = new SqlCommand(eskiEserAdıİsim, connect))
                    {
                        komut.Parameters.AddWithValue("@kimlik",textBox10.Text);
                        komut.Parameters.AddWithValue("@isbn", textBox10.Text);
                        object sutünEskiDegeri = komut.ExecuteScalar();
                        if (sutünEskiDegeri == null || sutünEskiDegeri == DBNull.Value)
                        {
                            MessageBox.Show("bu değere sahip bir İSBN veya KİMLİK numarası bulunamadı");
                            return;
                        }
                        
                        string eserAdıGuncellemeSqlKodu = $"update kitaplar set {guncellenenSutün} =@eserAdı where isbn=@isbn or kimlik=@kimlik";
                        using (SqlCommand komut2 = new SqlCommand(eserAdıGuncellemeSqlKodu, connect))
                        {
                            komut2.Parameters.AddWithValue("@isbn", textBox10.Text);
                            komut2.Parameters.AddWithValue("kimlik", textBox10.Text);
                            komut2.Parameters.AddWithValue("@eserAdı", textBox11.Text);
                            object sonuc2 = komut2.ExecuteNonQuery();
                            if (sonuc2 != null | sonuc2 != DBNull.Value)
                            {
                                MessageBox.Show($"{guncellenenSutün} {sutünEskiDegeri} iken {textBox11.Text} olarak güncellenmmiştir.");
                            }
                            else
                            {
                                MessageBox.Show("hatalı veri girişi");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("bir hata meydana geldi" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        void temizle(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
                else if (c.HasChildren)
                {
                    temizle(c); // İç içe paneller veya grup kutuları varsa onları da temizler
                }
            }
        }
        void databaseGuncelle()//İsim hatalı düzelt iki tane datagridview var!!
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "select * from kitaplar";
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
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
            }
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void Kitap_islemleri_Load(object sender, EventArgs e)
        {
            
            //KİTAP EKLEME 
            Dictionary<TextBox, Panel> kitapEklemeMetinKutularıPanelleri = new Dictionary<TextBox, Panel>
            {
                {textBox1,panel4 },
                {textBox2,panel5 },
                {textBox4,panel6 },
                {textBox5,panel9 },
                {textBox6,panel8 },
                {textBox8,panel11 }
            };
            foreach (var item in kitapEklemeMetinKutularıPanelleri)
            {
                var metinKutusu = item.Key;
                var panel = item.Value;

                metinKutusu.MouseEnter += (s, arg) => panel.BackColor = Color.FromArgb(80, 200, 120);
                metinKutusu.MouseLeave += (s, args) => panel.BackColor = Color.White;
            }

            
            Dictionary<DateTimePicker, Panel> kitapEklemeMetinKutularıDatePanelleri = new Dictionary<DateTimePicker, Panel>
            {
                {dateTimePicker2,panel7 },
                {dateTimePicker1,panel12 },
            };
            foreach (var item in kitapEklemeMetinKutularıDatePanelleri)
            {
                var metinKutusu = item.Key;
                var panel = item.Value;

                metinKutusu.MouseEnter += (s, arg) => panel.BackColor = Color.FromArgb(80, 200, 120);
                metinKutusu.MouseLeave += (s, args) => panel.BackColor = Color.White;
            }

            Button[] KitapEklemeButonları = { button3, button4 };

            foreach (var button in KitapEklemeButonları)
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }

            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "select * from kitaplar";
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
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
            }


            //KİTAP DÜZENLEME

            Button[] KitapDüzenlemeButonları = {button9,button10,button11,button12,button14,button17,button18,button19,button21};
           
            foreach (var button in KitapDüzenlemeButonları)
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }

            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "select * from kitaplar";
                    //datagridview da gösterilecek sutünlar alındı
                    using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sqlSelectKodu, connect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView3.DataSource = dt;
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
            }

            


            //KİTAP SİLME 
            Button[] KitapSilmeButonları = { button5, button6, button7 };
            foreach (var button in KitapSilmeButonları)
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }
            Dictionary<TextBox, Panel> kitapSilmeMetinKutularıPanelleri = new Dictionary<TextBox, Panel>
            {
                {textBox3,panel1 },
                {textBox9,panel2 },
            };
            foreach (var item in kitapSilmeMetinKutularıPanelleri)
            {
                var metinKutusu = item.Key;
                var panel = item.Value;

                metinKutusu.MouseEnter += (s, arg) => panel.BackColor = Color.FromArgb(80, 200, 120);
                metinKutusu.MouseLeave += (s, args) => panel.BackColor = Color.White;
            }

            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "select * from kitaplar";
                    //datagridview da gösterilecek sutünlar alındı
                    using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sqlSelectKodu, connect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView2.DataSource = dt;
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
            }


            //KİTAP DETAYLARI

            Button[] kitapDetayButonları = { button13,button16,button22,button23 ,button24,button25,button26,button27,button29,button30,button31,button32,button33};

            foreach (var button in kitapDetayButonları)
            {
                button.MouseEnter += (s, args) => button.ForeColor = Color.FromArgb(80, 200, 120);
                button.MouseLeave += (s, args) => button.ForeColor = Color.White;
            }
            textBox6.MaxLength = 13;

            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "select * from kitaplar";
                    //datagridview da gösterilecek sutünlar alındı
                    using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sqlSelectKodu, connect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView4.DataSource = dt;
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {//1 2 4 5 6  8 
                TextBox[] metinKutuları = { textBox1, textBox2, textBox4, textBox5, textBox6, textBox8 };
                if(metinKutuları.Any(metinler => string.IsNullOrWhiteSpace(metinler.Text)))
                {
                    MessageBox.Show("tüm bilgileri eksiksiz giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }//bir tane bile metin kutusun boş olup olmadığını foreach döngüsü gibi içlerinde gezerek kontrol eder
                if (dateTimePicker1.Value>DateTime.Now.Date && dateTimePicker2.Value > DateTime.Now.Date)
                {
                    MessageBox.Show("hatalı tarih girişi");
                    return;
                }
                if(pictureBox7.Visible)
                {
                    MessageBox.Show("hatalı girişleri düzeltmeniz gerekiyor", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string kitapEkleme = "insert into kitaplar (eser_adı,yazar,yayın_tarihi,yayınlayan,dil,isbn,yayın_gelis_tarihi,adet) values (@t1,@t2,@t3,@t4,@t5,@t6,@t7,@t8)";
                    using (SqlCommand komut = new SqlCommand(kitapEkleme, connect))
                    {
                        komut.Parameters.AddWithValue("@t1", textBox1.Text);
                        komut.Parameters.AddWithValue("@t2", textBox2.Text);
                        komut.Parameters.AddWithValue("@t3", dateTimePicker2.Value.Date);
                        komut.Parameters.AddWithValue("@t4", textBox4.Text);
                        komut.Parameters.AddWithValue("@t5", textBox5.Text);
                        komut.Parameters.AddWithValue("@t6", textBox6.Text);
                        komut.Parameters.AddWithValue("@t7", dateTimePicker1.Value.Date);
                        komut.Parameters.AddWithValue("@t8", textBox8.Text);
                        komut.ExecuteNonQuery();//nonquery delete,update,insert gibi işlemlerde kullanılır
                        databaseGuncelle();
                        MessageBox.Show("Kitap başarıyla eklenmiştir");
                        textBox1.Clear();
                        temizle(this);
                        dateTimePicker1.Value = DateTime.Now;
                        dateTimePicker2.Value = DateTime.Now;
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show("bir hata meydana geldi"+ex.Message);
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("lütfen kitabın isbn numarasını giriniz ");
            }
            else
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string kitapAraSqlKomutu = "select * from kitaplar where isbn =@isbn";
                    using (SqlCommand komut = new SqlCommand(kitapAraSqlKomutu, connect))
                    {
                        komut.Parameters.AddWithValue("@isbn", Convert.ToInt64(textBox7.Text));
                        using (SqlDataAdapter da = new SqlDataAdapter(komut))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = dt;
                            }
                            else
                            {
                                MessageBox.Show("Bu İSBN numarasına sahip bir kitap yok\nSilinmiş olabilir .", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;

                            }
                        }
                    }
                }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata meydana geldi: " + ex.Message);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            try
            {

                if (string.IsNullOrWhiteSpace(textBox9.Text))
                {
                    MessageBox.Show("lütfen kitabın isbn numarasını giriniz ");
                }
                else
                {
                    using (SqlConnection connect = new SqlConnection(databaseLink))
                    {
                        connect.Open();
                        string kitapAraSqlKomutu = "select * from kitaplar where isbn =@isbn or kimlik=@kimlik";
                        using (SqlCommand komut = new SqlCommand(kitapAraSqlKomutu, connect))
                        {
                            komut.Parameters.AddWithValue("@isbn", Convert.ToInt64(textBox9.Text));
                            komut.Parameters.AddWithValue("@kimlik",Convert.ToInt64(textBox9.Text));
                            using (SqlDataAdapter da = new SqlDataAdapter(komut))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    dataGridView2.DataSource = dt;
                                }
                                else
                                {
                                    MessageBox.Show("Bu İSBN numarasına sahip bir kitap yok\nSilinmiş olabilir .", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata meydana geldi: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("lütfen bir İSBN numarası giriniz");
                return;
            }
            DialogResult result = MessageBox.Show("Kitap silinecektir, onaylıyor musunuz?","Uyarı",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

            if (result == DialogResult.Yes) // Kullanıcı "Yes" seçerse silme işlemi gerçekleşir
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string kitapSilenSqlKodu = "DELETE FROM kitaplar WHERE isbn = @isbn or kimlik=@kimlik";

                    using (SqlCommand komut = new SqlCommand(kitapSilenSqlKodu, connect))
                    {
                        komut.Parameters.AddWithValue("@kimlik", textBox3.Text);
                        komut.Parameters.AddWithValue("@isbn", textBox3.Text);
                        
                        int affectedRows = komut.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Kitap başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Belirtilen ISBN numarasıyla kitap bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Silme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)//girilen değer sayı değilse ve silme tuşu değilse yazımı engeller
            {
                e.Handled = true;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"(^\d{13})$";
            if (long.TryParse(textBox6.Text, out long isbn))
            {
                if (Regex.IsMatch(textBox6.Text, pattern))
                {
                    pictureBox5.Visible = true;
                    pictureBox7.Visible = false;
                }
                else
                {
                    pictureBox5.Visible = false;
                    pictureBox7.Visible = true;
                }
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)//girilen değer sayı değilse ve silme tuşu değilse yazımı engeller
            {
                e.Handled = true;
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)//girilen değer sayı değilse ve silme tuşu değilse yazımı engeller
            {
                e.Handled = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    try
                    {
                        connect.Open();
                        string sqlSelectKodu = "select * from kitaplar";
                        //datagridview da gösterilecek sutünlar alındı
                        using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                        {
                            SqlDataAdapter da = new SqlDataAdapter(sqlSelectKodu, connect);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dataGridView2.DataSource = dt;
                        }
                    }
                    catch (Exception hata)
                    {
                        MessageBox.Show("hata meydana geldi " + hata.Message);
                    }
                }
        }

        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //panel 10
            //if(islemDropDownMenu % 2  == 0)
            //{
            //    panel10.Visible = true;
            //    islemDropDownMenu++;
            //}
            //else
            //{
            //    panel10.Visible = false;
            //    islemDropDownMenu++;
            //}
        }

        private void button21_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "select * from kitaplar";
                    //datagridview da gösterilecek sutünlar alındı
                    using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sqlSelectKodu, connect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView3.DataSource = dt;
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
            }
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            kitapDuzenle("eser_adı");
            
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            kitapDuzenle("yazar");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            kitapDuzenle("yayın_tarihi");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            kitapDuzenle("yayınlayan");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            kitapDuzenle("dil");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            kitapDuzenle("isbn");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            kitapDuzenle("yayın_gelis_tarihi");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            kitapDuzenle("adet");
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            kitapDetayları("select kimlik, eser_adı, alınmaSayısı, isbn from kitaplar where alınmaSayısı > 0 order by alınmaSayısı desc");
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            //kilik yada isbn numarası girilen kitabın kaç kez alındığını gösteren kodları buraya yaz.
            //select eser_adı,alınmaSayısı from kitaplar where isbn=1 or kimlik=1 
            if (string.IsNullOrWhiteSpace(textBox13.Text))
            {
                MessageBox.Show("Sorgulamak istediğiniz kitabın İSBN veya KİMLİK numarasını giriniz");
                return;
            }
            kitapDetayları("select eser_adı,alınmaSayısı from kitaplar where isbn =@isbn or kimlik =@kimlik ");
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox13.Text))
            {
                MessageBox.Show("kişinin tc kimlik numarasını giriniz");
            }
            kitapDetayları("select k.eserAdı,t.isim,t.soyAd from try_to_login t join kitapHareketleri k on k.kullanıcıID =t.kimlik where t.tc=@tc");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "select * from kitaplar order by eser_adı asc ";
                    //datagridview da gösterilecek sutünlar alındı
                    using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sqlSelectKodu, connect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView4.DataSource = dt;
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("içinde kitapların bulunduğu tabloyu ve ilgili bilgileri görmenizi sağlar\nveritabanını yenilemek içinde kullanabilirsiniz.");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gireceğiniz isbn veya kimlik numarasına denk gelen kitabın kaç kez alındığını görmenizi sağlar ");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("girilen tc kimlik numarasına sahip kişinin aldığı kitapları gösterir");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("En çok tercih edilen kitapların listesini verir"); 
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "SELECT k.kimlik, k.isbn, k.eser_adı, kh.kullanıcıID, kh.vermesiGerekenTarih FROM kitapHareketleri kh JOIN kitaplar k ON kh.kullanıcıID= k.kimlik WHERE kh.vermesiGerekenTarih< GETDATE() AND kh.geri_verildi_mi='verilmedi'";
                    //datagridview da gösterilecek sutünlar alındı
                    using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sqlSelectKodu, connect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView4.DataSource = dt;
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("geri verme tarihini geciktirenlerin listesini verir");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "select ttl.isim,ttl.soyad,kh.eserAdı from kitapHareketleri kh join try_to_login ttl on ttl.kimlik=kh.kullanıcıID where kh.geri_verildi_mi = 'verilmedi'";
                    //datagridview da gösterilecek sutünlar alındı
                    using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sqlSelectKodu, connect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView4.DataSource = dt;
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SqlConnection(databaseLink))
            {
                try
                {
                    connect.Open();
                    string sqlSelectKodu = "SELECT COUNT(k.eser_adı) AS toplam_kitap, COUNT(CASE WHEN kh.geri_verildi_mi = 'verilmedi' THEN 'verildi' END) AS oduncte_olanlar FROM kitaplar k LEFT JOIN kitapHareketleri kh ON kh.hareketID = k.kimlik;";
                    //datagridview da gösterilecek sutünlar alındı
                    using (SqlCommand komut = new SqlCommand(sqlSelectKodu, connect))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sqlSelectKodu, connect);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView4.DataSource = dt;
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata meydana geldi " + hata.Message);
                }
            }
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ödünç alınmış kitapları ve o kitabı alan kişileri gösterir");
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("kütüphanede ki toplam kitap sayısını ve kaç tanesinin ödünçte olduğunu gösterir");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if(hesabımDropDownMenu % 2 == 0)
            {
                panel14.Visible = true;
                hesabımDropDownMenu++;
            }
            else
            {
                panel14.Visible = false;
                hesabımDropDownMenu++;

            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if(HesapYönetimiDropDownMenu % 2 == 0)
            {
                panel15.Visible = true;
                HesapYönetimiDropDownMenu++;
            }
            else
            {
                panel15.Visible = false;
                HesapYönetimiDropDownMenu++;
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            hesapSilme hesapSilmeForm = new hesapSilme();
            hesapSilmeForm.Show();
            this.Hide();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            girisYap girisForm = new girisYap();
            girisForm.Show();
            this.Close();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            kayıtcs kayıtForm = new kayıtcs();
            kayıtForm.Show();
            this.Close();
        }

        private void button33_Click(object sender, EventArgs e)
        {
            şifreUnuttumcs şifreUnuttum = new şifreUnuttumcs();
            şifreUnuttum.Show();
            this.Close();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            sifreDegistir sifreDegistirForm = new sifreDegistir();
            sifreDegistirForm.Show();
            this.Close();
        }

        private void button31_Click(object sender, EventArgs e)
        {
            telNoGuncelle TNGForm = new telNoGuncelle();
            TNGForm.Show();
            this.Close();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            TcGuncelleme TcGuncelleForm = new TcGuncelleme();
            TcGuncelleForm.Show();
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form1 form1Form = new Form1();
            form1Form.Show();
            this.Close();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tc kimlik numarası girilen kişinin iade tarihine yakınlaşmış kitabı var mı diye bakılır");
        }

        private void button34_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(databaseLink))
                {
                    connect.Open();
                    string geciktirenleriGetirenSqlKodu = "select t.isim ,k.eserAdı,k.alısTarihi from kitapHareketleri k join try_to_login t on t.kimlik=k.kullanıcıID where k.tc=@tc and geri_verildi_mi='verilmedi' and datediff(day,cast(GETDATE() as date),vermesiGerekenTarih)<=3";
                    using (SqlCommand komut = new SqlCommand(geciktirenleriGetirenSqlKodu, connect))
                    {//join ile iki tablodan istenilen veriler alındı 
                        komut.Parameters.AddWithValue("@tc", Convert.ToInt64(textBox13.Text));

                        using (SqlDataAdapter da = new SqlDataAdapter(komut))
                        {//ve bu veriler datagridview'DE gösterildi
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                dataGridView4.DataSource = dt;
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

        private void button34_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Silme");
        }

        private void pictureBox13_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Tüm kitap bilgilerini eksiksiz girdikten sonra veritabanına kitap eklemeyi sağlar");
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Eklediğiz kitap vertabanında görünmüyorsa veritabanını yenilemek için kullanılır");
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            MessageBox.Show("kitap isbn numarası girerek kitap sorgulama işlemi yapmanızı sağlar");
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            kitapDüzenlemeButonAçıklamları("Eser adı");
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            kitapDüzenlemeButonAçıklamları("Yazar adı");
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            kitapDüzenlemeButonAçıklamları("Yayın Tarihi");
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            kitapDüzenlemeButonAçıklamları("Yayınlayan Adı");
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            kitapDüzenlemeButonAçıklamları("Dil adı");
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            kitapDüzenlemeButonAçıklamları("İSBN kodu");
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            kitapDüzenlemeButonAçıklamları("Yayın Giriş Tarihi");
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            kitapDüzenlemeButonAçıklamları("Adet");
        }
    }
}
