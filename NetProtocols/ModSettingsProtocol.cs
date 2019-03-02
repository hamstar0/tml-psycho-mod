using HamstarHelpers.Components.Network;
using Terraria;


namespace Psycho.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public PsychoConfigData Data;

		

		////////////////

		private ModSettingsProtocol() { }


		////////////////

		protected override void InitializeServerSendData( int to_who ) {
			this.Data = PsychoMod.Instance.Config;
		}

		////////////////

		protected override void ReceiveReply() {
			PsychoMod.Instance.ConfigJson.SetData( this.Data );

			var myplayer = Main.LocalPlayer.GetModPlayer<PsychoPlayer>();
			myplayer.HasEnteredWorld = true;
		}
	}
}
