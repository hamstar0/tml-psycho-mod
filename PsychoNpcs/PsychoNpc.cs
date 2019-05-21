using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using HamstarHelpers.Helpers.DebugHelpers;


namespace Psycho.PsychoNpcs {
	partial class PsychoNpc : GlobalNPC {
		private bool IsInitialized = false;



		////

		public int HealTimer { get; private set; }
		public int StalkTimer { get; private set; }

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
		}


		public override bool PreNPCLoot( NPC npc ) {
			var mymod = (PsychoMod)this.mod;

			if( this.IsInitialized ) {
				return mymod.Config.PsychoCanDropLoot;
			}

			return base.PreNPCLoot( npc );
		}


		public override bool PreAI( NPC npc ) {
			if( !PsychoNpc.IsOurPsycho(npc) ) {
				return base.PreAI( npc );
			}

			var mymod = (PsychoMod)this.mod;
//DebugHelpers.Print( "psycho_"+npc.whoAmI, "color:"+npc.color+", hide:"+npc.hide+", ai: "+string.Join(", ",npc.ai.Select(a=>a.ToString("N2"))), 20 );

			if( !this.IsInitialized ) {
				this.IsInitialized = true;

				if( npc.type == NPCID.Psycho ) {
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
