using HamstarHelpers.Components.Network;
using Psycho.NetProtocols;
using Terraria;
using Terraria.ModLoader;


namespace Psycho {
	class PsychoPlayer : ModPlayer {
		public static bool IsWarding( Player player, int[] buffIds ) {
			var mymod = PsychoMod.Instance;

			if( buffIds.Length > 0 ) {
				bool isWarding = false;

				foreach( int buffId in buffIds ) {
					int idx = player.FindBuffIndex( buffId );
					if( idx != -1 && player.buffTime[idx] >= 1 ) {
						isWarding = true;
					} else {
						isWarding = false;
						break;
					}
				}

				if( isWarding ) {
					return false;
				}
			}

			return false;
		}



		////////////////

		public bool HasEnteredWorld { get; internal set; }



		////////////////

		public override bool CloneNewInstances => false;

		public override void Initialize() {
			this.HasEnteredWorld = false;
		}

		////////////////

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (PsychoMod)this.mod;

			if( Main.netMode == 0 ) {
				if( !mymod.ConfigJson.LoadFile() ) {
					mymod.ConfigJson.SaveFile();
				}
			}

			if( Main.netMode == 0 ) {
				this.HasEnteredWorld = true;
			} else if( Main.netMode == 1 ) {
				PacketProtocolRequestToServer.QuickRequest<ModSettingsProtocol>( -1 );
			}
		}
	}
}
