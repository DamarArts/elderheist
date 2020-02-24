using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Patrol : MonoBehaviour
{
    [SerializeField]
    bool _patrolWaiting;

    [SerializeField]
    float _totalWaitTime = 3f;

    [SerializeField]
    float _switchProbability = 0.2f;

    [SerializeField]
    List<Waypoint> _patrolPoints;
    //----------------------------------------------------------------------------
    NavMeshAgent _navMeshAgent;
    //-----------------------------------------------------------------------------
    public GameObject shot, closeShot;
    public PlayerMovement _playerScript;
    public Transform Player, shotSpawn;
    //-----------------------------------------------------------------------------
    public bool _travelling, _waiting, _patrolForward, _chasing, _isRegening, _chasingPlay, _roamingPlay, _rechargPlay;
    int _currentPatrolIndex;
    //-----------------------------------------------------------------------------
    public float _waitTimer, _Distance, nextFire, damage, currentHealth, maxHealth;
    public float MinDistance, fireRate, GuardSpeed, GuardAccel, GuardStop, shootDistance, regeOverTime;
    //-----------------------------------------------------------------------------
    public Slider HealthBar;
    //-----------------------------------------------------------------------------
    private Color32 _red, _blue, _blueLight;
    private Color _redEmissions, _blueEmissions;
    public Renderer rend, rend2, rend3;
    public Light _lit;
    //-----------------------------------------------------------------------------

    public AudioSource guardAudio;
    public AudioClip roaming;
    public AudioClip chasing;
    public AudioClip resetting;
    public MainMenu MenuScript;

    public void Start()
    {

        _chasingPlay = false;
        _roamingPlay = false;
        _rechargPlay = false;
        _chasing = false;
        _travelling = false;
        _isRegening = false;

        guardAudio = GetComponent<AudioSource>();
        MenuScript = GameObject.FindGameObjectWithTag("Menu").GetComponent<MainMenu>();
        if (MenuScript.Hard == true)
        {
            maxHealth = 200f;
        }
        if (MenuScript.Medium == true)
        {
            maxHealth = 150f;
        }
        if (MenuScript.Easy == true)
        {
            maxHealth = 100f;
        }
        
        //maxHealth = 100f;
        currentHealth = maxHealth;
        HealthBar.value = 1;
        //---------------------------------------------
        _blue = new Color(255f, 255f, 255f, 255f);
        _blueLight = new Color(0f, 160f, 255f, 255f);
        _red = new Color(255f, 0f, 0f, 255f);
        _redEmissions = new Color(1f, 0f, 0f);
        _blueEmissions = new Color(0f, 0.7f, 1f);
        //--------------------------------------------- 
        _Distance = Vector3.Distance(transform.position, Player.transform.position);
        _navMeshAgent = this.GetComponent<NavMeshAgent>();


        if (_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh component is not attached to " + gameObject.name);
        }
        else
        {
            if (_patrolPoints != null && _patrolPoints.Count >= 2)
            {
                _currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.Log("insufficient patrol points for basic patrolling behavior.");
            }
        }
    }

    public void Update()
    {
        
        
        HealthBar.value = CalculateHealth();
        _Distance = Vector3.Distance(transform.position, Player.transform.position);
        MenuScript = GameObject.FindGameObjectWithTag("Menu").GetComponent<MainMenu>();
        if (MenuScript.Hard == true)
        {
            SetHard();
        }
        else if (MenuScript.Medium == true)
        {
            SetMedium();
        }
        else if (MenuScript.Easy == true)
        {
            SetEasy();
        }

        bool isActive = currentHealth > 0.0f;
        //--------------------------------------------------------------------------

        if (currentHealth == maxHealth)
        {
            _isRegening = false;
        }
        else if (currentHealth <= 0.0f)
        {
            _isRegening = true;
        }

        if (isActive && _isRegening == false)
        {
            if (_chasing == false)
            {
                _lit.color = _blueLight;
                _navMeshAgent.angularSpeed = 12;
                _navMeshAgent.speed = 20;
                _navMeshAgent.acceleration = 20;
                _navMeshAgent.stoppingDistance = 0;
                //--------------------------------------------------------
                rend.material.SetColor("_Color", _blue);
                rend.material.SetColor("_EmissionColor", _blueEmissions);
                rend2.material.SetColor("_Color", _blue);
                rend2.material.SetColor("_EmissionColor", _blueEmissions);
                rend3.material.SetColor("_Color", _blue);
                rend3.material.SetColor("_EmissionColor", _blueEmissions);
                //--------------------------------------------------------

                if (_travelling && _navMeshAgent.remainingDistance <= 1.0f)
                {
                    _travelling = false;


                    if (_patrolWaiting)
                    {
                        _waiting = true;
                        _waitTimer = 0f;
                    }
                    else
                    {
                        ChangePatrolPoint();
                        SetDestination();
                    }
                }
                if (_waiting)
                {
                    _waitTimer += Time.deltaTime;
                    if (_waitTimer >= _totalWaitTime)
                    {
                        _waiting = false;

                        ChangePatrolPoint();
                        SetDestination();
                    }
                }
            }
            if (Vector3.Distance(transform.position, Player.transform.position) > MinDistance)
            {
                _chasing = false;

            }

            if (Vector3.Distance(transform.position, Player.transform.position) <= MinDistance || _playerScript.isFiringDetection)
            {
                if (_playerScript.playerAlive == true && _playerScript._gameWon == false)
                {
                    _chasing = true;
                    SetChase();
                }
                else if (_playerScript.playerAlive == false || _playerScript._gameWon == true)
                {
                    SetDestination();
                }
                       
            }
        }

        else if (_isRegening == true)

        {
            //_playerScript.isFiringDetection = false;
            regenHealth();
            //---------------------------------------------------------
            rend.material.SetColor("_Color", _blue);
            rend.material.SetColor("_EmissionColor", _blueEmissions);
            rend2.material.SetColor("_Color", _blue);
            rend2.material.SetColor("_EmissionColor", _blueEmissions);
            rend3.material.SetColor("_Color", _blue);
            rend3.material.SetColor("_EmissionColor", _blueEmissions);
            //---------------------------------------------------------
        }

        if (_chasing == true && _chasingPlay == false && _isRegening == false)
        {
            guardAudio.Stop();
            guardAudio.clip = chasing;
            guardAudio.Play();
            _chasingPlay = true;
            _roamingPlay = false;
            _rechargPlay = false;
            //guardAudio.Stop();

        }
        if (_travelling == true && _roamingPlay == false)
        {
            //guardAudio.clip = chasing;
            guardAudio.Stop();
            guardAudio.clip = roaming;
            guardAudio.Play();
            _roamingPlay = true;
            _rechargPlay = false;
            _chasingPlay = false;
            //guardAudio.Stop();
        }

        if (_isRegening == true && _rechargPlay == false)
        {
            guardAudio.clip = chasing;
            guardAudio.Pause();
            guardAudio.clip = resetting;
            guardAudio.Play();
            _rechargPlay = true;
            _chasingPlay = false;
            _roamingPlay = false;
        }
    }

    private void SetChase()
    {

        _navMeshAgent.angularSpeed = 900;
        _navMeshAgent.speed= GuardSpeed;
        _navMeshAgent.acceleration = GuardAccel;
        _navMeshAgent.stoppingDistance = GuardStop;
        _lit.color = _red;
        rend.material.SetColor("_Color", _red);
        rend.material.SetColor("_EmissionColor", _redEmissions);
        rend2.material.SetColor("_Color", _red);
        rend2.material.SetColor("_EmissionColor", _redEmissions);
        rend3.material.SetColor("_Color", _red);
        rend3.material.SetColor("_EmissionColor", _redEmissions);
        Vector3 targetVector = Player.transform.position;
        _navMeshAgent.SetDestination(targetVector);
        _chasing = true;
        _travelling = false;
        _isRegening = false;
        if (_Distance > shootDistance)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            }
        }

            else
            {
                if (Time.time > nextFire)
                {
                nextFire = Time.time + fireRate;
                Instantiate(closeShot, shotSpawn.position, shotSpawn.rotation);
                }
            }


    }

    private void SetDestination()
    {

        Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;
        _navMeshAgent.SetDestination(targetVector);
        _travelling = true;
        _chasing = false;
        _isRegening = false;
        //guardAudio.clip = roaming;
       // guardAudio.Play();
    }
    private void ChangePatrolPoint()
    {
        if ( UnityEngine.Random.Range(0f, 1f) <= _switchProbability)
        {
            _patrolForward = !_patrolForward;
        }

        if (_patrolForward)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
        }
        else
        {
            if (--_currentPatrolIndex < 0)
            {
                _currentPatrolIndex = _patrolPoints.Count - 1;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "playerShot")
        {
            if (_isRegening == false)
           currentHealth = currentHealth - damage;
        }
    }

    void regenHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth + (regeOverTime * Time.deltaTime), 0.0f, maxHealth);
    }
    float CalculateHealth()
    {
        return currentHealth / maxHealth;
    }

    public void SetHard()
    {
        damage = UnityEngine.Random.Range(10f, 20f);
        GuardAccel = 35f;
        GuardSpeed = 30f;
        GuardStop = 15f;
        fireRate = 0.5f;
        MinDistance = 60f;
        regeOverTime = 30f;
    }
    public void SetMedium()
    {
        damage = UnityEngine.Random.Range(15f, 25f);
        GuardAccel = 30f;
        GuardSpeed = 25f;
        GuardStop = 15f;
        fireRate = 1f;
        MinDistance = 50f;
        regeOverTime = 20f;
    }
    public void SetEasy()
    {
        damage = UnityEngine.Random.Range(25f, 35f);
        GuardAccel = 17f;
        GuardSpeed = 15f;
        GuardStop = 15f;
        fireRate = 1.5f;
        MinDistance = 40f;
        regeOverTime = 15f;
    }
}
