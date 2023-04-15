using UnityEngine;

public class GameManager : MonoBehaviour
{


    public static GameManager instance;

    public Fusion.PlayerRef P1Ref;
    public Fusion.PlayerRef P2Ref;
    public Player P1;
    public Player P2;
    public SpawnTarget SpawnTarget1;
    public SpawnTarget SpawnTarget2;
    public CountDown CountDownPrefab;
    public Vector3 P1Position = new Vector3(0, .5f, 37);
    public Vector3 P2Position = new Vector3(0, .5f, -37);
    public GameObject PlayerPrefab;


    private void Awake()
    {
        CreateSingleton();
    }

    void CreateSingleton()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    public enum GameState
    {
        Init,
        Playing,
        Paused,
        GameOver
    }

    private GameState _currentState;

    public GameState CurrentState
    {
        get { return _currentState; }
        set
        {
            _currentState = value;
            switch (value)
            {
                case GameState.Init:
                    if (EventManager.instance != null)
                        EventManager.TriggerEvent("Init", null);
                    break;
                case GameState.Playing:
                    if (P1) P1.CanMove = true;
                    if (P2) P2.CanMove = true;
                    break;
                case GameState.Paused:
                    break;
                case GameState.GameOver:
                    break;
            }
        }
    }

    /*
    public static GameManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!gameManager)
                {
                    Debug.LogError(" No game manager!");
                }
                else
                {
                    //  Sets this to not be destroyed when reloading scene
                    DontDestroyOnLoad(gameManager);
                }
            }
            return gameManager;
        }
    }*/

    private void Start()
    {

        Invoke("Init", 3f);
    }

    void Init()
    {
        Debug.Log("INIT CALLED");
        //Asssign spawners to their global game manager counterparts
        SpawnTarget[] allSpawners = FindObjectsOfType<SpawnTarget>();
        foreach (SpawnTarget spawner in allSpawners)
        {
            switch (spawner.targetId)
            {
                case "1":
                    Debug.Log("Spawning Target 1");
                    SpawnTarget1 = spawner;
                    break;
                case "2":
                    Debug.Log("Spawning Target 2");
                    SpawnTarget2 = spawner;
                    break;
            }
            spawner.Spawn();
        }


        //Asssign players to their global game manager counterparts
        Player[] allPlayers = FindObjectsOfType<Player>();
        foreach (Player player in allPlayers)
        {
            switch (player.id)
            {
                case "1":
                    P1 = player;
                    break;
                case "2":
                    P2 = player;
                    break;
            }
            //Ensure players are frozen until we are in the playing state
            player.CanMove = false;
        }

        CurrentState = GameState.Init;

    }
}
