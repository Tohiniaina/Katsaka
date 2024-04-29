using Npgsql;

namespace Katsaka
{
    public class Prevision
    {
        private string idparcelle;
        private double nbrtolany;
        private double longueur;
        private double poidsunit;
        private double poidstotal;
        
        public string getIdparcelle() {
            return this.idparcelle;
        }
        public double getNbrtolany() {
            return this.nbrtolany;
        }
        public double getLongueur() {
            return this.longueur;
        }
        public double getPoidsunit() {
            return this.poidsunit;
        }
        public double getPoidstotal() {
            return this.poidstotal;
        }

        public static Prevision getPrevisionParcelle(NpgsqlConnection c, string idrecolte, Parcelle parcelle) {
            Prevision prevision = new Prevision();
            Recolte reco = new Recolte().getRecoById(c,idrecolte);
            Suivi lastSuivi = parcelle.getLastSuivi(c);

            double poidsunit = (lastSuivi.getLongueur()*reco.getPoidsunit())/reco.getLongueur();
            double nbrtolany = lastSuivi.getNbrtolany()*lastSuivi.getNbrtahony();
            double poidstotal = poidsunit*nbrtolany;
            double longueur = lastSuivi.getLongueur();

            if(Culture.checkCulture(c,parcelle.getIdparcelle()) == true) {
                Recolte recpar = Recolte.getRecoParcelle(c,parcelle.getIdparcelle()); 
                poidsunit = recpar.getPoidsunit();
                nbrtolany = recpar.getNbrtolany();
                poidstotal = recpar.getPoidsrecolte();
                longueur = recpar.getLongueur();
            }

            prevision.idparcelle = parcelle.getIdparcelle();
            prevision.nbrtolany = nbrtolany;
            prevision.longueur = longueur;
            prevision.poidsunit = poidsunit;
            prevision.poidstotal = poidstotal;

            return prevision;
        }
    }
}