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
    /// Interaction logic for FrmPorudzbina.xaml
    /// </summary>
    public partial class FrmPorudzbina : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;

        public FrmPorudzbina()
        {
            InitializeComponent();
            konekcija = kon.kreirajKonekciju();
            TxtBrojKomadaObuce.Focus();
            PopuniPadajucuListu();
        }

        public FrmPorudzbina(bool azuriraj, DataRowView red) : this()
        {
            this.azuriraj = azuriraj;
            this.red = red;
        }

        private void PopuniPadajucuListu()
        {
            try
            {
                konekcija.Open();

                string vratiRacune = @"select racunID, brojRacuna from [tbl.Racun]";
                SqlDataAdapter daRacun = new SqlDataAdapter(vratiRacune, konekcija);
                DataTable dtRacun = new DataTable();
                daRacun.Fill(dtRacun);
                CbRacun.ItemsSource = dtRacun.DefaultView;
                daRacun.Dispose();
                dtRacun.Dispose();

            }
            catch (SqlException)
            {
                MessageBox.Show("Text padajuće liste nisu popunjene!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally 
            {
                if (konekcija != null) 
                {
                    konekcija.Close();
                }
            }
        }
        private bool Validacija()
        {
            if (dpDatum.SelectedDate == null)
            {
                MessageBox.Show("Morate izabrati datum porudžbine.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpDatum.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtBrojKomadaObuce.Text))
            {
                MessageBox.Show("Morate uneti broj komada obuće.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtBrojKomadaObuce.Focus();
                return false;
            }

            if (!double.TryParse(TxtBrojKomadaObuce.Text, out double bko) || bko <= 0)
            {
                MessageBox.Show("Broj komada obuće ne može biti negativan broj.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtBrojKomadaObuce.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtIznosPorudzbine.Text))
            {
                MessageBox.Show("Morate uneti iznos porudžbine.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtIznosPorudzbine.Focus();
                return false;
            }

            if (!double.TryParse(TxtIznosPorudzbine.Text, out double iznos) || iznos <= 0)
            {
                MessageBox.Show("Iznos ne može biti negativan broj.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtIznosPorudzbine.Focus();
                return false;
            }

            if (CbRacun.SelectedValue == null)
            {
                MessageBox.Show("Morate izabrati stavku iz padajuće liste.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                DateTime date = (DateTime)dpDatum.SelectedDate;
                string datum = date.ToString("yyyy-MM-dd");
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@datumPorudzbine", SqlDbType.DateTime).Value = datum;
                cmd.Parameters.Add("@brojKomadaObuce", SqlDbType.Int).Value = TxtBrojKomadaObuce.Text;
                cmd.Parameters.Add("@popustNaKupovinu", SqlDbType.Bit).Value = Convert.ToInt32(cbxPopust.IsChecked);
                cmd.Parameters.Add("@iznosPorudzbine", SqlDbType.Decimal).Value = TxtIznosPorudzbine.Text;
                cmd.Parameters.Add("@racunID", SqlDbType.Int).Value = CbRacun.SelectedValue;
                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["Id"];
                    cmd.CommandText = @"update [tbl.Porudzbina] set datumPorudzbine = @datumPorudzbine, 
                    brojKomadaObuce = @brojKomadaObuce, popustNaKupovinu = @popustNaKupovinu, iznosPorudzbine = @iznosPorudzbine, 
                    racunID = @racunID where porudzbinaID = @id";
                    red = null;
                }
                else 
                {
                    cmd.CommandText = @"insert into [tbl.Porudzbina](datumPorudzbine, brojKomadaObuce, popustNaKupovinu, iznosPorudzbine, racunID) 
                            values(@datumPorudzbine, @brojKomadaObuce, @popustNaKupovinu, @iznosPorudzbine, @racunID)";
                } 
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Unos određenih vrednosti nije validan!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException)
            {
                MessageBox.Show("Došlo je do greške prilikom konverzije podataka!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Odaberite datum!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
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
