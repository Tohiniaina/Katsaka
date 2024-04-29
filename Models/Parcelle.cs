using Npgsql;
using System.Data.OleDb;
using MySql.Data.MySqlClient;

namespace Katsaka
{
    public class Parcelle
    {
        private string idparcelle;
        private string nomparcelle;
        private string idterrain;
        private double mesure;
        
        public string getIdparcelle() {
            return this.idparcelle;
        }
        public string getNomparcelle() {
            return this.nomparcelle;
        }
        public string getIdterrain() {
            return this.idterrain;
        }
        public double getMesure() {
            return this.mesure;
        }

        public void setIdparcelle(string id) {
            this.idparcelle = id;
        }

        public void setNomparcelle(string nom) {
            if(nom == null) {
                throw new ArgumentException("Nom terrain null");
            }
            this.nomparcelle = nom;
        }

        public void setIdterrain(string id) {
            if(id == null) {
                throw new ArgumentException("Id terrain null");
            }
            this.idterrain = id;
        }

        public void setMesure(double mes) {
            if(mes <= 0) {
                throw new ArgumentException("La mesure du terrain doit etre supérieur à 0");
            }
            this.mesure = mes;
        }

        public static List<Parcelle> getAllParcelle(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            List<Parcelle> parcelles = new List<Parcelle>();
            string query = "select * from parcelle";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Parcelle parcelle = new Parcelle();
                        parcelle.idparcelle = reader.GetString(0);
                        parcelle.nomparcelle = reader.GetString(1);
                        parcelle.idterrain = reader.GetString(2);
                        parcelle.mesure = reader.GetDouble(3);
                        parcelles.Add(parcelle);
                    }
                }
            }
            return parcelles;
        }

        public static Parcelle getParcelleById(NpgsqlConnection c, string id) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            string query = "select * from parcelle where idparcelle = '"+id+"'";
            Parcelle parcelle = new Parcelle();
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        parcelle.idparcelle = reader.GetString(0);
                        parcelle.nomparcelle = reader.GetString(1);
                        parcelle.idterrain = reader.GetString(2);
                        parcelle.mesure = reader.GetDouble(3);
                    }
                }
            }
            return parcelle;
        } 

        public string getIdRespo(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            string idrespo = "";
            string query = "select idresponsable from responsable where idparcelle = '"+this.idparcelle+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idrespo = reader.GetString(0);
                    }
                }
            }
            return idrespo;
        }

        public Culture getCulture(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            string query = "select * from culture where idparcelle = '"+this.idparcelle+"'";
            Culture culture = new Culture();
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        culture.setIdculture(reader.GetString(0));
                        culture.setIdparcelle(reader.GetString(1));
                        culture.setNbrtahony(reader.GetDouble(2));
                        culture.setDateculture(reader.GetDateTime(3));
                        culture.setEtat(reader.GetInt32(4));
                    }
                }
            }
            return culture;
        }

        public Mpamboly getMpamboly(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            string query = "select idmpamboly from champ where idparcelle = '"+this.idparcelle+"'";
            Mpamboly mpamboly = new Mpamboly();
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mpamboly = new Mpamboly().getMpambolyById(c, reader.GetString(0));
                    }
                }
            }
            return mpamboly;
        }

        public List<Suivi> getAllSuiviParcelle(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            List<Suivi> suivis = new List<Suivi>();
            string query = "SELECT * FROM suiviparcelle WHERE idparcelle = '"+this.idparcelle+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Suivi suivi = new Suivi();
                        suivi.setIdsuivi(reader.GetString(0));
                        suivi.setIdresponsable(reader.GetString(1));
                        suivi.setDatesuivi(reader.GetDateTime(2));
                        suivi.setNbrtahony(reader.GetDouble(3));
                        suivi.setNbrtolany(reader.GetDouble(4));
                        suivi.setLongueur(reader.GetDouble(5));
                        suivi.setNiveauverrete(reader.GetInt32(6));
                        suivi.setCroissance(reader.GetDouble(7));
                        suivi.setSemaine(reader.GetInt32(8));
                        suivis.Add(suivi);
                    }
                }
            }
            return suivis;
        }

        public Suivi getSuiviParcelleByDate(NpgsqlConnection c, DateTime date) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            Suivi suivi = new Suivi();
            string query = "select * from suiviparcelle where datesuivi = '"+date+"' and idparcelle = '"+this.idparcelle+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suivi.setIdsuivi(reader.GetString(0));
                        suivi.setIdresponsable(reader.GetString(1));
                        suivi.setDatesuivi(reader.GetDateTime(2));
                        suivi.setNbrtahony(reader.GetDouble(3));
                        suivi.setNbrtolany(reader.GetDouble(4));
                        suivi.setLongueur(reader.GetDouble(5));
                        suivi.setNiveauverrete(reader.GetInt32(6));
                        suivi.setCroissance(reader.GetDouble(7));
                        suivi.setSemaine(reader.GetInt32(8));
                    }
                }
            }
            return suivi;
        }

        public DateTime getDateSuivi(NpgsqlConnection c) {
            Suivi lastsuivi = getLastSuivi(c);
            DateTime date = new DateTime();
            if(lastsuivi.getIdsuivi() != null) {
                date = lastsuivi.getDatesuivi();
                Console.WriteLine("Last"+date);
            } else {
                Culture culture = this.getCulture(c);
                date = culture.getDateculture();
                Console.WriteLine("Cult"+date);
            }
            return date.AddDays(15);
        }

        public Suivi getLastSuivi(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            Suivi suivi = new Suivi();
            string query = "select * from suiviparcelle where idparcelle = '"+this.idparcelle+"' order by datesuivi desc limit 1";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suivi.setIdsuivi(reader.GetString(0));
                        suivi.setIdresponsable(reader.GetString(1));
                        suivi.setDatesuivi(reader.GetDateTime(2));
                        suivi.setNbrtahony(reader.GetDouble(3));
                        suivi.setNbrtolany(reader.GetDouble(4));
                        suivi.setLongueur(reader.GetDouble(5));
                        suivi.setNiveauverrete(reader.GetInt32(6));
                        suivi.setCroissance(reader.GetDouble(7));
                        suivi.setSemaine(reader.GetInt32(8));
                    }
                }
            }
            return suivi;
        }

        public Suivi getLastSuiviAvant(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            Suivi suivi = new Suivi();
            string query = "select * from suiviparcelle where idparcelle = '"+this.idparcelle+"' order by datesuivi desc limit 1 OFFSET 1";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suivi.setIdsuivi(reader.GetString(0));
                        suivi.setIdresponsable(reader.GetString(1));
                        suivi.setDatesuivi(reader.GetDateTime(2));
                        suivi.setNbrtahony(reader.GetDouble(3));
                        suivi.setNbrtolany(reader.GetDouble(4));
                        suivi.setLongueur(reader.GetDouble(5));
                        suivi.setNiveauverrete(reader.GetInt32(6));
                        suivi.setCroissance(reader.GetDouble(7));
                        suivi.setSemaine(reader.GetInt32(8));
                    }
                }
            }
            return suivi;
        }

        public Terrain getTerrain(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            Terrain terrain = new Terrain();
            string idterrain = "";
            string query = "select idterrain from parcelle where idparcelle = '"+this.idparcelle+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idterrain = reader.GetString(0);
                    }
                }
            }
            terrain = Terrain.getTerrainById(c,idterrain);
            return terrain;
        } 

        public List<Depense> getDepenseParcelle(OleDbConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectAccess();
            }
            List<Depense> depParcelle = new List<Depense>();
            string query = "select * from depense where idparcelle = '"+this.idparcelle+"'";
            using (OleDbCommand command = new OleDbCommand(query, c))
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Depense depense = new Depense();
                        depense.setIddepense(reader.GetInt32(0));
                        depense.setIdzezika(reader.GetInt32(1));
                        depense.setIdparcelle(reader.GetString(2));
                        depense.setQuantite((double)reader.GetDecimal(3));
                        depense.setDate(reader.GetDateTime(4));
                        depParcelle.Add(depense);
                    }
                }
            }
            return depParcelle;
        }

        public Recolte getRecolte() {
            NpgsqlConnection c = new SqlDB().ConnectPostgres(); 
            Recolte recolte = Recolte.getRecoParcelle(c,this.idparcelle);
            c.Close();
            return recolte;
        }

        public List<Rapport> getPourcentageZezika(MySqlConnection sqlco,NpgsqlConnection c,OleDbConnection co) {
            List<Rapport> allRapport = new List<Rapport>();
            
            List<Zezika> allZezika = new Zezika().getAllZezika(sqlco);

            double vokatraRecolte = this.getRecolte().getPoidsrecolte();
            for(int j=0; j<allZezika.Count; j++) {
                List<Depense> dep = this.getDepenseParcelle(co);
                double sommeqtezezika = 0;
                for(int k=0; k<dep.Count; k++) {
                    sommeqtezezika+=dep[k].getQuantite();
                }

                double qtezezika = 0;
                List<Depense> depzezika = new Depense().getDepenseByIdZezika(co,allZezika[j].getIdzezika(),this.getIdparcelle());
                for(int k=0; k<depzezika.Count; k++) {
                    qtezezika+=depzezika[k].getQuantite();
                }

                double percent = 0;
                if(qtezezika != 0) {
                    percent = (qtezezika*100)/sommeqtezezika;
                }

                double vokatra = 0;
                if(percent!=0) {
                    vokatra = (percent*vokatraRecolte)/100;
                }
                
                Rapport rapport = new Rapport();
                double totalvokatra = 0;
                rapport.setIdzezika(allZezika[j].getIdzezika());
                rapport.setQuantite(qtezezika);
                rapport.setVokatra(vokatra);
                rapport.setPourcentagezezika(percent);
                allRapport.Add(rapport);
            }
            
            return allRapport;
        }

        public Additif getAdditif(MySqlConnection sqlco,int idzezika,double percent) {
            string pourcent = percent.ToString().Replace(",",".");
            string query = "select * from detailsadditif where idparcelle = '"+this.idparcelle+"' and idzezika = "+idzezika+" and percentmin<="+pourcent+" and "+pourcent+"<=percentmax";
            Additif additif = new Additif();
            using (MySqlCommand command = new MySqlCommand(query, sqlco))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        additif.idajoutadditif = reader.GetInt32(0);
                        additif.idzezika = reader.GetInt32(1);
                        additif.idparcelle = reader.GetString(2);
                        additif.percentmin = reader.GetDouble(3);
                        additif.percentmax = reader.GetDouble(4);
                        additif.variation = reader.GetDouble(5);
                        Console.WriteLine("Variation : "+additif.variation+";"+reader.GetDouble(5));
                    }
                }
            }
            return additif;
        }

        public List<Rapport> checkAdditif(MySqlConnection sqlco,NpgsqlConnection c,OleDbConnection co) {
            List<Rapport> allRapport = this.getPourcentageZezika(sqlco,c,co);
            for(int i = 0; i<allRapport.Count; i++) {
                Recolte reco = this.getRecolte();
                
                double vokatra = (reco.getPoidsrecolte()*allRapport[i].getPourcentagezezika()/100);
                Additif additif = this.getAdditif(sqlco,allRapport[i].getIdzezika(),allRapport[i].getPourcentagezezika());
                double vo = 0;
                Console.WriteLine("Vokatra Avant : "+vokatra);
                Console.WriteLine("Variation : "+additif.variation);
                if(additif.variation>0) {
                    vo = vokatra+((vokatra*additif.variation)/100);
                } else {
                    vo = vokatra-((vokatra*(-additif.variation))/100);
                }
                Console.WriteLine("Vokatra Apres : "+vo);
                allRapport[i].setVokatra(vo);
            }
            return allRapport;
        }

        public void insert(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            try
            {
                string query = "INSERT INTO parcelle (nomparcelle, idterrain, mesure) VALUES ('"+this.nomparcelle+"','"+this.idterrain+"',"+this.mesure+")";
                using (NpgsqlCommand command = new NpgsqlCommand(query, c))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Insertion nouveau Parcelle réussie !");
            }
            catch (Exception ex)
            {
                // Gérer les exceptions de connexion ou de requête ici
                c.Close();
                throw new ArgumentException("Erreur lors de l'insertion du parcelle : " + ex.Message);
            }
        }
    }
}