using UnityEngine;

public class Delivery : MonoBehaviour
{
    bool delivering = false;
    GameObject deliveryChild;

    public Transform deliveryObjectHolder;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("NPC") && !delivering)
        {
            collider.GetComponent<NPC>().OnAccepting();
            
            Transform crate = collider.gameObject.GetComponent<NPC>().crate;
            crate.parent = transform;
            crate.position = deliveryObjectHolder.position;
            crate.localRotation = Quaternion.identity;
            
            deliveryChild = crate.gameObject; 

            delivering = true;

            QuestHandler.instance.inQuest = true;
            QuestHandler.instance.currentQuestDestination = crate.gameObject.GetComponent<Crate>().destination;
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