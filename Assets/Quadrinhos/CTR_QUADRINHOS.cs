using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CTR_QUADRINHOS : MonoBehaviour
{
    public int quadrinho;
    public int quadrinhoFinal;
    public Animator anim;
    public Button botao;

    // Start is called before the first frame update
    void Start()
    {
        quadrinho = 1;
    }

    public void Next()
    {
        if (quadrinho < quadrinhoFinal)
        {
            quadrinho ++;
            StartCoroutine(CooldownBotao(botao.GetComponent<Button>()));
            anim.SetInteger("Q", quadrinho);
        }
        else
        {
            SceneManager.LoadScene("Associacao - Sons");
        }
    }

    IEnumerator CooldownBotao(Button botao)
    {
        botao.interactable = false;
        yield return new WaitForSeconds(1f);
        botao.interactable = true;
    }
}
