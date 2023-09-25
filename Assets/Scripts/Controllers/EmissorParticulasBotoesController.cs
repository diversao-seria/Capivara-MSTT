using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissorParticulasBotoesController : MonoBehaviour
{  
    [SerializeField] private ParticleSystem sistema_particulas;
    [SerializeField] private Sprite spriteO, spriteI;
    [SerializeField] private Vector2Reference posPlayer;
    public Vector2 posicao;
    public char notaMexe;
    void Start()
    {
        if (notaMexe == 'I') 
        {
            sistema_particulas.textureSheetAnimation.SetSprite(0, spriteI);
        }

        else if (notaMexe == 'O')
        {
            sistema_particulas.textureSheetAnimation.SetSprite(0, spriteO);
        }
    }

    public void JogadorMoveu()
    {
        if (posicao == posPlayer.Value)
        {
            sistema_particulas.Play();
        }
    }
}
