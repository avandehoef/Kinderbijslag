using System;
using System.Linq;
using System.Windows.Forms;

//Een kinderbijslagregeling ziet er als volgt uit:
//  Voor elk kind jonger dan 12 jaar bedraagt de kinderbijslag € 150,= per kwartaal
//  Voor elk kind vanaf 12 jaar en jonger dan 18 jaar is dit bedrag € 235,=.
//  Afhankelijk van het totale aantal kinderen kan het berekende bedrag nog worden verhoogd met een opslagpercentage:
//      Dit opslagpercentage bedraagt 2% bij 3 of 4 kinderen, 3% bij 5 kinderen en 3.5% bij 6 of meer kinderen.

//  De te ontwikkelen software moet aan de hand van de gezinsgegevens de aan dat gezin toegekende kinderbijslag bepalen.
//  De leeftijd van elk kind moet aan de hand van geboortedatum en een peildatum worden berekend.

namespace Kinderbijslag
{
    public partial class Kinderbijslag : Form
    {
        public Kinderbijslag()
        {
            InitializeComponent();
        }

        decimal kbJongerTwaalf = 150.00m, kbOuderTwaalf = 235.00m, totaal;
        decimal opslagDrieVier = 1.02m, opslagVijf = 1.03m, opslagZesMeer = 1.035m;
        decimal subEen, subTwee, subDrie, subVier, subVijf, subZes, subZeven, subAcht, subNegen, subTien, subElf, subTwaalf, subDertien, subVeertien;
        int aantalKinderen, foutCorrectie, foutCorrectieTemp; // foutcorrectie is als een kind wordt opgegeven die ouder is dan 18 (en dus geen recht meer heeft op kinderbijslag), deze niet voor de opslag mag meetellen
        int maandNu = DateTime.Now.Month, dagNu = DateTime.Now.Day;
        DateTime grensAchtienPlus, grensTwaalfAchtien, nu; // bepaling van: 1) of iemand ouder is dan 18 (en dus geen recht meer heeft op kinderbijslag), 2) of de leeftijd tussen 12 en 18 valt en 3) of het kind jonger is dan 12
        DateTime kindEenDatum, kindTweeDatum, kindDrieDatum, kindVierDatum, kindVijfDatum, kindZesDatum, kindZevenDatum, kindAchtDatum, kindNegenDatum, kindTienDatum, kindElfDatum, kindTwaalfDatum, kindDertienDatum, kindVeertienDatum;
        string datumKindEen, datumKindTwee, datumKindDrie, datumKindVier, datumKindVijf, datumKindZes, datumKindZeven, datumKindAcht, datumKindNegen, datumKindTien, datumKindElf, datumKindTwaalf, datumKindDertien, datumKindVeertien;

        private void btnRegels_Click(object sender, EventArgs e)
        {
            Regels regels = new Regels();
            regels.Show();
        }

        int dagEen, maandEen, jaarEen, dagTwee, maandTwee, jaarTwee, dagDrie, maandDrie, jaarDrie, dagVier, maandVier, jaarVier,
            dagVijf, maandVijf, jaarVijf, dagZes, maandZes, jaarZes, dagZeven, maandZeven, jaarZeven, dagAcht, maandAcht, jaarAcht,
            dagNegen, maandNegen, jaarNegen, dagTien, maandTien, jaarTien, dagElf, maandElf, jaarElf, dagTwaalf, maandTwaalf, jaarTwaalf,
            dagDertien, maandDertien, jaarDertien, dagVeertien, maandVeertien, jaarVeertien;

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cBoxAantalKinderen_SelectedIndexChanged(object sender, EventArgs e)
        {
            aantalKinderen = Convert.ToInt16(cBoxAantalKinderen.Text);
            if (aantalKinderen == 1)
            {
                kindEen();
            }
            else if (aantalKinderen ==2)
            {
                kindTwee();
            }
            else if (aantalKinderen == 3)
            {
                kindDrie();
            }
            else if (aantalKinderen == 4)
            {
                kindVier();
            }
            else if (aantalKinderen == 5)
            {
                kindVijf();
            }
            else if (aantalKinderen == 6)
            {
                kindZes();
            }
            else if (aantalKinderen == 7)
            {
                kindZeven();
            }
            else if (aantalKinderen == 8)
            {
                kindAcht();
            }
            else if (aantalKinderen == 9)
            {
                kindNegen();
            }
            else if (aantalKinderen == 10)
            {
                kindTien();
            }
            else if (aantalKinderen == 11)
            {
                kindElf();                
            }
            else if (aantalKinderen == 12)
            {
                kindTwaalf();
            }
            else if (aantalKinderen == 13)
            {
                kindDertien();
            }
            else if (aantalKinderen == 14)
            {
                kindVeertien();
            }
        }

        private void Kinderbijslag_Load(object sender, EventArgs e)
        {
            cBoxAantalKinderen.SelectedIndex = 0;
            defaultState();
            textBoxGebDatumEen.Visible = true;
            cBoxKindEenDag.Visible = true;
            cBoxKindEenMaand.Visible = true;
            cBoxKindEenJaar.Visible = true;
            cBoxKindEenDag.Enabled = true;
            cBoxKindEenMaand.Enabled = true;
            cBoxKindEenJaar.Enabled = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            defaultState();            
            foreach (var cb in Controls.OfType<ComboBox>())
            {
                if (Controls.OfType<ComboBox>() != cBoxAantalKinderen)
                {
                    cb.SelectedIndex = 0;
                }
            }            
            textBoxGebDatumEen.Visible = true;
            cBoxKindEenDag.Visible = true;
            cBoxKindEenMaand.Visible = true;
            cBoxKindEenJaar.Visible = true;
            cBoxKindEenDag.Enabled = true;
            cBoxKindEenMaand.Enabled = true;
            cBoxKindEenJaar.Enabled = true;
        }

        private void btnBereken_Click(object sender, EventArgs e)
        {
            foutCorrectieTemp = 0;

            if (cBoxAantalKinderen.Text == "1")
            {
                kindEenLeeftijd();                
            }
            else if (cBoxAantalKinderen.Text == "2")
            {
                kindTweeLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "3")
            {
                kindDrieLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "4")
            {
                kindVierLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "5")
            {
                kindVijfLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "6")
            {
                kindZesLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "7")
            {
                kindZevenLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "8")
            {
                kindAchtLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "9")
            {
                kindNegenLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "10")
            {
                kindTienLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "11")
            {
                kindElfLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "12")
            {
                kindTwaalfLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "13")
            {
                kindDertienLeeftijd();
            }
            else if (cBoxAantalKinderen.Text == "14")
            {
                kindVeertienLeeftijd();
            }
            else
            {
                textBoxFout.Text = "Er is iets fout gegaan; probeer opnieuw";
            }

            aantalKinderen = Convert.ToInt16(cBoxAantalKinderen.Text);
            foutCorrectie = aantalKinderen - foutCorrectieTemp;

            if (aantalKinderen == 3 || aantalKinderen == 4)
            {
                if (foutCorrectie == 3 || foutCorrectie == 4)
                {
                    totaal = (subEen + subTwee + subDrie + subVier) * opslagDrieVier;
                }
                else
                {
                    totaal = (subEen + subTwee + subDrie + subVier);
                }                
            }
            else if (aantalKinderen == 5)
            {
                if (foutCorrectie == 5)
                {
                    totaal = (subEen + subTwee + subDrie + subVier + subVijf) * opslagVijf;
                }
                else if (foutCorrectie == 3 || foutCorrectie == 4)
                {
                    totaal = (subEen + subTwee + subDrie + subVier + subVijf) * opslagDrieVier;
                }
                else
                {
                    totaal = (subEen + subTwee + subDrie + subVier + subVijf);
                }

            }
            else if (aantalKinderen > 5)
            {
                if (foutCorrectie > 5)
                {
                    totaal = (subEen + subTwee + subDrie + subVier + subVijf + subZes + subZeven + subAcht + subNegen + subTien + subElf + subTwaalf + subDertien + subVeertien) * opslagZesMeer;
                }
                else if (foutCorrectie == 5)
                {
                    totaal = (subEen + subTwee + subDrie + subVier + subVijf + subZes + subZeven + subAcht + subNegen + subTien + subElf + subTwaalf + subDertien + subVeertien) * opslagVijf;
                }
                else if (foutCorrectie == 3 || foutCorrectie == 4)
                {
                    totaal = (subEen + subTwee + subDrie + subVier + subVijf + subZes + subZeven + subAcht + subNegen + subTien + subElf + subTwaalf + subDertien + subVeertien) * opslagDrieVier;
                }
                else
                {
                    totaal = (subEen + subTwee + subDrie + subVier + subVijf + subZes + subZeven + subAcht + subNegen + subTien + subElf + subTwaalf + subDertien + subVeertien);
                }
            }
            else
            {
                totaal = (subEen + subTwee + subDrie + subVier + subVijf + subZes + subZeven + subAcht + subNegen + subTien + subElf + subTwaalf + subDertien + subVeertien);
            }

            textBoxResult.Visible = true;
            textBoxResult.Text = "De kinderbijslag per kwartaal is: " + totaal.ToString("C");
        }

        public void kindEenLeeftijd()
        {
            datumBepaling();
            try
            {
                dagEen = Convert.ToInt16(cBoxKindEenDag.Text);
                maandEen = (cBoxKindEenMaand.SelectedIndex) + 1;
                jaarEen = Convert.ToInt16(cBoxKindEenJaar.Text);

                if (dagEen < 10)
                {
                    if (maandEen < 10)
                    {
                        datumKindEen = "0" + dagEen.ToString() + "0" + maandEen.ToString() + jaarEen.ToString();
                    }
                    else
                    {
                        datumKindEen = "0" + dagEen.ToString() + maandEen.ToString() + jaarEen.ToString();
                    }
                }
                else if (maandEen < 10)
                {
                    datumKindEen = dagEen.ToString() + "0" + maandEen.ToString() + jaarEen.ToString();
                }
                else
                {
                    datumKindEen = dagEen.ToString() + maandEen.ToString() + jaarEen.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindEen, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindEenDatum);

                if (kindEenDatum <= grensAchtienPlus)
                {
                    textBoxFoutEen.Visible = true;
                    textBoxFoutEen.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subEen = 0;
                    foutCorrectieTemp++;
                }
                else if (kindEenDatum > grensAchtienPlus && kindEenDatum < grensTwaalfAchtien)
                {
                    subEen = kbOuderTwaalf;
                    textBoxFoutEen.Text = "";
                    foutCorrectie = 1;
                }
                else if (kindEenDatum > grensTwaalfAchtien)
                {
                    subEen = kbJongerTwaalf;
                    textBoxFoutEen.Text = "";
                    foutCorrectie = 1;
                }
            }
            catch
            {

            }            
        }

        public void kindTweeLeeftijd()
        {
            datumBepaling();
            try
            {
                kindEenLeeftijd();
                dagTwee = Convert.ToInt16(cBoxKindTweeDag.Text);
                maandTwee = (cBoxKindTweeMaand.SelectedIndex) + 1;
                jaarTwee = Convert.ToInt16(cBoxKindTweeJaar.Text);

                if (dagTwee < 10)
                {
                    if (maandTwee < 10)
                    {
                        datumKindTwee = "0" + dagTwee.ToString() + "0" + maandTwee.ToString() + jaarTwee.ToString();
                    }
                    else
                    {
                        datumKindTwee = "0" + dagTwee.ToString() + maandTwee.ToString() + jaarTwee.ToString();
                    }
                }
                else if (maandTwee < 10)
                {
                    datumKindTwee = dagTwee.ToString() + "0" + maandTwee.ToString() + jaarTwee.ToString();
                }
                else
                {
                    datumKindTwee = dagTwee.ToString() + maandTwee.ToString() + jaarTwee.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindTwee, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindTweeDatum);

                if (kindTweeDatum <= grensAchtienPlus)
                {
                    textBoxFoutTwee.Visible = true;
                    textBoxFoutTwee.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subTwee = 0;
                    foutCorrectieTemp++;
                }
                else if (kindTweeDatum > grensAchtienPlus && kindTweeDatum < grensTwaalfAchtien)
                {
                    subTwee = kbOuderTwaalf;
                    textBoxFoutTwee.Text = "";
                }
                else if (kindTweeDatum > grensTwaalfAchtien)
                {
                    subTwee = kbJongerTwaalf;
                    textBoxFoutTwee.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindDrieLeeftijd()
        {
            datumBepaling();
            try
            {
                kindTweeLeeftijd();
                dagDrie = Convert.ToInt16(cBoxKindDrieDag.Text);
                maandDrie = (cBoxKindDrieMaand.SelectedIndex) + 1;
                jaarDrie = Convert.ToInt16(cBoxKindDrieJaar.Text);

                if (dagDrie < 10)
                {
                    if (maandDrie < 10)
                    {
                        datumKindDrie = "0" + dagDrie.ToString() + "0" + maandDrie.ToString() + jaarDrie.ToString();
                    }
                    else
                    {
                        datumKindDrie = "0" + dagDrie.ToString() + maandDrie.ToString() + jaarDrie.ToString();
                    }
                }
                else if (maandDrie < 10)
                {
                    datumKindDrie = dagDrie.ToString() + "0" + maandDrie.ToString() + jaarDrie.ToString();
                }
                else
                {
                    datumKindDrie = dagDrie.ToString() + maandDrie.ToString() + jaarDrie.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindDrie, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindDrieDatum);

                if (kindDrieDatum <= grensAchtienPlus)
                {
                    textBoxFoutDrie.Visible = true;
                    textBoxFoutDrie.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subDrie = 0;
                    foutCorrectieTemp++;
                }
                else if (kindDrieDatum > grensAchtienPlus && kindDrieDatum < grensTwaalfAchtien)
                {
                    subDrie = kbOuderTwaalf;
                    textBoxFoutDrie.Text = "";
                }
                else if (kindDrieDatum > grensTwaalfAchtien)
                {
                    subDrie = kbJongerTwaalf;
                    textBoxFoutDrie.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindVierLeeftijd()
        {
            datumBepaling();
            try
            {
                kindDrieLeeftijd();
                dagVier = Convert.ToInt16(cBoxKindVierDag.Text);
                maandVier = (cBoxKindVierMaand.SelectedIndex) + 1;
                jaarVier = Convert.ToInt16(cBoxKindVierJaar.Text);

                if (dagVier < 10)
                {
                    if (maandVier < 10)
                    {
                        datumKindVier = "0" + dagVier.ToString() + "0" + maandVier.ToString() + jaarVier.ToString();
                    }
                    else
                    {
                        datumKindVier = "0" + dagVier.ToString() + maandVier.ToString() + jaarVier.ToString();
                    }
                }
                else if (maandVier < 10)
                {
                    datumKindVier = dagVier.ToString() + "0" + maandVier.ToString() + jaarVier.ToString();
                }
                else
                {
                    datumKindVier = dagVier.ToString() + maandVier.ToString() + jaarVier.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindVier, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindVierDatum);

                if (kindVierDatum <= grensAchtienPlus)
                {
                    textBoxFoutVier.Visible = true;
                    textBoxFoutVier.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subVier = 0;
                    foutCorrectieTemp++;
                }
                else if (kindVierDatum > grensAchtienPlus && kindVierDatum < grensTwaalfAchtien)
                {
                    subVier = kbOuderTwaalf;
                    textBoxFoutVier.Text = "";
                }
                else if (kindVierDatum > grensTwaalfAchtien)
                {
                    subVier = kbJongerTwaalf;
                    textBoxFoutVier.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindVijfLeeftijd()
        {
            datumBepaling();
            try
            {
                kindVierLeeftijd();
                dagVijf = Convert.ToInt16(cBoxKindVijfDag.Text);
                maandVijf = (cBoxKindVijfMaand.SelectedIndex) + 1;
                jaarVijf = Convert.ToInt16(cBoxKindVijfJaar.Text);

                if (dagVijf < 10)
                {
                    if (maandVijf < 10)
                    {
                        datumKindVijf = "0" + dagVijf.ToString() + "0" + maandVijf.ToString() + jaarVijf.ToString();
                    }
                    else
                    {
                        datumKindVijf = "0" + dagVijf.ToString() + maandVijf.ToString() + jaarVijf.ToString();
                    }
                }
                else if (maandVijf < 10)
                {
                    datumKindVijf = dagVijf.ToString() + "0" + maandVijf.ToString() + jaarVijf.ToString();
                }
                else
                {
                    datumKindVijf = dagVijf.ToString() + maandVijf.ToString() + jaarVijf.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindVijf, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindVijfDatum);

                if (kindVijfDatum <= grensAchtienPlus)
                {
                    textBoxFoutVijf.Visible = true;
                    textBoxFoutVijf.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subVijf = 0;
                    foutCorrectieTemp++;
                }
                else if (kindVijfDatum > grensAchtienPlus && kindVijfDatum < grensTwaalfAchtien)
                {
                    subVijf = kbOuderTwaalf;
                    textBoxFoutVijf.Text = "";
                }
                else if (kindVijfDatum > grensTwaalfAchtien)
                {
                    subVijf = kbJongerTwaalf;
                    textBoxFoutVijf.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindZesLeeftijd()
        {
            datumBepaling();
            try
            {
                kindVijfLeeftijd();
                dagZes = Convert.ToInt16(cBoxKindZesDag.Text);
                maandZes = (cBoxKindZesMaand.SelectedIndex) + 1;
                jaarZes = Convert.ToInt16(cBoxKindZesJaar.Text);

                if (dagZes < 10)
                {
                    if (maandZes < 10)
                    {
                        datumKindZes = "0" + dagZes.ToString() + "0" + maandZes.ToString() + jaarZes.ToString();
                    }
                    else
                    {
                        datumKindZes = "0" + dagZes.ToString() + maandZes.ToString() + jaarZes.ToString();
                    }
                }
                else if (maandZes < 10)
                {
                    datumKindZes = dagZes.ToString() + "0" + maandZes.ToString() + jaarZes.ToString();
                }
                else
                {
                    datumKindZes = dagZes.ToString() + maandZes.ToString() + jaarZes.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindZes, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindZesDatum);

                if (kindZesDatum <= grensAchtienPlus)
                {
                    textBoxFoutZes.Visible = true;
                    textBoxFoutZes.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subZes = 0;
                    foutCorrectieTemp++;
                }
                else if (kindZesDatum > grensAchtienPlus && kindZesDatum < grensTwaalfAchtien)
                {
                    subZes = kbOuderTwaalf;
                    textBoxFoutZes.Text = "";
                }
                else if (kindZesDatum > grensTwaalfAchtien)
                {
                    subZes = kbJongerTwaalf;
                    textBoxFoutZes.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindZevenLeeftijd()
        {
            datumBepaling();
            try
            {
                kindZesLeeftijd();
                dagZeven = Convert.ToInt16(cBoxKindZevenDag.Text);
                maandZeven = (cBoxKindZevenMaand.SelectedIndex) + 1;
                jaarZeven = Convert.ToInt16(cBoxKindZevenJaar.Text);

                if (dagZeven < 10)
                {
                    if (maandZeven < 10)
                    {
                        datumKindZeven = "0" + dagZeven.ToString() + "0" + maandZeven.ToString() + jaarZeven.ToString();
                    }
                    else
                    {
                        datumKindZeven = "0" + dagZeven.ToString() + maandZeven.ToString() + jaarZeven.ToString();
                    }
                }
                else if (maandZeven < 10)
                {
                    datumKindZeven = dagZeven.ToString() + "0" + maandZeven.ToString() + jaarZeven.ToString();
                }
                else
                {
                    datumKindZeven = dagZeven.ToString() + maandZeven.ToString() + jaarZeven.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindZeven, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindZevenDatum);

                if (kindZevenDatum <= grensAchtienPlus)
                {
                    textBoxFoutZeven.Visible = true;
                    textBoxFoutZeven.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subZeven = 0;
                    foutCorrectieTemp++;
                }
                else if (kindZevenDatum > grensAchtienPlus && kindZevenDatum < grensTwaalfAchtien)
                {
                    subZeven = kbOuderTwaalf;
                    textBoxFoutZeven.Text = "";
                }
                else if (kindZevenDatum > grensTwaalfAchtien)
                {
                    subZeven = kbJongerTwaalf;
                    textBoxFoutZeven.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindAchtLeeftijd()
        {
            datumBepaling();
            try
            {
                kindZevenLeeftijd();
                dagAcht = Convert.ToInt16(cBoxKindAchtDag.Text);
                maandAcht = (cBoxKindAchtMaand.SelectedIndex) + 1;
                jaarAcht = Convert.ToInt16(cBoxKindAchtJaar.Text);

                if (dagAcht < 10)
                {
                    if (maandAcht < 10)
                    {
                        datumKindAcht = "0" + dagAcht.ToString() + "0" + maandAcht.ToString() + jaarAcht.ToString();
                    }
                    else
                    {
                        datumKindAcht = "0" + dagAcht.ToString() + maandAcht.ToString() + jaarAcht.ToString();
                    }
                }
                else if (maandAcht < 10)
                {
                    datumKindAcht = dagAcht.ToString() + "0" + maandAcht.ToString() + jaarAcht.ToString();
                }
                else
                {
                    datumKindAcht = dagAcht.ToString() + maandAcht.ToString() + jaarAcht.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindAcht, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindAchtDatum);

                if (kindAchtDatum <= grensAchtienPlus)
                {
                    textBoxFoutAcht.Visible = true;
                    textBoxFoutAcht.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subAcht = 0;
                    foutCorrectieTemp++;
                }
                else if (kindAchtDatum > grensAchtienPlus && kindAchtDatum < grensTwaalfAchtien)
                {
                    subAcht = kbOuderTwaalf;
                    textBoxFoutAcht.Text = "";
                }
                else if (kindAchtDatum > grensTwaalfAchtien)
                {
                    subAcht = kbJongerTwaalf;
                    textBoxFoutAcht.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindNegenLeeftijd()
        {
            datumBepaling();
            try
            {
                kindAchtLeeftijd();
                dagNegen = Convert.ToInt16(cBoxKindNegenDag.Text);
                maandNegen = (cBoxKindNegenMaand.SelectedIndex) + 1;
                jaarNegen = Convert.ToInt16(cBoxKindNegenJaar.Text);

                if (dagNegen < 10)
                {
                    if (maandNegen < 10)
                    {
                        datumKindNegen = "0" + dagNegen.ToString() + "0" + maandNegen.ToString() + jaarNegen.ToString();
                    }
                    else
                    {
                        datumKindNegen = "0" + dagNegen.ToString() + maandNegen.ToString() + jaarNegen.ToString();
                    }
                }
                else if (maandNegen < 10)
                {
                    datumKindNegen = dagNegen.ToString() + "0" + maandNegen.ToString() + jaarNegen.ToString();
                }
                else
                {
                    datumKindNegen = dagNegen.ToString() + maandNegen.ToString() + jaarNegen.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindNegen, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindNegenDatum);

                if (kindNegenDatum <= grensAchtienPlus)
                {
                    textBoxFoutNegen.Visible = true;
                    textBoxFoutNegen.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subNegen = 0;
                    foutCorrectieTemp++;
                }
                else if (kindNegenDatum > grensAchtienPlus && kindNegenDatum < grensTwaalfAchtien)
                {
                    subNegen = kbOuderTwaalf;
                    textBoxFoutNegen.Text = "";
                }
                else if (kindNegenDatum > grensTwaalfAchtien)
                {
                    subNegen = kbJongerTwaalf;
                    textBoxFoutNegen.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindTienLeeftijd()
        {
            datumBepaling();
            try
            {
                kindNegenLeeftijd();
                dagTien = Convert.ToInt16(cBoxKindTienDag.Text);
                maandTien = (cBoxKindTienMaand.SelectedIndex) + 1;
                jaarTien = Convert.ToInt16(cBoxKindTienJaar.Text);

                if (dagTien < 10)
                {
                    if (maandTien < 10)
                    {
                        datumKindTien = "0" + dagTien.ToString() + "0" + maandTien.ToString() + jaarTien.ToString();
                    }
                    else
                    {
                        datumKindTien = "0" + dagTien.ToString() + maandTien.ToString() + jaarTien.ToString();
                    }
                }
                else if (maandTien < 10)
                {
                    datumKindTien = dagTien.ToString() + "0" + maandTien.ToString() + jaarTien.ToString();
                }
                else
                {
                    datumKindTien = dagTien.ToString() + maandTien.ToString() + jaarTien.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindTien, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindTienDatum);

                if (kindTienDatum <= grensAchtienPlus)
                {
                    textBoxFoutTien.Visible = true;
                    textBoxFoutTien.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subTien = 0;
                    foutCorrectieTemp++;
                }
                else if (kindTienDatum > grensAchtienPlus && kindTienDatum < grensTwaalfAchtien)
                {
                    subTien = kbOuderTwaalf;
                    textBoxFoutTien.Text = "";
                }
                else if (kindTienDatum > grensTwaalfAchtien)
                {
                    subTien = kbJongerTwaalf;
                    textBoxFoutTien.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindElfLeeftijd()
        {
            datumBepaling();
            try
            {
                kindTienLeeftijd();
                dagElf = Convert.ToInt16(cBoxKindElfDag.Text);
                maandElf = (cBoxKindElfMaand.SelectedIndex) + 1;
                jaarElf = Convert.ToInt16(cBoxKindElfJaar.Text);

                if (dagElf < 10)
                {
                    if (maandElf < 10)
                    {
                        datumKindElf = "0" + dagElf.ToString() + "0" + maandElf.ToString() + jaarElf.ToString();
                    }
                    else
                    {
                        datumKindElf = "0" + dagElf.ToString() + maandElf.ToString() + jaarElf.ToString();
                    }
                }
                else if (maandElf < 10)
                {
                    datumKindElf = dagElf.ToString() + "0" + maandElf.ToString() + jaarElf.ToString();
                }
                else
                {
                    datumKindElf = dagElf.ToString() + maandElf.ToString() + jaarElf.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindElf, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindElfDatum);

                if (kindElfDatum <= grensAchtienPlus)
                {
                    textBoxFoutElf.Visible = true;
                    textBoxFoutElf.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subElf = 0;
                    foutCorrectieTemp++;
                }
                else if (kindElfDatum > grensAchtienPlus && kindElfDatum < grensTwaalfAchtien)
                {
                    subElf = kbOuderTwaalf;
                    textBoxFoutElf.Text = "";
                }
                else if (kindElfDatum > grensTwaalfAchtien)
                {
                    subElf = kbJongerTwaalf;
                    textBoxFoutElf.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindTwaalfLeeftijd()
        {
            datumBepaling();
            try
            {
                kindElfLeeftijd();
                dagTwaalf = Convert.ToInt16(cBoxKindTwaalfDag.Text);
                maandTwaalf = (cBoxKindTwaalfMaand.SelectedIndex) + 1;
                jaarTwaalf = Convert.ToInt16(cBoxKindTwaalfJaar.Text);

                if (dagTwaalf < 10)
                {
                    if (maandTwaalf < 10)
                    {
                        datumKindTwaalf = "0" + dagTwaalf.ToString() + "0" + maandTwaalf.ToString() + jaarTwaalf.ToString();
                    }
                    else
                    {
                        datumKindTwaalf = "0" + dagTwaalf.ToString() + maandTwaalf.ToString() + jaarTwaalf.ToString();
                    }
                }
                else if (maandTwaalf < 10)
                {
                    datumKindTwaalf = dagTwaalf.ToString() + "0" + maandTwaalf.ToString() + jaarTwaalf.ToString();
                }
                else
                {
                    datumKindTwaalf = dagTwaalf.ToString() + maandTwaalf.ToString() + jaarTwaalf.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindTwaalf, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindTwaalfDatum);

                if (kindTwaalfDatum <= grensAchtienPlus)
                {
                    textBoxFoutTwaalf.Visible = true;
                    textBoxFoutTwaalf.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subTwaalf = 0;
                    foutCorrectieTemp++;
                }
                else if (kindTwaalfDatum > grensAchtienPlus && kindTwaalfDatum < grensTwaalfAchtien)
                {
                    subTwaalf = kbOuderTwaalf;
                    textBoxFoutTwaalf.Text = "";
                }
                else if (kindTwaalfDatum > grensTwaalfAchtien)
                {
                    subTwaalf = kbJongerTwaalf;
                    textBoxFoutTwaalf.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindDertienLeeftijd()
        {
            datumBepaling();
            try
            {
                kindTwaalfLeeftijd();
                dagDertien = Convert.ToInt16(cBoxKindDertienDag.Text);
                maandDertien = (cBoxKindDertienMaand.SelectedIndex) + 1;
                jaarDertien = Convert.ToInt16(cBoxKindDertienJaar.Text);

                if (dagDertien < 10)
                {
                    if (maandDertien < 10)
                    {
                        datumKindDertien = "0" + dagDertien.ToString() + "0" + maandDertien.ToString() + jaarDertien.ToString();
                    }
                    else
                    {
                        datumKindDertien = "0" + dagDertien.ToString() + maandDertien.ToString() + jaarDertien.ToString();
                    }
                }
                else if (maandDertien < 10)
                {
                    datumKindDertien = dagDertien.ToString() + "0" + maandDertien.ToString() + jaarDertien.ToString();
                }
                else
                {
                    datumKindDertien = dagDertien.ToString() + maandDertien.ToString() + jaarDertien.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindDertien, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindDertienDatum);

                if (kindDertienDatum <= grensAchtienPlus)
                {
                    textBoxFoutDertien.Visible = true;
                    textBoxFoutDertien.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subDertien = 0;
                    foutCorrectieTemp++;
                }
                else if (kindDertienDatum > grensAchtienPlus && kindDertienDatum < grensTwaalfAchtien)
                {
                    subDertien = kbOuderTwaalf;
                    textBoxFoutDertien.Text = "";
                }
                else if (kindDertienDatum > grensTwaalfAchtien)
                {
                    subDertien = kbJongerTwaalf;
                    textBoxFoutDertien.Text = "";
                }
            }
            catch
            {

            }
        }

        public void kindVeertienLeeftijd()
        {
            datumBepaling();
            try
            {
                kindDertienLeeftijd();
                dagVeertien = Convert.ToInt16(cBoxKindVeertienDag.Text);
                maandVeertien = (cBoxKindVeertienMaand.SelectedIndex) + 1;
                jaarVeertien = Convert.ToInt16(cBoxKindVeertienJaar.Text);

                if (dagVeertien < 10)
                {
                    if (maandVeertien < 10)
                    {
                        datumKindVeertien = "0" + dagVeertien.ToString() + "0" + maandVeertien.ToString() + jaarVeertien.ToString();
                    }
                    else
                    {
                        datumKindVeertien = "0" + dagVeertien.ToString() + maandVeertien.ToString() + jaarVeertien.ToString();
                    }
                }
                else if (maandVeertien < 10)
                {
                    datumKindVeertien = dagVeertien.ToString() + "0" + maandVeertien.ToString() + jaarVeertien.ToString();
                }
                else
                {
                    datumKindVeertien = dagVeertien.ToString() + maandVeertien.ToString() + jaarVeertien.ToString();
                }

                string[] datumFormat = { "ddMMyyyy" };
                DateTime.TryParseExact(datumKindVeertien, datumFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out kindVeertienDatum);

                if (kindVeertienDatum <= grensAchtienPlus)
                {
                    textBoxFoutVeertien.Visible = true;
                    textBoxFoutVeertien.Text = "Dit kind is ouder dan 18 en geeft geen recht op kinderbijslag";
                    subVeertien = 0;
                    foutCorrectieTemp++;
                }
                else if (kindVeertienDatum > grensAchtienPlus && kindVeertienDatum < grensTwaalfAchtien)
                {
                    subVeertien = kbOuderTwaalf;
                    textBoxFoutVeertien.Text = "";
                }
                else if (kindVeertienDatum > grensTwaalfAchtien)
                {
                    subVeertien = kbJongerTwaalf;
                    textBoxFoutVeertien.Text = "";
                }
            }
            catch
            {

            }
        }

        public void datumBepaling()
        {
            nu = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            grensAchtienPlus = nu.AddYears(-18);
            grensTwaalfAchtien = nu.AddYears(-12);

            try
            {
                aantalKinderen = Convert.ToInt16(cBoxAantalKinderen.Text);
            }
            catch
            {
                textBoxFout.Text = "geef aantal kinderen op";
            }
        }

        private void defaultState()
        {
            subEen = 0; subTwee = 0; subDrie = 0; subVier = 0; subVijf = 0; subZes = 0; subZeven = 0; subAcht = 0; subNegen = 0; subTien = 0; subElf = 0; subTwaalf = 0; subDertien = 0; subVeertien = 0;

            foreach (var cb in Controls.OfType<ComboBox>())
            {
                if (Controls.OfType<ComboBox>() != cBoxAantalKinderen)
                {
                    //cb.SelectedIndex = 0;
                    cb.Enabled = false;
                    cb.Visible = false;
                }                
            }
            cBoxAantalKinderen.Visible = true;
            cBoxAantalKinderen.Enabled = true;            

            foreach (var txt in Controls.OfType<TextBox>())
            {
                txt.Visible = false;
            }
            textBoxWelcome.Visible = true;
            textBoxPeilDatum.Visible = true;
            textBoxAantalKinderen.Visible = true;
            textBoxPeilDatum.Text = "Peildatum: " + DateTime.Now.ToShortDateString();
        }

        private void kindEen()
        {
            defaultState();
            textBoxGebDatumEen.Visible = true;
            cBoxKindEenDag.Visible = true;
            cBoxKindEenMaand.Visible = true;
            cBoxKindEenJaar.Visible = true;
            cBoxKindEenDag.Enabled = true;
            cBoxKindEenMaand.Enabled = true;
            cBoxKindEenJaar.Enabled = true;
        }

        private void kindTwee()
        {
            defaultState();
            kindEen();
            textBoxGebDatumTwee.Visible = true;
            cBoxKindTweeDag.Visible = true;
            cBoxKindTweeMaand.Visible = true;
            cBoxKindTweeJaar.Visible = true;
            cBoxKindTweeDag.Enabled = true;
            cBoxKindTweeMaand.Enabled = true;
            cBoxKindTweeJaar.Enabled = true;
        }

        private void kindDrie()
        {
            defaultState();
            kindTwee();
            textBoxGebDatumDrie.Visible = true;
            cBoxKindDrieDag.Visible = true;
            cBoxKindDrieMaand.Visible = true;
            cBoxKindDrieJaar.Visible = true;
            cBoxKindDrieDag.Enabled = true;
            cBoxKindDrieMaand.Enabled = true;
            cBoxKindDrieJaar.Enabled = true;
        }

        private void kindVier()
        {
            defaultState();
            kindDrie();
            textBoxGebDatumVier.Visible = true;
            cBoxKindVierDag.Visible = true;
            cBoxKindVierMaand.Visible = true;
            cBoxKindVierJaar.Visible = true;
            cBoxKindVierDag.Enabled = true;
            cBoxKindVierMaand.Enabled = true;
            cBoxKindVierJaar.Enabled = true;
        }

        private void kindVijf()
        {
            defaultState();
            kindVier();
            textBoxGebDatumVijf.Visible = true;
            cBoxKindVijfDag.Visible = true;
            cBoxKindVijfMaand.Visible = true;
            cBoxKindVijfJaar.Visible = true;
            cBoxKindVijfDag.Enabled = true;
            cBoxKindVijfMaand.Enabled = true;
            cBoxKindVijfJaar.Enabled = true;
        }

        private void kindZes()
        {
            defaultState();
            kindVijf();
            textBoxGebDatumZes.Visible = true;
            cBoxKindZesDag.Visible = true;
            cBoxKindZesMaand.Visible = true;
            cBoxKindZesJaar.Visible = true;
            cBoxKindZesDag.Enabled = true;
            cBoxKindZesMaand.Enabled = true;
            cBoxKindZesJaar.Enabled = true;
        }

        private void kindZeven()
        {
            defaultState();
            kindZes();
            textBoxGebDatumZeven.Visible = true;
            cBoxKindZevenDag.Visible = true;
            cBoxKindZevenMaand.Visible = true;
            cBoxKindZevenJaar.Visible = true;
            cBoxKindZevenDag.Enabled = true;
            cBoxKindZevenMaand.Enabled = true;
            cBoxKindZevenJaar.Enabled = true;
        }

        private void kindAcht()
        {
            defaultState();
            kindZeven();
            textBoxGebDatumAcht.Visible = true;
            cBoxKindAchtDag.Visible = true;
            cBoxKindAchtMaand.Visible = true;
            cBoxKindAchtJaar.Visible = true;
            cBoxKindAchtDag.Enabled = true;
            cBoxKindAchtMaand.Enabled = true;
            cBoxKindAchtJaar.Enabled = true;
        }

        private void kindNegen()
        {
            defaultState();
            kindAcht();
            textBoxGebDatumNegen.Visible = true;
            cBoxKindNegenDag.Visible = true;
            cBoxKindNegenMaand.Visible = true;
            cBoxKindNegenJaar.Visible = true;
            cBoxKindNegenDag.Enabled = true;
            cBoxKindNegenMaand.Enabled = true;
            cBoxKindNegenJaar.Enabled = true;
        }

        private void kindTien()
        {
            defaultState();
            kindNegen();
            textBoxGebDatumTien.Visible = true;
            cBoxKindTienDag.Visible = true;
            cBoxKindTienMaand.Visible = true;
            cBoxKindTienJaar.Visible = true;
            cBoxKindTienDag.Enabled = true;
            cBoxKindTienMaand.Enabled = true;
            cBoxKindTienJaar.Enabled = true;
        }

        private void kindElf()
        {
            defaultState();
            kindTien();
            textBoxGebDatumElf.Visible = true;
            cBoxKindElfDag.Visible = true;
            cBoxKindElfMaand.Visible = true;
            cBoxKindElfJaar.Visible = true;
            cBoxKindElfDag.Enabled = true;
            cBoxKindElfMaand.Enabled = true;
            cBoxKindElfJaar.Enabled = true;
        }

        private void kindTwaalf()
        {
            defaultState();
            kindElf();
            textBoxGebDatumTwaalf.Visible = true;
            cBoxKindTwaalfDag.Visible = true;
            cBoxKindTwaalfMaand.Visible = true;
            cBoxKindTwaalfJaar.Visible = true;
            cBoxKindTwaalfDag.Enabled = true;
            cBoxKindTwaalfMaand.Enabled = true;
            cBoxKindTwaalfJaar.Enabled = true;
        }

        private void kindDertien()
        {
            defaultState();
            kindTwaalf();
            textBoxGebDatumDertien.Visible = true;
            cBoxKindDertienDag.Visible = true;
            cBoxKindDertienMaand.Visible = true;
            cBoxKindDertienJaar.Visible = true;
            cBoxKindDertienDag.Enabled = true;
            cBoxKindDertienMaand.Enabled = true;
            cBoxKindDertienJaar.Enabled = true;
        }

        private void kindVeertien()
        {
            defaultState();
            kindDertien();
            textBoxGebDatumVeertien.Visible = true;
            cBoxKindVeertienDag.Visible = true;
            cBoxKindVeertienMaand.Visible = true;
            cBoxKindVeertienJaar.Visible = true;
            cBoxKindVeertienDag.Enabled = true;
            cBoxKindVeertienMaand.Enabled = true;
            cBoxKindVeertienJaar.Enabled = true;
        }
    }
}