using System.Numerics;
using Photosynthesis;
using Raylib_cs;

class RenderUtils
{
    public static void importpositions() {
        Logger.sendmessage("generuji pozice kamenu na mape");
        for (int y = -2048; y < 2048; y++)
        {
            for (int x = -2048; x < 2048; x++)
            {
                if (new Random().Next(1,1024) == 512) 
                {
                    if (GameData.bases.ToArray().Length > 0) {
                        foreach (var item in GameData.bases.ToArray())
                        {
                            if (Vector2.Distance(item.cords,new Vector2(x,y)) >= 100 && Vector2.Distance(item.cords,GameData.lakecords + GameData.position) >= 100) {
                                GameData.rockpositions.Add(new Vector2(x,y));
                            }
                        }
                    }else {
                        GameData.rockpositions.Add(new Vector2(x,y));
                    }
                    //Console.WriteLine(x + " " + y);
                }
            }   
        }

        Logger.sendmessage("generuji pozice stromu na mape");
        for (int y = -2048; y < 2048; y++)
        {
            for (int x = -2048; x < 2048; x++)
            {
                if (new Random().Next(1,1024) == 512) 
                {
                    if (GameData.bases.ToArray().Length > 0) {
                        foreach (var item in GameData.bases.ToArray())
                        {
                            if (Vector2.Distance(item.cords,new Vector2(x,y)) >= 100 && Vector2.Distance(item.cords,GameData.lakecords + GameData.position) >= 100) {
                                GameData.treepositions.Add(new Vector2(x,y));
                            }
                        }
                    }else {
                        GameData.treepositions.Add(new Vector2(x,y));
                    }
                    //Console.WriteLine(x + " " + y);
                }
            }   
        }

        Logger.sendmessage("generuji pozice travy na mape");
        for (int y = -2048; y < 2048; y++)
        {
            for (int x = -2048; x < 2048; x++)
            {
                if (new Random().Next(1,1024) == 512) 
                {
                    if (GameData.bases.ToArray().Length > 0) {
                        foreach (var item in GameData.bases.ToArray())
                        {
                            if (Vector2.Distance(item.cords,new Vector2(x,y)) >= 100 && Vector2.Distance(item.cords,GameData.lakecords + GameData.position) >= 100) {
                                GameData.grassposition.Add(new Vector2(x,y));
                            }
                        }
                    }
                    else {
                        GameData.grassposition.Add(new Vector2(x,y));
                    }
                    //Console.WriteLine(x + " " + y);
                }
            }   
        }

        Logger.sendmessage("potvrzuji pozice");
        RenderUtils.confirmposition();

        Logger.sendmessage("nacteno");
        Raylib.WaitTime(2);
        Logger.clearlogger();
        GameData.dataloaded = true;
        GameData.currentscene = Scene.menu;
        return;
    }

    public static Texture2D[] LoadPeople() {
        List<Texture2D> texture2Ds = new List<Texture2D>();

        texture2Ds.Add(Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\person1.png"));
        texture2Ds.Add(Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\person2.png"));
        texture2Ds.Add(Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\person3.png"));
        texture2Ds.Add(Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\person4.png"));

        return texture2Ds.ToArray();
    }

    public static void confirmposition() {
        foreach (var item in GameData.bases)
        {
            var rocksToRemove = new List<Vector2>();
            foreach (var item1 in GameData.rockpositions)
            {
                if (Vector2.Distance(item.cords, item1) < 172 || Vector2.Distance(item.cords, GameData.lakecords + GameData.position) < 172)
                {
                    rocksToRemove.Add(item1);
                }
            }
            GameData.rockpositions.RemoveAll(rock => rocksToRemove.Contains(rock));
        
            var treesToRemove = new List<Vector2>();
            foreach (var item1 in GameData.treepositions)
            {
                if (Vector2.Distance(item.cords, item1) < 172 || Vector2.Distance(item.cords, GameData.lakecords + GameData.position) < 172)
                {
                    treesToRemove.Add(item1);
                }
            }
            GameData.treepositions.RemoveAll(tree => treesToRemove.Contains(tree));
        
            var grassToRemove = new List<Vector2>();
            foreach (var item1 in GameData.grassposition)
            {
                if (Vector2.Distance(item.cords, item1) < 172 || Vector2.Distance(item.cords, GameData.lakecords + GameData.position) < 172)
                {
                    grassToRemove.Add(item1);
                }
            }
            GameData.grassposition.RemoveAll(grass => grassToRemove.Contains(grass));
        }

        var rocksToRemoveForLake = new List<Vector2>();
        foreach (var item1 in GameData.rockpositions)
        {
            if (Vector2.Distance(GameData.lakecords, item1) < 172)
            {
                rocksToRemoveForLake.Add(item1);
            }
        }
        GameData.rockpositions.RemoveAll(rock => rocksToRemoveForLake.Contains(rock));
        
        var treesToRemoveForLake = new List<Vector2>();
        foreach (var item1 in GameData.treepositions)
        {
            if (Vector2.Distance(GameData.lakecords, item1) < 172)
            {
                treesToRemoveForLake.Add(item1);
            }
        }
        GameData.treepositions.RemoveAll(tree => treesToRemoveForLake.Contains(tree));

        var grassToRemoveForLake = new List<Vector2>();
        foreach (var item1 in GameData.grassposition)
        {
            if (Vector2.Distance(GameData.lakecords, item1) < 172)
            {
                grassToRemoveForLake.Add(item1);
            }
        }
        GameData.grassposition.RemoveAll(grass => grassToRemoveForLake.Contains(grass));

        return;
    }
    
    public static void rendermap() {


        for (int i = 0; i < GameData.bases.Count; i++)
        {
            GameData.Basedata baseData = GameData.bases[i];

            if (baseData.discovered) {
                Raylib.DrawLineEx(new Vector2(baseData.cords.X + GameData.position.X, baseData.cords.Y + GameData.position.Y), new Vector2((int)GameConfig.windowsize.X / 2 + (int)GameData.position.X,(int)GameConfig.windowsize.Y / 2 + (int)GameData.position.Y), 12, new Color(127, 106, 79,255));
            }

            switch (baseData.basetype)
            {
                case Basetype.village:
                    Raylib.DrawTexturePro(
                        Textures.house, 
                        new Rectangle(0,0 , Textures.house.Width, Textures.house.Height), 
                        new Rectangle(new Vector2(baseData.cords.X + GameData.position.X - Textures.house.Width /2, baseData.cords.Y + GameData.position.Y - Textures.house.Height /2), Textures.house.Width * 4, Textures.house.Height * 4),
                        Vector2.Zero, 
                        0, 
                        Color.White
                    );
                    break;
                case Basetype.city:
                    Raylib.DrawTexturePro(
                        Textures.city, 
                        new Rectangle(0,0 , Textures.city.Width, Textures.city.Height), 
                        new Rectangle(new Vector2(baseData.cords.X + GameData.position.X - Textures.city.Width /2, baseData.cords.Y + GameData.position.Y - Textures.city.Height /2), Textures.city.Width * 4, Textures.city.Height * 4),
                        Vector2.Zero, 
                        0, 
                        Color.White
                    );
                    break;
                case Basetype.metrocity:
                    Raylib.DrawTexturePro(
                        Textures.metrocity, 
                        new Rectangle(0,0 , Textures.metrocity.Width, Textures.metrocity.Height), 
                        new Rectangle(new Vector2(baseData.cords.X + GameData.position.X - Textures.metrocity.Width /2, baseData.cords.Y + GameData.position.Y - Textures.metrocity.Height /2), Textures.metrocity.Width * 4, Textures.metrocity.Height * 4),
                        Vector2.Zero, 
                        0, 
                        Color.White
                    );
                    break;
                default:
                    break;
            }

        if (GameData.peoplepositions.ToArray().Length > 0) {
            foreach (var item in GameData.peoplepositions)
            {
                GameData.textureint.TryGetValue(item,out int a);
                Raylib.DrawTextureEx(Textures.people[a],Vector2.Add(item,GameData.position),0,2,Color.White);
            }
        }

        Raylib.DrawTextEx(Raylib.GetFontDefault(),baseData.townname + $" {(baseData.yearwhenwasfounded)}",Vector2.Add(baseData.cords,GameData.position),20,0.5f,Color.White);

        }

        Raylib.DrawCircle((int)GameConfig.windowsize.X / 2 + (int)GameData.position.X,(int)GameConfig.windowsize.Y / 2 + (int)GameData.position.Y,120,Color.Brown);
        Raylib.DrawCircle((int)GameConfig.windowsize.X / 2 + (int)GameData.position.X,(int)GameConfig.windowsize.Y / 2 + (int)GameData.position.Y,100,new Color(102, 191, 255,255 * (1 - (int)Lakedetails.damagelvl / 100)));


        if (GameData.rockpositions.ToArray().Length > 0) {
            foreach (var item in GameData.rockpositions)
            {
                Vector2 screenPos = new Vector2(item.X + GameData.position.X, item.Y + GameData.position.Y);

                if (screenPos.X + Textures.rock.Width > 0 && screenPos.X < GameConfig.windowsize.X &&
                    screenPos.Y + Textures.rock.Height > 0 && screenPos.Y < GameConfig.windowsize.Y) 
                {
                    Raylib.DrawTextureEx(Textures.rock, screenPos, 0, .5f, Color.White);  
                }
            }
        }
        
        if (GameData.grassposition.ToArray().Length > 0) {
            foreach (var item in GameData.grassposition)
            {
                Vector2 screenPos = new Vector2(item.X + GameData.position.X, item.Y + GameData.position.Y);
        
                if (screenPos.X + Textures.grass.Width > 0 && screenPos.X < GameConfig.windowsize.X &&
                    screenPos.Y + Textures.grass.Height > 0 && screenPos.Y < GameConfig.windowsize.Y) 
                {
                    Raylib.DrawTextureEx(Textures.grass, screenPos, 0, .5f, Color.White);  
                }
            }
        }
        
        if (GameData.treepositions.ToArray().Length > 0) {
            foreach (var item in GameData.treepositions)
            {
                Vector2 screenPos = new Vector2(item.X + GameData.position.X, item.Y + GameData.position.Y);
        
                if (screenPos.X + Textures.tree.Width > 0 && screenPos.X < GameConfig.windowsize.X &&
                    screenPos.Y + Textures.tree.Height > 0 && screenPos.Y < GameConfig.windowsize.Y) 
                {
                    Raylib.DrawTextureEx(Textures.tree, screenPos, 0, 1.4f, Color.White);   
                }
            }
        }

        Raylib.DrawText("jezero",(int)GameConfig.windowsize.X / 2 + (int)GameData.position.X - (int)Raylib.MeasureTextEx(Raylib.GetFontDefault(),"jezero",20,0.5f).X / 2,(int)GameConfig.windowsize.Y / 2 + (int)GameData.position.Y - (int)Raylib.MeasureTextEx(Raylib.GetFontDefault(),"jezero",20,0.5f).Y / 2,20,Color.White);

        return; 
    }
    public static void renderui() {
        Raylib.DrawRectangleRec(new Rectangle(new Vector2(0,GameConfig.windowsize.Y / 1.1f),new Vector2(1000,1000)),Color.Gray);

        Vector2 populationtextsize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), "populace: " + Math.Clamp(GameData.nearbypopulation, 0, 10000), 18, 1.05f);
        Raylib.DrawTextEx(Raylib.GetFontDefault(),
            "populace: " + Math.Clamp(GameData.nearbypopulation, 0, 10000),
            new Vector2(8, GameConfig.windowsize.Y / 1.05f),
            18,
            0.5f,
            Color.Black);
        
        string pollutionText = "znedcisteni (%) : " + Math.Truncate(Lakedetails.damagelvl * 100) / 100 + " %";
        Vector2 pollutionTextSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), pollutionText, 18, 1.05f);
        Raylib.DrawTextEx(Raylib.GetFontDefault(),
            pollutionText,
            new Vector2(pollutionTextSize.X + 16, GameConfig.windowsize.Y / 1.05f),
            18,
            0.5f,
            Color.Black);

        string yearText = "rok: " + Math.Clamp(GameData.year,0,9999);
        Vector2 yearTextSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), yearText, 16, 0.05f);
        Raylib.DrawTextEx(Raylib.GetFontDefault(),
            yearText,
            new Vector2(populationtextsize.X * 2 + pollutionTextSize.X + 24 + 16, GameConfig.windowsize.Y / 1.05f),
            16,
            0.05f,
            Color.Black);
        return;
        
    }
    
    public static void DrawOutlinedText(string text , int posX, int posY, int fontSize, Color color, int outlineSize, Color outlineColor) {
        Raylib.DrawText(text, posX - outlineSize, posY - outlineSize, fontSize, outlineColor);
        Raylib.DrawText(text, posX + outlineSize, posY - outlineSize, fontSize, outlineColor);
        Raylib.DrawText(text, posX - outlineSize, posY + outlineSize, fontSize, outlineColor);
        Raylib.DrawText(text, posX + outlineSize, posY + outlineSize, fontSize, outlineColor);
        Raylib.DrawText(text, posX, posY, fontSize, color);
    }

    public static float distancecalc(Vector2 a,Vector2 b) {
        return Vector2.Distance(a,b);
    }
}