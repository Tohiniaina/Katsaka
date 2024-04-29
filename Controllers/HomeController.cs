using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Katsaka.Models;
using Npgsql;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace Katsaka.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Culture()
    {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        List<Parcelle> parcelles = Parcelle.getAllParcelle(c);
        c.Close();
        return View(parcelles);
    }

    public IActionResult Suivi()
    {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        List<Parcelle> parcelles = Parcelle.getAllParcelle(c);
        c.Close();
        return View(parcelles);
    }

    public IActionResult Recolte()
    {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        List<Parcelle> parcelles = Parcelle.getAllParcelle(c);
        c.Close();
        return View(parcelles);
    }

    public IActionResult VoirSuivi()
    {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        List<Parcelle> parcelles = Parcelle.getAllParcelle(c);
        c.Close();
        return View(parcelles);
    }
    
    public IActionResult DetailSuivi()
    {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        string idparcelle = Request.Form["parcelle"];
        Parcelle p = Parcelle.getParcelleById(c,idparcelle);
        List<Suivi> suivis = p.getAllSuiviParcelle(c);
        c.Close();
        return View(suivis);
    }

    public IActionResult ChoixTerrain()
    {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        List<Terrain> terrains = Terrain.getAllTerrain(c);
        c.Close();
        return View(terrains);
    }

    public IActionResult EcranAnomalie(string param)
    {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        Suivi suivi = new Suivi().getSuiviById(c,param); 
        List<Anomalie> listAno = suivi.getAnomalie(c);
        c.Close();
        return View(listAno);
    }

    public IActionResult Resultat(string param)
    {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        Recolte reco = new Recolte().getRecoById(c,param);
        Parcelle parcelle = Parcelle.getParcelleById(c,reco.getIdparcelle(c));
        Suivi last = parcelle.getLastSuivi(c);
        new Anomalie().checkAnomalieRecolte(last,reco,c);
        List<Anomalie> listAno = reco.getAnomalie(c);

        Terrain terrain = parcelle.getTerrain(c);
        List<Prevision> prev = terrain.getPrevision(c,reco.getIdrecolte());
        for (int i = 0; i < prev.Count; i++)
        {
            Console.WriteLine("Parcelle : "+prev[i].getIdparcelle());
            Console.WriteLine("Tolany : "+prev[i].getNbrtolany());
            Console.WriteLine("Poids Unitaire : "+prev[i].getPoidsunit());
            Console.WriteLine("Poids total : "+prev[i].getPoidstotal());
        }
        Console.WriteLine("Poids total du terrain : "+terrain.getPrevisionPoidsTerrain(c,reco.getIdrecolte()));
        var resultat = new Tuple<List<Anomalie>, List<Prevision>, double>(listAno,prev,terrain.getPrevisionPoidsTerrain(c,reco.getIdrecolte()));
        c.Close();
        return View(resultat);
    }

    public IActionResult Zezika()
    {
        return View();
    }

    public IActionResult AjoutZezika()
    {
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        List<Parcelle> parcelles = Parcelle.getAllParcelle(c);
        c.Close();
        MySqlConnection con = new SqlDB().ConnectMySQL();
        List<Zezika> allZezika = new Zezika().getAllZezika(con);
        con.Close();
        var resultat = new Tuple<List<Parcelle>, List<Zezika>>(parcelles,allZezika);
        return View(resultat);
    }

    public IActionResult Depense()
    {
        OleDbConnection c = new SqlDB().ConnectAccess();
        List<Depense> allDepense = new Depense().getAllDepense(c);
        c.Close();
        var resultat = new Tuple<List<Depense>, double>(allDepense,new Depense().getTotal());
        return View(resultat);
    }

    public IActionResult Anomalie()
    {   
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        Terrain terrain = new Terrain();
        terrain.setIdterrain("TER1");
        terrain.getAnomalies(c);
        List<Anomalie> anos = new Anomalie().getAllAnomalie(c);
        c.Close();
        return View(anos);
    }

    public IActionResult Rapport()
    {   
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        MySqlConnection sqlco = new SqlDB().ConnectMySQL();
        OleDbConnection oleco = new SqlDB().ConnectAccess();
        Parcelle parcelle = Parcelle.getParcelleById(c,"PAR1");

        Terrain terrain = parcelle.getTerrain(c);
        List<Rapport> rapport = terrain.getRapportRecolte(c,oleco,sqlco);
        List<Zezika> allZezika = new Zezika().getAllZezika(sqlco);

        List<Parcelle> rapportQualite = terrain.getParcelleByQualite(c);

        List<List<Rapport>> all = new  List<List<Rapport>>(); 
        
        for(int i=0; i<rapportQualite.Count; i++) {
            List<Rapport> rappo = rapportQualite[i].getPourcentageZezika(sqlco,c,oleco);
            all.Add(rappo);
        }

        oleco.Close();
        sqlco.Close();
        c.Close();
        
        var resultat = new Tuple<List<Rapport>, List<Zezika>, List<List<Rapport>>, List<Parcelle>, List<Rapport>>(rapport,allZezika,all,rapportQualite,rapport.OrderBy(Rapport => Rapport.getRapportprix()).ToList());
        return View(resultat);
    }

    public IActionResult Additif()
    {   
        NpgsqlConnection c = new SqlDB().ConnectPostgres();
        MySqlConnection sqlco = new SqlDB().ConnectMySQL();
        OleDbConnection oleco = new SqlDB().ConnectAccess();
        Parcelle parcelle = Parcelle.getParcelleById(c,"PAR1");

        Terrain terrain = parcelle.getTerrain(c);

        List<Parcelle> rapportQualite = terrain.getParcelleByQualite(c);

        List<List<Rapport>> all = new  List<List<Rapport>>(); 
        List<Zezika> allZezika = new Zezika().getAllZezika(sqlco);
        
        for(int i=0; i<rapportQualite.Count; i++) {
            List<Rapport> rappo = rapportQualite[i].checkAdditif(sqlco,c,oleco);
            all.Add(rappo);
        }

        oleco.Close();
        sqlco.Close();
        c.Close();
        
        var resultat = new Tuple<List<List<Rapport>>,List<Zezika>>(all,allZezika);
        return View(resultat);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
