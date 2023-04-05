using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    public Player P1;
    public Player P2;
    public SpawnTarget SpawnTarget1;
    public SpawnTarget SpawnTarget2;
    
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
    }

    private void Start()
    {
        Invoke("Init", 1f);
    }

    void Init()
    {
        Debug.Log("Init");
        // kick off init state
        SpawnTarget[] allSpawners = FindObjectsOfType<SpawnTarget>();
        
        foreach (SpawnTarget spawner in allSpawners)
        {
            switch (spawner.targetId)
            {
                case "1":
                    SpawnTarget1 = spawner;
                    break;
                case "2":
                    SpawnTarget2 = spawner;
                    break;
            }
            spawner.Spawn();
        }
        
        Player[] allPlayers = FindObjectsOfType<Player>();
        foreach (Player player in allPlayers)
        {
            switch (player.id)
            {
                case"1":
                    P1 = player;
                    break;
                case "2":
                    P2 = player;
                    break;
            }
        }

      
       

    }
}
