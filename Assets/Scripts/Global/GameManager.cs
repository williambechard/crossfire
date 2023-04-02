using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    public int currentFloor;
    public int Health = 3;
    public int Strength = 1;
    public float Speed = 1.75f;
    public float XP;

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
                    gameManager.Init();

                    //  Sets this to not be destroyed when reloading scene
                    DontDestroyOnLoad(gameManager);
                }
            }
            return gameManager;
        }
    }


    void Init()
    {
        currentFloor = 1;
    }
}
