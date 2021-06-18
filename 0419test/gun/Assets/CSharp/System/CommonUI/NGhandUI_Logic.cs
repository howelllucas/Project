using UnityEngine;

namespace EZ {

	public partial class NGhandUI : MonoBehaviour {

        public void SetBtnTsf(Transform tsf)
        {
            AdaptNode.rectTransform.position = tsf.position;
            tsf.SetParent(BtnNode.rectTransform, true);
        }
	}
}
