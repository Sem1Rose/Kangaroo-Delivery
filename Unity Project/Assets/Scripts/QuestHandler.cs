using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    GameObject questDestinationObject;

    public GameObject destinationPrefab;
    public bool inQuest = false;
    public Vector3 currentQuestDestination;
    
    public static QuestHandler instance;

    void Awake()
    {
        if(instance != null)
            Debug.LogError("more than one Quest Handler in the scene");
        instance = this;
    }

    void Update()
    {
        if(!FindAnyObjectByType<Crate>())
        {
            QuestGenerator.instance.GenerateQuest();
        }
    }

    public void UpdateQuest()
    {
        if(questDestinationObject != null)
        {
            Destroy(questDestinationObject);
            questDestinationObject = null;
        }

        questDestinationObject = Instantiate(destinationPrefab, currentQuestDestination, Quaternion.identity);
        questDestinationObject.GetComponent<Destination>().pos = currentQuestDestination; 
    }
}