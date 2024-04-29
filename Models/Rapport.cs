using Npgsql;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace Katsaka
{
    public class Rapport
    {
        private int idzezika;
        private double pourcentagezezika;
        private double quantite;
        private double vokatra;
        private double rapport;
        private double rapportPrix;
        private double prixmoyenne;

        public int getIdzezika() {
            return this.idzezika;
        }
        public double getPourcentagezezika() {
            return this.pourcentagezezika;
        }
        public Zezika getZezika() {
            MySqlConnection c = new SqlDB().ConnectMySQL();
            Zezika zezika = Zezika.getZezikaById(c,this.idzezika);
            c.Close();
            return zezika;
        }
        public double getPrixmoyenne() {
            OleDbConnection c = new SqlDB().ConnectAccess();
            return Depense.getPrixMoyenneZezika(c,this.idzezika);
            c.Close();
        }
        public double getQuantite() {
            return this.quantite;
        }
        public double getVokatra() {
            return this.vokatra;
        }
        public double getRapport() {
            if(vokatra != 0)
                return this.vokatra/this.quantite;
            
            return 0;
        }
        public double getRapportprix() {
            OleDbConnection oleco = new SqlDB().ConnectAccess();
            return (this.vokatra/(this.getVokatra()*Depense.getPrixMoyenneZezika(oleco,this.idzezika)));
            oleco.Close();
        }
        
        public void setIdzezika(int idzezika) {
            if(idzezika == null) {
                throw new ArgumentException("Id zezika null");
            }
            this.idzezika = idzezika;
        }
        public void setPourcentagezezika(double percent) {
            this.pourcentagezezika = percent;
        }
        public void setQuantite(double qte) {
            if(qte < 0) {
                throw new ArgumentException("Quantite invalide");
            }
            this.quantite = qte;
        }
        public void setVokatra(double vokatra) {
            // if(vokatra < 0) {
            //     throw new ArgumentException("Vokatra invalide");
            // }
            this.vokatra = vokatra;
        }
    }
}