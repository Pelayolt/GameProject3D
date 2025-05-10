using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    private float myTime = 0.0f;
    private int score = 0;
    private int enemyChain = 0;
    private float cooldown = 20.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() //se crea la clase ScoreSystem como un Singleton
    {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        }else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        myTime += Time.deltaTime; //se actualiza el tiempo que ha pasado desde el inicio del nivel
        if (cooldown > 0.0f){
            cooldown-= Time.deltaTime; //una vez se inicia una cadena de enemigos se produce un periodo de cooldown hasta que deja de poderse encadenar puntos
            if (cooldown <= 0.0f){
                enemyChain = 0;
            }
        }
    }

    void ResetScore(){ //función para reiniciar la puntuación, ya que se va a reutilizar para cada distinta fase
        score = 0;
        myTime = 0f;
    }

    void EnemyKilled(){
        enemyChain++;
        cooldown = 20.0f;
        score += 100*enemyChain;
    }

    void DamageFelt(){
        score -= 50;
        if (score < 0)
            score = 0;
    }

    void Healed(){
        score += 25;
    }

    void CalculateScore(){ //cálculo final de la puntuación
        int pointPool = 5000;
        score += pointPool;
        score-= (int)(myTime*10); //se resta a la puntuación final el tiempo que se tarda en terminiar el nivel, teniendo en cuenta hasta los decimales de segundo
    }
}
