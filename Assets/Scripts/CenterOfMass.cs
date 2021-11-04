using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CenterOfMass : MonoBehaviour
{
    public Vector2 centerOfMass;
    public bool awake;
    protected Rigidbody2D r;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        r.centerOfMass = centerOfMass;
        r.WakeUp();
        awake = !r.IsSleeping();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.rotation * centerOfMass, 1f);
    }
}
