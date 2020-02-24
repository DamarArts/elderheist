using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRotation : MonoBehaviour
{
    public float offsetx, offsety, offsetz;
    public float _rotateX,_rotateY, _rotateZ;
    public Transform _Gaurd;
    void Update()
    {
        transform.position = new Vector3(_Gaurd.transform.position.x , offsety,_Gaurd.transform.position.z);
        transform.Rotate(new Vector3(_rotateX, _rotateY, _rotateZ) * Time.deltaTime);

    }
}
