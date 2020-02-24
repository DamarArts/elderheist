
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeShot : MonoBehaviour
{

    private GameObject Player;
    private float _distance;
    private Rigidbody Shot;
    public GameObject shotExplosion;
    public GameObject ProjectilSound;
    public float speed;
    private Vector3 PlayerPosition;
    private Vector3 movementVector1;
    public float _minDist;
    private Vector3 lastPosition;

    public float lifetime;

    private void Start()
    {
        
        Instantiate(ProjectilSound, transform.position, transform.rotation);
        Shot = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player");


        Destroy(gameObject, lifetime);

    }
    void Update()
    {
     
                _distance = Vector3.Distance(transform.position, Player.transform.position);
                PlayerPosition = Player.transform.position;
                movementVector1 = (PlayerPosition - transform.position).normalized * speed;
                transform.position += movementVector1 * Time.deltaTime;
     
    }
     private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Shot" && collision.collider.tag != "Guard")
        {
            Destroy(this.gameObject);
            Instantiate(shotExplosion, transform.position, transform.rotation);
        }

    }

}