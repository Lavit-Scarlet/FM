using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Audio;

namespace FM
{
    public static class CustomSounds
    {
        public static void UpdateLoopingSound(ref SlotId slot, SoundStyle style, float volume, float pitch = 0f, Vector2? position = null)
        {
            SoundEngine.TryGetActiveSound(slot, out var sound);

            if (volume > 0f)
            {
                if (sound == null)
                {
                    slot = SoundEngine.PlaySound(style with { Volume = volume, Pitch = pitch }, position);
                    return;
                }

                sound.Position = position;
                sound.Volume = volume;
            }
            else if (sound != null)
            {
                sound.Stop();

                slot = SlotId.Invalid;
            }
            
        }
        public static readonly SoundStyle VoidRoar = new("FM/Assets/Sounds/Mobs/VoidBossSound/VoidRoar");
    }
}