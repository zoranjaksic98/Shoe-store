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
    /// Interaction logic for FrmObuca.xaml
    /// </summary>
    public partial class FrmObuca : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private bool azuriraj;
        private DataRowView red;

        public FrmObuca()
        {
            InitializeComponent();
            konekcija = kon.kreirajKonekciju();
            TxtCena.Focus();
            PopuniPadajuceListe();
        }

        public FrmObuca(bool azuriraj, DataRowView red) : this()
        {
            this.azuriraj = azuriraj;
            this.red = red;
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();

                string vratiProizvodjace = @"select proizvodjacID, ime from [tbl.Proizvodjac]";
                SqlDataAdapter daProizvodjac = new SqlDataAdapter(vratiProizvodjace, konekcija);
                DataTable dtProizvodjac = new DataTable();
                daProizvodjac.Fill(dtProizvodjac);
                CbProizvodjac.ItemsSource = dtProizvodjac.DefaultView;
                daProizvodjac.Dispose();
                dtProizvodjac.Dispose();

                string vratiVrste = @"select vrstaID, nazivVrste from [tbl.Vrsta]";
                SqlDataAdapter daVrsta = new SqlDataAdapter(vratiVrste, konekcija);
                DataTable dtVrsta = new DataTable();
                daVrsta.Fill(dtVrsta);
                CbVrsta.ItemsSource = dtVrsta.DefaultView;
                daVrsta.Dispose();
                dtVrsta.Dispose();

                string vratiKategorije = @"select kategorijaID, vrstaKategorije from [tbl.Kategorija]";
                SqlDataAdapter daKategorija = new SqlDataAdapter(vratiKategorije, konekcija);
                DataTable dtKategorija = new DataTable();
                daKategorija.Fill(dtKategorija);
                CbKategorija.ItemsSource = dtKategorija.DefaultView;
                daKategorija.Dispose();
                dtKategorija.Dispose();

                string vratiKolekcije = @"select kolekcijaID, nazivKolekcije from [tbl.Kolekcija]";
                SqlDataAdapter daKolekcija = new SqlDataAdapter(vratiKolekcije, konekcija);
                DataTable dtKolekcija = new DataTable();
                daKolekcija.Fill(dtKolekcija);
                CbKolekcija.ItemsSource = dtKolekcija.DefaultView;
                daKolekcija.Dispose();
                dtKolekcija.Dispose();

                string vratiKupce = @"select kupacID, imeKupca + ' ' + prezimeKupca as kupac from [tbl.Kupac]";
                SqlDataAdapter daKupac = new SqlDataAdapter(vratiKupce, konekcija);
                DataTable dtKupac = new DataTable();
                daKupac.Fill(dtKupac);
                CbKupac.ItemsSource = dtKupac.DefaultView;
                daKupac.Dispose();
                dtKupac.Dispose();

                string vratiPorudzbine = @"select porudzbinaID, datumPorudzbine from [tbl.Porudzbina]";
                SqlDataAdapter daPorudzbina = new SqlDataAdapter(vratiPorudzbine, konekcija);
                DataTable dtPorudzbina = new DataTable();
                daPorudzbina.Fill(dtPorudzbina);
                CbPorudzbina.ItemsSource = dtPorudzbina.DefaultView;
                daPorudzbina.Dispose();
                dtPorudzbina.Dispose();
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
            if (string.IsNullOrWhiteSpace(TxtCena.Text))
            {
                MessageBox.Show("Morate uneti cenu obuće.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtCena.Focus();
                return false;
            }

            if (!double.TryParse(TxtCena.Text, out double cena) || cena <= 0)
            {
                MessageBox.Show("Cena ne može biti negativan broj.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtCena.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtBoja.Text))
            {
                MessageBox.Show("Morate uneti boju obuće.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtBoja.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtVelicina.Text))
            {
                MessageBox.Show("Morate uneti veličinu obuće.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtVelicina.Focus();
                return false;
            }

            if (!int.TryParse(TxtVelicina.Text, out int velicina) || velicina <= 0)
            {
                MessageBox.Show("Veličina ne može biti negativan broj.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtBoja.Focus();
                return false;
            }
            

            if (CbProizvodjac.SelectedValue == null || CbVrsta.SelectedValue == null || CbKategorija.SelectedValue == null ||
                CbKolekcija.SelectedValue == null || CbKupac.SelectedValue == null || CbPorudzbina.SelectedValue == null)
            {
                MessageBox.Show("Morate izabrati sve stavke iz padajućih lista.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                cmd.Parameters.Add("@cena", SqlDbType.Decimal).Value = TxtCena.Text;
                cmd.Parameters.Add("@boja", SqlDbType.NVarChar).Value = TxtBoja.Text;
                cmd.Parameters.Add("@velicina", SqlDbType.Int).Value = TxtVelicina.Text;
                cmd.Parameters.Add("@proizvodjacID", SqlDbType.Int).Value = CbProizvodjac.SelectedValue;
                cmd.Parameters.Add("@vrstaID", SqlDbType.Int).Value = CbVrsta.SelectedValue;
                cmd.Parameters.Add("@kategorijaID", SqlDbType.Int).Value = CbKategorija.SelectedValue;
                cmd.Parameters.Add("@kolekcijaID", SqlDbType.Int).Value = CbKolekcija.SelectedValue;
                cmd.Parameters.Add("@kupacID", SqlDbType.Int).Value = CbKupac.SelectedValue;
                cmd.Parameters.Add("@porudzbinaID", SqlDbType.Int).Value = CbPorudzbina.SelectedValue;
                if (azuriraj)
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["Id"];
                    cmd.CommandText = @"update [tbl.Obuca] set cena = @cena, boja = @boja, velicina = @velicina,
                                proizvodjacID = @proizvodjacID, vrstaID = @vrstaID, kategorijaID = @kategorijaID,
                                kolekcijaID = @kolekcijaID, kupacID = @kupacID, porudzbinaID = @porudzbinaID where obucaID = @id";
                    red = null;
                }
                else 
                {
                    cmd.CommandText = @"insert into [tbl.Obuca](cena, boja, velicina, proizvodjacID, vrstaID, kategorijaID, 
                                        kolekcijaID, kupacID, porudzbinaID)
                  values(@cena, @boja, @velicina, @proizvodjacID, @vrstaID, @kategorijaID, @kolekcijaID, @kupacID, @porudzbinaID)";
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
