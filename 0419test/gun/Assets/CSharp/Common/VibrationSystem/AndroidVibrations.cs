using System;
using UnityEngine;

namespace Shining.VibrationSystem {

	public partial class Vibrations {

#if UNITY_ANDROID

		private class AndroidVibrations : IVibrationSys {
			private const string androidBuildClassName = "android.os.Build$VERSION";
			private const string sdkVersionFieldName = "SDK_INT";
			private const int minHapticSDKVersion = 26;
			private const string hasVibratorMethodName = "hasVibrator";
			private const string hasAmplitudeControlMethodName = "hasAmplitudeControl";
			private const string vibrateOnceMethod = "createOneShot";
			private const string waveformVibrationMethod = "createWaveform";
			private const string vibrateMethod = "vibrate";
			private const string cancelVibrationMethodName = "cancel";

			private const int NotSet = -1;

			private AndroidJavaClass unityPlayer;
			private AndroidJavaObject vibrator;
			private AndroidJavaObject currentActivity;
			private AndroidJavaClass vibrationEffectClass;
			private int defaultAmplitude;

			private Action<Pattern> VibratePattern;
			private Action<Vibe> VibrateOnce;

			object[] parameters2 = new object[2];
			object[] parameters3 = new object[3];

			private int SDK_ver = NotSet;

			public AndroidVibrations() {
				unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
				vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
				if ((SDK_ver = getSDKInt()) >= minHapticSDKVersion) {
					Debug.Log("Vibrations: has haptic sdk version");
					var amp = HasAmplituideControl();
					Debug.Log("Vibrations: has amplitude control - " + amp);
					vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
					defaultAmplitude = vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE");

					VibrateOnce = HapticVibration;
					VibratePattern = HapticVibration;
				} else {
					Debug.Log("Vibrations: basics only");
					VibrateOnce = BasicVibration;
					VibratePattern = BasicVibration;
				}
				Debug.LogWarningFormat("SDK Version : {0} !", SDK_ver);
			}

			private void HapticVibration(Vibe vibe) {
				int amplitude = vibe.amplitude < 0 ? defaultAmplitude : vibe.amplitude;
				CreateVibrationEffect(vibrateOnceMethod, vibe.durationMs, amplitude);
			}

			private void BasicVibration(Vibe vibe) {
				OldVibrate(vibe);
			}

			private void Fallback(Vibe vibe) {
				Handheld.Vibrate();
			}

			private void HapticVibration(Pattern pattern) {
				CreateVibrationEffect(waveformVibrationMethod, pattern);
			}

			private void BasicVibration(Pattern pattern) {
				OldVibrate(pattern);
			}

			public void Play(Vibe vibe) {
				VibrateOnce(vibe);
			}

			public void Play(Pattern p) {
				VibratePattern(p);
			}

			private void CreateVibrationEffect(string function, Pattern p) {
				if (p.GetAmplitudes() == null) {
					parameters2[0] = p.GetTimings();
					parameters2[1] = p.GetRepeatIndex();
					CreateVibrationEffect(function, parameters2);
				} else {
					parameters3[0] = p.GetTimings();
					parameters3[1] = p.GetAmplitudes();
					parameters3[2] = p.GetRepeatIndex();
					CreateVibrationEffect(function, parameters3);
				}
			}

			private void CreateVibrationEffect(string function, params object[] args) {
				using (AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>(function, args)) {
					vibrator.Call(vibrateMethod, vibrationEffect);
				}
			}

			private void OldVibrate(Vibe v) {
				vibrator.Call(vibrateMethod, v.durationMs);
			}
			private void OldVibrate(Pattern p) {
				vibrator.Call(vibrateMethod, p.GetTimings(), p.GetRepeatIndex());
			}

			public bool HasVibrator() {
				return vibrator.Call<bool>(hasVibratorMethodName);
			}

			public bool HasAmplituideControl() {
				if (SDK_ver >= minHapticSDKVersion) {
					return vibrator.Call<bool>(hasAmplitudeControlMethodName);
				} else {
					return false;
				}

			}

			public void Cancel() {
				if (vibrator != null)
					vibrator.Call(cancelVibrationMethodName);
			}

			private static int getSDKInt() {
				using (var version = new AndroidJavaClass(androidBuildClassName)) {
					return version.GetStatic<int>(sdkVersionFieldName);
				}
			}
		}

#endif

	}

}
