using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathing : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator _ani;
    private NavMeshAgent _nav;
    public Transform Target; // The target to chase
    public enum STATE
    {
        idle,
        chase,
        attack,
        stunned
    }
    public STATE _state;
    public float notice_range; // Notices when player comes within a certain range
    public float attack_range; // The attack range for the mutant
    public float Ani_CD;
    public float stunned;

    void Start()
    {
        // Get your animator and navmeshagent component references
        _ani = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();

        return;
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == STATE.idle)
        {
            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {
                RaycastHit _hit;
                Vector3 _dir = Target.position - transform.position;
                if (Physics.Raycast(transform.position, _dir, out _hit))
                {
                    if (_hit.transform == Target)
                    {
                        _state = STATE.chase;
                    }
                }
            }
        }
        else if (_state == STATE.chase)
        {
            if (Vector3.Distance(transform.position, Target.position) < notice_range)
            {
                _nav.SetDestination(Target.position);
                if (Vector3.Distance(transform.position, Target.position) < attack_range)
                {
                    _state = STATE.attack;
                    _ani.SetTrigger("isAttacking");
                }
            }
            else
            {
                _state = STATE.idle;
            }
        }
        else if (_state == STATE.attack)
        {
            if (Vector3.Distance(transform.position, Target.position) > attack_range)
            {
                _state = STATE.chase;
                _ani.SetTrigger("isNotAttacking");
            }
            else
            {
                transform.LookAt(Target.position);
            }
        }

        _ani.SetFloat("Speed", _nav.velocity.magnitude);
    }
}