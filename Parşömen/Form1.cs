using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Parşömen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
        }


        private int SekmeSayısı = 0;

        private List<string> tumSemboller = new List<string> { "●", "◆", "★", "✓", "⇒" }; 

        private string varsayilanSembol= "●"; 
        string dosyaYolu = string.Empty;
       



        private void NewTab()
        {
           
            RichTextBox Belge = new RichTextBox();
            Belge.Name = "Belge";
            Belge.Dock = DockStyle.Fill;
            Belge.ContextMenuStrip = contextMenuStrip1;

          
            TabPage YeniSekme = new TabPage();
            SekmeSayısı += 1;

         
            string dosyaAdi = "Belge" + SekmeSayısı;
            string uzanti = ".rtf";
            string sekmeBasligi = dosyaAdi + uzanti;

            YeniSekme.Name = sekmeBasligi;
            YeniSekme.Text = sekmeBasligi;

        
            YeniSekme.Controls.Add(Belge);

           
            tabControl1.TabPages.Add(YeniSekme);
           
        }



        private void deleteTab() {


            DialogResult sonuc = MessageBox.Show(
        "Bu sekmeyi kapatmak istediğinize emin misiniz?",
        "Sekmeyi Kapat",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    );

            if (sonuc == DialogResult.Yes)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);

                if (tabControl1.TabPages.Count == 0)
                {
                NewTab();
             }


              
            }
        }
        
        private void deleteAllTab() {
            DialogResult sonuc = MessageBox.Show(
    "Tüm sekmeleri kapatmak istediğinize emin misiniz?",
    "Onay",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question
);

            if (sonuc == DialogResult.Yes)
            {
                tabControl1.TabPages.Clear();
                SekmeSayısı = 0;
                NewTab();
            }







        
            
        }


        private void save()
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
            {
                RichTextBox aktifBelge = tabControl1.SelectedTab.Controls[0] as RichTextBox;

                if (aktifBelge != null)
                {
                    string mevcutDosyaYolu = tabControl1.SelectedTab.Tag as string;

                    if (!string.IsNullOrEmpty(mevcutDosyaYolu))
                    {
                        try
                        {
                            string uzanti = Path.GetExtension(mevcutDosyaYolu).ToLower();

                            if (uzanti == ".rtf")
                                aktifBelge.SaveFile(mevcutDosyaYolu, RichTextBoxStreamType.RichText);
                            else
                                File.WriteAllText(mevcutDosyaYolu, aktifBelge.Text, Encoding.UTF8);

                            MessageBox.Show("Dosya başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                          
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Kaydederken hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
               
                        saveAs();
                    }
                }
                else
                {
                    MessageBox.Show("Aktif belge bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Kayıt edilecek belge yok.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void saveAs()
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
            {
                RichTextBox aktifBelge = tabControl1.SelectedTab.Controls[0] as RichTextBox;

                if (aktifBelge != null)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                    string varsayilanDosyaAdi = tabControl1.SelectedTab.Text;
                    string uzanti = Path.GetExtension(varsayilanDosyaAdi).ToLower();
                    if (string.IsNullOrEmpty(uzanti)) uzanti = ".txt";

                    saveFileDialog1.FileName = varsayilanDosyaAdi;
                    saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    saveFileDialog1.Filter =
      "RTF Dosyası (*.rtf)|*.rtf|" +
      "HTML Dosyası (*.html)|*.html|" +
      "HTM Dosyası (*.htm)|*.htm|" +
      "CSS Dosyası (*.css)|*.css|" +
      "JavaScript Dosyası (*.js)|*.js|" +
      "JSON Dosyası (*.json)|*.json|" +
      "XML Dosyası (*.xml)|*.xml|" +
      "CSV Dosyası (*.csv)|*.csv|" +
      "C# Kaynak Dosyası (*.cs)|*.cs|" +
      "C++ Kaynak Dosyası (*.cpp)|*.cpp|" +
      "Başlık Dosyası (*.h)|*.h|" +
      "Java Dosyası (*.java)|*.java|" +
      "Python Dosyası (*.py)|*.py|" +
      "SQL Dosyası (*.sql)|*.sql|" +
      "Log Dosyası (*.log)|*.log|" +
      "INI Dosyası (*.ini)|*.ini|" +
      "Yapılandırma Dosyası (*.conf)|*.conf|" +
      "Metin Dosyası (*.txt)|*.txt|" +
      "Tüm Dosyalar|*.*";
                    saveFileDialog1.Title = "Farklı Kaydet";

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string secilenUzanti = Path.GetExtension(saveFileDialog1.FileName).ToLower();

                            if (secilenUzanti == ".rtf")
                                aktifBelge.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                            else
                            {

                                DialogResult sonuc = MessageBox.Show(
                                           "Seçtiğiniz dosya türü metin biçimlendirmelerini (kalın, renkli yazı vb.) tam olarak desteklemeyebilir.\nYine de kaydetmek istiyor musunuz?",
                                           "Biçimlendirme Uyarısı",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Warning);

                                if (sonuc == DialogResult.No)
                                    return; 




                                File.WriteAllText(saveFileDialog1.FileName, aktifBelge.Text, Encoding.UTF8);
                            }
                            tabControl1.SelectedTab.Tag = saveFileDialog1.FileName; 

                            MessageBox.Show("Dosya başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tabControl1.SelectedTab.Tag = saveFileDialog1.FileName;
                            tabControl1.SelectedTab.Text = Path.GetFileName(saveFileDialog1.FileName);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Kaydetme sırasında hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Aktif belge bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Kayıt edilecek belge yok.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void open() 
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog1.Filter = "Desteklenen Dosyalar|*.txt;*.html;*.htm;*.css;*.js;*.json;*.xml;*.csv;*.cs;*.cpp;*.h;*.java;*.py;*.sql;*.log;*.ini;*.conf;*.rtf|Tüm Dosyalar|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
             dosyaYolu = openFileDialog1.FileName;


                foreach (TabPage sekme in tabControl1.TabPages)
                {
                    if (sekme.Tag != null && sekme.Tag.ToString() == dosyaYolu)
                    {
                        MessageBox.Show("Bu dosya zaten açık.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; 
                    }
                }




                string dosyaAdiVeUzanti = Path.GetFileName(dosyaYolu); 

                try
                {
                    
                    RichTextBox belge = new RichTextBox();
                    belge.Dock = DockStyle.Fill;
                    belge.ContextMenuStrip = contextMenuStrip1;

                    TabPage yeniSekme = new TabPage();
                    yeniSekme.Text = dosyaAdiVeUzanti;
                    yeniSekme.Name = dosyaAdiVeUzanti;
                    yeniSekme.Controls.Add(belge);

                    tabControl1.TabPages.Add(yeniSekme);
                    tabControl1.SelectedTab = yeniSekme;
                    yeniSekme.Tag = dosyaYolu;

                    string[] desteklenenUzantilar = new string[]
                    {
                ".txt", ".html", ".htm", ".css", ".js", ".json", ".xml", ".csv",
                ".cs", ".cpp", ".h", ".java", ".py", ".sql", ".log", ".ini",".rtf", ".conf"
                    };

                    string uzanti = Path.GetExtension(dosyaYolu).ToLower();
                    
                    if (Array.Exists(desteklenenUzantilar, u => u == uzanti))
                    {


                        if (uzanti == ".rtf")
                        {
                            
                            belge.LoadFile(dosyaYolu, RichTextBoxStreamType.RichText);

                        
                        }
                        else
                        {
                           
                            string icerik = File.ReadAllText(dosyaYolu, Encoding.UTF8);
                            belge.Text = icerik;
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bu dosya türü desteklenmiyor veya düz metin değil.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya açılırken hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }




        private RichTextBox ŞimdikiBelgeAl
        {
            get { return (RichTextBox)tabControl1.SelectedTab.Controls["Belge"]; }
        }

      


        private void redo()
        {
            try
            {
                if (tabControl1.SelectedTab != null)
                {
                    RichTextBox aktifRTB = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                    if (aktifRTB != null)
                    {
                        aktifRTB.Redo();
                    }
                    else
                    {
                        MessageBox.Show("Yinele işlemi bu dosya türünde desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Aktif bir sekme bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Bu dosya formatında bu işlem desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void undo()
        {
            try
            {
                if (tabControl1.SelectedTab != null)
                {
                    RichTextBox aktifRTB = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                    if (aktifRTB != null)
                    {
                        aktifRTB.Undo();
                    }
                    else
                    {
                        MessageBox.Show("Geri alma işlemi bu dosya türünde desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Aktif bir sekme bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Bu dosya formatında bu işlem desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void cut()
        {
            try
            {
                if (tabControl1.SelectedTab != null)
                {
                    RichTextBox aktifRTB = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                    if (aktifRTB != null && aktifRTB.SelectedText.Length > 0)
                    {
                        aktifRTB.Cut();
                    }
                    else
                    {
                        MessageBox.Show("Kesilecek metin bulunamadı veya bu işlem desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Aktif bir sekme bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Bu dosya formatında bu işlem desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }    
    
        private void paste()
        {
            try
            {
                if (tabControl1.SelectedTab != null)
                {
                    RichTextBox aktifRTB = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                    if (aktifRTB != null)
                    {
                        aktifRTB.Paste();
                    }
                    else
                    {
                        MessageBox.Show("Yapıştırma işlemi bu dosya türünde desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Aktif bir sekme bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Bu dosya formatında bu işlem desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void copy()
        {
            try
            {
                if (tabControl1.SelectedTab != null)
                {
                    RichTextBox aktifRTB = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                    if (aktifRTB != null && aktifRTB.SelectedText.Length > 0)
                    {
                        aktifRTB.Copy();
                    }
                    else
                    {
                        MessageBox.Show("Kopyalanacak metin bulunamadı veya bu işlem desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Aktif bir sekme bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Bu dosya formatında bu işlem desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void selectAll()
        {
            try
            {
                if (tabControl1.SelectedTab != null)
                {
                    RichTextBox aktifRTB = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();

                    if (aktifRTB != null)
                    {
                        aktifRTB.SelectAll();
                    }
                    else
                    {
                        MessageBox.Show("Seçme işlemi bu dosya türünde desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Aktif bir sekme bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Bu dosya formatında bu işlem desteklenmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }



        private void getFontCollection()
        {
            InstalledFontCollection LocalFonts = new InstalledFontCollection();
            foreach (FontFamily items in LocalFonts.Families)
            {
                fontTypeComboBox.Items.Add(items.Name);
            }
            fontTypeComboBox.SelectedIndex = 0;
        }

        private void FontSize()
        {
            for (int i = 1; i < 75; i++)
            {
                fontSizeCombobox.Items.Add(i);
            }
            fontSizeCombobox.SelectedIndex = 12;

        }




        private void Form1_Load(object sender, EventArgs e)
        {
            NewTab();
            getFontCollection();
            FontSize();
            varsayilanSembol = "●";
            this.KeyPreview = true;









          
            foreach (ToolStripItem item in ListBtn.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem && menuItem.Tag?.ToString() == varsayilanSembol)
                {
                    menuItem.Checked = true;
                    break;
                }
            }
        }

      

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ŞimdikiBelgeAl != null && ŞimdikiBelgeAl.Text.Length > 0)
            {
                toolStripStatusLabel1.Text = "Toplam Karakter Sayısı = " + ŞimdikiBelgeAl.Text.Length.ToString();
            }
            else
            {
                toolStripStatusLabel1.Text = "Toplam Karakter Sayısı = 0";
            }
        }

      

        private string HtmlToPlainText(string html)
        {
            string sade = Regex.Replace(html, "<.*?>", string.Empty); 
            sade = WebUtility.HtmlDecode(sade); 
            return sade;
        }

        private void newBtn_Click(object sender, EventArgs e)
        {
            NewTab();
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            open();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            save();
        }

        private void menuNewBtn_Click(object sender, EventArgs e)
        {
            NewTab();
        }

        private void menuOpenBtn_Click(object sender, EventArgs e)
        {
            open();
        }

        private void menuSaveBtn_Click(object sender, EventArgs e)
        {
            save();
        }

        private void menuSaveAsBtn_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        private void menuQuitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuCutBtn_Click(object sender, EventArgs e)
        {
            cut();
        }

        private void menuCopyBtn_Click(object sender, EventArgs e)
        {
            copy();
        }

        private void menuPasteBtn_Click(object sender, EventArgs e)
        {
            paste();
        }

        private void menuSelectAllBtn_Click(object sender, EventArgs e)
        {
            selectAll();
        }

        private void menuUndoBtn_Click(object sender, EventArgs e)
        {
            undo();
        }

        private void menuRendoBtn_Click(object sender, EventArgs e)
        {
            redo();
        }

        private void saveAsBtn_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            deleteTab();
        }

        private void deleteAllBtn_Click(object sender, EventArgs e)
        {
            deleteAllTab(); 
        }

        private void cutBtn_Click(object sender, EventArgs e)
        {
            cut();
        }

        private void copyBtn_Click(object sender, EventArgs e)
        {
            copy();
        }

        private void pasteBtn_Click(object sender, EventArgs e)
        {
            paste();
        }

        private void selectAllBtn_Click(object sender, EventArgs e)
        {
            selectAll();
        }

        private void boldBtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                Font Bold = new Font(ŞimdikiBelgeAl.SelectionFont.FontFamily,
                           ŞimdikiBelgeAl.SelectionFont.SizeInPoints, FontStyle.Bold);

                Font Regular = new Font(ŞimdikiBelgeAl.SelectionFont.FontFamily,
                    ŞimdikiBelgeAl.SelectionFont.SizeInPoints, FontStyle.Regular);

                if (ŞimdikiBelgeAl.SelectionFont.Bold)
                {
                    ŞimdikiBelgeAl.SelectionFont = Regular;
                }
                else
                {
                    ŞimdikiBelgeAl.SelectionFont = Bold;
                }
            }
           
            catch (Exception)
            {
          
                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
}

        private void ıtalicBtn_Click(object sender, EventArgs e)
        {

            try
            {
                Font Italic = new Font(ŞimdikiBelgeAl.SelectionFont.FontFamily,
               ŞimdikiBelgeAl.SelectionFont.SizeInPoints, FontStyle.Italic);

                Font Regular = new Font(ŞimdikiBelgeAl.SelectionFont.FontFamily,
                    ŞimdikiBelgeAl.SelectionFont.SizeInPoints, FontStyle.Regular);

                if (ŞimdikiBelgeAl.SelectionFont.Italic)
                {
                    ŞimdikiBelgeAl.SelectionFont = Regular;
                }
                else
                {
                    ŞimdikiBelgeAl.SelectionFont = Italic;
                }
            }
            
            catch (Exception)
            {
                
                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }   

        private void underlineBtn_Click(object sender, EventArgs e)
        {
            try { 
            Font Underline = new Font(ŞimdikiBelgeAl.SelectionFont.FontFamily,
         ŞimdikiBelgeAl.SelectionFont.SizeInPoints, FontStyle.Underline);

            Font Regular = new Font(ŞimdikiBelgeAl.SelectionFont.FontFamily,
                ŞimdikiBelgeAl.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (ŞimdikiBelgeAl.SelectionFont.Underline)
            {
                ŞimdikiBelgeAl.SelectionFont = Regular;
            }
            else
            {
                ŞimdikiBelgeAl.SelectionFont = Underline;
            }

            }
            catch (Exception)
            {
            
                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void strikeoutBtn_Click(object sender, EventArgs e)
        {
            try { 
            Font Strikeout = new Font(ŞimdikiBelgeAl.SelectionFont.FontFamily,
           ŞimdikiBelgeAl.SelectionFont.SizeInPoints, FontStyle.Strikeout);

            Font Regular = new Font(ŞimdikiBelgeAl.SelectionFont.FontFamily,
                ŞimdikiBelgeAl.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (ŞimdikiBelgeAl.SelectionFont.Strikeout)
            {
                ŞimdikiBelgeAl.SelectionFont = Regular;
            }
            else
            {
                ŞimdikiBelgeAl.SelectionFont = Strikeout;
            }
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textIncreaseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                float YeniFontBüyüklüğü = ŞimdikiBelgeAl.SelectionFont.SizeInPoints + 2;

                Font YeniBüyüklük = new Font(ŞimdikiBelgeAl.SelectionFont.Name, YeniFontBüyüklüğü, ŞimdikiBelgeAl.SelectionFont.Style);

                ŞimdikiBelgeAl.SelectionFont = YeniBüyüklük;
            }
            catch (Exception)
            {
                
                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textDecreaseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                float YeniFontBüyüklüğü = ŞimdikiBelgeAl.SelectionFont.SizeInPoints - 2;

                Font YeniBüyüklük = new Font(ŞimdikiBelgeAl.SelectionFont.Name, YeniFontBüyüklüğü, ŞimdikiBelgeAl.SelectionFont.Style);

                ŞimdikiBelgeAl.SelectionFont = YeniBüyüklük;
            }
            catch (Exception)
            {
                
                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textColorBtn_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                  
                    ŞimdikiBelgeAl.SelectionColor = colorDialog1.Color;
                }
                catch (Exception)
                {
                    
                    MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void uppercaseBtn_Click(object sender, EventArgs e)
        {
            try { 
            ŞimdikiBelgeAl.SelectedText = ŞimdikiBelgeAl.SelectedText.ToUpper();
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lowercaseBtn_Click(object sender, EventArgs e)
        {
            try { 
            ŞimdikiBelgeAl.SelectedText = ŞimdikiBelgeAl.SelectedText.ToLower();
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void leftAlignBtn_Click(object sender, EventArgs e)
        {
            try { 
            ŞimdikiBelgeAl.SelectionAlignment = HorizontalAlignment.Left;
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void centerAlignBtn_Click(object sender, EventArgs e)
        {
            try { 
            ŞimdikiBelgeAl.SelectionAlignment = HorizontalAlignment.Center;
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void rightAlignBtn_Click(object sender, EventArgs e)
        {
            try { 
            ŞimdikiBelgeAl.SelectionAlignment = HorizontalAlignment.Right;
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void indentIncreaseBtn_Click(object sender, EventArgs e)
        {
            try { 
            ŞimdikiBelgeAl.SelectionIndent += 20;
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ındentDecreaseBtn_Click(object sender, EventArgs e)
        {
            try { 
            if (ŞimdikiBelgeAl.SelectionIndent >= 20)
                ŞimdikiBelgeAl.SelectionIndent -= 20;
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void zoomInBtn_Click(object sender, EventArgs e)
        {
            try
            {

            
            if (ŞimdikiBelgeAl.ZoomFactor < 5.0f) 
            {
                ŞimdikiBelgeAl.ZoomFactor += 0.1f;
                upgradeZoomPercent();
            }
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void zoomOutBtn_Click(object sender, EventArgs e)
        {
            try { 

            if (ŞimdikiBelgeAl.ZoomFactor > 0.5f) 
            {
                ŞimdikiBelgeAl.ZoomFactor -= 0.1f;
                upgradeZoomPercent();
            }
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }




        private void upgradeZoomPercent()
        {
            int yuzde = (int)(ŞimdikiBelgeAl.ZoomFactor * 100); 
            zoomPercent.Text = $"{yuzde}%"; 
        }



     


       private void ListBtn_ButtonClick(object sender, EventArgs e)
        {

           

          
            addList();
         
        }

        private void listSymbol1_Click(object sender, EventArgs e)
        {

       

            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null) return;

         
            varsayilanSembol = clickedItem.Tag.ToString();

           
            foreach (ToolStripItem item in ListBtn.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    menuItem.Checked = false;
                }
            }

         
            clickedItem.Checked = true;

       
            addList();

        }

        private void listSymbol2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null) return;

            varsayilanSembol = clickedItem.Tag.ToString();

            foreach (ToolStripItem item in ListBtn.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    menuItem.Checked = false;
                }
            }

            clickedItem.Checked = true;

            addList();

        }

        private void listSymbol3_Click(object sender, EventArgs e)
        {

            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null) return;

            varsayilanSembol = clickedItem.Tag.ToString();

            foreach (ToolStripItem item in ListBtn.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    menuItem.Checked = false;
                }
            }

            clickedItem.Checked = true;

            addList();

        }

        private void listSymbol4_Click(object sender, EventArgs e)
        {


            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null) return;

            varsayilanSembol = clickedItem.Tag.ToString();

            foreach (ToolStripItem item in ListBtn.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    menuItem.Checked = false;
                }
            }

            clickedItem.Checked = true;

            addList();

        }


        private void addList()
        {

            try { 
            if (string.IsNullOrEmpty(varsayilanSembol))
                varsayilanSembol = "•"; 


            if (ŞimdikiBelgeAl == null || string.IsNullOrEmpty(ŞimdikiBelgeAl.SelectedText))
                return;


            
            string[] lines = ŞimdikiBelgeAl.SelectedText.Split(new[] { '\n' }, StringSplitOptions.None);
            StringBuilder sb = new StringBuilder();

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine))
                {
                    sb.AppendLine(line);
                }
                else if (trimmedLine.StartsWith(varsayilanSembol + " "))
                {
                    sb.AppendLine(trimmedLine.Substring(varsayilanSembol.Length + 1));
                }
                else
                {
                    string eskiSembol = tumSemboller.FirstOrDefault(s => trimmedLine.StartsWith(s + " "));
                    if (!string.IsNullOrEmpty(eskiSembol))
                    {
                        sb.AppendLine($"{varsayilanSembol} {trimmedLine.Substring(eskiSembol.Length + 1)}");
                    }
                    else
                    {
                        sb.AppendLine($"{varsayilanSembol} {trimmedLine}");
                    }
                }
            }
        
            int selectionStart = ŞimdikiBelgeAl.SelectionStart;
            ŞimdikiBelgeAl.SelectedText = sb.ToString().TrimEnd();
            ŞimdikiBelgeAl.SelectionStart = selectionStart;
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

 

        private void defaultSymbol_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null) return;

         
            varsayilanSembol = clickedItem.Tag.ToString();

            foreach (ToolStripItem item in ListBtn.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    menuItem.Checked = false;
                }
            }

            clickedItem.Checked = true;

            addList();
        }

     

        private void textInkBtn_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                ŞimdikiBelgeAl.SelectionBackColor = colorDialog1.Color;

            }
        }
        private void fontTypeComboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try { 
            Font newFont = new Font(fontTypeComboBox.SelectedItem.ToString(),
                ŞimdikiBelgeAl.SelectionFont.Size,
                 ŞimdikiBelgeAl.SelectionFont.Style);

            ŞimdikiBelgeAl.SelectionFont = newFont;
            }
            catch (Exception)
            {

                MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }




        }
        private void fontSizeCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                float newSize;
                float.TryParse(
                    fontSizeCombobox.SelectedItem.ToString(),
                    out newSize);

                Font newFont = new Font(ŞimdikiBelgeAl.SelectionFont.Name,
                    newSize, ŞimdikiBelgeAl.SelectionFont.Style);

                ŞimdikiBelgeAl.SelectionFont = newFont;
            
            }
catch (Exception)
{

    MessageBox.Show("Bu dosya formatında kullanılmayı desteklemiyor", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
}

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O) 
            {
                open();
            }
            if (e.Control && e.KeyCode == Keys.N) 
            {
                NewTab();


            }
            if (e.Control && e.KeyCode == Keys.S) 
            {
                save();
            }
            if (e.Control && e.KeyCode == Keys.W) 
            {
              deleteTab();
            }
            if (e.Control && e.KeyCode == Keys.E)
            {
                deleteAllTab();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult sonuc = MessageBox.Show(
      "Kaydetmeyi unutuğunuz dosyalar olabilir... \n " +
      "Uygulamayı kapatmak istediğinize emin misiniz?",
      "Çıkış",
      MessageBoxButtons.YesNo,
      MessageBoxIcon.Question
  );

            if (sonuc == DialogResult.No)
            {
                e.Cancel = true; 
            }
        }
    }
}
