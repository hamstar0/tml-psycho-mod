using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Psycho.NetProtocols;
using Psycho.PsychoNpcs;
using Terraria;
using Terraria.ModLoader;


namespace Psycho {
	class PsychoPlayer : ModPlayer {
		public static int NearestPsychoDist = -1;
		public static bool IsPsychoAlerted = false;



		////////////////

		public static bool IsWarding( Player player, int[] buffIds ) {
			var mymod = PsychoMod.Instance;

			if( buffIds.Length > 0 ) {
				bool isWarding = false;

				foreach( int buffId in buffIds ) {
					int idx = player.FindBuffIndex( buffId );
					if( idx != -1 && player.buffTime[idx] >= 1 ) {
						isWarding = true;
					} else {
						isWarding = false;
						break;
					}
				}

				if( isWarding ) {
					return false;
				}
			}

			return false;
		}



		////////////////

		public bool HasEnteredWorld { get; internal set; }
		
		public override bool CloneNewInstances => false;



		////////////////

		public override void Initialize() {
			this.HasEnteredWorld = false;
		}

		////////////////

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (PsychoMod)this.mod;

			if( Main.netMode == 0 ) {
				if( !mymod.ConfigJson.LoadFile() ) {
					mymod.ConfigJson.SaveFile();
				}
			}

			if( Main.netMode == 0 ) {
				this.HasEnteredWorld = true;
			} else if( Main.netMode == 1 ) {
				PacketProtocolRequestToServer.QuickRequest<ModSettingsProtocol>( -1 );
			}
		}


		////////////////

		public override void PreUpdate() {
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			if( !WorldHelpers.IsAboveWorldSurface( this.player.position ) ) {
				if( !this.player.dead ) {
					if( Main.netMode != 2 ) {
						if( PsychoPlayer.IsPsychoAlerted && PsychoPlayer.NearestPsychoDist != -1 ) {
							float scale = MathHelper.Clamp( ((PsychoPlayer.NearestPsychoDist / PsychoNpc.AlertDistance) - 0.25f), 0f, 1f );

							if( Timers.GetTimerTickDuration( "PsychoNearSound" ) <= 0 ) {
								Timers.SetTimer( "PsychoNearSound", 1, () => {
									int soundSlot = this.mod.GetSoundSlot( SoundType.Custom, "Sounds/Custom/PsychoNear" );
									Main.PlaySound( (int)SoundType.Custom, -1, -1, soundSlot, 1f - scale );

									return false;
								} );
							}
						}
					}
				}
			}

			PsychoPlayer.IsPsychoAlerted = false;
			PsychoPlayer.NearestPsychoDist = -1;
		}
	}
}
