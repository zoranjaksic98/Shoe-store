using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProdavnicaObuce_IT34_2024.Forme
{
    /// <summary>
    /// Interaction logic for FrmKategorija.xaml
    /// </summary>
    public partial class FrmKategorija : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;

        public FrmKategorija()
        {
            InitializeComponent();
            konekcija = kon.kreirajKonekciju();
            TxtVrstaKategorije.Focus();
        }

        public FrmKategorija(bool azuriraj, DataRowView red) : this()
        {
            this.azuriraj = azuriraj;
            this.red = red;
        }

        private bool Validacija()
        {
            if (string.IsNullOrWhiteSpace(TxtVrstaKategorije.Text))
            {
                MessageBox.Show("Morate uneti vrstu kategorije.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtVrstaKategorije.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtPodvrstaKategorije.Text))
            {
                MessageBox.Show("Morate uneti podvrstu kategorije.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtPodvrstaKategorije.Focus();
                return false;
            }

            return true;
        }

        private void BtnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (!Validacija()) 
            {
                return;
            }
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@vrstaKategorije", SqlDbType.NVarChar).Value = TxtVrstaKategorije.Text;
                cmd.Parameters.Add("@podvrstaKategorije", SqlDbType.NVarChar).Value = TxtPodvrstaKategorije.Text;
                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["Id"];
                    cmd.CommandText = @"update [tbl.Kategorija] set vrstaKategorije = @vrstaKategorije, 
                                        podvrstaKategorije = @podvrstaKategorije where kategorijaID = @id";
                    red = null;
                }
                else
                {
                    cmd.CommandText = @"insert into [tbl.Kategorija](vrstaKategorije, podvrstaKategorije)
                                    values (@vrstaKategorije, @podvrstaKategorije)";
                }      
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unos određenih vrednosti nije validan!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally 
            {
                if (konekcija != null) 
                {
                    konekcija.Close();
                }
            }


        }

        private void BtnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
