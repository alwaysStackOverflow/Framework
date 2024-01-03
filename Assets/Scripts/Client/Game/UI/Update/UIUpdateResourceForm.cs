using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class UIUpdateResourceForm : MonoBehaviour
    {
		[SerializeField]
		private Text _descriptionText = null;

		[SerializeField]
		private Slider _progressSlider = null;

		public void SetProgress(float progress, string description)
		{
			_progressSlider.value = progress;
			_descriptionText.text = description;
		}
	}
}
