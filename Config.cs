using HamstarHelpers.Utilities.Config;
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

		public int[] PsychoWardingNeedsBuffs = new int[] {
			//BuffID.HeartLamp,
			//BuffID.Sunflower,
			BuffID.StarInBottle,
			BuffID.Campfire
		};
		

		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new PsychoConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= PsychoConfigData.ConfigVersion ) {
				return false;
			}

			if( vers_since < new Version( 1, 0, 1 ) ) {
				if( PsychoConfigData._1_0_0_PsychoSpawnChance == this.PsychoSpawnChance ) {
					this.PsychoSpawnChance = new_config.PsychoSpawnChance;
				}
			}
			if( vers_since < new Version( 1, 0, 2 ) ) {
				if( PsychoConfigData._1_0_1_PsychoSpawnChance == this.PsychoSpawnChance ) {
					this.PsychoSpawnChance = new_config.PsychoSpawnChance;
				}
			}
			if( vers_since < new Version( 1, 1, 0 ) ) {
				if( PsychoConfigData._1_0_2_PsychoSpawnChance == this.PsychoSpawnChance ) {
					this.PsychoSpawnChance = new_config.PsychoSpawnChance;
				}
			}

			this.VersionSinceUpdate = PsychoConfigData.ConfigVersion.ToString();

			return true;
		}

		////////////////

		public string _OLD_SETTINGS_BELOW = "";

		public bool PsychoWardedByHeartLantern = true;

		public readonly static float _1_0_0_PsychoSpawnChance = 0.02f;
		public readonly static float _1_0_1_PsychoSpawnChance = 0.04f;
		public readonly static float _1_0_2_PsychoSpawnChance = 0.015f;
	}
}
