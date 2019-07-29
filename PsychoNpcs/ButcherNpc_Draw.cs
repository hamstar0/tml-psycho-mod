using HamstarHelpers.Helpers.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace Psycho.PsychoNpcs {
	partial class ButcherNpc : GlobalNPC {
		public override void PostDraw( NPC npc, SpriteBatch sb, Color drawColor ) {
			if( !this.IsInitialized ) { return; }

			Texture2D tex = Main.npcTexture[npc.type];
			int frameHeight = tex.Height / Main.npcFrameCount[npc.type];
			int frame = npc.frame.Y / frameHeight;

			Color mainColor;
			Color? overlayColor;
			PsychoMod.GetColors( npc, drawColor, out mainColor, out overlayColor );

			var pos = npc.position;
			pos.X += (Main.rand.NextFloat() * 24f) - 12f;
			pos.Y += (Main.rand.NextFloat() * 24f) - 12f;

			float colorRand = Main.rand.NextFloat();

			mainColor *= colorRand;
			NPCDrawHelpers.DrawSimple( sb, npc, frame, pos, 0f, npc.scale, mainColor );

			if( overlayColor.HasValue ) {
				overlayColor = overlayColor.Value * colorRand;
				NPCDrawHelpers.DrawSimple( sb, npc, frame, pos, 0f, npc.scale, overlayColor.Value );
			}
		}
	}
}
