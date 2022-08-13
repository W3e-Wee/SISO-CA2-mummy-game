using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class mummyPathing : MonoBehaviour
{
    public Transform Target;
    public PlayerHP PlayerHP;
    public Animator _ani;
    public UnityEngine.AI.NavMeshAgent _nav;
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
    public float attack_range;
    public float ani_CD;
    public float stunTimer;
    public float slimeId;
    public float playerDistance;
    public int randomNumber;


    void Start()
    {
        // Get your animator and navmeshagent component references
        slimes = GameObject.FindGameObjectsWithTag("slime");

        _ani = GetComponent<Animator>();
        _nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

        
        return;
    }

    public void DamageEvent(int dmgTaken)
    {
        Debug.Log(dmgTaken);
        PlayerHP.Damage(dmgTaken);
    }



    void Update()
    {

        Vector3 _dir = Target.position - transform.position;
        playerDistance = Vector3.Distance(transform.position, Target.position);
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
                if (Vector3.Distance(transform.position, Target.position) < attack_range)
                {
                    _state = STATE.attacking;
                    _ani.SetTrigger("isAttacking");
                    // PlayerHP.Damage(10);
                }
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
            // Debug.Log(Vector3.Distance(this.transform.position, test.position) < 2f);
            // Debug.Log("Does Mummy see player? ");
            // Debug.Log(Vector3.Distance(transform.position, Target.position) < notice_range);
            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {
                RaycastHit _hit;
                // Debug.Log(Physics.Raycast(transform.position, _dir, out _hit));
                if (Physics.Raycast(transform.position, _dir, out _hit))
                {
                    Debug.Log(_hit.transform.name);
                    if (_hit.transform == Target)
                    {
                        _state = STATE.chasing;
                    }
                }
            }
            else if (Vector3.Distance(this.transform.position, test.position) < 2f)
            {
                _state = STATE.idle;
            }
        }

        else if(_state == STATE.attacking)
		{
            this.transform.LookAt(Target);
            _nav.isStopped = true;

            ani_CD -= Time.deltaTime;
            if (ani_CD <= 0)
            {
                _ani.SetTrigger("isAttacking");
                ani_CD = 1.2f;
            }
			// It is attacking during this state
			// Check if it is outside of the attack range
			if(Vector3.Distance(transform.position, Target.position) > attack_range)
			{
				// target is outside, change back to chase state
				_state = STATE.chasing;
                _nav.isStopped = false;
			}
		}
        else if(_state == STATE.stunned) {
            _nav.isStopped = true;
            _ani.SetTrigger("isStunned");
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                _ani.SetTrigger("isStunned");
                _state = STATE.idle;
                stunTimer = 5f;
            }
        }

        _ani.SetFloat("Speed", _nav.velocity.magnitude);
    }
}
