using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bugdom.Critters.Hoppers
{
    // This is an example gun designed to best demonstrate the various tML hooks that can be used for ammo-related specifications.
    public class MiniSickle : ModProjectile
    {

        private float AI_Timer = 0f;
        private float rotation_Direction = 1f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.DemonScythe];
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Sickle;
            Projectile.friendly = true;
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.penetrate = -1;
            Projectile.scale = 0.7f;

            if (Main.rand.Next(16) < 8) {
                rotation_Direction = 1;
            }
        }

        public override void AI()
        {
            AI_Timer++;
            if (AI_Timer >= 120)
            {
                Projectile.Kill();
            }

            base.AI();

            Projectile.scale += 0.008f;
            Projectile.rotation = AI_Timer * rotation_Direction * 0.35f;
        }
    }
}