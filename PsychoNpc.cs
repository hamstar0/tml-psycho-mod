using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;


namespace Psycho {
	partial class PsychoNpc : GlobalNPC {
		private bool IsInitialized = false;
		private bool _WasDay = Main.dayTime;


		////

		public int HealTimer { get; private set; }

		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;



		////////////////

		public override GlobalNPC Clone() {
			var clone = (PsychoNpc)base.Clone();
			clone.IsInitialized = this.IsInitialized;
			clone.HealTimer = this.HealTimer;
			return clone;
		}
		

		////////////////

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			var mymod = (PsychoMod)this.mod;
			
			if( PsychoNpc.CanSpawnPsycho( spawnInfo ) ) {
				pool[ NPCID.Psycho ] = mymod.Config.PsychoSpawnChance;
			}
			if( PsychoNpc.CanSpawnButcher( spawnInfo ) ) {
				pool[ NPCID.Butcher ] = mymod.Config.ButcherSpawnChance;
			}
			if( PsychoNpc.CanSpawnSniper( spawnInfo ) ) {
				pool[ NPCID.SkeletonSniper ] = mymod.Config.SniperSpawnChance;
			}
		}


		public override bool PreNPCLoot( NPC npc ) {
			var mymod = (PsychoMod)this.mod;

			switch( npc.type ) {
			case NPCID.Psycho:
				return mymod.Config.PsychoCanDropLoot;
			case NPCID.Butcher:
				return mymod.Config.ButcherCanDropLoot;
			case NPCID.SkeletonSniper:
				return mymod.Config.SniperCanDropLoot;
			}

			return base.PreNPCLoot( npc );
		}


		public override bool PreAI( NPC npc ) {
			if( !PsychoNpc.IsOurPsycho(npc) && !PsychoNpc.IsOurButcher(npc) && !PsychoNpc.IsOurSniper(npc) ) {
				return base.PreAI( npc );
			}

			var mymod = (PsychoMod)this.mod;

			if( !this.IsInitialized ) {
				this.IsInitialized = true;

				if( npc.type == NPCID.Psycho || npc.type == NPCID.Butcher || npc.type == NPCID.SkeletonSniper ) {
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
