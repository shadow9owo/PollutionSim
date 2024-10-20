using System.Numerics;
using Photosynthesis;

class Simulation
{
    private static GameData.Basedata basedata = new GameData.Basedata();
    static int yearsuntilavillageiscreatednearby = 0;

    public static void fastforward() 
    {
        if (GameData.currentscene != Scene.playing) return;
        if (GameData.textureint.Count > 0) {GameData.textureint.Clear();}
        GameData.nearbypopulation = 0;
        for (int i = 0; i < GameData.bases.Count; i++)
        {
            GameData.Basedata baseData = GameData.bases[i];

            baseData.population += new Random().Next(1,Math.Clamp(2 * (int)baseData.basetype * baseData.population / 3,1,10));

            GameData.nearbypopulation = GameData.nearbypopulation + baseData.population;

            if (!Lakedetails.issecuredbyunesco) {
                if (Lakedetails.usedforhygine) {
                    Lakedetails.damagelvl += baseData.population / 4 / 100;
                }
                if (Lakedetails.usedformanifactioring) {
                    Lakedetails.damagelvl += baseData.population / 1.5f / 100;
                }
                Lakedetails.damagelvl += baseData.population / 3 / 100;
            }else {
                Lakedetails.damagelvl += baseData.population / 6 / 100;
            }
            if (Lakedetails.damagelvl >= 100) {
                GameData.currentscene = Scene.end;
                return;
            }

            baseData.exploredradious += baseData.population * (int)baseData.basetype;
            
            if (baseData.population < 50) {
                if (baseData.basetype != Basetype.village) {
                    
                }
                baseData.basetype = Basetype.village;
            }
            else if (baseData.population > 1500) {
                if (baseData.basetype != Basetype.city) {
                    Logger.sendmessage("vesnice byla vylepsena na mesto"); 
                }
                baseData.basetype = Basetype.city;
            }
            else if (baseData.population < 500000 && baseData.population > 10000) {
                if (baseData.basetype != Basetype.metrocity) {
                    Logger.sendmessage("mesto bylo vylepseno na metropoli"); 
                }
                baseData.basetype = Basetype.metrocity;
            }
            if (RenderUtils.distancecalc(baseData.cords,new Vector2(GameConfig.windowsize.X / 2,GameConfig.windowsize.Y / 2)) > baseData.exploredradious) {
                
            }else {
                if (!baseData.discovered) {
                    baseData.discovered = true;
                    Logger.sendmessage("jezero bylo objeveno vesnici c." + Math.Clamp(i,1,9999));
                    baseData.yearoflakediscovery = (int)GameData.year;
                }
            }
            if (baseData.discovered) {
                if (GameData.year >= 1945) {
                    if (Lakedetails.damagelvl > 50) {
                        if (new Random().Next(1,3) == 2 && !Lakedetails.issecuredbyunesco) {
                            Lakedetails.issecuredbyunesco = true;
                            Logger.sendmessage("jezero je od ted chraneno unescem");
                        }
                    }
                }
                if (GameData.year >= 1760 && !Lakedetails.usedformanifactioring) {
                    if (new Random().Next(1,20) == 10 && !Lakedetails.usedformanifactioring) {
                        Lakedetails.usedformanifactioring = true;
                        Logger.sendmessage("lidi se rozhodly pouzivat toto jezero na vyrobu produktu");
                    }
                }

                if (GameData.year >= 50 && !Lakedetails.usedforhygine) {
                    Lakedetails.usedforhygine = true;
                    Logger.sendmessage("lidi se rozhodli pouzivat toto jezero pro hygienu");
                }
            }
            GameData.bases[i] = baseData;
        }
        Logger.sendmessage("rok preskocen");

        GameData.year += 1;
        if (yearsuntilavillageiscreatednearby <= 0) {
            basedata.basetype = Basetype.village;
            basedata.cords = new Vector2(new Random().Next(-948,948),new Random().Next(-948,948));
            basedata.townname = GameData.townnames[new Random().Next(0,GameData.townnames.Length)];
            basedata.yearwhenwasfounded = (int)GameData.year;
            while (RenderUtils.distancecalc(basedata.cords,new Vector2(GameConfig.windowsize.X / 2,GameConfig.windowsize.Y / 2)) < 100)
            {
                basedata.cords = new Vector2(new Random().Next(-948,948),new Random().Next(-948,948));
            }
            basedata.population = 1;
            yearsuntilavillageiscreatednearby = new Random().Next(25,75);
            Logger.sendmessage("vesnice byla zalozena na " + basedata.cords.X * -1 + " X " + basedata.cords.Y * -1 + " Y ");
            GameData.bases.Add(basedata);
        }else {
            yearsuntilavillageiscreatednearby -= 1;
        }
        int totalofdaysthatitrained = new Random().Next(0,200); //i know what a great variable name
        Lakedetails.damagelvl = Math.Clamp(Lakedetails.damagelvl - (Lakedetails.damagelvl / 100 * totalofdaysthatitrained),0,100);

        if (totalofdaysthatitrained >= 150) 
        {
            GameConfig.renderrainparticles = true;
        }
        else 
        {
            GameConfig.renderrainparticles = false;
        }

        if (GameData.peoplepositions.Count > 0) 
        {
            GameData.peoplepositions.Clear();
        }
        for (int y = -1024; y < 1024; y++)
        {
            for (int x = -1024; x < 1024; x++)
            {
                if (new Random().Next(1,2048) == 1024) 
                {
                    if (GameData.peoplepositions.Count < GameData.nearbypopulation) {
                        if (!GameData.textureint.ContainsKey(new Vector2(x,y))) {
                            GameData.textureint[new Vector2(x,y)] = new Random().Next(0,Textures.people.Length);
                        }
                        foreach (var item in GameData.bases)
                        {
                            if (Vector2.Distance(item.cords,new Vector2(x,y)) < 100) {
                                GameData.peoplepositions.Add(new Vector2(x,y));
                                break;
                            }
                        }
                    }
                }
            }   
        }
        
        RenderUtils.confirmposition();
        
        return;
    }
}