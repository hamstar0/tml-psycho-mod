using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;


namespace Psycho.Sounds.Custom {
	public class PsychoNear : ModSound {
		public override SoundEffectInstance PlaySound( ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type ) {
			soundInstance.Volume = volume * 0.35f;
			soundInstance.Pan = pan;
			return soundInstance;
		}
	}
}
