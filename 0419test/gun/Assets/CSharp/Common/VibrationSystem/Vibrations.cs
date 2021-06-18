using EZ;
using System;
using UnityEngine;

namespace Shining.VibrationSystem {
	public partial class Vibrations {

		private static Vibrations s_instance;
		public static Vibrations instance {
			get {
				if (s_instance == null) {
					s_instance = new Vibrations();
				}
				return s_instance;
			}
		}

		private IVibrationSys mVibro;

		private Vibrations() {
			try {
#if UNITY_ANDROID && !UNITY_EDITOR
				mVibro = new AndroidVibrations();
#elif (UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
				mVibro = new IosVibrations();
#endif
				if (mVibro == null) {
					mVibro = new DefaultVibrationsAsLog();
					//Debug.Log("Initialized default log vibration system");
				}
			} catch (Exception ex) {
				throw new Exception("Vibrations constructor must be called after the Player is initialized", ex);
			}
		}

		public bool HasVibrator() {
			return mVibro.HasVibrator();
		}

		public bool HasAmplitudes() {
			return mVibro.HasAmplituideControl();
		}

		public void Vibrate(Vibe v) {
            if (Global.gApp.gAudioSource.vibe == 0)
            {
                return;
            }

            mVibro.Play(v);
		}

		public void Vibrate(Pattern p) {
            if (Global.gApp.gAudioSource.vibe == 0)
            {
                return;
            }

            mVibro.Play(p);
		}

		public void Vibrate30ms() {
            Vibrate(mVibe30ms);
		}

		public void Vibrate500ms() {
            Vibrate(mVibe500ms);
		}

        public void Vibrate1ms()
        {
            Vibrate(mVibe1ms);
        }

        public void Stop() {
			mVibro.Cancel();
		}

		public static Vibe Buzz(long durationMs) { return new Vibe(durationMs); }

		public static Pattern MakePattern(params Vibe[] vibes) { return new Pattern(vibes); }

		private Vibe mVibe30ms = new Vibe(30L);
		private Vibe mVibe500ms = new Vibe(500L);
        private Vibe mVibe1ms = new Vibe(1L);

    }

}
