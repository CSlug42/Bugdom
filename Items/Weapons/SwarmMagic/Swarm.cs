using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;
using Terraria.Audio;

namespace Bugdom.Items.Weapons.SwarmMagic
{
    public class Swarm : ModProjectile
    {
        private int captured = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 20;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 29;
            Projectile.friendly = true;
            Projectile.width = 25;
            Projectile.height = 18;
            Projectile.penetrate = 10;
            captured = 0;
            Projectile.scale = 1.3f;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (NPCID.Sets.CountsAsCritter[target.type] && NPC.CheckCatchNPC(target, Entity.Hitbox, new Item(ItemID.BugNet, 1, 0), Main.player[Main.myPlayer], true) && captured < 4) {
                captured++;
                Projectile.frameCounter = 8;
                Projectile.frame = captured * 4;
                Projectile.damage += (int)Math.Round(Projectile.damage * 0.1);
                Projectile.damage += 8;
            }
            if (NPCID.Sets.CountsAsCritter[target.type])
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 6; k++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Projectile.oldVelocity.X * -0.5f, Projectile.oldVelocity.Y * -0.5f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmber, Projectile.oldVelocity.X * -0.5f, Projectile.oldVelocity.Y * -0.5f);
            }

            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, Projectile.position);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (++Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];

                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.GemDiamond, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

                if (++Projectile.frame >= 4 + captured * 4)
                    Projectile.frame = captured * 4;
            }

            base.AI();
        }
    }
}