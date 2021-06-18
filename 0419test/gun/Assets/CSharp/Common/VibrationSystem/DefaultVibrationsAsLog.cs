using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shining.VibrationSystem {

    public partial class Vibrations {

        class DefaultVibrationsAsLog : IVibrationSys {

			public bool HasVibrator() {
				Debug.Log("Check HasVibrator");
				return false;
			}

			public void Cancel() {
				Debug.Log("Stopped vibrations");
			}
            
            public bool HasAmplituideControl() {
                Debug.Log("Checked amplitude");
                return false;
            }

			public void Play(Pattern p) {
				Debug.Log("Played " + p);
			}

			public void Play(Vibe p) {
				//Debug.Log("Played single:" + p);
			}

        }

    }

}
