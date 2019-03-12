using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Psycho.PsychoNpcs {
	partial class PsychoNpc : GlobalNPC {
		public static bool IsOurPsycho( NPC npc ) {
			if( npc.type != NPCID.Psycho ) { return false; }

			var pos = npc.position;
			return !WorldHelpers.IsAboveWorldSurface(pos) && !WorldHelpers.IsWithinUnderworld(pos);
		}


		////////////////

		public static bool CanSpawnPsycho( NPCSpawnInfo spawnInfo ) {
			if( Main.eclipse ) { return false; }

			var mymod = PsychoMod.Instance;
			if( mymod.Config.DebugModeInfo ) {
				IEnumerable<string> buffIndexes = mymod.Config.PsychoWardingNeedsBuffs.Select(
					idx => spawnInfo.player.FindBuffIndex( idx )+""
				);
				DebugHelpers.Print( "spawnpsycho", "sc:" + mymod.Config.PsychoSpawnChance
					+ ", iaws:" + WorldHelpers.IsAboveWorldSurface( spawnInfo.player.position )
					+ ", iwu:" + WorldHelpers.IsWithinUnderworld( spawnInfo.player.position )
					+ ", nb:" + mymod.Config.PsychoWardingNeedsBuffs.Length
					+ ", b:" + string.Join( ",", buffIndexes ),
					60
				);
			}

			if( mymod.Config.PsychoSpawnChance == 0 ) {
				return false;
			}

			var pos = spawnInfo.player.position;
			if( WorldHelpers.IsAboveWorldSurface( pos ) || WorldHelpers.IsWithinUnderworld( pos ) ) {
				return false;
			}
			
			if( mymod.Config.PsychoWardingNeedsBuffs.Length > 0 ) {
				foreach( int buff_id in mymod.Config.PsychoWardingNeedsBuffs ) {
					int idx = spawnInfo.player.FindBuffIndex( buff_id );
					if( idx != -1 && spawnInfo.player.buffTime[idx] > 0 ) {
						return false;
					}
				}
			}
			
			return true;
		}



		////////////////
		
		public void Initialize( NPC npc ) {
			var mymod = PsychoMod.Instance;

			npc.lavaImmune = true;
			this.HealTimer = 0;
			
			if( mymod.Config.DebugModeInfo ) {
				Main.NewText( npc.TypeName+" " +npc.whoAmI+" spawned at " + npc.position );
				LogHelpers.Log( npc.TypeName+" " + npc.whoAmI + " spawned at " + npc.position );
			}
		}
	}
}
