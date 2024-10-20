using Raylib_cs;
using System.Numerics;

public enum Basetype
{
    village = 1,
    city,
    metrocity
}

public enum Scene
{
    loading,
    menu,
    playing,
    paused,
    end
}

public class Lakedetails
{
    public static bool usedformanifactioring {get;set;} = false;
    public static bool usedforhygine {get;set;} = false;
    public static bool issecuredbyunesco {get;set;} = false;
    public static float damagelvl {get;set;} = 0;

}

class GameData
{
    public static string[] townnames = {
        "Prague",
        "Brno",
        "Ostrava",
        "Plzen",
        "Liberec",
        "Olomouc",
        "Ceske Budejovice",
        "Hradec Kralove",
        "Pardubice",
        "Usti nad Labem",
        "Zlin",
        "Havirov",
        "Kladno",
        "Most",
        "Opava",
        "Frydek-Mistek",
        "Jihlava",
        "Teplice",
        "Karvina",
        "Karlovy Vary",
        "Chomutov",
        "Jablonec nad Nisou",
        "Mlada Boleslav",
        "Prostejov",
        "Prerov",
        "Ceska Lipa",
        "Trebic",
        "Tabor",
        "Znojmo",
        "Kolin",
        "Pribram",
        "Pisek",
        "Kromeriz",
        "Orlova",
        "Lisen",
        "Brevnov",
        "Vsetin"
    };
    public static Dictionary<Vector2,int> textureint = new Dictionary<Vector2, int>();
    public static List<Rectangle> notrenderable = new List<Rectangle>();
    public static bool dataloaded = false;
    public static List<Vector2> peoplepositions = new List<Vector2>();
    public static List<Vector2> grassposition = new List<Vector2>();
    public static List<Vector2> treepositions = new List<Vector2>();
    public static List<Vector2> rockpositions = new List<Vector2>();
    public struct Basedata
    {
        public bool discovered {get;set;}
        public int exploredradious {get;set;}
        public int population {get;set;}
        public Basetype basetype {get;set;}
        public Vector2 cords {get;set;}
        public int yearwhenwasfounded {get;set;}
        public int yearoflakediscovery {get;set;}
        public string townname {get;set;}
    }

    public struct Particle() {
        public Vector2 position;
        public float velocity;
        public Color color;
    }

    public static Vector2 position;
    public static Scene currentscene;
    public static Vector2 lakecords;
    public static List<Basedata> bases = new List<GameData.Basedata>();
    public static List<Particle> ParticleList = new List<Particle>();
    public static Dictionary<int,int> positionoffsets = new Dictionary<int, int>();
    public static int nearbypopulation;
    public static float year;
    public static readonly int maxparticles = 512;
}

class GameConfig 
{
    public static bool renderrainparticles = false;
    public static bool debug = false;
    public static bool shouldexit = false;
    public static Vector2 windowsize;
    public static float timescale = 1;
}