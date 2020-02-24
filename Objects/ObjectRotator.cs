using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float _rotateY, _rotateX, _rotatez, _posX, _posY, _posZ, _targetPosX, _targetPosY, _targetPosZ;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(_posY, _targetPosY, Mathf.PingPong(Time.time,1)), transform.position.z);

        transform.Rotate(new Vector3(_rotateX, _rotateY, _rotatez) * Time.deltaTime);
    }
}
