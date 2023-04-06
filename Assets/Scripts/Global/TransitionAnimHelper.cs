using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimHelper : MonoBehaviour
{

    public void MiddleTransition()
    {
        LevelManager.Instance.MiddleTransition();
    }

    public void EndTransition()
    {
        LevelManager.Instance.EndTransition();
    }


}
