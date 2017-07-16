using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonBehaviour : MonoBehaviour
{
    private Button button;

	[SerializeField]
	protected IntroFadeOut fadeScene;

	protected bool wasClicked = false;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
		if(!wasClicked)
		{
			fadeScene.startSequence();
			wasClicked = true;
		}
    }
}