using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PortalEntrance : MonoBehaviour
{
    public float enterDuration = 0.5f;   // tempo do salto at� o portal
    public float jumpHeight = 1.2f;      // altura do salto
    public float shrinkDuration = 0.4f;  // tempo para sumir
    public float stretchAmount = 1.2f;   // esticada inicial
    public float spinSpeed = 360f;       // giro opcional
    public GameObject referencia;

    public GameObject player;

    private string cenaParaCarregar;
    private bool animating = false;

    public UnityEvent salvarDados;

    public void EntrarNoPortalComCena(string cena)
    {
        if (!animating)
        {
            cenaParaCarregar = cena;
            StartCoroutine(EntrarNoPortalCoroutine(player.transform));
        }
    }
    IEnumerator EntrarNoPortalCoroutine(Transform player)
    {
        animating = true;

        Vector3 startPos = player.position;
        Vector3 portalPos = referencia.transform.position;
        Vector3 originalScale = player.localScale;

        float t = 0;
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            float k = t / 0.15f;

            player.localScale = new Vector3(
                originalScale.x * (1 - k * 0.1f),
                originalScale.y * (1 + k * (stretchAmount - 1)),
                originalScale.z
            );

            yield return null;
        }

        t = 0;
        while (t < enterDuration)
        {
            t += Time.deltaTime;
            float k = t / enterDuration;

            player.position = Vector3.Lerp(startPos, portalPos, k);

            float jump = Mathf.Sin(k * Mathf.PI) * jumpHeight;
            player.position += Vector3.up * jump;

            player.Rotate(0, spinSpeed * Time.deltaTime, 0);

            yield return null;
        }

        t = 0;
        while (t < shrinkDuration)
        {
            t += Time.deltaTime;
            float k = t / shrinkDuration;

            player.localScale = Vector3.Lerp(originalScale, Vector3.zero, k);

            yield return null;
        }

        this.player.SetActive(false);

        animating = false;

        salvarDados?.Invoke();
        TransitionManager.Instance.StartTransition(cenaParaCarregar);
    }
}
