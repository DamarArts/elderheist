using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavansLookAt : MonoBehaviour
{
    private GameObject Player;
    private Vector3 PlayerPosition;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        PlayerPosition = Player.transform.position;
        transform.LookAt(PlayerPosition);
    }
}
