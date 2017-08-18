using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using HamstarHelpers.WorldHelpers;


namespace Psycho {
	class PsychoInfo {
		public int HealTimer = 0;

		public PsychoInfo( NPC npc ) {
			npc.lavaImmune = true;
		}
	}



	class MyGlobalNpc : GlobalNPC {
		public static IDictionary<int, PsychoInfo> Spawned = new Dictionary<int, PsychoInfo>();
		

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawn_info ) {
			var pos = spawn_info.player.position;
			if( WorldHelpers.IsAboveWorldSurface( pos ) || WorldHelpers.IsWithinUnderworld( pos ) ) {
				return;
			}

			var mymod = (Psycho)this.mod;

			pool[NPCID.Psycho] = mymod.Config.Data.PsychoSpawnChance;
		}


		public override bool PreNPCLoot( NPC npc ) {
			if( npc.type == NPCID.Psycho ) {
				var mymod = (Psycho)this.mod;
				var npc_pos = npc.position;

				if( !WorldHelpers.IsAboveWorldSurface(npc_pos) && !WorldHelpers.IsWithinUnderworld(npc_pos) ) {
					return mymod.Config.Data.PsychoCanDropLoot;
				}
			}

			return base.PreNPCLoot( npc );
		}


		////////////////

		public static void UpdateAll( Player player ) {
			int distance = 16 * 200;    // Proximity to underground player
			
			for( int i=0; i<Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];

				if( npc == null || !npc.active || npc.type != NPCID.Psycho ) {
					if( MyGlobalNpc.Spawned.ContainsKey(i) ) {
						MyGlobalNpc.Spawned.Remove( i );
					}
					continue;
				}

				if( !MyGlobalNpc.Spawned.ContainsKey( i ) ) {
					var npc_pos = npc.position;

					if( WorldHelpers.IsAboveWorldSurface(npc_pos) || WorldHelpers.IsWithinUnderworld(npc_pos) ) {
						continue;
					}
					if( Math.Abs( Vector2.Distance( npc_pos, player.position ) ) > distance ) {
						continue;   // Not our business?
					}

					MyGlobalNpc.Spawned[i] = new PsychoInfo( npc );
				}

				var npc_info = npc.GetGlobalNPC<MyGlobalNpc>();
				npc_info.UpdateMe( npc );
			}
		}

		////////////////

		private void UpdateMe( NPC npc ) {
			var mymod = (Psycho)this.mod;
			var info = MyGlobalNpc.Spawned[ npc.whoAmI ];

			if( npc.life < npc.lifeMax ) {
				if( info.HealTimer >= mymod.Config.Data.PsychoHealRate ) {
					info.HealTimer = 0;
					this.HealMe( npc );
				} else {
					info.HealTimer++;
				}
			} else {
				info.HealTimer = 0;
			}
		}

		public void HealMe( NPC npc ) {
			var mymod = (Psycho)this.mod;
			int new_life = Math.Min( npc.life + mymod.Config.Data.PsychoHealAmount, npc.lifeMax );
			int healed = new_life - npc.life;

			npc.life = new_life;

			if( healed > 0 ) {
				int ct = CombatText.NewText( npc.getRect(), Color.Green, "+"+healed );
				Main.combatText[ct].lifeTime = 60;
			}
		}
	}
}
