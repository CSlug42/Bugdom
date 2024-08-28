using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bugdom.Items.Weapons.Slingshot
{
    // This is an example gun designed to best demonstrate the various tML hooks that can be used for ammo-related specifications.
    public class SlugBall : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.width = 15;
            Projectile.height = 15;
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 6; k++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5, Projectile.oldVelocity.X * -0.5f, Projectile.oldVelocity.Y * -0.5f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 250, Projectile.oldVelocity.X * -0.5f, Projectile.oldVelocity.Y * -0.5f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 93, Projectile.oldVelocity.X * -0.5f, Projectile.oldVelocity.Y * -0.5f);
                
            }
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            base.AI();
        }
    }
}