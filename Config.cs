using HamstarHelpers.Components.Config;
using System;
using Terraria.ID;


namespace Psycho {
	public class PsychoConfigData : ConfigurationDataBase {
		public readonly static string ConfigFileName = "Psycho Config.json";


		////////////////

		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		public bool DebugModeInfo = false;

		public int PsychoHealRate = (int)( 60f * 1.5f );
		public int PsychoHealAmount = 50;

		public float PsychoSpawnChance = 0.03f; //0.018f;
		public bool PsychoCanDropLoot = true;
		
		public float ButcherSpawnChance = 0.0025f;
		public bool ButcherCanDropLoot = true;

		public float SniperSpawnChance = 0.015f;
		public bool SniperJungleOnly = true;
		public bool SniperCanDropLoot = false;

		public int[] PsychoWardingNeedsBuffs = new int[0];
		public int[] ButcherWardingNeedsBuffs = new int[0];
		public int[] SniperWardingNeedsBuffs = new int[0];
		


		////////////////

		private void SetDefaults() {
			this.PsychoWardingNeedsBuffs = new int[] {
				//BuffID.HeartLamp,
				//BuffID.Sunflower,
				BuffID.StarInBottle
				//BuffID.Campfire
			};

			this.ButcherWardingNeedsBuffs = new int[] {
				//BuffID.HeartLamp,
				BuffID.StarInBottle
			};

			this.SniperWardingNeedsBuffs = new int[] {
				//BuffID.HeartLamp,
				BuffID.StarInBottle
			};
		}
		

		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new PsychoConfigData();
			new_config.SetDefaults();

			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= PsychoMod.Instance.Version ) {
				return false;
			}

			if( this.VersionSinceUpdate == "" ) {
				this.SetDefaults();
			}

			if( vers_since < new Version(1, 3, 2, 1) ) {
				this.PsychoSpawnChance = new_config.PsychoSpawnChance;
			}

			if( vers_since < new Version(1, 4, 0) ) {
				this.SetDefaults();
			}
			if( vers_since < new Version( 1, 4, 0, 1 ) ) {
				this.ButcherSpawnChance = new_config.ButcherSpawnChance;
			}
			if( vers_since < new Version( 1, 4, 2 ) ) {
				if( this.ButcherSpawnChance == 0.005f ) {
					this.ButcherSpawnChance = new_config.ButcherSpawnChance;
				}
			}
			if( vers_since < new Version(1, 5, 0) ) {
				this.ButcherWardingNeedsBuffs = new_config.ButcherWardingNeedsBuffs;
				this.SniperWardingNeedsBuffs = new_config.SniperWardingNeedsBuffs;
			}

			this.VersionSinceUpdate = PsychoMod.Instance.Version.ToString();

			return true;
		}
	}
}
