using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace Shining.VibrationSystem {

	public partial class Vibrations {


#if UNITY_IOS || UNITY_IPHONE

		private class IosVibrations : IVibrationSys {

			[DllImport("__Internal")]
			private static extern bool _HasVibrator();

			[DllImport("__Internal")]
			private static extern void _Vibrate();

			[DllImport("__Internal")]
			private static extern void _VibratePop();

			[DllImport("__Internal")]
			private static extern void _VibratePeek();

			[DllImport("__Internal")]
			private static extern void _VibrateNope();

			private bool mVibrating = false;

			public void Cancel() {
				mVibrating = false;
			}

			public bool HasAmplituideControl() {
				return false;
			}

			public bool HasVibrator() {
				return _HasVibrator();
			}

			public void Play(Pattern p) {
				if (!_HasVibrator()) { return; }
				mVibrating = true;
				CoroutineRunner.Start(PlayPattern(p));
			}

			public void Play(Vibe p) {
				if (!_HasVibrator()) { return; }
				mVibrating = true;
				CoroutineRunner.Start(PlayVibe(p));
			}

			private IEnumerator PlayPattern(Pattern p) {
				long[] timings = p.GetTimings();
				int loopAt = p.GetRepeatIndex();
				bool loop = loopAt >= 0;
				int start = loopAt < 0 ? 0 : loopAt;
				int len = timings.Length >> 1;
				while (mVibrating) {
					for (int i = start; i < len; i++) {
						int ii = i << 1;
						long delay = timings[ii];
						if (delay > 0L) {
							yield return new WaitForSecondsRealtime(delay * 0.001f);
							if (!mVibrating) { break; }
						}
						long dur = timings[ii + 1];
						if (dur >= 500L) {
							_Vibrate();
						} else if (dur >= 100L) {
							_VibratePop();
						} else {
							_VibratePeek();
						}
						yield return new WaitForSecondsRealtime(dur * 0.001f);
					}
					if (!loop) { break; }
					start = 0;
				}
			}

			private IEnumerator PlayVibe(Vibe p) {
				if (p.startDelayMs > 0L) {
					yield return new WaitForSecondsRealtime(p.startDelayMs * 0.001f);
					if (!mVibrating) { yield break; }
				}
				if (p.durationMs >= 500L) {
					_Vibrate();
				} else if (p.durationMs >= 100L) {
					_VibratePop();
				} else {
					_VibratePeek();
				}
			}

		}

#endif

	}

}
