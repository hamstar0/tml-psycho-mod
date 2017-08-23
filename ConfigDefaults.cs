using System;


namespace Psycho {
	public class ConfigurationData {
		public readonly static Version ConfigVersion = new Version( 1, 0, 2 );


		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		public float PsychoSpawnChance = 0.015f;
		public int PsychoHealRate = (int)(60f * 1.5f);
		public int PsychoHealAmount = 50;
		public bool PsychoCanDropLoot = true;



		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new ConfigurationData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= ConfigurationData.ConfigVersion ) {
				return false;
			}

			if( vers_since < new Version( 1, 0, 1 ) ) {
				if( ConfigurationData._1_0_0_PsychoSpawnChance == this.PsychoSpawnChance ) {
					this.PsychoSpawnChance = new_config.PsychoSpawnChance;
				}
			}
			if( vers_since < new Version( 1, 0, 2 ) ) {
				if( ConfigurationData._1_0_1_PsychoSpawnChance == this.PsychoSpawnChance ) {
					this.PsychoSpawnChance = new_config.PsychoSpawnChance;
				}
			}

			this.VersionSinceUpdate = ConfigurationData.ConfigVersion.ToString();

			return true;
		}

		////////////////

		public string _OLD_SETTINGS_BELOW = "";

		public readonly static float _1_0_1_PsychoSpawnChance = 0.04f;
		public readonly static float _1_0_0_PsychoSpawnChance = 0.02f;
	}
}
