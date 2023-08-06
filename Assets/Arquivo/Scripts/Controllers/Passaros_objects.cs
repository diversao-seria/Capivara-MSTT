using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Numero_passaros", menuName ="coletavel/novo coletavel" )] //Criar a opcao de ScriptableObject no menu de projects

public class Passaros_objects : ScriptableObject        //Mudei de MonoBehaviour para SriptableObject, pois queremos crirar um asset e nao um codigo comum
{
    public int Quantidade;                              // Variavel do numero de passaros

    
    
}
