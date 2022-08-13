using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;


public class SlimeJelly : MonoBehaviour

{

    public float Intensity = 1f;
    public float Mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;
    private Mesh OriginalMesh, MeshClone;
    private MeshRenderer _renderer;
    private JellyVertex[] jv;
    private Vector3[] vertexArray;

    public mummyPathing mummyPathing;
    public bool isDead = false;
    public bool Icalled = false;

    public float respawnTimer = 50f;
    public float callTimerCd = 0f;
    public Image healthBar;
    public float health, maxHealth = 800;
    float lerpSpeed;
    public ParticleSystem deathParticles;
    public Animator _SlimeAni;

    // Start is called before the first frame update

    void Start()
    {
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        GetComponent<MeshFilter>().sharedMesh = MeshClone;
        _renderer = GetComponent<MeshRenderer>();
        jv = new JellyVertex[MeshClone.vertices.Length];
        for (int i = 0; i < MeshClone.vertices.Length; i++)
        {
            jv[i] = new JellyVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
        }
        deathParticles = transform.GetChild(1).GetComponentInChildren<ParticleSystem>();
        mummyPathing = GameObject.Find("mummy_mob").GetComponent<mummyPathing>();
    }

    void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
        if (callTimerCd > 0)
        {
            callTimerCd -= Time.deltaTime;
        }
        
        if (isDead == true)
        {
            _SlimeAni.SetTrigger("isDie");
           
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
            _SlimeAni.ResetTrigger("isRevive");
            if (callTimerCd <= 0)
            {
                // Debug.Log("I can call!");
                if (Icalled == true && isDead == false)
                {
                    // Debug.Log("Called the big boss!");
                    mummyPathing.test = this.transform;
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

    void HealthBarChange()
    {
        // Debug.Log("HealthBarChange");
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, lerpSpeed);
    } 
    void ColorChange()
    {
        // Debug.Log("ColorChange");
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));

        healthBar.color = healthColor;
    }
   
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