using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMainMenuButtonBehaviour : MonoBehaviour
{
    private Button button;

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
        SceneManager.LoadScene(0);
    }
}