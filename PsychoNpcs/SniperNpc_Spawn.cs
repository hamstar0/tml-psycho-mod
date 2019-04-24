using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Psycho.PsychoNpcs {
	partial class SniperNpc : GlobalNPC {
		public static bool IsOurSniper( NPC npc ) {
			if( npc.type != NPCID.SkeletonSniper ) { return false; }
			if( !npc.HasPlayerTarget ) { return false; }

			Player targetPlr = Main.player[ npc.target ];

			return targetPlr.ZoneJungle && !targetPlr.ZoneDungeon;
		}


		////////////////
		
		public static bool CanSpawnSniper( NPCSpawnInfo spawnInfo ) {
			if( spawnInfo.player.ZoneDungeon ) { return false; }

			if( !PsychoNpc.CanSpawnPsycho(spawnInfo) ) {
				return false;
			}

			var mymod = PsychoMod.Instance;
			if( mymod.Config.DebugModeInfo ) {
				IEnumerable<string> buffIndexes = mymod.Config.SniperWardingNeedsBuffs.Select(
					idx => spawnInfo.player.FindBuffIndex( idx ) + ""
				);
				DebugHelpers.Print( "spawnsniper", "sc:" + mymod.Config.SniperSpawnChance
					+ ", iaws:" + WorldHelpers.IsAboveWorldSurface( spawnInfo.player.position )
					+ ", iwu:" + WorldHelpers.IsWithinUnderworld( spawnInfo.player.position )
					+ ", jung:" + mymod.Config.SniperJungleOnly + " = " + spawnInfo.player.ZoneJungle
					+ ", ward:" + PsychoPlayer.IsWarding( spawnInfo.player, mymod.Config.SniperWardingNeedsBuffs ),
					60
				);
			}

			if( mymod.Config.SniperSpawnChance == 0 ) {
				return false;
			}

			if( mymod.Config.SniperJungleOnly && !spawnInfo.player.ZoneJungle ) {
				return false;
			}

			if( PsychoPlayer.IsWarding( spawnInfo.player, mymod.Config.SniperWardingNeedsBuffs ) ) {
				return false;
			}

			return true;
		}



		////////////////
		
		public void Initialize( NPC npc ) {
			var mymod = PsychoMod.Instance;

			npc.lavaImmune = true;
			this.InitializeSniper( npc );
			
			if( mymod.Config.DebugModeInfo ) {
				Main.NewText( npc.TypeName+" " +npc.whoAmI+" spawned at " + npc.position );
				LogHelpers.Log( npc.TypeName+" " + npc.whoAmI + " spawned at " + npc.position );
			}
		}


		public void InitializeSniper( NPC npc ) {
			npc.life = npc.lifeMax = 10;
			npc.defense = 300;
			npc.damage = Main.expertMode ? 600 : 300;
		}
	}
}
