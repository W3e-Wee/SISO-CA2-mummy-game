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

    public AudioSource chaseSound;
    public AudioSource attackSound;
    public AudioSource stunSound;

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
    public int attack_damage;
    public float stunTimer;
    public float slimeId;
    public float playerDistance;
    public int randomNumber;

    private bool ChaseAudioPlayed = false;
    private bool inChase = false;

    void Start()
    {
        // Get your animator and navmeshagent component references
        slimes = GameObject.FindGameObjectsWithTag("slime");

        _ani = GetComponent<Animator>();
        _nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

        chaseSound = this.transform.GetChild(0).GetComponent<AudioSource>();
        attackSound = this.transform.GetChild(1).GetComponent<AudioSource>();
        stunSound = this.transform.GetChild(2).GetComponent<AudioSource>();
        return;
    }

    public void DamageEvent()
    {
        attackSound.Play();
        PlayerHP.Damage(attack_damage);
    }

            
        public void stun()
        {
            stunSound.Play();
        }



    void Update()
    {
        if (inChase == true && ChaseAudioPlayed == true){
            Debug.Log("Chase sound playing");
            chaseSound.Play();
            ChaseAudioPlayed = false;
        }
        else if (inChase == false && ChaseAudioPlayed == true)
        {
            chaseSound.Stop();
            ChaseAudioPlayed = false;
        }

        Vector3 _dir = Target.position - transform.position;
        playerDistance = Vector3.Distance(transform.position, Target.position);
        Debug.DrawRay(transform.position, _dir, Color.red);
        if (_state == STATE.idle)
        {   
            inChase = false;
            ChaseAudioPlayed = true;
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
                        inChase = true;
                        _state = STATE.chasing;
                    }
                }
            }
        }
        else if (_state == STATE.roaming)
        {
            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {
                inChase = true;
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
                chaseSound.Stop();
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
                        inChase = true;
                        _state = STATE.chasing;
                    }
                }
            }
            else if (Vector3.Distance(this.transform.position, test.position) < 2f)
            {
                inChase = false;
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
                inChase = true;
				_state = STATE.chasing;
                _nav.isStopped = false;
			}
		}
        else if(_state == STATE.stunned) {
            inChase = false;
            _nav.isStopped = true;
            _ani.SetTrigger("isStunned");
            if (stunTimer <= 0)
            {
                _state = STATE.idle;
                stunTimer = 5f;
            } else {
                stunTimer -= Time.deltaTime;
            }
        }

        _ani.SetFloat("Speed", _nav.velocity.magnitude);
    }
}
