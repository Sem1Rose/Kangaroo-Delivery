using UnityEngine;

public class Delivery : MonoBehaviour
{
    bool delivering = false;
    GameObject deliveryChild;

    public Transform deliveryObjectHolder;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Crate") && !delivering)
        {
            collider.transform.parent = transform;
            collider.transform.position = deliveryObjectHolder.position;
            collider.transform.localRotation = Quaternion.identity;
            
            deliveryChild = collider.gameObject; 

            delivering = true;

            QuestHandler.instance.inQuest = true;
            QuestHandler.instance.currentQuestDestination = collider.gameObject.GetComponent<Crate>().destination;
            QuestHandler.instance.UpdateQuest();    
        }
        else if(collider.gameObject.CompareTag("Destination"))
        {
            if(collider.GetComponent<Destination>().pos == QuestHandler.instance.currentQuestDestination && delivering)
            {
                delivering = false;

                QuestHandler.instance.inQuest = false;

                Destroy(deliveryChild);
                deliveryChild = null;
                Destroy(collider.gameObject);
            }
        }
    }
}