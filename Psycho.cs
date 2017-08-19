using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Config;
using Psycho.NetProtocol;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Psycho {
    public class Psycho : Mod {
		public JsonConfig<ConfigurationData> Config { get; private set; }


		public Psycho() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			string filename = "Psycho Config.json";
			this.Config = new JsonConfig<ConfigurationData>( filename, "Mod Configs", new ConfigurationData() );
		}


		public override void Load() {
			var hamhelpmod = ModLoader.GetMod( "HamstarHelpers" );
			var min_vers = new Version( 1, 0, 17 );

			if( hamhelpmod.Version < min_vers ) {
				throw new Exception( "Hamstar's Helpers must be version " + min_vers.ToString() + " or greater." );
			}

			this.LoadConfig();
		}

		private void LoadConfig() {
			try {
				if( !this.Config.LoadFile() ) {
					this.Config.SaveFile();
				}
			} catch( Exception e ) {
				DebugHelpers.Log( e.Message );
				this.Config.SaveFile();
			}

			if( this.Config.Data.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Psycho updated to " + ConfigurationData.ConfigVersion.ToString() );
				this.Config.SaveFile();
			}
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			if( Main.netMode == 1 ) {   // Client
				ClientPacketHandlers.HandlePacket( this, reader );
			} else if( Main.netMode == 2 ) {    // Server
				ServerPacketHandlers.HandlePacket( this, reader, player_who );
			}
		}
	}
}
