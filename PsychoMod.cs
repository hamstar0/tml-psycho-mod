using HamstarHelpers.Helpers.TModLoader.Mods;
using Terraria.ModLoader;


namespace Psycho {
    partial class PsychoMod : Mod {
		public static PsychoMod Instance { get; private set; }



		////////////////

		public PsychoConfig Config => ModContent.GetInstance<PsychoConfig>();



		////////////////

		public PsychoMod() {
			PsychoMod.Instance = this;
		}

		////////////////

		public override void Load() {
		}

		public override void Unload() {
			PsychoMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof(PsychoAPI), args );
		}
	}
}
