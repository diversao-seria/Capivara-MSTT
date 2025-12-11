#Como utilizar as transições de portais

1. Adicionar os prefabs: portal, Canvas Transition e Transition Manager na cena.
2. Configurar o Transition Manager:
   - Definir o Canvas Transition no campo "Canvas Transition".
   - Definir o Image Transition no campo "Image Transition".
3. Ajustes o Portal:
	public float enterDuration = 0.5f;   // tempo do salto até o portal
    public float jumpHeight = 1.2f;      // altura do salto
    public float shrinkDuration = 0.4f;  // tempo para sumir
    public float stretchAmount = 1.2f;   // esticada inicial
    public float spinSpeed = 360f;       // giro opcional

