using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Psycho.PsychoNpcs {
	partial class ButcherNpc : GlobalNPC {
		public static bool IsOurButcher( NPC npc ) {
			if( npc.type != NPCID.Butcher ) { return false; }
			
			var pos = npc.position;
			return WorldHelpers.IsAboveWorldSurface(pos) && !Main.eclipse;
		}


		////////////////
		
		public static bool CanSpawnButcher( NPCSpawnInfo spawnInfo ) {
			if( Main.eclipse ) { return false; }

			var mymod = PsychoMod.Instance;
			if( mymod.Config.DebugModeInfo ) {
				IEnumerable<string> buffIndexes = mymod.Config.ButcherWardingNeedsBuffs.Select(
					idx => spawnInfo.player.FindBuffIndex( idx ) + ""
				);
				DebugHelpers.Print( "spawnbutcher", "sc:" + mymod.Config.ButcherSpawnChance
					+ ", iaws:" + WorldHelpers.IsAboveWorldSurface( spawnInfo.player.position )
					+ ", iwu:" + WorldHelpers.IsWithinUnderworld( spawnInfo.player.position )
					+ ", ward:" + PsychoPlayer.IsWarding( spawnInfo.player, mymod.Config.ButcherWardingNeedsBuffs ),
					60
				);
			}

			if( mymod.Config.ButcherSpawnChance == 0 ) {
				return false;
			}

			var pos = spawnInfo.player.position;
			if( !WorldHelpers.IsAboveWorldSurface( pos ) ) {
				return false;
			}

			if( PsychoPlayer.IsWarding(spawnInfo.player, mymod.Config.ButcherWardingNeedsBuffs) ) {
				return false;
			}

			return !Main.dayTime;
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
