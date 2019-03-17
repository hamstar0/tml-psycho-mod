using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.TmlHelpers;
using Terraria;


namespace Psycho.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public PsychoConfigData Data;

		

		////////////////

		private ModSettingsProtocol() { }


		////////////////

		protected override void InitializeServerSendData( int toWho ) {
			this.Data = PsychoMod.Instance.Config;
		}

		////////////////

		protected override void ReceiveReply() {
			var mymod = PsychoMod.Instance;
			mymod.ConfigJson.SetData( this.Data );

			var myplayer = (PsychoPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, mymod, "PsychoPlayer" );
			myplayer.HasEnteredWorld = true;
		}
	}
}
