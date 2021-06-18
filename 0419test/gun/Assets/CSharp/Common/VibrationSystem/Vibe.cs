namespace Shining.VibrationSystem {

    public class Vibe {

        public long startDelayMs { get; private set; }
		public long durationMs { get; private set; }
        public int amplitude { get; private set; }

        private Vibe() { amplitude = 255; }

        public Vibe(long durationMs) {
            this.durationMs = durationMs;
			amplitude = 255;
		}

        public Vibe Amplitude(byte amplitude) {
            this.amplitude = amplitude;
            return this;
        }

        public Vibe Delay(long delayInPatternMs) {
            startDelayMs = delayInPatternMs;
            return this;
        }

		public override string ToString() {
			return startDelayMs > durationMs && amplitude >= 0 ? amplitude.ToString() : "Default";
		}

    }

}
