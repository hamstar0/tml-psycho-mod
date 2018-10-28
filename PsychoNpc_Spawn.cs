using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Psycho {
	partial class PsychoNpc : GlobalNPC {
		public static bool IsOurPsycho( NPC npc ) {
			if( npc.type != NPCID.Psycho ) { return false; }

			var pos = npc.position;
			return !WorldHelpers.IsAboveWorldSurface(pos) && !WorldHelpers.IsWithinUnderworld(pos);
		}


		public static bool IsOurButcher( NPC npc ) {
			if( npc.type != NPCID.Butcher ) { return false; }

			var pos = npc.position;
			return WorldHelpers.IsAboveWorldSurface(pos) && !Main.eclipse;
		}


		public static bool IsOurSniper( NPC npc ) {
			if( npc.type != NPCID.SkeletonSniper ) { return false; }
			if( !npc.HasPlayerTarget ) { return false; }

			Player target_plr = Main.player[ npc.target ];

			return target_plr.ZoneJungle && !target_plr.ZoneDungeon;
		}


		////////////////

		public static bool CanSpawnPsycho( NPCSpawnInfo spawn_info ) {
			if( Main.eclipse ) { return false; }

			var mymod = PsychoMod.Instance;

			if( mymod.Config.PsychoSpawnChance == 0 ) {
				return false;
			}

			var pos = spawn_info.player.position;
			if( WorldHelpers.IsAboveWorldSurface( pos ) || WorldHelpers.IsWithinUnderworld( pos ) ) {
				return false;
			}
			
			if( mymod.Config.PsychoWardingNeedsBuffs.Length > 0 ) {
				foreach( int buff_id in mymod.Config.PsychoWardingNeedsBuffs ) {
					int idx = spawn_info.player.FindBuffIndex( buff_id );
					if( idx == -1 || spawn_info.player.buffTime[idx] <= 0 ) {
						return true;
					}
				}
				return false;
			}
			
			return true;
		}


		public static bool CanSpawnButcher( NPCSpawnInfo spawn_info ) {
			if( Main.eclipse ) { return false; }

			var mymod = PsychoMod.Instance;

			if( mymod.Config.ButcherSpawnChance == 0 ) {
				return false;
			}

			var pos = spawn_info.player.position;
			if( !WorldHelpers.IsAboveWorldSurface( pos ) ) {
				return false;
			}

			if( mymod.Config.ButcherWardingNeedsBuffs.Length > 0 ) {
				foreach( int buff_id in mymod.Config.ButcherWardingNeedsBuffs ) {
					int idx = spawn_info.player.FindBuffIndex( buff_id );
					if( idx == -1 || spawn_info.player.buffTime[idx] <= 0 ) {
						return true;
					}
				}
				return false;
			}

			return true;
		}


		public static bool CanSpawnSniper( NPCSpawnInfo spawn_info ) {
			if( spawn_info.player.ZoneDungeon ) { return false; }

			if( !PsychoNpc.CanSpawnPsycho(spawn_info) ) {
				return false;
			}

			var mymod = PsychoMod.Instance;

			if( mymod.Config.SniperSpawnChance == 0 ) {
				return false;
			}

			if( mymod.Config.SniperJungleOnly && !spawn_info.player.ZoneJungle ) {
				return false;
			}

			if( mymod.Config.SniperWardingNeedsBuffs.Length > 0 ) {
				foreach( int buff_id in mymod.Config.SniperWardingNeedsBuffs ) {
					int idx = spawn_info.player.FindBuffIndex( buff_id );
					if( idx == -1 || spawn_info.player.buffTime[idx] <= 0 ) {
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
			this.HealTimer = 0;

			if( npc.type == NPCID.SkeletonSniper ) {
				this.InitializeSniper( npc );
			}
			
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
