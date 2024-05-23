using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CTR_QUADRINHOS : MonoBehaviour
{
    public int quadrinho;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        quadrinho = 1;
    }

    public void Next()
    {
        if (quadrinho < 11)
        {
            quadrinho ++;
            anim.SetInteger("Q", quadrinho);
        }
        else
        {
            SceneManager.LoadScene("Associacao - Sons");
        }
        
    }
}
