using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.World;
using System;
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
				IEnumerable<string> buffIndexes = mymod.Config.SniperWardingNeedsBuffTypes.Select(
					idx => spawnInfo.player.FindBuffIndex( idx.Value ) + ""
				);
				DebugHelpers.Print( "spawnsniper", "sc:" + mymod.Config.SniperSpawnChance
					+ ", iaws:" + WorldHelpers.IsAboveWorldSurface( spawnInfo.player.position )
					+ ", iwu:" + WorldHelpers.IsWithinUnderworld( spawnInfo.player.position )
					+ ", jung:" + mymod.Config.SniperJungleOnly + " = " + spawnInfo.player.ZoneJungle
					+ ", ward:" + PsychoPlayer.IsWarding( spawnInfo.player, mymod.Config.SniperWardingNeedsBuffTypes.Select(b=>b.Value).ToList() ),
					60
				);
			}

			if( mymod.Config.SniperSpawnChance == 0 ) {
				return false;
			}

			if( mymod.Config.SniperJungleOnly && !spawnInfo.player.ZoneJungle ) {
				return false;
			}

			if( PsychoPlayer.IsWarding( spawnInfo.player, mymod.Config.SniperWardingNeedsBuffTypes.Select(b=>b.Value).ToList() ) ) {
				return false;
			}

			return true;
		}



		////////////////
		
		public void Initialize( NPC npc ) {
			var mymod = PsychoMod.Instance;

			if( mymod.Config.AllPsychosAreInvincible ) {
				npc.dontTakeDamage = true;
				npc.dontTakeDamageFromHostiles = true;
			}

			npc.lavaImmune = true;
			this.InitializeSniper( npc );
			
			if( mymod.Config.DebugModeInfo ) {
				Main.NewText( npc.TypeName+" " +npc.whoAmI+" spawned at " + npc.position );
				LogHelpers.Log( npc.TypeName+" " + npc.whoAmI + " spawned at " + npc.position );
			}
		}


		public void InitializeSniper( NPC npc ) {
			var mymod = PsychoMod.Instance;

			npc.life = npc.lifeMax = mymod.Config.SniperSpawnHp;
			npc.defense = mymod.Config.SniperSpawnArmor;

			if( mymod.Config.AllPsychosAlwaysInstaKill ) {
				npc.damage = Int32.MaxValue / 4;
			} else {
				npc.damage = Main.expertMode ?
					mymod.Config.SniperHardModeDamage
					: mymod.Config.SniperPreHardModeDamage;
			}
		}
	}
}
