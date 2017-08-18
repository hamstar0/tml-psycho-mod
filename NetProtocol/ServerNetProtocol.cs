using HamstarHelpers.DebugHelpers;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Psycho.NetProtocol {
	public enum ServerNetProtocolTypes : byte {
		RequestModSettings=0
	}


	public static class ServerNetProtocol {
		public static void RoutePacket( Psycho mymod, BinaryReader reader, int player_who ) {
			ServerNetProtocolTypes protocol = (ServerNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case ServerNetProtocolTypes.RequestModSettings:
				//if( mymod.IsDebugInfoMode() ) { DebugHelpers.Log( "RouteReceivedServerPackets.RequestModSettings" ); }
				ServerNetProtocol.ReceiveModSettingsRequestOnServer( mymod, reader, player_who );
				break;
			default:
				/*if( mymod.IsDebugInfoMode() ) {*/ DebugHelpers.Log( "RouteReceivedServerPackets ...? " + protocol ); //}
				break;
			}
		}


		
		////////////////
		// Server Senders
		////////////////

		public static void SendModSettingsFromServer( Psycho mymod, Player player ) {
			// Server only
			if( Main.netMode != 2 ) { return; }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)ClientNetProtocolTypes.ModSettings );
			packet.Write( (string)mymod.Config.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}


		
		////////////////
		// Server Receivers
		////////////////

		private static void ReceiveModSettingsRequestOnServer( Psycho mymod, BinaryReader reader, int player_who ) {
			// Server only
			if( Main.netMode != 2 ) { return; }

			ServerNetProtocol.SendModSettingsFromServer( mymod, Main.player[player_who] );
		}
	}
}
