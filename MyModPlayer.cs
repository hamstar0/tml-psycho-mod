using Terraria;
using Terraria.ModLoader;


namespace Psycho {
	class MyModPlayer : ModPlayer {
		public override void PreUpdate() {
			if( this.player.whoAmI == Main.myPlayer || Main.netMode == 2 ) {
				MyGlobalNpc.UpdateAll( this.player );
			}
		}
	}
}
