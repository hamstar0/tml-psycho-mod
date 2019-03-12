﻿using HamstarHelpers.Helpers.DebugHelpers;
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

			if( mymod.Config.SniperSpawnChance == 0 ) {
				return false;
			}

			if( mymod.Config.SniperJungleOnly && !spawnInfo.player.ZoneJungle ) {
				return false;
			}

			if( mymod.Config.SniperWardingNeedsBuffs.Length > 0 ) {
				foreach( int buffId in mymod.Config.SniperWardingNeedsBuffs ) {
					int idx = spawnInfo.player.FindBuffIndex( buffId );
					if( idx == -1 || spawnInfo.player.buffTime[idx] <= 0 ) {
						return true;
					}
				}
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