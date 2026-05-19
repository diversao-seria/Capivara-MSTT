using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Unity.VisualScripting;

[CreateAssetMenu]
public class FMODEvents : ScriptableObject
{
    // public static FMODEvents instance { get; private set; }

    [field: Header("Musica")]
    [field: SerializeField] public EventReference musica { get; private set; }
    
    [field: Header("MSTT")]
    [field: SerializeField] public EventReference MSTTGrave { get; private set; }
    [field: SerializeField] public EventReference MSTTAgudo { get; private set; }
    [field: SerializeField] public EventReference MSTTErro { get; private set; }
    [field: SerializeField] public EventReference MSTTAcerto { get; private set; }
    [field: SerializeField] public EventReference MSTTConfirma { get; private set; }

    [field: Header("Player")]
    [field: SerializeField] public EventReference anda_bloqueado { get; private set; }
    [field: SerializeField] public EventReference anda_grama { get; private set; }
    [field: SerializeField] public EventReference anda_plataforma_agudo { get; private set; }
    [field: SerializeField] public EventReference anda_plataforma_grave { get; private set; }

    [field: Header("Plataforma")]
    [field: SerializeField] public EventReference plataforma_grave { get; private set; }
    [field: SerializeField] public EventReference plataforma_agudo { get; private set; }

    [field: Header("Porta")]
    [field: SerializeField] public EventReference porta_abre_agudo { get; private set; }
    [field: SerializeField] public EventReference porta_abre_grave { get; private set; }
    [field: SerializeField] public EventReference porta_fecha_grave { get; private set; }
    [field: SerializeField] public EventReference porta_fecha_agudo { get; private set; }

    [field: Header("Instrumentos")]
    [field: SerializeField] public EventReference coleta_violao { get; private set; }
    [field: SerializeField] public EventReference coleta_flauta { get; private set; }
    [field: SerializeField] public EventReference coleta_teclado { get; private set; }
    [field: SerializeField] public EventReference coleta_baixo { get; private set; }
    [field: SerializeField] public EventReference coleta_bateria { get; private set; }

    [field: Header("Narrações")]
    [field: SerializeField] public EventReference feedbackBotaoFaseGrave { get; private set; }
    [field: SerializeField] public EventReference feedbackBotaoFaseAgudo { get; private set; }
    [field: SerializeField] public EventReference fala { get; private set; }



   /* private void Awake()
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
    */


}
