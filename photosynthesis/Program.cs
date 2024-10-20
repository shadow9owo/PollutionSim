using System.Numerics;
using Raylib_cs;

namespace Photosynthesis;

class Textures {
    public static Texture2D house = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\house.png");
    public static Texture2D city = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\city.png");
    public static Texture2D metrocity = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\metrocity.png");
    public static Texture2D tree = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\tree.png");
    public static Texture2D grass = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\grass.png");
    public static Texture2D rock = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\rock.png");
    public static Texture2D[] people = RenderUtils.LoadPeople();
    public static Texture2D background = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\background.png");
}
class Sounds
{
    public static Sound rain = Raylib.LoadSound(Directory.GetCurrentDirectory() + "\\assets\\rain.wav");
    public static Sound bird = Raylib.LoadSound(Directory.GetCurrentDirectory() + "\\assets\\bird.wav"); 
}

class Program
{
    public static void Main()
    {
        GameConfig.windowsize = new Vector2(800,480);
        
        GameData.lakecords = new Vector2(GameConfig.windowsize.X / 2 - 50,GameConfig.windowsize.Y / 2 - 50);

        Raylib.InitWindow((int)GameConfig.windowsize.X,(int)GameConfig.windowsize.Y, "Simulace");

        Raylib.InitAudioDevice();

        GameData.currentscene = Scene.loading;

        Raylib.SetExitKey(0);

        Thread thread = new Thread(new ThreadStart(RenderUtils.importpositions));
        thread.Start();

        Logger.sendmessage("nastavuji def hodnoty particlu");
        GameData.Particle part = new GameData.Particle();

        for (int i = 0; i < GameData.maxparticles; i++)
        {
            part.color = new Color(108,128,148,128);
            part.position = new Vector2(new Random().Next(0,(int)GameConfig.windowsize.X),0);
            part.velocity = new Random().Next(1,32);
            GameData.ParticleList.Add(part);
        }

        Raylib.ToggleFullscreen();

        while (!Raylib.WindowShouldClose() && !GameConfig.shouldexit)
        {
            SoundManager.Soundmanagerfunc();

            Rectangle play,quit,mouse;

            mouse = new Rectangle(new Vector2(Raylib.GetMouseX(),Raylib.GetMouseY()),new Vector2(10,10));
            play = new Rectangle(new Vector2(10,40),new Vector2(Raylib.MeasureTextEx(Raylib.GetFontDefault(),"Spustit Simulaci",24,0.5f).X,60));
            quit = new Rectangle(new Vector2(10,120),new Vector2(Raylib.MeasureTextEx(Raylib.GetFontDefault(),"Ukoncit program",24,0.5f).X,60));

            Raylib.BeginDrawing();

            switch (GameData.currentscene)
            {
                case Scene.loading:
                    Raylib.ClearBackground(Color.Black);
                    Logger.renderlogger();
                    break;
                case Scene.menu:
                    Raylib.ClearBackground(Color.Black);
                    Raylib.DrawTexturePro(
                        Textures.background,
                        new Rectangle(0, 0, Textures.background.Width, Textures.background.Height),
                        new Rectangle(0, 0, GameConfig.windowsize.X, GameConfig.windowsize.Y),
                        new Vector2(0, 0),
                        0,
                        Color.White
                    );
                    if (Raylib.CheckCollisionRecs(mouse,play)) {
                        Raylib.DrawRectangleRec(play,new Color(255,255,255,40));
                        if (Raylib.IsMouseButtonPressed(MouseButton.Left) && GameData.dataloaded) {
                            GameData.currentscene = Scene.playing;
                        }
                    }else {
                        Raylib.DrawRectangleRec(play,new Color(255,255,255,20));
                    }
                    if (Raylib.CheckCollisionRecs(mouse,quit)) {
                        Raylib.DrawRectangleRec(quit,new Color(255,255,255,40));
                        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) {
                            GameConfig.shouldexit = true;
                        }
                    }else {
                        Raylib.DrawRectangleRec(quit,new Color(255,255,255,20));
                    }
                    Raylib.DrawTextPro(Raylib.GetFontDefault(),"Spustit Simulaci",new Vector2(play.X + (play.Width - Raylib.MeasureTextEx(Raylib.GetFontDefault(),"Spustit Simulaci",18,0.5f).X) / 2,play.Y + (play.Height - Raylib.MeasureTextEx(Raylib.GetFontDefault(),"Spustit Simulaci",18,0.5f).Y) / 2),Vector2.Zero,0,18,0.5f,Color.White);
                    Raylib.DrawTextPro(Raylib.GetFontDefault(),"Ukoncit program",new Vector2(quit.X + (quit.Width - Raylib.MeasureTextEx(Raylib.GetFontDefault(),"Ukoncit program",18,0.5f).X) / 2,quit.Y + (quit.Height - Raylib.MeasureTextEx(Raylib.GetFontDefault(),"Ukoncit program",18,0.5f).Y) / 2),Vector2.Zero,0,18,0.5f,Color.White);
                    Raylib.DrawText("VYROBENO S RAYLIBEM C# || VERSION : 1.0 || SHADOWDEV 2024",28,(int)GameConfig.windowsize.Y - 28,20,Color.White);
                    Raylib.DrawText("Simulace Jezera (znedcisteni)",((int)GameConfig.windowsize.X / 2) - (int)Raylib.MeasureTextEx(Raylib.GetFontDefault(),"Simulace Jezera (znedcisteni)",12,0.5f).X,28,20,Color.White);
                    Raylib.DrawText("ovladani:\nW,A,S,D (pohyb)\nE (posunout cas vpred o rok)",((int)GameConfig.windowsize.X / 2) - (int)Raylib.MeasureTextEx(Raylib.GetFontDefault(),"crazy",36,0.5f).X,96,20,Color.White);
                    break;
                case Scene.playing:
                    Raylib.ClearBackground(Color.Green);
                    RenderUtils.rendermap();
                    if (GameConfig.renderrainparticles) {
                        Raylib.DrawRectangle(0,0,(int)GameConfig.windowsize.X,(int)GameConfig.windowsize.Y,new Color(60,83,105,128));
                    }else {
                        Raylib.DrawRectangle(0,0,(int)GameConfig.windowsize.X,(int)GameConfig.windowsize.Y,new Color(48,48,48,128));
                    }
                    for (int i = 0; i < GameData.maxparticles; i++)
                    {
                        GameData.positionoffsets.TryGetValue(i,out int a);
                        if (a.ToString().Length < 1) {
                            a = 0;
                        }
                        if (a > GameConfig.windowsize.Y) {
                            a = -10;
                        }
                        GameData.positionoffsets[i] = a + (int)GameData.ParticleList.ElementAt(i).velocity;
                        if (GameConfig.renderrainparticles) {
                            Raylib.DrawRectangle((int)GameData.ParticleList.ElementAt(i).position.X,(int)GameData.ParticleList.ElementAt(i).position.Y + a,2,10,GameData.ParticleList.ElementAt(i).color);
                        }
                    }
                    RenderUtils.renderui();
                    break;
                case Scene.paused:
                    Raylib.ClearBackground(Color.Green);
                    RenderUtils.rendermap();
                    if (GameConfig.renderrainparticles) {
                        Raylib.DrawRectangle(0,0,(int)GameConfig.windowsize.X,(int)GameConfig.windowsize.Y,new Color(60,83,105,128));
                    }else {
                        Raylib.DrawRectangle(0,0,(int)GameConfig.windowsize.X,(int)GameConfig.windowsize.Y,new Color(48,48,48,128));
                    }
                    for (int i = 0; i < GameData.maxparticles; i++)
                    {
                        GameData.positionoffsets.TryGetValue(i,out int a);
                        if (a.ToString().Length < 1) {
                            a = 0;
                        }
                        if (a > GameConfig.windowsize.Y) {
                            a = -10;
                        }
                        if (GameConfig.renderrainparticles) {
                            Raylib.DrawRectangle((int)GameData.ParticleList.ElementAt(i).position.X,(int)GameData.ParticleList.ElementAt(i).position.Y + a,2,10,GameData.ParticleList.ElementAt(i).color);
                        }
                    }
                    Rectangle rec1 = new Rectangle(new Vector2(30,GameConfig.windowsize.Y * 0.8f),new Vector2(200,60));

                    if (Raylib.CheckCollisionRecs(mouse,rec1)) 
                    {
                        Raylib.DrawText("Odejit",(int)(30),(int)(rec1.Y + rec1.Height / 4),20,Color.White);
                        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) {
                            GameConfig.shouldexit = true;
                        }
                    }else {
                        Raylib.DrawText("Odejit",(int)(30),(int)(rec1.Y + rec1.Height / 4),20,Color.Gray);
                    }
                    Raylib.DrawText("PAUSED",0,0,20,Color.White);
                    break;
                case Scene.end:
                    Raylib.ClearBackground(Color.Black);
                    Raylib.DrawTexturePro(
                        Textures.background,
                        new Rectangle(0, 0, Textures.background.Width, Textures.background.Height),
                        new Rectangle(0, 0, GameConfig.windowsize.X, GameConfig.windowsize.Y),
                        new Vector2(0, 0),
                        0,
                        Color.White
                    );
                    Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), "jezero bylo 100% znedcisteno", 22, 0.5f);
                    Raylib.DrawText("jezero bylo 100% znedcisteno", ((int)GameConfig.windowsize.X / 2) - ((int)textSize.X / 2), 20, 22, Color.White);

                    textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), "jezero trvalo znecistit " + GameData.year + "let\npobliz byli " + GameData.bases.Count + " vesnice/mesta\n\nsimulace byla inspirovana hrou one chance\n(az na to ze s ni nema nic spolecneho)\n\nnaprogramovano za 96 hodin\n\ngithub @shadow9owo\n\nshadowstudios.eu\n\n#sh3d2024", 22, 0.5f);
                    Raylib.DrawText("jezero trvalo znecistit " + GameData.year + "let\npobliz bylo " + GameData.bases.Count + " vesnic/mest\n\nsimulace byla inspirovana hrou one chance\n(az na to ze s ni nema nic spolecneho)\n\nnaprogramovano za 96 hodin\n\ngithub @shadow9owo\n\nhttp://shadowstudios.eu\n\n#sh3d2024", ((int)GameConfig.windowsize.X / 2) - ((int)textSize.X / 2), 96, 22, Color.White);
                    Rectangle rec = new Rectangle(new Vector2(GameConfig.windowsize.X / 2,GameConfig.windowsize.Y * 0.8f),new Vector2(200,60));

                    if (Raylib.CheckCollisionRecs(mouse,rec)) 
                    {
                        Raylib.DrawText("Odejit",(int)(rec.X + rec.Width / 4),(int)(rec.Y + rec.Height / 4),20,Color.White);
                        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) {
                            GameConfig.shouldexit = true;
                        }
                    }else {
                        Raylib.DrawText("Odejit",(int)(rec.X + rec.Width / 4),(int)(rec.Y + rec.Height / 4),20,Color.Gray);
                    }

                    break;
            }

            Input.handleinput();

            Logger.renderlogger();

            switch (GameConfig.debug)
            {
                case true:
                    Raylib.DrawText(Raylib.GetFPS() + "FPS || " + (int)GameData.position.X + " X " + (int)GameData.position.Y + " Y ",(int)GameConfig.windowsize.X - (int)Raylib.MeasureTextEx(Raylib.GetFontDefault(),Raylib.GetFPS() + "FPS || " + (int)GameData.position.X + " X " + (int)GameData.position.Y + " Y ",36,0.5f).X,0,20,Color.Red);
                    break;
                case false:
                    break;
            }

            Raylib.EndDrawing();
        }
        Raylib.CloseAudioDevice();

        Raylib.CloseWindow();
    }
}