using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public GameObject limiter;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;
    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            targetPos = target.transform.position;

            // prevent following under the scene
            if (targetPos.y < 0)
            {
                targetPos.y = 0;
            }

            if (targetPos.y > 2)
            {
                targetPos.y = 2;
            }

            if (targetPos.x > 19)
            {
                targetPos.x = 19;
            }

            if (targetPos.x < -8.75)
            {
                targetPos.x = -8.75f;
            }

            Vector3 targetDirection = (targetPos - posNoZ);

            interpVelocity = targetDirection.magnitude * 5f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.65f);

        }
    }
}
