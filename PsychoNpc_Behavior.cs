using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Psycho {
	partial class PsychoNpc : GlobalNPC {
		public static bool IsValidNpc( NPC npc ) {
			if( npc.type != NPCID.Psycho ) { return false; }
			var pos = npc.position;
			return !( WorldHelpers.IsAboveWorldSurface( pos ) || WorldHelpers.IsWithinUnderworld( pos ) );
		}


		public static bool CanSpawnPsycho( NPCSpawnInfo spawn_info ) {
			var mymod = PsychoMod.Instance;

			if( mymod.Config.PsychoSpawnChance == 0 ) {
				return false;
			}

			var pos = spawn_info.player.position;
			if( WorldHelpers.IsAboveWorldSurface( pos ) || WorldHelpers.IsWithinUnderworld( pos ) ) {
				return false;
			}

			foreach( int buff_id in mymod.Config.PsychoWardingNeedsBuffs ) {
				int idx = spawn_info.player.FindBuffIndex( buff_id );
				if( idx == -1 || spawn_info.player.buffTime[idx] <= 0 ) {
					return true;
				}
			}
			
			return false;
		}

		public static bool CanSpawnButcher( NPCSpawnInfo spawn_info ) {
			var mymod = PsychoMod.Instance;

			if( mymod.Config.ButcherSpawnChance == 0 ) {
				return false;
			}

			var pos = spawn_info.player.position;
			if( !WorldHelpers.IsAboveWorldSurface( pos ) ) {
				return false;
			}

			foreach( int buff_id in mymod.Config.ButcherWardingNeedsBuffs ) {
				int idx = spawn_info.player.FindBuffIndex( buff_id );
				if( idx == -1 || spawn_info.player.buffTime[idx] <= 0 ) {
					return true;
				}
			}

			return false;
		}



		////////////////
		
		public void Initialize( NPC npc ) {
			npc.lavaImmune = true;
			this.HealTimer = 0;

			var mymod = (PsychoMod)ModLoader.GetMod( "Psycho" );
			
			if( mymod.Config.DebugModeInfo ) {
				Main.NewText( npc.TypeName+" " +npc.whoAmI+" spawned at " + npc.position );
				LogHelpers.Log( npc.TypeName+" " + npc.whoAmI + " spawned at " + npc.position );
			}
		}


		////////////////

		public void PreUpdateSingle( NPC npc ) {
			this.PreUpdateLocal( npc );
			this.PreUpdateWorld( npc );
		}

		public void PreUpdateClient( NPC npc ) {
			this.PreUpdateLocal( npc );
		}

		public void PreUpdateServer( NPC npc ) {
			this.PreUpdateWorld( npc );
		}
		
		////////////////

		public void PostUpdateSingle( NPC npc ) {
			this.PostUpdateLocal( npc );
			this.PostUpdateWorld( npc );
		}

		public void PostUpdateClient( NPC npc ) {
			this.PostUpdateLocal( npc );
		}

		public void PostUpdateServer( NPC npc ) {
			this.PostUpdateWorld( npc );
		}


		////////////////

		private void PreUpdateLocal( NPC npc ) {
			float max_distance = 16 * 25;    // Proximity to underground player

			if( Main.netMode != 2 && !WorldHelpers.IsAboveWorldSurface( Main.LocalPlayer.position ) ) {
				float dist = Math.Abs( Vector2.Distance( npc.position, Main.LocalPlayer.position ) );
				float scale = MathHelper.Clamp( (dist / max_distance) - 0.25f, 0f, 1f );

//DebugHelpers.SetDisplay("psychodist", (int)dist+" : "+scale, 20 );
				MusicHelpers.SetVolumeScale( scale );
			}

			if( npc.type == NPCID.Butcher ) {
				Rectangle butcher_rect = npc.getRect();

				if( Timers.GetTimerTickDuration( "PsychoButcher_" + npc.whoAmI ) <= 0 ) {
					Timers.SetTimer( "PsychoButcher_" + npc.whoAmI, 23 + Main.rand.Next(0, 3), () => {
						Main.PlaySound( SoundID.Item22.SoundId, (int)npc.position.X, (int)npc.position.Y, SoundID.Item22.Style, 0.65f );
						return false;
					} );
				}
			}
		}


		private void PreUpdateWorld( NPC npc ) {
			float max_distance = 16 * 150;    // Proximity to underground player

			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }

				if( Math.Abs( Vector2.Distance( npc.position, player.position ) ) <= max_distance ) {
					this.UpdateHeal( npc ); // At most once per frame, when relevant
					break;
				}
			}

			this._WasDay = Main.dayTime;
			Main.dayTime = false;
		}


		////////////////

		private void PostUpdateLocal( NPC npc ) {
		}


		private void PostUpdateWorld( NPC npc ) {
			Main.dayTime = this._WasDay;
		}
	}
}
