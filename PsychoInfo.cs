using HamstarHelpers.TmlHelpers;
using HamstarHelpers.WorldHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Psycho {
	class PsychoInfo : AltNPCInfo {
		public static bool IsValidNpc( NPC npc ) {
			if( npc.type != NPCID.Psycho ) { return false; }
			var pos = npc.position;
			return !( WorldHelpers.IsAboveWorldSurface( pos ) || WorldHelpers.IsWithinUnderworld( pos ) );
		}

		public static bool CanSpawn( PsychoConfigData config, NPCSpawnInfo spawn_info ) {
			var pos = spawn_info.player.position;
			if( WorldHelpers.IsAboveWorldSurface( pos ) || WorldHelpers.IsWithinUnderworld( pos ) ) {
				return false;
			}

			foreach( int buff_id in config.PsychoWardingNeedsBuffs ) {
				if( spawn_info.player.FindBuffIndex(buff_id) == -1 ) {
					return true;
				}
			}
			
			return false;
		}



		////////////////

		public int HealTimer { get; private set; }


		////////////////

		public override bool CanInitialize( NPC npc ) {
			return PsychoInfo.IsValidNpc( npc );
		}

		public override void Initialize( NPC npc ) {
			npc.lavaImmune = true;
			this.HealTimer = 0;

			var mymod = (PsychoMod)ModLoader.GetMod( "Psycho" );
			if( mymod.Config.Data.IsDebugInfo() ) {
				Main.NewText( "Psycho "+npc.whoAmI+" spawned at " + npc.position );
			}
		}

		////////////////

		public void Update( PsychoConfigData config ) {
			int distance = 16 * 200;    // Proximity to underground player

			for( int i = 0; i < Main.player.Length; i++ ) {
				Player player = Main.player[i];
				if( player == null || !player.active ) { continue; }

				if( Math.Abs( Vector2.Distance( this.Npc.position, player.position ) ) <= distance ) {
					this.UpdateMe( config ); // At most once per frame, when relevant
					break;
				}
			}
		}


		private void UpdateMe( PsychoConfigData config ) {
			if( this.Npc.life < this.Npc.lifeMax ) {
				if( this.HealTimer >= config.PsychoHealRate ) {
					this.HealTimer = 0;
					this.HealMe( config );
				} else {
					this.HealTimer++;
				}
			} else {
				this.HealTimer = 0;
			}
		}


		public void HealMe( PsychoConfigData config ) {
			NPC npc = this.Npc;
			int new_life = Math.Min( npc.life + config.PsychoHealAmount, npc.lifeMax );
			int healed = new_life - npc.life;

			npc.life = new_life;

			if( healed > 0 ) {
				int ct = CombatText.NewText( npc.getRect(), Color.Green, "+" + healed );
				Main.combatText[ct].lifeTime = 60;
			}
		}
	}
}
