using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;


namespace Psycho {
	partial class PsychoNpc : GlobalNPC {
		private bool IsInitialized = false;

		////

		public int HealTimer { get; private set; }



		////////////////

		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;

		public override GlobalNPC Clone() {
			var clone = (PsychoNpc)base.Clone();
			clone.IsInitialized = this.IsInitialized;
			clone.HealTimer = this.HealTimer;
			return clone;
		}
		

		////////////////

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawn_info ) {
			var mymod = (PsychoMod)this.mod;
			
			if( PsychoNpc.CanSpawnPsycho( spawn_info ) ) {
				pool[ NPCID.Psycho ] = mymod.Config.PsychoSpawnChance;
			}
			if( !Main.eclipse && PsychoNpc.CanSpawnButcher( spawn_info ) ) {
				pool[ NPCID.Butcher ] = mymod.Config.ButcherSpawnChance;
			}
		}


		public override bool PreNPCLoot( NPC npc ) {
			var mymod = (PsychoMod)this.mod;

			switch( npc.type ) {
			case NPCID.Psycho:
				return mymod.Config.PsychoCanDropLoot;
			case NPCID.Butcher:
				return mymod.Config.ButcherCanDropLoot;
			}

			return base.PreNPCLoot( npc );
		}


		public override bool PreAI( NPC npc ) {
			if( npc.type != NPCID.Psycho && (npc.type != NPCID.Butcher || Main.eclipse) ) {
				return base.PreAI( npc );
			}

			var mymod = (PsychoMod)this.mod;

			if( !this.IsInitialized ) {
				this.IsInitialized = true;

				if( npc.type == NPCID.Psycho || npc.type == NPCID.Butcher ) {
					this.Initialize( npc );
				}
			}

			if( this.IsInitialized && Main.netMode != 1 ) {
				if( Main.netMode == 2 ) {
					this.UpdateServer( npc );
				} else if( Main.netMode == 1 ) {
					this.UpdateClient( npc );
				} else {
					this.UpdateSingle( npc );
				}
			}

			return base.PreAI( npc );
		}
	}
}
