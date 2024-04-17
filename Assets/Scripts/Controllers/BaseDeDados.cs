using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using UnityEngine.SceneManagement;

// Desabilitamos essa biblioteca por causar erros na build. Habilitar caso dê problemas.
// using UnityEditor.SearchService;


public class BaseDeDados : MonoBehaviour
{
    public static BaseDeDados instance;

    //banco de dados
    MongoClient client = new MongoClient("mongodb+srv://userDS:DScapivara2023@cluster0.s7umrfl.mongodb.net/login?retryWrites=true&w=majority");
    
    
    IMongoDatabase database;    

    //dados a serem coletados
    public List<string> InfoMSTT_jogo;
    public List<string> InfoFase_jogo;    
    public string TempoTotal_jogo;

    public int Fase=0;
    public string TempoNaFase;
    public IntReference tentativas;

    public StringReference RespostaMSTT;
    public string TempoDeRespostaMSTT;

#region CRONOMETROS
    //cronometro Geral
    public bool Cronometro;
    float segundos;
    int min;
    int horas;

    //cronometro MSTT
    public bool CronometroMSTT;
    float segundosMSTT;
    int minMSTT;
    int horasMSTT;

    //cronometro Fase
    public bool CronometroFase;
    float segundosFase;
    int minFase;
    int horasFase;

    // Gerenciar cenas
    private string nomeCenaAnterior;

#endregion

    
    

    // Start is called before the first frame update
    void Start()
    {       

        database = client.GetDatabase("capivara");
        AtivarCronometro();        
        nomeCenaAnterior = SceneManager.GetActiveScene().name;
    }

    public void Awake()
    {


        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }



    }


    void FixedUpdate()
    {
#region Cronometro
        if (Cronometro)
        {
            segundos += Time.deltaTime;
        }

        if (segundos >= 60)
        {
            min++;
            segundos = 0;
        }
        if (min >= 60)
        {
            horas++;
            min = 0;
        }
#endregion

#region CronometroMSTT
        if (CronometroMSTT)
        {
            segundosMSTT += Time.deltaTime;
        }

        if (segundosMSTT >= 60)
        {
            minMSTT++;
            segundosMSTT = 0;
        }
        if (minMSTT >= 60)
        {
            horasMSTT++;
            minMSTT = 0;
        }
#endregion

#region CronometroFase
        if (CronometroFase)
        {
            segundosFase += Time.deltaTime;
        }

        if (segundosFase >= 60)
        {
            minFase++;
            segundosFase = 0;
        }
        if (minFase >= 60)
        {
            horasFase++;
            minFase = 0;
        }
#endregion

    }


    public void Salvar()
    {   
        PausarCronometro();     
        AtualizarTempoTotal();
        SalvarDadosMongo();
        ZerarCronometro();
    }

#region Controle Cronometro
    private void ZerarCronometro()
    {
        segundos = 0;
        min = 0;
        horas = 0;
    }

    private void PausarCronometro()
    {
        Cronometro = false;
    }
    private void AtivarCronometro()
    {
        Cronometro = true;
    }
#endregion

#region Controle CronometroMSTT
    private void ZerarCronometroMSTT()
    {
        segundosMSTT = 0;
        minMSTT = 0;
        horasMSTT = 0;
    }

    private void PausarCronometroMSTT()
    {
        CronometroMSTT = false;
    }
    private void AtivarCronometroMSTT()
    {
        CronometroMSTT = true;
    }
#endregion

#region Controle CronometroFase
    private void ZerarCronometroFase()
    {
        segundosFase = 0;
        minFase = 0;
        horasFase = 0;
    }

    private void PausarCronometroFase()
    {
        CronometroFase = false;
    }
    private void AtivarCronometroFase()
    {
        CronometroFase = true;
    }
#endregion

#region Atualizar tempos
    private void AtualizarTempoTotal()
    {
        string time = System.DateTime.UtcNow.ToLocalTime().ToString();
        if (horas <= 1)
        {
            if (horas == 0)
            {
                 TempoTotal_jogo = time + " Tempo de jogo = " + min.ToString() + "min e " + segundos.ToString("00") + "s";
            }
            else
            {
                TempoTotal_jogo = time + " Tempo de jogo = " + horas.ToString() + "h e " + min.ToString() + "min";
            }

        }
        else
        {
            TempoTotal_jogo = time + " Tempo de jogo = " + horas.ToString() + "hrs e " + min.ToString() + "min";
        }

        print(TempoTotal_jogo);
    }

    private void AtualizarTempoMSTT()
    {
        
        if (horas <= 1)
        {
            if (horas == 0)
            {
                TempoDeRespostaMSTT = minMSTT.ToString() + "min e " + segundosMSTT.ToString("00") + "s";
            }
            else
            {
                TempoDeRespostaMSTT = horasMSTT.ToString() + "h e " + minMSTT.ToString() + "min";
            }

        }
        else
        {
            TempoDeRespostaMSTT = horasMSTT.ToString() + "hrs e " + minMSTT.ToString() + "min";
        }
        
    }

    private void AtualizarTempoFase()
    {

        if (horas <= 1)
        {
            if (horas == 0)
            {
                TempoNaFase = minFase.ToString() + "min e " + segundosFase.ToString("00") + "s";
            }
            else
            {
                TempoNaFase = horasFase.ToString() + "h e " + minFase.ToString() + "min";
            }

        }
        else
        {
            TempoNaFase = horasFase.ToString() + "hrs e " + minFase.ToString() + "min";
        }

    }
#endregion

    public void onGameOver()
    {
        tentativas.Value += 1;
    }

    public void jogadorVenceuNivel()
    {
        AtualizarInfoFase();
                
    }

    public void AtualizarFase()
    {
        if (nomeCenaAnterior != SceneManager.GetActiveScene().name)
        {
            Fase += 1;
            nomeCenaAnterior = SceneManager.GetActiveScene().name;
            tentativas.Value = 1;
            AtivarCronometroFase();
        }       
    }

    public void IniciarMSTT()
    {
        AtivarCronometroMSTT();        
    }

    private void AtualizarInfoFase()
    {
        Debug.Log(tentativas.Value);
        PausarCronometroFase();
        AtualizarTempoFase();        
        string time = System.DateTime.UtcNow.ToLocalTime().ToString();
        var infoFase = time + " Fase: " + Fase.ToString() + " Tempo da fase: " + TempoNaFase + " Tentativas: " + tentativas.Value.ToString();
        InfoFase_jogo.Add(infoFase);
        ZerarCronometroFase();    
    }

    public void AtualizarInfoMSTT()
    {
        // PausarCronometroMSTT();
        AtualizarTempoMSTT();
        string time = System.DateTime.UtcNow.ToLocalTime().ToString();
        var infoMSTT = time + " Fase: " + Fase.ToString() + " Resposta inserida: " + RespostaMSTT.Value + " Resposta esperada: " + PlayerPrefs.GetString("sequence") + " Tempo de resposta: " + TempoDeRespostaMSTT;
        InfoMSTT_jogo.Add(infoMSTT);
        ZerarCronometroMSTT();       
    }

    public void FimMSTT()
    {
        // AtualizarInfoMSTT();
        PausarCronometroMSTT();
    }

   

#region Controle dos dados

    //dados a serem armazenados
    public class Dados
    {
        public ObjectId _id { get; set; }      
        public List<string> InfoMSTT { get; set; }        
        public List<string> InfoFase { get; set; }        
        public string TempoDeJogo { get; set; }   
        
    }

    private void SalvarDadosMongo()
    {
       // string time = System.DateTime.UtcNow.ToLocalTime().ToString();

        //dados a serem armazenados = dados coletados
        var dados = new Dados { InfoMSTT = InfoMSTT_jogo, InfoFase = InfoFase_jogo, TempoDeJogo = TempoTotal_jogo };

        inserirRecord(dados);
        print("dados salvos");
        print("ID:"+ dados._id);
        Debug.Log("InfoMSTT: ");
        for (int i = 0; i < InfoMSTT_jogo.Count; i++)
        {
            Debug.Log(InfoMSTT_jogo[i]);
        }
        Debug.Log("InfoFase: ");
        for (int i = 0; i < InfoFase_jogo.Count; i++)
        {
            Debug.Log(InfoFase_jogo[i]);
        }
        Debug.Log("Tempo total de jogo: " + TempoTotal_jogo + "ID: "+ dados._id);
    }

    private async void inserirRecord<T>(T record)
    {
        var collection = database.GetCollection<T>("dados");
        await collection.InsertOneAsync(record);
    }
#endregion
}