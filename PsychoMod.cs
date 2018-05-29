using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Config;
using Psycho.NetProtocols;
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
				PsychoMod.Instance.ConfigJson.LoadFile();
			}
		}


		////////////////

		public JsonConfig<PsychoConfigData> ConfigJson { get; private set; }
		public PsychoConfigData Config { get { return this.ConfigJson.Data; } }


		////////////////

		public PsychoMod() {
			PsychoMod.Instance = this;

			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.ConfigJson = new JsonConfig<PsychoConfigData>( PsychoConfigData.ConfigFileName,
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
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Psycho updated to " + PsychoConfigData.ConfigVersion.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			PsychoMod.Instance = null;
		}
	}
}
