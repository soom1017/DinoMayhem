using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    Rigidbody rigid;

    public float force = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Pushed()
    {
        rigid.AddForce(Vector3.right * force);
    }
}
