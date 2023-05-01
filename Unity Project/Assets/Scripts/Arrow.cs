using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float turnSmoothness = .15f;
    float movementRotationVel;

    void Update()
    {
        if(QuestHandler.instance.inQuest)
        {
            transform.GetChild(0).gameObject.SetActive(true);

            Vector3 toDestination = QuestHandler.instance.currentQuestDestination - transform.position;

            float angle = Mathf.Atan2(toDestination.x, toDestination.z) * Mathf.Rad2Deg;
            float movementAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref movementRotationVel, turnSmoothness);
            transform.rotation = Quaternion.Euler(0f, movementAngle, 0f);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
