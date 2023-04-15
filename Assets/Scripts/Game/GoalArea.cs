using UnityEngine;

public class GoalArea : MonoBehaviour
{
    public string targetPlayerId;
    private void OnTriggerEnter(Collider other)
    {

        MonoBehaviour[] list = other.attachedRigidbody.gameObject.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour mb in list)
        {

            if (mb.CompareTag("Target"))
            {
                IDeactivate obj = (IDeactivate)mb;
                //if (obj.IsIDDependant)
                //{
                //    if (obj.Id == targetPlayerId)
                //        obj.DeActivate();
                // }
                // else
                obj.DeActivate(targetPlayerId);
            }

        }
    }
}
