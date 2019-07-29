using HamstarHelpers.Helpers.TModLoader.Mods;
using Terraria.ModLoader;


namespace Psycho {
    partial class PsychoMod : Mod {
		public static PsychoMod Instance { get; private set; }



		////////////////

		public PsychoConfigData Config => this.GetConfig<PsychoConfigData>();



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
