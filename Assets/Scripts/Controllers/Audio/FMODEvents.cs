using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }

    [field: Header("Musica")]
    [field: SerializeField] public EventReference musica { get; private set; }
    
    [field: Header("MSTT")]
    [field: SerializeField] public EventReference MSTTGrave { get; private set; }
    [field: SerializeField] public EventReference MSTTAgudo { get; private set; }

    [field: Header("Narrações")]
    [field: SerializeField] public EventReference feedbackBotaoFaseGrave { get; private set; }
    [field: SerializeField] public EventReference feedbackBotaoFaseAgudo { get; private set; }
    [field: SerializeField] public EventReference fala { get; private set; }



    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Mais de um Audio Controller na cena.");
        }
        else
        {
            instance = this;
        }
    }


}
