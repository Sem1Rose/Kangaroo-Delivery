using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    public GameObject NPCPrefab;
    public GameObject destinationPrefab; 
    public static QuestGenerator instance;

    void Awake()
    {
        if(instance != null)
            Debug.LogError("more than one Quest Generator in the scene!");
        instance = this;
    }
    
    public void GenerateQuest()
    {
        Vector3 NPCPos = new Vector3(Random.Range(-100f, 100f), 1f, Random.Range(-100f, 100f));
        Vector3 destinationPos = new Vector3(Random.Range(-100f, 100f), 2f, Random.Range(-100f, 100f));

        GameObject NPC = Instantiate(NPCPrefab, NPCPos, Quaternion.identity);

        NPC.GetComponent<NPC>().crate.GetComponent<Crate>().destination = destinationPos;
    }
}
