using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool isMoving = false;
    public float moveSpeed = 1.0f;
    private bool canMove = false;

    void SetupListener()
    {
        EventManager.StartListening("PlayerMove", Handle_PlayerMove);
    }

    private void OnEnable()
    {
        Invoke("SetupListener", .00001f);
    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
            EventManager.StopListening("PlayerMove", Handle_PlayerMove);
    }
    
    void Handle_PlayerMove(Dictionary<string, object> message)
    {
        if (canMove)
            {
                //up down, or left right, not both
                Vector2 v = (Vector2)message["value"];

                if (v.x != 0 && v.y != 0) v = new Vector2(v.x, 0);

                Vector2 move = (Vector2)transform.position + v;
 
            }
         
    }
}
