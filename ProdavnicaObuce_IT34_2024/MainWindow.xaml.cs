using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProdavnicaObuce_IT34_2024.Forme;

namespace ProdavnicaObuce_IT34_2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool azuriraj;
        private DataRowView red;
        private string ucitanaTabela;
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();

        #region Select upiti
        private static string kategorijeSelect = @"select kategorijaID as Id, vrstaKategorije as 'Vrsta kategorije',
                                                podvrstaKategorije as 'Podvrsta kategorije' from [tbl.Kategorija]";
        private static string kolekcijeSelect = @"select kolekcijaID as Id, nazivKolekcije as 'Naziv kolekcije' from [tbl.Kolekcija]";
        private static string kupciSelect = @"select kupacID as Id, imeKupca as 'Ime kupca', prezimeKupca as 'Prezime kupca', 
                                            emailKupca as 'E-mail kupca', kontaktKupca as 'Broj telefona kupca' from [tbl.Kupac]";
        private static string obuceSelect = @"select obucaID as Id, cena as Cena, boja as Boja, velicina as Veličina, ime as Proizvođač, 
                                            nazivVrste as 'Naziv vrste', vrstaKategorije as 'Vrsta kategorije',
                                            nazivKolekcije as 'Naziv Kolekcije', emailKupca as 'E-mail kupca', 
                                            datumPorudzbine as 'Datum porudžbine' from [tbl.Obuca]
                                            join [tbl.Proizvodjac] on [tbl.Obuca].proizvodjacID = [tbl.Proizvodjac].proizvodjacID
                                            join [tbl.Vrsta] on [tbl.Obuca].vrstaID = [tbl.Vrsta].vrstaID
                                            join [tbl.Kategorija] on [tbl.Obuca].kategorijaID = [tbl.Kategorija].kategorijaID
                                            join [tbl.Kolekcija] on [tbl.Obuca].kolekcijaID = [tbl.Kolekcija].kolekcijaID
                                            join [tbl.Kupac] on [tbl.Obuca].kupacID = [tbl.Kupac].kupacID
                                            join [tbl.Porudzbina] on [tbl.Obuca].porudzbinaID = [tbl.Porudzbina].porudzbinaID";
        private static string porudzbineSelect = @"select porudzbinaID as Id, datumPorudzbine as 'Datum porudžbine', 
                                                brojKomadaObuce as 'Broj komada obuće', popustNaKupovinu as 'Popust na kupovinu', 
                                                iznosPorudzbine as 'Iznos porudžbine', brojRacuna as 'Broj računa' from [tbl.Porudzbina]
                                                join [tbl.Racun] on [tbl.Porudzbina].racunID = [tbl.Racun].racunID";
        private static string proizvodjaciSelect = @"select proizvodjacID as Id, ime as Ime, zemljaPorekla as 'Zemlja porekla' 
                                                    from [tbl.Proizvodjac]";
        private static string racuniSelect = @"select racunID as Id, brojRacuna as 'Broj računa' from [tbl.Racun]";
        private static string vrsteSelect = @"select vrstaID as Id, nazivVrste as 'Naziv vrste' from [tbl.Vrsta]";
        #endregion

        #region Select sa uslovom
        private static string selectUslovKategorije = @"select * from [tbl.Kategorija] where kategorijaID =";
        private static string selectUslovKolekcije = @"select * from [tbl.Kolekcija] where kolekcijaID =";
        private static string selectUslovKupci = @"select * from [tbl.Kupac] where kupacID =";
        private static string selectUslovObuce = @"select * from [tbl.Obuca] where obucaID =";
        private static string selectUslovPorudzbine = @"select * from [tbl.Porudzbina] where porudzbinaID =";
        private static string selectUslovProizvodjaci = @"select * from [tbl.Proizvodjac] where proizvodjacID =";
        private static string selectUslovRacuni = @"select * from [tbl.Racun] where racunID =";
        private static string selectUslovVrste = @"select * from [tbl.Vrsta] where vrstaID =";
        #endregion

        #region Delete naredbe
        private static string kategorijeDelete = @"delete from [tbl.Kategorija] where kategorijaID =";
        private static string kolekcijeDelete = @"delete from [tbl.Kolekcija] where kolekcijaID =";
        private static string kupciDelete = @"delete from [tbl.Kupac] where kupacID =";
        private static string obuceDelete = @"delete from [tbl.Obuca] where obucaID =";
        private static string porudzbineDelete = @"delete from [tbl.Porudzbina] where porudzbinaID =";
        private static string proizvodjaciDelete = @"delete from [tbl.Proizvodjac] where proizvodjacID =";
        private static string racuniDelete = @"delete from [tbl.Racun] where racunID =";
        private static string vrsteDelete = @"delete from [tbl.Vrsta] where vrstaID =";
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.kreirajKonekciju();
            UcitajPodatke(kupciSelect);
        }

        private void UcitajPodatke(string selectUpit)
        {
            try
            {
                konekcija.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(selectUpit, konekcija);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dataGridCentralni != null) 
                {
                    dataGridCentralni.ItemsSource = dt.DefaultView;
                }
                ucitanaTabela = selectUpit;
                adapter.Dispose();
                dt.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Neuspešno učitani podaci!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally 
            {
                if (konekcija != null) 
                {
                    konekcija.Close();
                }
            }
        }

        private void btnKategorije_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(kategorijeSelect);
        }

        private void btnKolekcije_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(kolekcijeSelect);
        }

        private void btnKupci_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(kupciSelect);
        }

        private void btnObuce_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(obuceSelect);
        }

        private void btnPorudzbine_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(porudzbineSelect);
        }

        private void btnProizvodjaci_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(proizvodjaciSelect);
        }

        private void btnRacuni_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(racuniSelect);
        }

        private void btnVrste_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(vrsteSelect);
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;
            if (ucitanaTabela.Equals(kategorijeSelect))
            {
                prozor = new FrmKategorija();
                prozor.ShowDialog();
                UcitajPodatke(kategorijeSelect);
            }
            else if (ucitanaTabela.Equals(kolekcijeSelect)) 
            {
                prozor = new FrmKolekcija();
                prozor.ShowDialog();
                UcitajPodatke(kolekcijeSelect);
            }
            else if (ucitanaTabela.Equals(kupciSelect))
            {
                prozor = new FrmKupac();
                prozor.ShowDialog();
                UcitajPodatke(kupciSelect);
            }
            else if (ucitanaTabela.Equals(obuceSelect))
            {
                prozor = new FrmObuca();
                prozor.ShowDialog();
                UcitajPodatke(obuceSelect);
            }
            else if (ucitanaTabela.Equals(porudzbineSelect))
            {
                prozor = new FrmPorudzbina();
                prozor.ShowDialog();
                UcitajPodatke(porudzbineSelect);
            }
            else if (ucitanaTabela.Equals(proizvodjaciSelect))
            {
                prozor = new FrmProizvodjac();
                prozor.ShowDialog();
                UcitajPodatke(proizvodjaciSelect);
            }
            else if (ucitanaTabela.Equals(racuniSelect))
            {
                prozor = new FrmRacun();
                prozor.ShowDialog();
                UcitajPodatke(racuniSelect);
            }
            else if (ucitanaTabela.Equals(vrsteSelect))
            {
                prozor = new FrmVrsta();
                prozor.ShowDialog();
                UcitajPodatke(vrsteSelect);
            }
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(kategorijeSelect))
            {
                PopuniFormu(selectUslovKategorije);
                UcitajPodatke(kategorijeSelect);
            }
            else if (ucitanaTabela.Equals(kolekcijeSelect)) 
            {
                PopuniFormu(selectUslovKolekcije);
                UcitajPodatke(kolekcijeSelect);
            }
            else if (ucitanaTabela.Equals(kupciSelect))
            {
                PopuniFormu(selectUslovKupci);
                UcitajPodatke(kupciSelect);
            }
            else if (ucitanaTabela.Equals(obuceSelect))
            {
                PopuniFormu(selectUslovObuce);
                UcitajPodatke(obuceSelect);
            }
            else if (ucitanaTabela.Equals(porudzbineSelect))
            {
                PopuniFormu(selectUslovPorudzbine);
                UcitajPodatke(porudzbineSelect);
            }
            else if (ucitanaTabela.Equals(proizvodjaciSelect))
            {
                PopuniFormu(selectUslovProizvodjaci);
                UcitajPodatke(proizvodjaciSelect);
            }
            else if (ucitanaTabela.Equals(racuniSelect))
            {
                PopuniFormu(selectUslovRacuni);
                UcitajPodatke(racuniSelect);
            }
            else if (ucitanaTabela.Equals(vrsteSelect))
            {
                PopuniFormu(selectUslovVrste);
                UcitajPodatke(vrsteSelect);
            }
        }

        private void PopuniFormu(string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                red = (DataRowView)dataGridCentralni.SelectedItems[0];
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["Id"];
                cmd.CommandText = selectUslov + "@id";
                SqlDataReader citac = cmd.ExecuteReader();
                if (citac.Read())
                {
                    if (ucitanaTabela.Equals(kategorijeSelect))
                    {
                        FrmKategorija prozorKategorija = new FrmKategorija(azuriraj, red);
                        prozorKategorija.TxtVrstaKategorije.Text = citac["vrstaKategorije"].ToString();
                        prozorKategorija.TxtPodvrstaKategorije.Text = citac["podvrstaKategorije"].ToString();
                        prozorKategorija.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(kolekcijeSelect))
                    {
                        FrmKolekcija prozorKolekcija = new FrmKolekcija(azuriraj, red);
                        prozorKolekcija.TxtNazivKolekcije.Text = citac["nazivKolekcije"].ToString();
                        prozorKolekcija.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(kupciSelect))
                    {
                        FrmKupac prozorKupac = new FrmKupac(azuriraj, red);
                        prozorKupac.TxtImeKupca.Text = citac["imeKupca"].ToString();
                        prozorKupac.TxtPrezimeKupca.Text = citac["prezimeKupca"].ToString();
                        prozorKupac.TxtEmailKupca.Text = citac["emailKupca"].ToString();
                        prozorKupac.TxtKontaktKupca.Text = citac["kontaktKupca"].ToString();
                        prozorKupac.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(obuceSelect))
                    {
                        FrmObuca prozorObuca = new FrmObuca(azuriraj, red);
                        prozorObuca.TxtCena.Text = citac["cena"].ToString();
                        prozorObuca.TxtBoja.Text = citac["boja"].ToString();
                        prozorObuca.TxtVelicina.Text = citac["velicina"].ToString();
                        prozorObuca.CbProizvodjac.SelectedValue = citac["proizvodjacID"];
                        prozorObuca.CbVrsta.SelectedValue = citac["vrstaID"];
                        prozorObuca.CbKategorija.SelectedValue = citac["kategorijaID"];
                        prozorObuca.CbKolekcija.SelectedValue = citac["kolekcijaID"];
                        prozorObuca.CbKupac.SelectedValue = citac["kupacID"];
                        prozorObuca.CbPorudzbina.SelectedValue = citac["porudzbinaID"];
                        prozorObuca.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(porudzbineSelect))
                    {
                        FrmPorudzbina prozorPorudzbina = new FrmPorudzbina(azuriraj, red);
                        prozorPorudzbina.dpDatum.SelectedDate = (DateTime)citac["datumPorudzbine"];
                        prozorPorudzbina.TxtBrojKomadaObuce.Text = citac["brojKomadaObuce"].ToString();
                        prozorPorudzbina.cbxPopust.IsChecked = (bool)citac["popustNaKupovinu"];
                        prozorPorudzbina.TxtIznosPorudzbine.Text = citac["iznosPorudzbine"].ToString();
                        prozorPorudzbina.CbRacun.SelectedValue = citac["racunID"];
                        prozorPorudzbina.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(proizvodjaciSelect))
                    {
                        FrmProizvodjac prozorProizvodjac = new FrmProizvodjac(azuriraj, red);
                        prozorProizvodjac.TxtIme.Text = citac["ime"].ToString();
                        prozorProizvodjac.TxtZemljaPorekla.Text = citac["zemljaPorekla"].ToString();
                        prozorProizvodjac.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(racuniSelect))
                    {
                        FrmRacun prozorRacun = new FrmRacun(azuriraj, red);
                        prozorRacun.TxtBrojRacuna.Text = citac["brojRacuna"].ToString();
                        prozorRacun.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(vrsteSelect)) 
                    {
                        FrmVrsta prozorVrsta = new FrmVrsta(azuriraj, red);
                        prozorVrsta.TxtNazivVrste.Text = citac["nazivVrste"].ToString();
                        prozorVrsta.ShowDialog();
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally 
            {
                if (konekcija != null) 
                {
                    konekcija.Close();
                }
                azuriraj = false;
            }
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(kategorijeSelect))
            {
                ObrisiZapis(kategorijeDelete);
                UcitajPodatke(kategorijeSelect);
            }
            else if (ucitanaTabela.Equals(kolekcijeSelect)) 
            {
                ObrisiZapis(kolekcijeDelete);
                UcitajPodatke(kolekcijeSelect);
            }
            else if (ucitanaTabela.Equals(kupciSelect))
            {
                ObrisiZapis(kupciDelete);
                UcitajPodatke(kupciSelect);
            }
            else if (ucitanaTabela.Equals(obuceSelect))
            {
                ObrisiZapis(obuceDelete);
                UcitajPodatke(obuceSelect);
            }
            else if (ucitanaTabela.Equals(porudzbineSelect))
            {
                ObrisiZapis(porudzbineDelete);
                UcitajPodatke(porudzbineSelect);
            }
            else if (ucitanaTabela.Equals(proizvodjaciSelect))
            {
                ObrisiZapis(proizvodjaciDelete);
                UcitajPodatke(proizvodjaciSelect);
            }
            else if (ucitanaTabela.Equals(racuniSelect))
            {
                ObrisiZapis(racuniDelete);
                UcitajPodatke(racuniSelect);
            }
            else if (ucitanaTabela.Equals(vrsteSelect))
            {
                ObrisiZapis(vrsteDelete);
                UcitajPodatke(vrsteSelect);
            }
        }

        private void ObrisiZapis(string deleteUpit)
        {
            try
            {
                konekcija.Open();
                red = (DataRowView)dataGridCentralni.SelectedItems[0];
                MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni da želite da obrišete?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezultat == MessageBoxResult.Yes) 
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = konekcija
                    };
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["Id"];
                    cmd.CommandText = deleteUpit + "@id";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoje povezani podaci u nekim tabelama!", "Greška!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally 
            {
                if (konekcija != null) 
                {
                    konekcija.Close();
                }
            }
        }
    }
}
