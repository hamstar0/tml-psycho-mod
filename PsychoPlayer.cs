using Psycho.NetProtocols;
using Terraria;
using Terraria.ModLoader;


namespace Psycho {
	class PsychoPlayer : ModPlayer {
		public bool HasEnteredWorld = false;


		public override void Initialize() {
			this.HasEnteredWorld = false;
		}

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (PsychoPlayer)client_clone;
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
