using Npgsql;

namespace Katsaka
{
    public class Culture
    {
        private string idculture;
        private string idparcelle;
        private double nbrtahony;
        private DateTime dateculture;
        private int etat = 0;

        public string getIdculture() {
            return this.idculture;
        }
        public string getIdparcelle() {
            return this.idparcelle;
        }
        public double getNbrtahony() {
            return this.nbrtahony;
        }
        public DateTime getDateculture() {
            return this.dateculture;
        }

        public int getEtat() {
            return this.etat;
        }

        public void setIdculture(string id) {
            this.idculture = id;
        }

        public void setIdparcelle(string id) {
            if(id == null) {
                throw new ArgumentException("Id parcelle null");
            }
            this.idparcelle = id;
        }

        public void setNbrtahony(double nbr) {
            if(nbr < 0) {
                throw new ArgumentException("Le nombre du tahon-katsaka doit etre positive");
            }
            this.nbrtahony = nbr;
        }

        public void setEtat(int et) {
            this.etat = et;
        }

        public void setDateculture(DateTime date) {
            this.dateculture = date;
        }

        public List<Culture> getAllCulture(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            List<Culture> cultures = new List<Culture>();
            string query = "select * from culture";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Culture culture = new Culture();
                        culture.idculture = reader.GetString(0);
                        culture.idparcelle = reader.GetString(1);
                        culture.nbrtahony = reader.GetDouble(2);
                        culture.dateculture = reader.GetDateTime(3);
                        culture.etat = reader.GetInt32(4);
                        cultures.Add(culture);
                    }
                }
            }
            return cultures;
        }

        public Culture getCultureById(NpgsqlConnection c, string id) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            string query = "select * from culture where idculture = '"+id+"'";
            Culture culture = new Culture();    
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        culture.idculture = reader.GetString(0);
                        culture.idparcelle = reader.GetString(1);
                        culture.nbrtahony = reader.GetDouble(2);
                        culture.dateculture = reader.GetDateTime(3);
                        culture.etat = reader.GetInt32(4);
                    }
                }
            }
            return culture;
        } 

        public static Culture getCultureParcelleEnCours(NpgsqlConnection c, string idparcelle) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            string query = "select * from culture where idparcelle = '"+idparcelle+"' and etat = 0";    
            Culture culture = new Culture();
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        culture.idculture = reader.GetString(0);
                        culture.idparcelle = reader.GetString(1);
                        culture.nbrtahony = reader.GetDouble(2);
                        culture.dateculture = reader.GetDateTime(3);
                        culture.etat = reader.GetInt32(4);
                    }
                }
            }
            return culture;
        } 

        public static Boolean checkCulture(NpgsqlConnection c, string idparcelle) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            string query = "select * from culture where idparcelle = '"+idparcelle+"' and etat = 1";    
            Culture culture = new Culture();
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        culture.idculture = reader.GetString(0);
                        culture.idparcelle = reader.GetString(1);
                        culture.nbrtahony = reader.GetDouble(2);
                        culture.dateculture = reader.GetDateTime(3);
                        culture.etat = reader.GetInt32(4);
                    }
                }
            }
            if(culture.getIdculture() != null) {
                return true;
            }
            return false;
        } 

        public void recolter(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            try
            {
                string query = "UPDATE culture SET etat = 1 WHERE idparcelle = '"+this.idparcelle+"' AND etat = 0";
                using (NpgsqlCommand command = new NpgsqlCommand(query, c))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Update Recolte réussie !");
            }
            catch (Exception ex)
            {
                // Gérer les exceptions de connexion ou de requête ici
                c.Close();
                throw new ArgumentException("Erreur lors de l'update du recolte : " + ex.Message);
            }
        }
        public void insert(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            Culture cultureEnCours = getCultureParcelleEnCours(c,this.idparcelle);
            Console.WriteLine(cultureEnCours.getIdculture());
            if(cultureEnCours.idculture != null) {
                throw new ArgumentException("Il y a déjà un culture en cours sur ce parcelle");
            }
            try
            {
                string query = "INSERT INTO culture (idparcelle, nbrtahony, dateculture, etat) VALUES ('"+this.idparcelle+"',"+this.nbrtahony+",'"+this.dateculture+"',"+this.etat+")";
                using (NpgsqlCommand command = new NpgsqlCommand(query, c))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Insertion nouveau Culture réussie !");
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