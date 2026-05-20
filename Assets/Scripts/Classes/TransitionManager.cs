using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [Header("Canvas de Transição")]
    public Canvas transitionCanvas;
    public Image fadeImage;

    [Header("Configurações")]
    public float transitionDuration = 1.5f;
    private string cutoffProperty = "_Cutoff";

    public float initialDistance = 0f;
    public float finalDistance = 2.3f;

    private bool isBusy = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (transitionCanvas != null)
            transitionCanvas.enabled = false;
    }


    public void StartTransition(string sceneName)
    {
        if (!isBusy)
            StartCoroutine(DoTransition(sceneName));
    }

    IEnumerator DoTransition(string sceneName)
    {
        isBusy = true;

        transitionCanvas.enabled = true;
        DontDestroyOnLoad(transitionCanvas.gameObject);
        DontDestroyOnLoad(gameObject);

        Material mat = fadeImage.material;
        float timer = 0f;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            //float value = Mathf.Lerp(0f, 3.3f, timer / transitionDuration);
            //mat.SetFloat(cutoffProperty, value);
            float value = Mathf.Lerp(initialDistance, finalDistance, timer /transitionDuration);
            Vector3 currentPosition = transitionCanvas.transform.localPosition;
            transitionCanvas.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, value);
            yield return null;
        }
        mat.SetFloat(cutoffProperty, 3.3f);

        yield return new WaitForSeconds(0.2f);

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        while (!load.isDone)
            yield return null;

        timer = 0f;
        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            //float value = Mathf.Lerp(3.3f, 0f, timer / transitionDuration);
            //mat.SetFloat(cutoffProperty, value);
            float value = Mathf.Lerp(finalDistance, initialDistance, timer /transitionDuration);
            print(value);
            Vector3 currentPosition = transitionCanvas.transform.localPosition;
            transitionCanvas.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, value);
            yield return null;
        }
        mat.SetFloat(cutoffProperty, 0f);

        Destroy(transitionCanvas.gameObject);
        
        Destroy(gameObject);
    }
}
