using HamstarHelpers.DebugHelpers;
using HamstarHelpers.WorldHelpers;
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


		public static bool CanSpawn( PsychoConfigData config, NPCSpawnInfo spawn_info ) {
			var pos = spawn_info.player.position;
			if( WorldHelpers.IsAboveWorldSurface( pos ) || WorldHelpers.IsWithinUnderworld( pos ) ) {
				return false;
			}

			foreach( int buff_id in config.PsychoWardingNeedsBuffs ) {
				int idx = spawn_info.player.FindBuffIndex( buff_id );
				if( idx == -1 || spawn_info.player.buffTime[idx] <= 0 ) {
					return true;
				}
			}
			
			return false;
		}



		////////////////

		public int HealTimer { get; private set; }


		////////////////
		
		public void Initialize( NPC npc ) {
			npc.lavaImmune = true;
			this.HealTimer = 0;

			var mymod = (PsychoMod)ModLoader.GetMod( "Psycho" );
			if( mymod.Config.DebugModeInfo ) {
				Main.NewText( "Psycho "+npc.whoAmI+" spawned at " + npc.position );
				LogHelpers.Log( "Psycho " + npc.whoAmI + " spawned at " + npc.position );
			}
		}

		////////////////

		public void UpdateServer( NPC npc ) {
			float max_distance = 16 * 200;    // Proximity to underground player

			if( Main.netMode != 2 && !WorldHelpers.IsAboveWorldSurface( Main.LocalPlayer.position ) ) {
				float dist = Math.Abs( Vector2.Distance( npc.position, Main.LocalPlayer.position ) );
				float scale = dist / ( max_distance * 0.5f );

DebugHelpers.SetDisplay("psychodist", (int)dist+" : "+scale, 20 );
				if( scale < 1f ) {
					Main.musicVolume = scale;
				}
			}

			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }

				if( Math.Abs( Vector2.Distance( npc.position, player.position ) ) <= max_distance ) {
					this.UpdateNearPlayer( npc ); // At most once per frame, when relevant
					break;
				}
			}
		}


		private void UpdateNearPlayer( NPC npc ) {
			var mymod = (PsychoMod)this.mod;

			if( npc.life < npc.lifeMax ) {
				if( this.HealTimer >= mymod.Config.PsychoHealRate ) {
					this.HealTimer = 0;
					this.HealMe( npc );
				} else {
					this.HealTimer++;
				}
			} else {
				this.HealTimer = 0;
			}
		}


		public void HealMe( NPC npc ) {
			var mymod = (PsychoMod)this.mod;
			int new_life = Math.Min( npc.life + mymod.Config.PsychoHealAmount, npc.lifeMax );
			int healed = new_life - npc.life;

			npc.life = new_life;

			if( healed > 0 ) {
				int ct = CombatText.NewText( npc.getRect(), Color.Green, "+" + healed );
				Main.combatText[ct].lifeTime = 60;

				npc.netUpdate = true;
			}
		}
	}
}
