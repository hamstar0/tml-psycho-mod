using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Config;
using Psycho.NetProtocol;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Psycho {
    class PsychoMod : Mod {
		public static PsychoMod Instance { get; private set; }

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-psycho-mod"; } }

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + PsychoConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( PsychoMod.Instance != null ) {
				PsychoMod.Instance.Config.LoadFile();
			}
		}


		////////////////

		public JsonConfig<PsychoConfigData> Config { get; private set; }


		////////////////

		public PsychoMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.Config = new JsonConfig<PsychoConfigData>( PsychoConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new PsychoConfigData() );
		}

		////////////////

		public override void Load() {
			var hamhelpmod = ModLoader.GetMod( "HamstarHelpers" );
			var min_vers = new Version( 1, 2, 0 );
			if( hamhelpmod.Version < min_vers ) {
				throw new Exception( "Hamstar Helpers must be version " + min_vers.ToString() + " or greater." );
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
				ErrorLogger.Log( "Psycho updated to " + PsychoConfigData.ConfigVersion.ToString() );
				this.Config.SaveFile();
			}
		}

		////////////////

		public override void PostSetupContent() {
			PsychoInfo.RegisterInfoType<PsychoInfo>();
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			if( Main.netMode == 1 ) {   // Client
				ClientPacketHandlers.HandlePacket( this, reader );
			} else if( Main.netMode == 2 ) {    // Server
				ServerPacketHandlers.HandlePacket( this, reader, player_who );
			}
		}


		////////////////

		public bool IsDebugInfo() {
			return (this.Config.Data.DEBUGMODE & 1) > 0;
		}
	}
}
