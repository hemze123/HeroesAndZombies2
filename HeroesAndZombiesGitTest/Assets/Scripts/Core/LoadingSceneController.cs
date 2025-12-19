using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    [Header("UI References")]
    public Slider progressBar;
    public TMP_Text progressText;

    private static string targetScene;

    // Dışarıdan çağrılacak
    public static void LoadScene(string sceneName)
    {
        targetScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        yield return new WaitForSeconds(0.2f); 

        AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            if (progressBar != null) progressBar.value = progress;
            if (progressText != null) progressText.text = (progress * 100f).ToString("F0") + "%";

            if (op.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.3f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
