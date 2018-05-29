﻿using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;


namespace Psycho {
	partial class PsychoNpc : GlobalNPC {
		public override bool CloneNewInstances { get { return true; } }

		public override GlobalNPC Clone() {
			var clone = (PsychoNpc)base.Clone();
			clone.HealTimer = this.HealTimer;
			return clone;
		}
		

		////////////////

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawn_info ) {
			var config = ((PsychoMod)this.mod).Config;

			if( PsychoNpc.CanSpawn( config, spawn_info ) ) {
				pool[NPCID.Psycho] = config.PsychoSpawnChance;
			}
		}


		public override bool PreNPCLoot( NPC npc ) {
			if( npc.type == NPCID.Psycho ) {
				var mymod = (PsychoMod)this.mod;
				return mymod.Config.PsychoCanDropLoot;
			}

			return base.PreNPCLoot( npc );
		}


		public override bool PreAI( NPC npc ) {
			var mymod = (PsychoMod)this.mod;

			if( !this.IsInitialized ) {
				this.IsInitialized = true;
				if( npc.type == NPCID.Psycho ) {
					this.Initialize( npc );
				}
			}

			if( this.IsInitialized ) {
				this.UpdateHeal( npc );
			}

			return true;
		}
	}
}
