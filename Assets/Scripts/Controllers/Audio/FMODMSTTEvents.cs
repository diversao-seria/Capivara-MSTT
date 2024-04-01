using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODMSTTEvents : MonoBehaviour
{
    public static FMODMSTTEvents instance { get; private set; }
    

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Mais de um FMODEvents na cena.");
        }
        instance = this;
    }


}
