using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Psycho.PsychoNpcs {
	partial class PsychoNpc : GlobalNPC {
		private void UpdateHeal( NPC npc ) {
			var mymod = (PsychoMod)this.mod;
			
			if( npc.life < npc.lifeMax ) {
				if( this.HealTimer >= mymod.Config.PsychoHealRate ) {
					this.HealTimer = 0;
					this.HealMe( npc );
				} else {
					this.HealTimer++;
				}
			} else {
				this.HealTimer = 0;
			}
		}


		public void HealMe( NPC npc ) {
			var mymod = (PsychoMod)this.mod;
			int newLife = Math.Min( npc.life + mymod.Config.PsychoHealAmount, npc.lifeMax );
			int healed = newLife - npc.life;

			npc.life = newLife;

			if( healed > 0 ) {
				int ct = CombatText.NewText( npc.getRect(), Color.Green, "+" + healed );
				if( ct >= 0 && ct < Main.combatText.Length && Main.combatText[ct] != null ) {
					Main.combatText[ct].lifeTime = 60;
				}

				npc.netUpdate = true;
			}
		}
	}
}
