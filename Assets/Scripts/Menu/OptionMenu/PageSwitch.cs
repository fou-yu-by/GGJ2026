using UnityEngine;

namespace Menu.OptionMenu
{
	public class PageSwitch : MonoBehaviour
	{
		[Header("Pages")]
		[SerializeField] private GameObject controlsPage;
		[SerializeField] private GameObject settingsPage;

		[Header("Defaults")]
		[SerializeField] private bool showControlsOnStart = true;

		private void Start()
		{
			if (showControlsOnStart)
			{
				ShowControlsPage();
			}
			else
			{
				ShowSettingsPage();
			}
		}

		public void ShowControlsPage()
		{
			SetPageVisible(controlsPage, true);
			SetPageVisible(settingsPage, false);
		}

		public void ShowSettingsPage()
		{
			SetPageVisible(controlsPage, false);
			SetPageVisible(settingsPage, true);
		}

		private void SetPageVisible(GameObject page, bool visible)
		{
			if (page == null)
			{
				return;
			}

			page.SetActive(visible);
		}
	}
}
