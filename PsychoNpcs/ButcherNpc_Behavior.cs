using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Psycho.PsychoNpcs {
	partial class ButcherNpc : GlobalNPC {
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
			string timerName = "PsychoButcher_" + npc.whoAmI;

			if( Timers.GetTimerTickDuration( timerName ) <= 0 ) {
				Timers.SetTimer( timerName, 23 + Main.rand.Next(0, 3), false, () => {
					Main.PlaySound( SoundID.Item22.SoundId, (int)npc.position.X, (int)npc.position.Y, SoundID.Item22.Style, 0.65f );
					return false;
				} );
			}
		}


		private void PreUpdateWorld( NPC npc ) {
			float maxDistance = 16 * 150;    // Proximity to underground player

			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }

				if( Math.Abs( Vector2.Distance( npc.position, player.position ) ) <= maxDistance ) {
					this.UpdateHeal( npc ); // At most once per frame, when relevant
					break;
				}
			}

			/*if( ButcherNpc.WasDay == null ) {
				ButcherNpc.WasDay = Main.dayTime;
				Main.dayTime = false;   // To be continued (in PostUpdateWorld)...
			}*/
		}


		////////////////

		private void PostUpdateLocal( NPC npc ) {
		}


		private void PostUpdateWorld( NPC npc ) {
			/*if( ButcherNpc.WasDay != null ) {
				Main.dayTime = (bool)ButcherNpc.WasDay;
				ButcherNpc.WasDay = null;
			}*/
		}
	}
}
