using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;


namespace Psycho {
	class MyGlobalNpc : GlobalNPC {
		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawn_info ) {
			var config = ((PsychoMod)this.mod).Config.Data;

			if( PsychoInfo.CanSpawn( config, spawn_info ) ) {
				pool[NPCID.Psycho] = config.PsychoSpawnChance;
			}
		}


		public override bool PreNPCLoot( NPC npc ) {
			var npc_info = (PsychoInfo)PsychoInfo.GetNpcInfo( this.mod, npc.whoAmI );
			if( npc_info != null ) {
				var config = ((PsychoMod)this.mod).Config.Data;
				return config.PsychoCanDropLoot;
			}

			return base.PreNPCLoot( npc );
		}


		public override bool PreAI( NPC npc ) {
			var npc_info = (PsychoInfo)PsychoInfo.GetNpcInfo( this.mod, npc.whoAmI );

			if( npc_info != null ) {
				npc_info.Update( ((PsychoMod)this.mod).Config.Data );
			}
			return true;
		}
	}
}
