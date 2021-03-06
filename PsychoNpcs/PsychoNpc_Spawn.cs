﻿using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.World;
using System;
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
				IEnumerable<string> buffIndexes = mymod.Config.PsychoWardingNeedsBuffTypes.Select(
					idx => spawnInfo.player.FindBuffIndex( idx.Value )+""
				);
				DebugHelpers.Print( "spawnpsycho", "sc:" + mymod.Config.PsychoSpawnChance
					+ ", iaws:" + WorldHelpers.IsAboveWorldSurface( spawnInfo.player.position )
					+ ", iwu:" + WorldHelpers.IsWithinUnderworld( spawnInfo.player.position )
					+ ", ward:" + PsychoPlayer.IsWarding( spawnInfo.player, mymod.Config.PsychoWardingNeedsBuffTypes
							.Select(b=>b.Value)
							.ToList() ),
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
			
			if( PsychoPlayer.IsWarding( spawnInfo.player, mymod.Config.PsychoWardingNeedsBuffTypes.Select(b=>b.Value).ToList() ) ) {
				return false;
			}

			return true;
		}



		////////////////
		
		public void Initialize( NPC npc ) {
			var mymod = PsychoMod.Instance;

			this.InitializePsycho( npc );

			if( mymod.Config.DebugModeInfo ) {
				Main.NewText( npc.TypeName+" " +npc.whoAmI+" spawned at " + npc.position );
				LogHelpers.Log( npc.TypeName+" " + npc.whoAmI + " spawned at " + npc.position );
			}
		}


		private void InitializePsycho( NPC npc ) {
			var mymod = PsychoMod.Instance;

			if( mymod.Config.AllPsychosAreInvincible ) {
				npc.dontTakeDamage = true;
				npc.dontTakeDamageFromHostiles = true;
			}

			if( mymod.Config.AllPsychosAlwaysInstaKill ) {
				npc.damage = Int32.MaxValue / 4;
				npc.takenDamageMultiplier = 1f;
			}

			npc.lavaImmune = true;
			this.HealTimer = 0;
		}
	}
}
