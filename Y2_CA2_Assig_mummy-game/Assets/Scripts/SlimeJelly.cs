using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------
// Author		: Clarence Loh
// Date  		: 2022-08-09
// Modified By	: Clarence
// Modified Date: 2022-08-09
// Description	: Script to handle Slime Events
//---------------------------------------------------------------------------------
public class SlimeJelly : MonoBehaviour
{
    [Header("Slime Jiggle Settings")]
    // Slime Jiggle
    public float Intensity = 1f;
    public float Mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;
    private Mesh OriginalMesh, MeshClone;
    private MeshRenderer _renderer;
    private JellyVertex[] jv;
    private Vector3[] vertexArray;

    [Header("Mummy File Settings")]
    public mummyPathing mummyPathing;
    public bool isDead = false;
    public bool Icalled = false;

    [Header("Slime health Settings")]
    public float respawnTimer = 50f;
    public float callTimerCd = 0f;
    public Image healthBar;
    public float health, maxHealth = 800;
    float lerpSpeed;
    public ParticleSystem deathParticles;
    public Animator _SlimeAni;
    public AudioSource _slimeHit;
    public AudioSource _slimeDeath;
    public bool damageAudioPlayed = true;
    public bool takingDamage = false;
    // Start is called before the first frame update

    void Start()
    {
        // Jiggle Assignments
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        GetComponent<MeshFilter>().sharedMesh = MeshClone;
        _renderer = GetComponent<MeshRenderer>();
        jv = new JellyVertex[MeshClone.vertices.Length];
        for (int i = 0; i < MeshClone.vertices.Length; i++)
        {
            jv[i] = new JellyVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
        }

        // Mummy Assignments
        mummyPathing = GameObject.Find("mummy_mob").GetComponent<mummyPathing>();

        // SLime Assignments
        _slimeDeath = this.transform.GetChild(2).GetComponent<AudioSource>();
        _slimeHit = this.transform.GetChild(3).GetComponent<AudioSource>();
        healthBar = this.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        deathParticles = this.transform.GetChild(1).GetComponent<ParticleSystem>();
        _SlimeAni = GetComponent<Animator>();
        damageAudioPlayed = true;
        
    }

    // This is played in the Animations
    public void deathSoundPlay() {
        _slimeDeath.Play();
    }

    void Update()
    {
        // get the dmg particle
        var dmg = deathParticles.emission;

        // control health bar speed
        lerpSpeed = 3f * Time.deltaTime;

        // play a looping sound of slime getting hit
        if (takingDamage == true && damageAudioPlayed == true){
            Debug.Log("Damage sound playing");
            _slimeHit.Play();
            damageAudioPlayed = false;
        }
        // stops the looping sound
        else if (takingDamage == false && damageAudioPlayed == false)
        {
            Debug.Log("Damage sound Stoped playing");
            _slimeHit.Stop();
            damageAudioPlayed = false;
        }

        // decrease the cooldown of the call timer to call mummy
        if (callTimerCd > 0)
        {
            callTimerCd -= Time.deltaTime;
        }
        
        // if full health hide healthbar
        if (health == maxHealth) {
            takingDamage = false;
            damageAudioPlayed = true;
            dmg.enabled = false;
            if (healthBar.enabled == true)
            {
                healthBar.enabled = false;
            }
            // Debug.Log("Slime is healthy");

            // if damaged show healthbar
        } else if (health < maxHealth && health > 0) {
            healthBar.enabled = true;
            takingDamage = true;
            // Debug.Log("Slime is hurt");
            dmg.enabled = true;
        }
        // if health is zero hide healthbar
        else if (health == 0) {
            // Debug.Log("Slime is Dead");
            _SlimeAni.SetTrigger("isDie");
            isDead = true;
            dmg.enabled = false;
            takingDamage = false;
            if (healthBar.enabled == true){
                healthBar.enabled = false;
            }

        }

        // when the slime dies.
        if (isDead == true)
        {
            if (respawnTimer > 0)
            {
                respawnTimer -= Time.deltaTime;
            }
            if (respawnTimer <= 0)
            {
                _SlimeAni.ResetTrigger("isDie");
                _SlimeAni.SetTrigger("isRevive");
                isDead = false;
                health = maxHealth;
                respawnTimer = 50f;
            }
        }
        else 
        {
            // This happens when the slime revives as respawn timer hits 0
            _SlimeAni.ResetTrigger("isRevive");
            if (callTimerCd <= 0)
            {
                // Debug.Log("I can call!");
                if (Icalled == true && isDead == false)
                {
                    // Debug.Log("Called the big boss!");
                    mummyPathing.slimeLocation = this.transform;
                    mummyPathing.callTest = true;
                    mummyPathing._state = mummyPathing.STATE.checking;
                    Icalled = false;
                    callTimerCd = 30f;
                }
            }
            HealthBarChange();
            ColorChange();
        }
    }

    // Affect the health bar of the slime
    void HealthBarChange()
    {
        // Debug.Log("HealthBarChange");
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, lerpSpeed);
    } 
    // Affect the health bar of the slime
    void ColorChange()
    {
        // Debug.Log("ColorChange");
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));

        healthBar.color = healthColor;
    }
   
   // code to make jelly jiggle on movement
    void FixedUpdate()
    {
        vertexArray = OriginalMesh.vertices;

        for (int i = 0; i < jv.Length; i++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[jv[i].ID]);
            float intensity = (1 - (_renderer.bounds.max.y - target.y) / _renderer.bounds.size.y) * Intensity;
            jv[i].Shake(target, Mass, stiffness, damping);
            target = transform.InverseTransformPoint(jv[i].Position);
            vertexArray[jv[i].ID] = Vector3.Lerp(vertexArray[jv[i].ID], target, intensity);
        }

        MeshClone.vertices = vertexArray;

    }

    // code to make jelly jiggle on movement
    public class JellyVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 velocity, Force;
        public JellyVertex(int _id, Vector3 _pos)
        {
            ID = _id;
            Position = _pos;
        }

        public void Shake(Vector3 target, float m, float s, float d)

        {
            Force = (target - Position) * s;
            velocity = (velocity + Force / m) * d;
            Position += velocity;
            if ((velocity + Force + Force / m).magnitude < 0.001f)
            {
                Position = target;
            }
        }

    }
}