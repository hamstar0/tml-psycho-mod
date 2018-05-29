using HamstarHelpers.Utilities.Network;
using Terraria;


namespace Psycho.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public PsychoConfigData Data;

		////////////////

		public override void SetServerDefaults() {
			this.Data = PsychoMod.Instance.Config;
		}

		protected override void ReceiveWithClient() {
			PsychoMod.Instance.ConfigJson.SetData( this.Data );

			var myplayer = Main.LocalPlayer.GetModPlayer<PsychoPlayer>();
			myplayer.HasEnteredWorld = true;
		}
	}
}
