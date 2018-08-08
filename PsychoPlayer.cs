using HamstarHelpers.Components.Network;
using Psycho.NetProtocols;
using Terraria;
using Terraria.ModLoader;


namespace Psycho {
	class PsychoPlayer : ModPlayer {
		public bool HasEnteredWorld { get; internal set; }


		////////////////

		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
			this.HasEnteredWorld = false;
		}

		////////////////

		public override void OnEnterWorld( Player player ) {
			if( this.player.whoAmI == Main.myPlayer ) { return; }

			var mymod = (PsychoMod)this.mod;

			if( Main.netMode == 0 ) {
				if( !mymod.ConfigJson.LoadFile() ) {
					mymod.ConfigJson.SaveFile();
				}
			}

			if( Main.netMode == 0 ) {
				this.HasEnteredWorld = true;
			} else if( Main.netMode == 1 ) {
				PacketProtocol.QuickRequestToServer<ModSettingsProtocol>();
			}
		}
	}
}
