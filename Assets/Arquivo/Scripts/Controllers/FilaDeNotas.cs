using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class FilaDeNotas : MonoBehaviour
{
    char note;

    public GameObject canvas; // Canvas reference

    public StringReference notesSO; // List of notes Importante terminar com 3 Zs.

    public int contadorNotas = 0; // contador que lida com a posicao da lista estamos

    public Sprite noteI; // Sprite for the I note
    public Sprite noteO; // Sprite for the O note
    public Sprite noteZ; // Sprite for the Z note

    // Create a dictionary to store the UI images for each note
    private Dictionary<string, Sprite> noteSprites = new Dictionary<string, Sprite>();

    // evento emitido ao tocar uma nota. Passa a nota tocada como parametro do tipo Char
    public static event Action<char> NotePlayed;

    // evento emitido ao reiniciar o jogo após o esgotamento da quantidade de notas disponível
    public UnityEvent gameOver;


    void Awake()
    {
        notesSO.UseConstant = true;
        notesSO.Value = notesSO.Value.ToUpper(); // Make sure the notes are uppercase
        notesSO.UseConstant = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        noteSprites.Add("I", noteI); // Load the I Sprite
        noteSprites.Add("O", noteO); // Load the O Sprite
        noteSprites.Add("Z", noteZ); // Load the Z Sprite

        UpdateNotes(); // Set the first notes on the slots     
    }

    // Method to listen to the event of the player moving
    public void OnPlayerMoved()
    {
        note = notesSO.Value[contadorNotas]; // Get the first note from the string

        //

        if(note == 'Z') // If the note is Z, it means that the player failed to solve the puzzle
        {
            contadorNotas = 0;
            gameOver?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Muda o contador de notas para a proxima
        contadorNotas += 1;

        //Emite o evento responsavel por informar que a nota foi tocada
        NotePlayed?.Invoke(note);

        UpdateNotes(); // Update the slots
        // TODO: Atualmente os sprites apenas são trocados, mas não há uma animação ou alguma pista visual
        // que ajude no feedback para que o jogador entenda que as notas estão "andando" na fila
    }

    // Method to change the sprite from the slots
    private void UpdateNotes ()
    {
        String currentNotes = notesSO.Value;

        // Get the slots from canvas
        GameObject slot3 = canvas.transform.Find("Slot1").gameObject;
        GameObject slot2 = canvas.transform.Find("Slot2").gameObject;
        GameObject slot1 = canvas.transform.Find("Slot3").gameObject;

        // if the string is smaller than 3, pad it with Z
        if ((currentNotes.Length - contadorNotas) < 3)
        {
            currentNotes = currentNotes.PadRight(3, 'Z');
        }

        // Change the images of the slots
        slot1.GetComponent<Image>().sprite = noteSprites[currentNotes[contadorNotas].ToString()];
        slot2.GetComponent<Image>().sprite = noteSprites[currentNotes[contadorNotas+1].ToString()];
        slot3.GetComponent<Image>().sprite = noteSprites[currentNotes[contadorNotas+2].ToString()];

        // Change the image of the slots based on the note string (I, O or Z)
    }
}
