using System.Collections;
using System.Collections.Generic;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        quadrinho ++;
        anim.SetInteger("Q", quadrinho);
    }
}
