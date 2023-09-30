using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardioes_controler : MonoBehaviour

{
    [SerializeField] private char notaMexe;
    private ParticleSystem sistema_particulas;
    [SerializeField] private Sprite spriteO, spriteI;
    // Start is called before the first frame update
    void Start()
    {
        sistema_particulas = this.GetComponent<ParticleSystem>();

        if (notaMexe == 'I') {
            sistema_particulas.textureSheetAnimation.SetSprite(0, spriteI);
        }

        else if (notaMexe == 'O')
        {
            sistema_particulas.textureSheetAnimation.SetSprite(0, spriteO);
        }        
    }
    void OnEnable()

    {
        GridController.NotePlayed += Guardioes_mexe;
    }
    void OnDisable()
    {
        GridController.NotePlayed -= Guardioes_mexe;
    }


    // Update is called once per frame
    void Guardioes_mexe(char note)
    {
        if (note == notaMexe)
        {
           sistema_particulas.Play();
        }
    }
}
