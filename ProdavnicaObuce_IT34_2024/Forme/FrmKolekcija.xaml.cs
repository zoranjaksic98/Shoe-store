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
    /// Interaction logic for FrmKolekcija.xaml
    /// </summary>
    public partial class FrmKolekcija : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;

        public FrmKolekcija()
        {
            InitializeComponent();
            konekcija = kon.kreirajKonekciju();
            TxtNazivKolekcije.Focus();
        }

        public FrmKolekcija(bool azuriraj, DataRowView red) : this()
        {
            this.azuriraj = azuriraj;
            this.red = red;
        }
        private bool Validacija()
        {
            if (string.IsNullOrWhiteSpace(TxtNazivKolekcije.Text))
            {
                MessageBox.Show("Morate uneti naziv kolekcije.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtNazivKolekcije.Focus();
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
                cmd.Parameters.Add("@nazivKolekcije", SqlDbType.NVarChar).Value = TxtNazivKolekcije.Text;
                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["Id"];
                    cmd.CommandText = @"update [tbl.Kolekcija] set nazivKolekcije = @nazivKolekcije where kolekcijaID = @id";
                    red = null;
                }
                else 
                {
                    cmd.CommandText = @"insert into [tbl.Kolekcija](nazivKolekcije) values (@nazivKolekcije)";
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
