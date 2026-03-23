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
    /// Interaction logic for FrmKupac.xaml
    /// </summary>
    public partial class FrmKupac : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;

        public FrmKupac()
        {
            InitializeComponent();
            konekcija = kon.kreirajKonekciju();
            TxtImeKupca.Focus();
        }

        public FrmKupac(bool azuriraj, DataRowView red) : this()
        {
            this.azuriraj = azuriraj;
            this.red = red;
        }

        private bool Validacija()
        {
            if (string.IsNullOrWhiteSpace(TxtImeKupca.Text))
            {
                MessageBox.Show("Morate uneti ime kupca.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtImeKupca.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtPrezimeKupca.Text))
            {
                MessageBox.Show("Morate uneti prezime kupca.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtPrezimeKupca.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtEmailKupca.Text))
            {
                MessageBox.Show("Morate uneti e-mail kupca.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtEmailKupca.Focus();
                return false;
            }

            if (!TxtEmailKupca.Text.Contains("@"))
            {
                MessageBox.Show("E-mail mora da sadrži @.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtEmailKupca.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtKontaktKupca.Text))
            {
                MessageBox.Show("Morate uneti kontakt kupca.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtKontaktKupca.Focus();
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
                cmd.Parameters.Add("@imeKupca", SqlDbType.NVarChar).Value = TxtImeKupca.Text;
                cmd.Parameters.Add("@prezimeKupca", SqlDbType.NVarChar).Value = TxtPrezimeKupca.Text;
                cmd.Parameters.Add("@emailKupca", SqlDbType.NVarChar).Value = TxtEmailKupca.Text;
                cmd.Parameters.Add("@kontaktKupca", SqlDbType.NVarChar).Value = TxtKontaktKupca.Text;
                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["Id"];
                    cmd.CommandText = @"update [tbl.Kupac] set imeKupca = @imeKupca, prezimeKupca = @prezimeKupca, 
                                        emailKupca = @emailKupca, kontaktKupca = @kontaktKupca where kupacID = @id";
                    red = null;
                }
                else 
                {
                    cmd.CommandText = @"insert into [tbl.Kupac](imeKupca, prezimeKupca, emailKupca, kontaktKupca) 
                                    values (@imeKupca, @prezimeKupca, @emailKupca, @kontaktKupca)";
                } 
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Unos određenih podataka nije validan!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
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
