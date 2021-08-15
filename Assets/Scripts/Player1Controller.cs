using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    Rigidbody rigid;
    Vector3 playerPos;

    static float moveSpeed = 1.0f;
    public float shootingRange = 100f;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Shoot();
        }
    }

    public void Movement()
    {
        playerPos = this.transform.position;

        if(Input.GetKey(KeyCode.W))
        {
            playerPos += Vector3.forward * moveSpeed * Time.deltaTime;
            rigid.MovePosition(playerPos);
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerPos += Vector3.left * moveSpeed * Time.deltaTime;
            rigid.MovePosition(playerPos);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerPos += Vector3.back * moveSpeed * Time.deltaTime;
            rigid.MovePosition(playerPos);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerPos += Vector3.right * moveSpeed * Time.deltaTime;
            rigid.MovePosition(playerPos);
        }
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, shootingRange))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.Pushed();
            }
        }
    }
}
