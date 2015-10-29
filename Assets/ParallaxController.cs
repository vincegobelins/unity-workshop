using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour
{

    public float interpVelocity;
    public float velocity = 0;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;
    void Start()
    {
        targetPos = transform.position;
    }

    
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            targetPos = target.transform.position;

            targetPos.y = 0;

            Vector3 targetDirection = (targetPos - posNoZ);
            interpVelocity = targetDirection.magnitude * 5f;
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            // give a position to background 
            gameObject.transform.position = Vector3.Lerp(transform.position/velocity, targetPos + offset, 0.65f);

        }
    }
}
