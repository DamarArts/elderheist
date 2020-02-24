
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{

    private GameObject Player;
    private float _distance;
    private Rigidbody shot;
    public GameObject shotExplosion;
    public GameObject ProjectilSound;
    public float speed;
    private Vector3 PlayerPosition;
    private Vector3 CurrentShotPosition;
    private Vector3 movementVector1;
    private Vector3 movementVector2;
    public float _minDist;
    private Vector3 lastPosition;

    public float lifetime;

    public bool FollowPlayer, keepGoing, IsTriggered, isMelee;

    private void Start()
    {
        
        Instantiate(ProjectilSound, transform.position, transform.rotation);
        shot = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player");
        //overshot = false;
        IsTriggered = false;

        Destroy(gameObject, lifetime);

    }
    void Update()
    {

        
        _distance = Vector3.Distance(transform.position, Player.transform.position);

        if (_distance > _minDist && IsTriggered == false)
        {

                PlayerPosition = Player.transform.position;
                movementVector1 = (PlayerPosition - transform.position).normalized * speed;
                transform.position += movementVector1 * Time.deltaTime;

        }

        else
        {
            IsTriggered = true;
                transform.position += movementVector1 * Time.deltaTime;

        }


        
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