using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;


namespace Psycho.PsychoNpcs {
	partial class ButcherNpc : GlobalNPC {
		//private static bool? WasDay = null;



		////////////////

		private bool IsInitialized = false;
		
		////

		public int HealTimer { get; private set; }

		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;



		////////////////

		public override GlobalNPC Clone() {
			var clone = (ButcherNpc)base.Clone();
			clone.IsInitialized = this.IsInitialized;
			clone.HealTimer = this.HealTimer;
			return clone;
		}
		

		////////////////

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			var mymod = (PsychoMod)this.mod;
			
			if( ButcherNpc.CanSpawnButcher( spawnInfo ) ) {
				pool[ NPCID.Butcher ] = mymod.Config.ButcherSpawnChance;
			}
		}


		public override bool PreNPCLoot( NPC npc ) {
			var mymod = (PsychoMod)this.mod;

			if( this.IsInitialized ) {
				return mymod.Config.ButcherCanDropLoot;
			}

			return base.PreNPCLoot( npc );
		}


		public override bool PreAI( NPC npc ) {
			if( !ButcherNpc.IsOurButcher(npc) ) {
				return base.PreAI( npc );
			}

			var mymod = (PsychoMod)this.mod;

			if( !this.IsInitialized ) {
				this.IsInitialized = true;

				if( npc.type == NPCID.Butcher ) {
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


		public override void AI( NPC npc ) {
			if( this.IsInitialized ) {
				if( Main.netMode == 2 ) {
					this.PostUpdateServer( npc );
				} else if( Main.netMode == 1 ) {
					this.PostUpdateClient( npc );
				} else {
					this.PostUpdateSingle( npc );
				}
			}
		}
	}
}
