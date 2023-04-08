using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public void TriggerEvent(string str)
    {
        EventManager.TriggerEvent(str , null);
    }
}
