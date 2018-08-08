using HamstarHelpers.Components.Config;
using System;
using Terraria.ID;


namespace Psycho {
	public class PsychoConfigData : ConfigurationDataBase {
		public readonly static Version ConfigVersion = new Version( 1, 3, 0 );
		public readonly static string ConfigFileName = "Psycho Config.json";


		////////////////

		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		public bool DebugModeInfo = false;

		public float PsychoSpawnChance = 0.018f;
		public int PsychoHealRate = (int)(60f * 1.5f);
		public int PsychoHealAmount = 50;
		public bool PsychoCanDropLoot = true;

		public int[] PsychoWardingNeedsBuffs = new int[0];


		////////////////

		private void SetDefaults() {
			this.PsychoWardingNeedsBuffs = new int[] {
				//BuffID.HeartLamp,
				//BuffID.Sunflower,
				BuffID.StarInBottle
				//BuffID.Campfire
			};
		}
		

		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new PsychoConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= PsychoConfigData.ConfigVersion ) {
				return false;
			}
			
			if( this.VersionSinceUpdate == "" ) {
				this.SetDefaults();
			}

			this.VersionSinceUpdate = PsychoConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
