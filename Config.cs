using HamstarHelpers.Classes.UI.ModConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria.ID;
using Terraria.ModLoader.Config;


namespace Psycho {
	class MyFloatInputElement : FloatInputElement { }




	public class LargeIntSetting {
		[Range( 0, 2000)]
		public int Value;
	}




	public class PsychoConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[DefaultValue(true)]
		public bool Enabled = true;


		[DefaultValue( false )]
		public bool DebugModeInfo = false;

		////

		[DefaultValue( true )]
		public bool AllPsychosAreInvincible = true;

		[DefaultValue( true )]
		public bool AllPsychosAlwaysInstaKill = true;


		[Range( 0, 60 * 60 )]
		[DefaultValue( (int)( 60f * 1.5f ) )]
		public int PsychoHealRate = (int)( 60f * 1.5f );

		[Range( 0, 1000 )]
		[DefaultValue( 50 )]
		public int PsychoHealAmount = 50;


		[Range( 0f, 1f )]
		[DefaultValue( 0.03f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float PsychoSpawnChance = 0.03f; //0.018f;

		[DefaultValue( true )]
		public bool PsychoCanDropLoot = true;


		[Range( 0f, 1f )]
		[DefaultValue( 0.0125f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ButcherSpawnChance = 0.0125f;

		[DefaultValue( true )]
		public bool ButcherCanDropLoot = true;


		[Range( 0f, 1f )]
		[DefaultValue( 0.015f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float SniperSpawnChance = 0.015f;

		[DefaultValue( true )]
		public bool SniperJungleOnly = true;

		[DefaultValue( false )]
		public bool SniperCanDropLoot = false;

		[Range( 1, 10000 )]
		[DefaultValue( 600 )]
		public int SniperHardModeDamage = 600;

		[Range( 1, 10000 )]
		[DefaultValue( 400 )]
		public int SniperPreHardModeDamage = 400;

		[Range( 1, 10000 )]
		[DefaultValue( 10 )]
		public int SniperSpawnHp = 10;

		[Range( 1, 10000 )]
		[DefaultValue( 300 )]
		public int SniperSpawnArmor = 300;


		public List<LargeIntSetting> PsychoWardingNeedsBuffTypes = new List<LargeIntSetting>() {
			//BuffID.HeartLamp,
			//BuffID.Sunflower,
			new LargeIntSetting{ Value = BuffID.StarInBottle }
			//BuffID.Campfire
		};

		public List<LargeIntSetting> ButcherWardingNeedsBuffTypes = new List<LargeIntSetting>() {
			//BuffID.HeartLamp,
			new LargeIntSetting{ Value = BuffID.StarInBottle }
		};

		public List<LargeIntSetting> SniperWardingNeedsBuffTypes = new List<LargeIntSetting>() {
			//BuffID.HeartLamp,
			new LargeIntSetting{ Value = BuffID.StarInBottle }
		};


		[Header( "\n \nOBSOLETE SETTINGS BELOW" )]
		public List<int> PsychoWardingNeedsBuffs = new List<int>() {
			//BuffID.HeartLamp,
			//BuffID.Sunflower,
			BuffID.StarInBottle
			//BuffID.Campfire
		};

		public List<int> ButcherWardingNeedsBuffs = new List<int>() {
			//BuffID.HeartLamp,
			BuffID.StarInBottle
		};

		public List<int> SniperWardingNeedsBuffs = new List<int>() {
			//BuffID.HeartLamp,
			BuffID.StarInBottle
		};



		////////////////

		public override ModConfig Clone() {
			var clone = (PsychoConfig)base.Clone();

			clone.PsychoWardingNeedsBuffTypes = new List<LargeIntSetting>( this.PsychoWardingNeedsBuffTypes );
			clone.ButcherWardingNeedsBuffTypes = new List<LargeIntSetting>( this.ButcherWardingNeedsBuffTypes );
			clone.SniperWardingNeedsBuffTypes = new List<LargeIntSetting>( this.SniperWardingNeedsBuffTypes );

			return clone;
		}
	}
}
