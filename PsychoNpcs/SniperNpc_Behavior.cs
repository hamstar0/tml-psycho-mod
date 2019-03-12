using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.MiscHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Psycho.PsychoNpcs {
	partial class SniperNpc : GlobalNPC {
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
			float max_distance = 16 * 25;    // Proximity to underground player

			if( Main.netMode != 2 && !WorldHelpers.IsAboveWorldSurface( Main.LocalPlayer.position ) ) {
				float dist = Math.Abs( Vector2.Distance( npc.position, Main.LocalPlayer.position ) );
				float scale = MathHelper.Clamp( (dist / max_distance) - 0.25f, 0f, 1f );

//DebugHelpers.SetDisplay("psychodist", (int)dist+" : "+scale, 20 );
				MusicHelpers.SetVolumeScale( scale );
			}
		}


		private void PreUpdateWorld( NPC npc ) {
			Main.dayTime = false;
		}
	}
}
