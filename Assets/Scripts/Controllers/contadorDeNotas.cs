using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class contadorDeNotas : MonoBehaviour
{
    public HorizontalLayoutGroup layoutGroup;

    [SerializeField] private int quantidadePassos;

    public UnityEvent gameOver;

    private List<GameObject> partesBarra = new List<GameObject>();
    // private GameObject[] partesBarra;

    public GameObject parteBarraPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < quantidadePassos; i++)
        {
            GameObject instance = Instantiate(parteBarraPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            instance.transform.SetParent(transform, false);
            partesBarra.Add(instance);
        }
        //layoutGroup.enabled = false;
    }

    public void jogadorMoveu()
    {
        if (partesBarra.Any()) // checa se ainda há barrinhas na interface
        {
           // partesBarra[0].SetActive(false);
           partesBarra[0].GetComponent<Image>().enabled = false;
           partesBarra.Remove(partesBarra[0]);
        }
        else
        {
            gameOver?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
