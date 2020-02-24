
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShot : MonoBehaviour
{

    //private GameObject Player;
    private Rigidbody shot;
    public GameObject shotExplosion;
    public GameObject ProjectilSound;
    public float speed;
    private Vector3 PlayerPosition;
    private Vector3 movementVector;

    public float distance;
    public float lifetime;

    private void Start()
    {
        Instantiate(ProjectilSound, transform.position, transform.rotation);
        shot = GetComponent<Rigidbody>();
        var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        position = Camera.main.ScreenToWorldPoint(position);
        transform.LookAt(position);
        movementVector = (position - transform.position).normalized * speed;

        Destroy(gameObject, lifetime);



    }
    void Update()
    {
        transform.position += movementVector * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Shield" && collision.collider.tag != "Player")
        {
            Destroy(this.gameObject);
            Instantiate(shotExplosion, transform.position, transform.rotation);
        }

    }

}