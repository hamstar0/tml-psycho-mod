using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using Terraria;


namespace Psycho.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public PsychoConfigData Data;


		////////////////

		private ModSettingsProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }


		////////////////

		protected override void SetServerDefaults( int to_who ) {
			this.Data = PsychoMod.Instance.Config;
		}

		////////////////

		protected override void ReceiveWithClient() {
			PsychoMod.Instance.ConfigJson.SetData( this.Data );

			var myplayer = Main.LocalPlayer.GetModPlayer<PsychoPlayer>();
			myplayer.HasEnteredWorld = true;
		}
	}
}
