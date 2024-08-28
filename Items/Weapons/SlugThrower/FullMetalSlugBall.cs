using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bugdom.Items.Weapons.SlugThrower
{
    // This is an example gun designed to best demonstrate the various tML hooks that can be used for ammo-related specifications.
    public class FullMetalSlugBall : ModProjectile
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
            Projectile.penetrate = -1;
        }

        public override void Kill(int timeLeft)
        {
            NPC.NewNPCDirect(null, Entity.position, ModContent.NPCType<FullMetalSlug>(), 0, 0f, 0f, 0f, 0f, 255);
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