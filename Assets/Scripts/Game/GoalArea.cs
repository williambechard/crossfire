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
                Target t = mb.GetComponent<Target>();
                t.DeActivate(targetPlayerId);
                //IDeactivate obj = (IDeactivate)mb;

                //obj.DeActivate(targetPlayerId);
            }

        }
    }
}
