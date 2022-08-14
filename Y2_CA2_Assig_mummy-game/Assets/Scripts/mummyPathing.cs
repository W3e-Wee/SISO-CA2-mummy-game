using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//---------------------------------------------------------------------------------
// Author		: Clarence Loh
// Date  		: 2022-08-09
// Modified By	: Clarence
// Modified Date: 2022-08-09
// Description	: This script handles the mummy AI
//---------------------------------------------------------------------------------
public class mummyPathing : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform Target;
    public PlayerHP PlayerHP;

    [Header("Mummy Settings")]
    public Animator _ani;
    public UnityEngine.AI.NavMeshAgent _nav;
    public AudioSource chaseSound;
    public AudioSource attackSound;
    public AudioSource stunSound;

    [Header("Mummy Game Play Settings")]
    public float playerDistance;
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
    public float idleTimer;
    public float notice_range;
    public float attack_range;
    public float ani_CD;
    public int attack_damage;
    public float stunTimer;
    

    [Header("Slime State")]
    public GameObject[] slimes;
    public float slimeId;
    public Transform slimeLocation;
    public bool callTest = false; 
    public int randomNumber;

    // Settings to prevent overlap.
    private bool ChaseAudioPlayed = false;
    private bool inChase = false;

    void Start()
    {
        // Get your "Slime", "animator", "navmeshagent" and "sound" component references
        slimes = GameObject.FindGameObjectsWithTag("slime");

        _ani = GetComponent<Animator>();
        _nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

        chaseSound = this.transform.GetChild(0).GetComponent<AudioSource>();
        attackSound = this.transform.GetChild(1).GetComponent<AudioSource>();
        stunSound = this.transform.GetChild(2).GetComponent<AudioSource>();
        return;
    }

    // Hooked up to the Animator's "Attack" trigger.
    public void DamageEvent()
    {
        attackSound.Play();
        PlayerHP.Damage(attack_damage);
    }

    // Hooked up to the Animator's "Stun" trigger.
    public void stun()
    {
        stunSound.Play();
    }

    // AI CONTROLLERS
    void Update()
    {
        // Check if in Chase? Play Chase Audio
        // ChaseAudioPlayed sees if there is already this audio playing.
        if (inChase == true && ChaseAudioPlayed == true){
            Debug.Log("Chase sound playing");
            chaseSound.Play();
            ChaseAudioPlayed = false;
        }
        // Check if no longer in chase Disable Sound
        // ChaseAudioPlayed sees if there is already this audio playing.
        else if (inChase == false && ChaseAudioPlayed == true)
        {
            chaseSound.Stop();
            ChaseAudioPlayed = false;
        }

        // Draws a line towards the player
        Vector3 _dir = Target.position - transform.position;
        // Updates public variable playerDistance with the distance between the player and the mummy for debugging purposes.
        playerDistance = Vector3.Distance(transform.position, Target.position);
        // Visualise the Ray
        Debug.DrawRay(transform.position, _dir, Color.red);

        // HERE ON OUT IS MUMMY AI STUFF
        if (_state == STATE.idle)
        {   
            // the mummy is idle, if player is in range chase player
            inChase = false;
            ChaseAudioPlayed = true;
            
            // These codes here and If statements are for the random slime call. to keep the mummy moving to find the player eventually.
            idleTimer -= Time.deltaTime;
            // if idleTimer reaches 0, get a random slime and move to it. 
            if (idleTimer <= 0)
            {
                randomNumber = Random.Range(0, slimes.Length);
                idleTimer = 20f;
                _nav.SetDestination(slimes[randomNumber].transform.position);
                _state = STATE.roaming;
            }

            // This checks the distance between mummy and player and if player is in range, the mummy will chase the player.
            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {
                // This ensures that there is no wall between player and mummy as mummy can't see through walls
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
            // the mummy is roaming, if player is in range chase player
            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {
                inChase = true;
                _state = STATE.chasing;
            }
            
            // Once the mummy reaches the Slime and does not see the player, the mummy will go back to idle.
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
            // the mummy is chasing, if player is in range chase player
            // Debug.Log("chasing " + Vector3.Distance(transform.position, Target.position));
            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {
                // this here ensures mummy keeps on following the player
                _nav.SetDestination(Target.position);

                // if in range smack the player.
                if (Vector3.Distance(transform.position, Target.position) < attack_range)
                {
                    _state = STATE.attacking;
                    _ani.SetTrigger("isAttacking");
                }
            }
            
            // If the player is out of the notice distance stop the sound and go back to idle
            else
            {
                chaseSound.Stop();
                _state = STATE.idle;
            }
        }

        // This is called when the Slime hitbox is triggered by the player to call the mummy.
        else if(_state == STATE.checking)
        {
            // go to the slime location
            _nav.SetDestination(slimeLocation.position);
            callTest = false;

            // if in range chase player.
            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {

                RaycastHit _hit;
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

            // if the mummy reaches the slime and the player is not there go back to idle
            else if (Vector3.Distance(this.transform.position, slimeLocation.position) < 2f)
            {
                inChase = false;
                _state = STATE.idle;
            }
        }

        // if player is in attack range
        else if(_state == STATE.attacking)
		{
            // makes the mummy look at the target
            this.transform.LookAt(Target);
            // stop the mummy movement
            _nav.isStopped = true;

            // Attack CD here to player doesn't die too fast
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

        // If the mummy gets stunned by the flashlight.
        else if(_state == STATE.stunned) {

            // stop chase, stop movement, play stun animation
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

        // this handles the blend tree for the mummy animation
        _ani.SetFloat("Speed", _nav.velocity.magnitude);
    }
}
