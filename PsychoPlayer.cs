﻿using HamstarHelpers.Helpers.World;
using HamstarHelpers.Services.Timers;
using Microsoft.Xna.Framework;
using Psycho.PsychoNpcs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Psycho {
	class PsychoPlayer : ModPlayer {
		public static int NearestPsychoDist = -1;
		public static bool IsPsychoAlerted = false;



		////////////////

		public static bool IsWarding( Player player, IList<int> buffIds ) {
			var mymod = PsychoMod.Instance;

			if( buffIds.Count > 0 ) {
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
				this.HasEnteredWorld = true;
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
							string timerName = "PsychoNearSound";

							if( Timers.GetTimerTickDuration(timerName) <= 0 ) {
								Timers.SetTimer( timerName, 1, true, () => {
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
