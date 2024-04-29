using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Katsaka.Models;
using Npgsql;
using System.Data.OleDb;

namespace Katsaka.Controllers;

public class InsertionController : Controller
{
    private readonly ILogger<InsertionController> _logger;

    public InsertionController(ILogger<InsertionController> logger)
    {
        _logger = logger;
    }

    public IActionResult InsertSuivi()
    {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();

        string idparcelle = Request.Form["parcelle"];
        double nbr_tahony = Double.Parse(Request.Form["nbr_tahony"]);
        double nbr_tolany = Double.Parse(Request.Form["nbr_tolany"]);
        double long_tolany = Double.Parse(Request.Form["long_tolany"]);
        int verrete = int.Parse(Request.Form["verrete"]);

        Parcelle parcelle = new Parcelle();
        parcelle.setIdparcelle(idparcelle);

        Suivi avantdernier = parcelle.getLastSuivi(c);
        int semaine = avantdernier.getSemaine();
        double croissance = 0;
        Console.WriteLine("Premier suivi : "+avantdernier.getIdparcelle);
        if(avantdernier.getIdparcelle() == null) {
            croissance = long_tolany;
            Console.WriteLine("Premier suivi");
            semaine = 1;
        }
        croissance = long_tolany-avantdernier.getLongueur();

        Suivi suivi = new Suivi();
        suivi.setIdparcelle(idparcelle);
        suivi.setNbrtahony(nbr_tahony);
        suivi.setNbrtolany(nbr_tolany);
        suivi.setLongueur(long_tolany);
        suivi.setNiveauverrete(verrete);
        suivi.setDatesuivi(parcelle.getDateSuivi(c));
        suivi.setCroissance(croissance);
        suivi.setSemaine(semaine);
        suivi.insert(c);

        Suivi last = parcelle.getLastSuivi(c);
        // Anomalie.checkAnomalie(avantdernier,last);
        // List<Anomalie> listAno = last.getAnomalie();
        // if(listAno.Count > 0) {
        //     return RedirectToAction("EcranAnomalie","Home", new {param = last.getIdsuivi()});    
        // }

        c.Close();
        return RedirectToAction("Index","Home");
    }

    public IActionResult InsertCulture()
    {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        string idparcelle = Request.Form["parcelle"];
        double nbr_tahony = Double.Parse(Request.Form["nbrtahony"]);
        DateTime date_culture = DateTime.Parse(Request.Form["date_culture"]);
        Culture culture = new Culture();
        culture.setIdparcelle(idparcelle);
        culture.setNbrtahony(nbr_tahony);
        culture.setDateculture(date_culture);
        culture.insert(c);
        c.Close();
        return RedirectToAction("Index","Home");
    }

    public IActionResult InsertRecolte() {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        string idparcelle = Request.Form["parcelle"];
        double nbrtolany = Double.Parse(Request.Form["nbrtolany"]);
        double poids = Double.Parse(Request.Form["poids"]);
        double longueur = Double.Parse(Request.Form["longueur"]);
        Parcelle parcelle = Parcelle.getParcelleById(c,idparcelle);
        string idrespo = parcelle.getIdRespo(c);
        DateTime datereco = parcelle.getLastSuivi(c).getDatesuivi().AddDays(15);

        Recolte reco = new Recolte();
        reco.setIdresponsable(idrespo);
        reco.setNbrtolany(nbrtolany);
        reco.setLongueur(longueur);
        reco.setPoidsrecolte(poids);
        reco.setDaterecolte(datereco);

        reco.insert(c);
        Culture culture = Culture.getCultureParcelleEnCours(c,idparcelle);
        culture.recolter(c);
        string id = Recolte.getRecoParcelle(c,idparcelle).getIdrecolte();
        c.Close();
        return RedirectToAction("Resultat","Home", new {param = id});
    }

    public void AjouterZezika()
    {
        OleDbConnection c = new SqlDB().ConnectAccess();
        string idparcelle = Request.Form["parcelle"];
        int idzezika = int.Parse(Request.Form["zezika"]);
        double qtezezika = double.Parse(Request.Form["qtezezika"]);
        double prixunitaire = double.Parse(Request.Form["prixunitaire"]);
        DateTime daty = DateTime.Parse(Request.Form["daty"]);
        Depense depense = new Depense();
        depense.setIdzezika(idzezika);
        depense.setIdparcelle(idparcelle);
        depense.setQuantite(qtezezika);
        depense.setPrixunitaire(prixunitaire);
        depense.setDate(daty);
        depense.insert(c);
        c.Close();
        // return RedirectToAction("Index","Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
