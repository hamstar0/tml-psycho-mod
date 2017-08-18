using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Config;
using System;
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
			if( hamhelpmod.Version < new Version( 1, 0, 17 ) ) {
				throw new Exception( "Hamstar's Helpers must be version " + hamhelpmod.Version.ToString() + " or greater." );
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
	}
}
