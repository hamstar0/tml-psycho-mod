using System;


namespace Psycho {
	public class ConfigurationData {
		public readonly static Version ConfigVersion = new Version( 1, 0, 0 );


		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		public float PsychoSpawnChance = 0.02f;
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
			
			this.VersionSinceUpdate = ConfigurationData.ConfigVersion.ToString();

			return true;
		}
	}
}
