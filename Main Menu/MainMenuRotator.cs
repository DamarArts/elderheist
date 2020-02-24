using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotator : MonoBehaviour
{
    public float _rotateX, _rotateY, _rotatez;
    void Update()
    {
        transform.Rotate(new Vector3(_rotateX, _rotateY, _rotatez) * Time.deltaTime);
    }
}
