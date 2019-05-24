using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Psycho {
    partial class PsychoMod : Mod {
		public static void GetColors( NPC npc, Color drawColor, out Color mainColor, out Color? overlayColor ) {
			mainColor = npc.GetAlpha( drawColor );

			if( npc.color != default(Color) ) {
				overlayColor = npc.GetColor( drawColor );
			} else {
				overlayColor = default( Color );
			}
		}
	}
}
