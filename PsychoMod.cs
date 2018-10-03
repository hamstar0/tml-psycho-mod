﻿using HamstarHelpers.Components.Config;
using Terraria.ModLoader;


namespace Psycho {
    partial class PsychoMod : Mod {
		public static PsychoMod Instance { get; private set; }



		////////////////

		public JsonConfig<PsychoConfigData> ConfigJson { get; private set; }
		public PsychoConfigData Config { get { return this.ConfigJson.Data; } }



		////////////////

		public PsychoMod() {
			PsychoMod.Instance = this;
			
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
				ErrorLogger.Log( "Psycho updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			PsychoMod.Instance = null;
		}
	}
}
