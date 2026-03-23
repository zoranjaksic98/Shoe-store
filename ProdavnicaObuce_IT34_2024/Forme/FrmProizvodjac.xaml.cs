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
    /// Interaction logic for FrmProizvodjac.xaml
    /// </summary>
    public partial class FrmProizvodjac : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;

        public FrmProizvodjac()
        {
            InitializeComponent();
            konekcija = kon.kreirajKonekciju();
            TxtIme.Focus();
        }

        public FrmProizvodjac(bool azuriraj, DataRowView red) : this()
        {
            this.azuriraj = azuriraj;
            this.red = red;
        }
        private bool Validacija()
        {
            if (string.IsNullOrWhiteSpace(TxtIme.Text))
            {
                MessageBox.Show("Morate uneti ime proizvodjača.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtIme.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtZemljaPorekla.Text))
            {
                MessageBox.Show("Morate uneti zemlju porekla proizvodjača.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtZemljaPorekla.Focus();
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
                cmd.Parameters.Add("@ime", SqlDbType.NVarChar).Value = TxtIme.Text;
                cmd.Parameters.Add("@zemljaPorekla", SqlDbType.NVarChar).Value = TxtZemljaPorekla.Text;
                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["Id"];
                    cmd.CommandText = @"update [tbl.Proizvodjac] set ime = @ime, zemljaPorekla = @zemljaPorekla 
                                        where proizvodjacID = @id";
                    red = null;
                }
                else 
                {
                    cmd.CommandText = @"insert into [tbl.Proizvodjac](ime, zemljaPorekla)
                                    values(@ime, @zemljaPorekla)";
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
