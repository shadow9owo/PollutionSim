using Photosynthesis;
using Raylib_cs;

class SoundManager
{
    public static float volume = 1.0f;

    public static void Soundmanagerfunc() {
        if (GameData.currentscene != Scene.playing) 
        {
            Raylib.StopSound(Sounds.bird);
            Raylib.StopSound(Sounds.rain);
        }
        else
        {
            Raylib.SetSoundVolume(Sounds.bird, volume);
            Raylib.SetSoundVolume(Sounds.rain, volume);

            if (!Raylib.IsSoundPlaying(Sounds.bird)) {
                Raylib.PlaySound(Sounds.bird);
            }

            if (GameConfig.renderrainparticles) {
                if (!Raylib.IsSoundPlaying(Sounds.rain)) {
                    Raylib.PlaySound(Sounds.rain);
                }
            }
            else
            {
                Raylib.StopSound(Sounds.rain);
            }
        }
    }
}
