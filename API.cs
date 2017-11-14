namespace Psycho {
	public static class PsychoAPI {
		public static PsychoConfigData GetModSettings() {
			return PsychoMod.Instance.Config.Data;
		}
	}
}
