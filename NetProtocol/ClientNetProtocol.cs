using HamstarHelpers.DebugHelpers;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Psycho.NetProtocol {
	public enum ClientNetProtocolTypes : byte {
		ModSettings=128,
	}


	public static class ClientNetProtocol {
		public static void RoutePacket( Psycho mymod, BinaryReader reader ) {
			ClientNetProtocolTypes protocol = (ClientNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case ClientNetProtocolTypes.ModSettings:
				//if( mymod.IsDebugInfoMode() ) { DebugHelpers.Log( "RouteReceivedClientPackets.ModSettings" ); }
				ClientNetProtocol.ReceiveModSettingsOnClient( mymod, reader );
				break;
			default:
				/*if( mymod.IsDebugInfoMode() ) {*/ DebugHelpers.Log( "RouteReceivedClientPackets ...? "+protocol ); //}
				break;
			}
		}



		////////////////
		// Client Senders
		////////////////

		public static void SendModSettingsRequestFromClient( Psycho mymod ) {
			// Clients only
			if( Main.netMode != 1 ) { return; }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)ServerNetProtocolTypes.RequestModSettings );

			packet.Send();
		}



		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModSettingsOnClient( Psycho mymod, BinaryReader reader ) {
			// Clients only
			if( Main.netMode != 1 ) { return; }
			
			mymod.Config.DeserializeMe( reader.ReadString() );
		}
	}
}
