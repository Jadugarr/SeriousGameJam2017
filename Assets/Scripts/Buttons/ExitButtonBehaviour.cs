using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExitButtonBehaviour : MonoBehaviour
{
    private Button button;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnExitButtonClicked);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(OnExitButtonClicked);
    }

    private void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}