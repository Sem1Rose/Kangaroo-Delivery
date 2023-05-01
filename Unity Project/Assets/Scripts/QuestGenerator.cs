using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    public GameObject deliveryPrefab;
    public GameObject destinationPrefab; 
    
    void Start()
    {
        Vector3 deliveryPos = new Vector3(Random.Range(-20f, 20f), 1f, Random.Range(-20f, 20f));
        Vector3 destinationPos = new Vector3(Random.Range(-20f, 20f), 1f, Random.Range(-20f, 20f));

        GameObject delivery = Instantiate(deliveryPrefab, deliveryPos, Quaternion.identity);

        delivery.GetComponent<Crate>().destination = destinationPos;
    }
}
