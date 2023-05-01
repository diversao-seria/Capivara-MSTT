using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Eventos : MonoBehaviour
{
    public UnityEvent entrouNoColisor;
    public UnityEvent saiuDoColisor;


    private void OnTriggerEnter(Collider other) {  

        entrouNoColisor.Invoke();

    }

    private void OnTriggerExit(Collider other){

        saiuDoColisor.Invoke();
        
    }
}
