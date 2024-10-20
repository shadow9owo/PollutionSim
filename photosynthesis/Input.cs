using System.Numerics;
using Raylib_cs;

class Input
{
    static float timer = 0;
    public static void handleinput() 
    {
        if (GameData.currentscene == Scene.menu || GameData.currentscene == Scene.loading) return;
        if (Raylib.IsKeyPressed(KeyboardKey.Escape)) 
        {
            if (GameData.currentscene == Scene.paused) {
                GameData.currentscene = Scene.playing;
                GameConfig.timescale = 1;
            }else if (GameData.currentscene != Scene.menu) {
                GameConfig.timescale = 0;
                GameData.currentscene = Scene.paused;
            }
        }
        if (Raylib.IsKeyPressed(KeyboardKey.F3)) {
            GameConfig.debug = !GameConfig.debug;
        }
        if (timer >= 0.08) {
            if (GameData.currentscene == Scene.playing) {
                if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up)) 
                {
                    GameData.position.Y += 8;
                }
                else if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down)) 
                {
                    GameData.position.Y -= 8;
                }
                else if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left)) 
                {
                    GameData.position.X += 8;
                }
                else if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right)) 
                {
                    GameData.position.X -= 8;
                }
                GameData.position.X = Math.Clamp(GameData.position.X,-1024,1024);
                GameData.position.Y = Math.Clamp(GameData.position.Y,-1024,1024);
            }
            timer = 0;
        }else {
            timer = timer + Raylib.GetFrameTime();
        }
        if (Raylib.IsKeyPressed(KeyboardKey.E)) 
        {
            Simulation.fastforward();
        }
        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) {
            Console.WriteLine(Vector2.Subtract(Raylib.GetMousePosition(),GameData.position).X +"," + Vector2.Add(Raylib.GetMousePosition(),GameData.position).Y);
        }
        return;
    }
}