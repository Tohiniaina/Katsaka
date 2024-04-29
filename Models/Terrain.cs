using System.Data.OleDb;
using MySql.Data.MySqlClient;
using Npgsql;

namespace Katsaka
{
    public class Terrain
    {
        private string idterrain;

        private double mesure;
        
        public void setIdterrain(string id) {
            this.idterrain = id;
        }
        public string getIdterrain() {
            return this.idterrain;
        }
        public double getMesure() {
            return this.mesure;
        }

        public static List<Terrain> getAllTerrain(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            List<Terrain> terrains = new List<Terrain>();
            string query = "select * from terrain";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Terrain terrain = new Terrain();
                        terrain.idterrain = reader.GetString(0);
                        terrain.mesure = reader.GetDouble(1);
                        terrains.Add(terrain);
                    }
                }
            }
            return terrains;
        }

        public List<Parcelle> getParcelleTerrain(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            List<Parcelle> parcelles = new List<Parcelle>();
            string query = "select * from parcelle where idterrain = '"+this.idterrain+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Parcelle parcelle = new Parcelle();
                        parcelle.setIdparcelle(reader.GetString(0));
                        parcelle.setNomparcelle(reader.GetString(1));
                        parcelle.setIdterrain(reader.GetString(2));
                        parcelle.setMesure(reader.GetDouble(3));
                        parcelles.Add(parcelle);
                    }
                }
            }
            return parcelles;
        }

        public static Terrain getTerrainById(NpgsqlConnection c, string id) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            Terrain terrain = new Terrain();
            string query = "select * from terrain where idterrain = '"+id+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        terrain.idterrain = reader.GetString(0);
                        terrain.mesure = reader.GetDouble(1);
                    }
                }
            }
            return terrain;
        } 

        public void getAnomalies(NpgsqlConnection c) {
            List<Parcelle> parcelle = this.getParcelleTerrain(c);
            List<Anomalie> anos = new List<Anomalie>();
            for (int i = 0; i < parcelle.Count; i++)
            {
                Suivi suivi1 = parcelle[i].getLastSuiviAvant(c);
                Suivi suivi2 = parcelle[i].getLastSuivi(c);
                if(suivi1.getNbrtahony() > suivi2.getNbrtahony()) {
                    Anomalie ano = new Anomalie();
                    ano.setIdsuivi(suivi2.getIdsuivi());
                    ano.setIdtypeanomalie("TYA1");
                    ano.insert(c);
                }
                if(suivi1.getNbrtolany() > suivi2.getNbrtolany()) {
                    Anomalie ano = new Anomalie();
                    ano.setIdsuivi(suivi2.getIdsuivi());
                    ano.setIdtypeanomalie("TYA2");
                    ano.insert(c);
                }
                if(Anomalie.getMoyenneCroissance(c,suivi2.getIdparcelle()) > suivi2.getCroissance()) {
                    Anomalie ano = new Anomalie();
                    ano.setIdsuivi(suivi2.getIdsuivi());
                    ano.setIdtypeanomalie("TYA3");
                    ano.insert(c);
                }
                if(suivi1.getNiveauverrete() > suivi2.getNiveauverrete()) {
                    Anomalie ano = new Anomalie();
                    ano.setIdsuivi(suivi2.getIdsuivi());
                    ano.setIdtypeanomalie("TYA4");
                    ano.insert(c);
                }   
            }
        }

        public List<Prevision> getPrevision(NpgsqlConnection c, string idrecolte) {
            List<Prevision> previsions = new List<Prevision>();
            List<Parcelle> listParcelle = getParcelleTerrain(c);

            for (int i = 0; i < listParcelle.Count; i++)
            {   
                Parcelle parcelle = listParcelle[i];
                Prevision pre = Prevision.getPrevisionParcelle(c, idrecolte, parcelle);
                previsions.Add(pre);
            }

            return previsions;
        }

        public double getPrevisionPoidsTerrain(NpgsqlConnection c, string idrecolte) {
            double poids = 0;
            List<Prevision> previsions = getPrevision(c, idrecolte);
            for (int i = 0; i < previsions.Count; i++)
            {
                poids+=previsions[i].getPoidstotal();
            }
            return poids;
        }

        public List<Rapport> getRapportRecolte(NpgsqlConnection c,OleDbConnection co,MySqlConnection sqlco) {
            List<Parcelle> parcelles = this.getParcelleTerrain(c);
            List<Rapport> allRapport = new List<Rapport>();
            List<Zezika> allZezika = new Zezika().getAllZezika(sqlco);
            
            for(int j=0; j<allZezika.Count; j++) {
                Rapport rapport = new Rapport();
                double qteutilise = 0;
                double totalvokatra = 0;
                rapport.setIdzezika(allZezika[j].getIdzezika());
                for(int i=0; i<parcelles.Count; i++) {
                    List<Depense> dep = parcelles[i].getDepenseParcelle(co);
                    double sommeqtezezika = 0;
                    for(int k=0; k<dep.Count; k++) {
                        sommeqtezezika+=dep[k].getQuantite();
                    }
                    double qtezezika = 0;
                    List<Depense> depzezika = new Depense().getDepenseByIdZezika(co,allZezika[j].getIdzezika(),parcelles[i].getIdparcelle());
                    for(int k=0; k<depzezika.Count; k++) {
                        qtezezika+=depzezika[k].getQuantite();
                    }
                    
                    double percent = 0;
                    if(qtezezika != 0) {
                        percent = (qtezezika*100)/sommeqtezezika;
                    }
                    double vokatra = 0;
                    if(percent!=0) {
                        vokatra = (percent*parcelles[i].getRecolte().getPoidsrecolte())/100;
                    }
                    totalvokatra+=vokatra;
                    qteutilise+=qtezezika;
                }
                rapport.setQuantite(qteutilise);
                rapport.setVokatra(totalvokatra);
                allRapport.Add(rapport);
            }
            return allRapport.OrderByDescending(Rapport => Rapport.getRapport()).ToList();
        }

        public List<Parcelle> getParcelleByQualite(NpgsqlConnection c) {
            List<Parcelle> parcelles = this.getParcelleTerrain(c);  
            var parcelletrier = parcelles.OrderByDescending(Parcelle => Parcelle.getRecolte().getPoidsunit()).ToList();

            return parcelletrier;
        }

        public void insert(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            try
            {
                string query = "INSERT INTO terrain (mesure) VALUES ("+this.mesure+")";
                using (NpgsqlCommand command = new NpgsqlCommand(query, c))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Insertion nouveau Terrain réussie !");
            }
            catch (Exception ex)
            {
                // Gérer les exceptions de connexion ou de requête ici
                c.Close();
                throw new ArgumentException("Erreur lors de l'insertion des données : " + ex.Message);
            }
        }
    }
}