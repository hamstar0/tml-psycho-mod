using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria.ID;
using Terraria.ModLoader.Config;


namespace Psycho {
	public class PsychoConfigData : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[DefaultValue(true)]
		public bool Enabled = true;


		public bool DebugModeInfo = false;


		[DefaultValue( true )]
		public bool AllPsychosAreInvincible = true;

		[DefaultValue( true )]
		public bool AllPsychosAlwaysInstaKill = true;


		[DefaultValue( (int)( 60f * 1.5f ) )]
		public int PsychoHealRate = (int)( 60f * 1.5f );

		[DefaultValue( 50 )]
		public int PsychoHealAmount = 50;


		[DefaultValue( 0.03f )]
		public float PsychoSpawnChance = 0.03f; //0.018f;

		[DefaultValue( true )]
		public bool PsychoCanDropLoot = true;


		public float ButcherSpawnChance = 0.0125f;

		[DefaultValue( true )]
		public bool ButcherCanDropLoot = true;


		[DefaultValue( 0.015f )]
		public float SniperSpawnChance = 0.015f;

		[DefaultValue( true )]
		public bool SniperJungleOnly = true;

		[DefaultValue( false )]
		public bool SniperCanDropLoot = false;

		[DefaultValue( 600 )]
		public int SniperHardModeDamage = 600;

		[DefaultValue( 400 )]
		public int SniperPreHardModeDamage = 400;

		[DefaultValue( 10 )]
		public int SniperSpawnHp = 10;

		[DefaultValue( 300 )]
		public int SniperSpawnArmor = 300;


		public int[] PsychoWardingNeedsBuffs = new int[0];

		public int[] ButcherWardingNeedsBuffs = new int[0];

		public int[] SniperWardingNeedsBuffs = new int[0];



		////////////////

		[OnDeserialized]
		internal void OnDeserializedMethod( StreamingContext context ) {
			if( this.PsychoWardingNeedsBuffs != null ) {
				return;
			}

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
	}
}
