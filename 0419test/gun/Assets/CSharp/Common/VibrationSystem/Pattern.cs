namespace Shining.VibrationSystem {

	public class Pattern {

		Vibe[] vibes;
		private long[] timings;
		private int[] amplitudes;
		private int repeatIndex = -1;

		private Pattern() { }

		public Pattern(params Vibe[] vibes) {
			this.vibes = vibes;

			timings = new long[vibes.Length * 2];
			amplitudes = new int[vibes.Length * 2];
			int amplitudesCount = 0;
			for (int i = 0; i < vibes.Length; i++) {
				int delayIndex = i << 1;
				int valueIndex = delayIndex + 1;
				Vibe vibe = vibes[i];
				timings[delayIndex] = vibe.startDelayMs;
				timings[valueIndex] = vibe.durationMs;
				if (vibe.amplitude >= 0) {
					amplitudesCount++;
				}
				amplitudes[delayIndex] = 0;
				amplitudes[valueIndex] = vibe.amplitude;
			}
			if (amplitudesCount == 0) { amplitudes = null; }
		}

		public Pattern LoopAt(int repeatIndex) {
			this.repeatIndex = repeatIndex;
			return this;
		}

		public long[] GetTimings() { return timings; }
		public int[] GetAmplitudes() { return amplitudes; }
		public int GetRepeatIndex() { return repeatIndex; }

		public override string ToString() {
			var res = "Pattern of " + vibes.Length + " Elements: \n";
			for (int i = 0; i < vibes.Length; i++) {
				res += (i) + ". " + vibes[i].ToString() +
					((i == repeatIndex) ? " <Looped here" : "") + "\n";
			}
			return res;
		}

	}

}
