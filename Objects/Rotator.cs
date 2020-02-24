using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public GameObject _grind;
    public float _time;
    public PlayerMovement playerScript;
    bool _instanciated;




    private void Update()
    {
        if (playerScript._CeilingSpin1 == true)
        {
            
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0f, 130f, 0f), _time*Time.deltaTime);


            if (!_instanciated)
            {
                Instantiate(_grind, transform.position, transform.rotation);
                _instanciated = true;
            }
        }
        if (playerScript._CeilingSpin2 == true)
        {
            playerScript._CeilingSpin1 = false;
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0f, 87f, 0f), _time * Time.deltaTime);

            if (_instanciated)
            {
                Instantiate(_grind, transform.position, transform.rotation);
                _instanciated = false;
            }

        }
    }


}
