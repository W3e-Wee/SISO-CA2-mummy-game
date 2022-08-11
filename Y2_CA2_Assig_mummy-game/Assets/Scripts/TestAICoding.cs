using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TestAICoding : MonoBehaviour
{
    public Transform Target;
    private Animator _ani;
    private UnityEngine.AI.NavMeshAgent _nav;
    public Transform test;
    public bool callTest = false; 

    public enum STATE
    {
        idle,
        roaming,
        checking,
        chasing,
        attacking,
        stunned
    }
    public STATE _state;

    public GameObject[] slimes;
    public float idleTimer;
    public float notice_range;
    public float slimeId;
    public int randomNumber;


    void Start()
    {
        // Get your animator and navmeshagent component references
        slimes = GameObject.FindGameObjectsWithTag("slime");

        _ani = GetComponent<Animator>();
        _nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

        return;
    }

    void Update()
    {
        Vector3 _dir = Target.position - transform.position;
        Debug.DrawRay(transform.position, _dir, Color.red);
        if (_state == STATE.idle)
        {   
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0)
            {
                randomNumber = Random.Range(0, slimes.Length);
                idleTimer = 20f;
                _nav.SetDestination(slimes[randomNumber].transform.position);
                _state = STATE.roaming;
            }
            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {
                RaycastHit _hit;
                if (Physics.Raycast(transform.position, _dir, out _hit))
                {
                    if (_hit.transform == Target)
                    {
                        _state = STATE.chasing;
                    }
                }
            }
        }
        else if (_state == STATE.roaming)
        {
            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {
                _state = STATE.chasing;
            }
            // Debug.Log(Vector3.Distance(this.transform.position, slimes[randomNumber].transform.position));
            else if (Vector3.Distance(this.transform.position, slimes[randomNumber].transform.position) < 0.5f)
            {
                _state = STATE.idle;
            }
            else {
                _state = STATE.roaming;
            }

        }
        else if (_state == STATE.chasing)
        {
            // Debug.Log("chasing " + Vector3.Distance(transform.position, Target.position));
            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {
                _nav.SetDestination(Target.position);
                // if (Vector3.Distance(transform.position, Target.position) < attack_range)
                // {
                    // _state = STATE.attack;
                    // _ani.SetTrigger("isAttacking");
                // }
            }
            else
            {
                _state = STATE.idle;
            }
        }
        else if(_state == STATE.checking)
        {
            _nav.SetDestination(test.position);
            callTest = false;

            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {
                RaycastHit _hit;
                if (Physics.Raycast(transform.position, _dir, out _hit))
                {
                    if (_hit.transform == Target)
                    {
                        _state = STATE.chasing;
                    }
                }
            }
            else if (Vector3.Distance(this.transform.position, test.position) < 0.5f)
            {
                _state = STATE.idle;
            }
            else {
                _state = STATE.checking;

            }
        }
        // _nav.SetDestination(Target.position);
        // Debug.Log(_state);
        // _ani.SetFloat("Speed", _nav.velocity.magnitude);
    }
}