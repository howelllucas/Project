//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Dialogue : ScriptableObject {

		[SerializeField, HideInInspector]
		private DialogueItem[] _Items;
		public DialogueItem[] items { get { return _Items; } }

		public DialogueItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				DialogueItem item = _Items[index];
				if (item.id == id) { return item; }
				if (id < item.id) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class DialogueItem {

		[SerializeField, HideInInspector]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private int[] _Dialogues;
		public int[] dialogues { get { return _Dialogues; } }

		[SerializeField, HideInInspector]
		private string[] _Person1;
		public string[] person1 { get { return _Person1; } }

		[SerializeField, HideInInspector]
		private string[] _Person2;
		public string[] person2 { get { return _Person2; } }

		[SerializeField, HideInInspector]
		private int[] _SpeakPerson;
		public int[] speakPerson { get { return _SpeakPerson; } }

		[SerializeField, HideInInspector]
		private float _ActionDelay;
		public float actionDelay { get { return _ActionDelay; } }

		[SerializeField, HideInInspector]
		private float _StartDelay;
		public float startDelay { get { return _StartDelay; } }

		[SerializeField, HideInInspector]
		private float _LeastTime;
		public float leastTime { get { return _LeastTime; } }

		public override string ToString() {
			return string.Format("[DialogueItem]{{id:{0}, dialogues:{1}, person1:{2}, person2:{3}, speakPerson:{4}, actionDelay:{5}, startDelay:{6}, leastTime:{7}}}",
				id, array2string(dialogues), array2string(person1), array2string(person2), array2string(speakPerson), actionDelay, startDelay, leastTime);
		}

		private string array2string(System.Array array) {
			int len = array.Length;
			string[] strs = new string[len];
			for (int i = 0; i < len; i++) {
				strs[i] = string.Format("{0}", array.GetValue(i));
			}
			return string.Concat("[", string.Join(", ", strs), "]");
		}

	}

}
