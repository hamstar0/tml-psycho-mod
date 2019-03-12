using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;


namespace Psycho.PsychoNpcs {
	partial class SniperNpc : GlobalNPC {
		private bool IsInitialized = false;


		////
		
		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;



		////////////////

		public override GlobalNPC Clone() {
			var clone = (SniperNpc)base.Clone();
			clone.IsInitialized = this.IsInitialized;
			return clone;
		}
		

		////////////////

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			var mymod = (PsychoMod)this.mod;
			
			if( SniperNpc.CanSpawnSniper( spawnInfo ) ) {
				pool[ NPCID.SkeletonSniper ] = mymod.Config.SniperSpawnChance;
			}
		}

		////

		public override bool PreNPCLoot( NPC npc ) {
			var mymod = (PsychoMod)this.mod;

			if( this.IsInitialized ) {
				return mymod.Config.SniperCanDropLoot;
			}

			return base.PreNPCLoot( npc );
		}

		////

		public override bool PreAI( NPC npc ) {
			if( !SniperNpc.IsOurSniper(npc) ) {
				return base.PreAI( npc );
			}

			var mymod = (PsychoMod)this.mod;

			if( !this.IsInitialized ) {
				this.IsInitialized = true;

				if( npc.type == NPCID.SkeletonSniper ) {
					this.Initialize( npc );
				}
			}

			if( this.IsInitialized ) {
				if( Main.netMode == 2 ) {
					this.PreUpdateServer( npc );
				} else if( Main.netMode == 1 ) {
					this.PreUpdateClient( npc );
				} else {
					this.PreUpdateSingle( npc );
				}
			}

			return base.PreAI( npc );
		}
	}
}
