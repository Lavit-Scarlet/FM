using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.Audio;
using FM.Content.NPCs.Bosses.Void;

namespace FM.Globals
{
    public class FMPlayer : ModPlayer
    {
        public int shootDelay = 0;
        public float attackSpeedMod = 1;
        public int Shake = 0;

        public override void ModifyScreenPosition()
		{
			Main.screenPosition.Y += Main.rand.Next(-Shake, Shake);
			Main.screenPosition.X += Main.rand.Next(-Shake, Shake);
			if (Shake > 0) { Shake--; }
		}

        public static FMPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<FMPlayer>();
		}

        public override void ResetEffects()
        {
            attackSpeedMod = 1;
        }

        public override void PostUpdate()
        {
            if (shootDelay > 0)
            {
                shootDelay--;

            }
            if (shootDelay == 1)
            {
                Rectangle textPos = new Rectangle((int)Player.position.X, (int)Player.position.Y - 10, Player.width, Player.height);
                CombatText.NewText(textPos, new Color(255, 0, 0, 255), "Cooldown over");
                SoundEngine.PlaySound(new SoundStyle($"{nameof(FM)}/Assets/Sounds/CooldownOver"), Player.Center);
            }

        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            Player.respawnTimer = 480;
            base.Kill(damage, hitDirection, pvp, damageSource);
        }

        public static int ApplyAttackGeneric(Player player, DamageClass damageClass, float startingUseTime)
		{
			float AndGeneric = player.GetTotalAttackSpeed(damageClass);
			return (int)(startingUseTime / (AndGeneric + ModPlayer(player).attackSpeedMod - 1));
		}
        public static int ApplyAttackSpeedClassModWithGeneric(Player player, DamageClass damageClass, float startingUseTime)
		{
			float AndGeneric = player.GetTotalAttackSpeed(damageClass);
			return (int)(startingUseTime / (AndGeneric + ModPlayer(player).attackSpeedMod - 1));
		}

        public override float UseAnimationMultiplier(Item item)
		{
			return UseTimeMultiplier(item);
		}

		public override float UseTimeMultiplier(Item item)
		{
			float standard = attackSpeedMod;
			int time = item.useAnimation;
			int cannotPass = 2;
			float current = time / standard;
			if (current < cannotPass)
			{
				standard = time / 2f;
			}
			return base.UseTimeMultiplier(item);
		}
    }
}