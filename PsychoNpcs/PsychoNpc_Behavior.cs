using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Misc;
using HamstarHelpers.Helpers.World;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Psycho.PsychoNpcs {
	partial class PsychoNpc : GlobalNPC {
		public const float AlertDistance = 16 * 47;



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

		private void PreUpdateLocal( NPC npc ) {
			if( WorldHelpers.IsAboveWorldSurface( Main.LocalPlayer.position ) ) {
				return;
			}

			if( Main.netMode != 2 ) {
				if( !Main.LocalPlayer.dead ) {
					float dist = Math.Abs( Vector2.Distance( npc.position, Main.LocalPlayer.position ) );
					float scale = MathHelper.Clamp( ((dist / PsychoNpc.AlertDistance) - 0.25f), 0f, 1f );

					if( PsychoPlayer.NearestPsychoDist == -1 || dist < PsychoPlayer.NearestPsychoDist ) {
						PsychoPlayer.NearestPsychoDist = (int)dist;
//DebugHelpers.Print( "psychodist_"+npc.whoAmI, (int)dist+" : "+scale, 20 );
						MusicHelpers.SetVolumeScale( scale );
					}

					if( npc.ai[2] == 1f ) {
						if( !PsychoPlayer.IsPsychoAlerted ) {
							PsychoPlayer.IsPsychoAlerted = npc.HasPlayerTarget && npc.target == Main.LocalPlayer.whoAmI;
						}
					}
				}
			}
		}


		private void PreUpdateWorld( NPC npc ) {
			int maxDistance = 16 * 150;    // Furthest proximity to an underground player
			int stalkDist = 16 * 64;

			int closestPlrDist = -1;
			int closestPlrWho = -1;

			bool isHealing = false;

			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }

				float dist = Math.Abs( Vector2.Distance( npc.position, player.position ) );

				if( !isHealing && dist <= maxDistance ) {
					isHealing = true;
					this.UpdateHeal( npc ); // At most once per frame, when relevant
				}

				// Is hiding
				if( npc.ai[2] == 1f ) {
					if( dist < closestPlrDist || closestPlrDist == -1 ) {
						closestPlrDist = (int)dist;
						closestPlrWho = i;
					}
				}
			}

			if( closestPlrWho != -1 ) {
				if( closestPlrDist > stalkDist && closestPlrDist < (stalkDist * 2) ) {
					this.UpdateStalk( npc );
				}
			}
		}
	}
}
