using HamstarHelpers.Components.Config;
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
				if( !PsychoMod.Instance.ConfigJson.LoadFile() ) {
					PsychoMod.Instance.ConfigJson.SaveFile();
				}
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

			var new_config = new PsychoConfigData();
			//new_config.SetDefaults();

			PsychoMod.Instance.ConfigJson.SetData( new_config );
			PsychoMod.Instance.ConfigJson.SaveFile();
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
