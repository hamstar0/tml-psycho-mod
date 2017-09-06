using Psycho.NetProtocol;
using Terraria;
using Terraria.ModLoader;


namespace Psycho {
	class MyModPlayer : ModPlayer {
		public bool HasEnteredWorld = false;


		public override void Initialize() {
			this.HasEnteredWorld = false;
		}

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (MyModPlayer)client_clone;
			clone.HasEnteredWorld = this.HasEnteredWorld;
		}

		public override void OnEnterWorld( Player player ) {
			if( this.player.whoAmI == Main.myPlayer ) {
				var mymod = (PsychoMod)this.mod;

				if( Main.netMode != 2 ) {   // Not server
					if( !mymod.Config.LoadFile() ) {
						mymod.Config.SaveFile();
					}
				}

				if( Main.netMode == 1 ) {   // Client
					ClientPacketHandlers.SendModSettingsRequestFromClient( mymod );
				}

				this.HasEnteredWorld = true;
			}
		}
	}
}
