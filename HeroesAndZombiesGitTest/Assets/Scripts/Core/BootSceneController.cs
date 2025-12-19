using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BootSceneController : MonoBehaviour
{
    [SerializeField] private float splashDuration = 3f; // nece  saniye görünsün

    private void Start()
    {
        StartCoroutine(LoadMenu());
    }

    private IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(splashDuration);


        SceneManager.LoadScene("Menu");
    }
}
