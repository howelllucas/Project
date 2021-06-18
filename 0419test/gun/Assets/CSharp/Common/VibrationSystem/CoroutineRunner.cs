using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shining.VibrationSystem {

	public class CoroutineRunner : MonoBehaviour {

		public static void Start(IEnumerator routine) {
			CoroutineRunner ins = instance;
			if (ins == null) { return; }
			ins.StartCoroutine(routine);
		}

		private static CoroutineRunner s_instance;
		private static CoroutineRunner instance {
			get {
#if UNITY_EDITOR
				if (s_instance == null && Application.isPlaying)
#else
				if (s_instance == null)
#endif
				{
					GameObject go = new GameObject("CoroutineRunner");
					DontDestroyOnLoad(go);
					s_instance = go.AddComponent<CoroutineRunner>();
					s_instance.enabled = false;
				}
				return s_instance;
			}
		}

	}

}
