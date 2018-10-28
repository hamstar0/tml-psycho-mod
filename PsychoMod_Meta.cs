using HamstarHelpers.Components.Config;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Psycho {
    partial class PsychoMod : Mod {
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
	}
}
