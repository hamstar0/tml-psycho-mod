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

		public float ButcherSpawnChance = 0.0075f;
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
			var newConfig = new PsychoConfigData();
			newConfig.SetDefaults();

			var versSince = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( versSince >= PsychoMod.Instance.Version ) {
				return false;
			}

			if( this.VersionSinceUpdate == "" ) {
				this.SetDefaults();
			}

			if( versSince < new Version(1, 3, 2, 1) ) {
				this.PsychoSpawnChance = newConfig.PsychoSpawnChance;
			}

			if( versSince < new Version(1, 4, 0) ) {
				this.SetDefaults();
			}
			if( versSince < new Version( 1, 4, 0, 1 ) ) {
				this.ButcherSpawnChance = newConfig.ButcherSpawnChance;
			}
			if( versSince < new Version( 1, 4, 2 ) ) {
				if( this.ButcherSpawnChance == 0.005f ) {
					this.ButcherSpawnChance = newConfig.ButcherSpawnChance;
				}
			}
			if( versSince < new Version(1, 5, 0) ) {
				this.ButcherWardingNeedsBuffs = newConfig.ButcherWardingNeedsBuffs;
				this.SniperWardingNeedsBuffs = newConfig.SniperWardingNeedsBuffs;
			}
			if( versSince < new Version( 1, 5, 2 ) ) {
				if( this.ButcherSpawnChance == 0.0025f ) {
					this.ButcherSpawnChance = newConfig.ButcherSpawnChance;
				}
			}
			if( versSince < new Version( 1, 5, 2, 1 ) ) {
				if( this.ButcherSpawnChance == 0.05f ) {
					this.ButcherSpawnChance = newConfig.ButcherSpawnChance;
				}
			}
			if( versSince < new Version( 1, 5, 3 ) ) {
				if( this.ButcherSpawnChance == 0.005f ) {
					this.ButcherSpawnChance = newConfig.ButcherSpawnChance;
				}
			}

			this.VersionSinceUpdate = PsychoMod.Instance.Version.ToString();

			return true;
		}
	}
}
