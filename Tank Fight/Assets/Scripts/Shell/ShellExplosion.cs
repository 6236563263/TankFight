using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    //public LayerMask m_TankMask;
   //public ParticleSystem m_ExplosionParticles;       
    //public AudioSource m_ExplosionAudio;              
    public float maxDamage = 25f;
    public float minDamage = 15f;
    //public float m_ExplosionForce = 1000f;
    public float m_MaxLifeTime = 10f;                  
    //public float m_ExplosionRadius = 5f;
    public GameObject shellExplosionPrefab;
    public float explosionRadius = 1f;
    public LayerMask tankMask;
    //public AudioSource explosionAudio;
    private bool bounce = false;
    private void Start()
    {
        //GameObject.Instantiate(shellExplosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,explosionRadius, tankMask);
        Instantiate(shellExplosionPrefab, transform.position, transform.rotation);
        foreach(Collider c in colliders)
        {
            c.GetComponent<TankHpABC>().TakeDamage(Random.Range(minDamage,maxDamage));
        }
        if(other.tag !="Ground")    
        {
            Destroy(this.gameObject); 
        }
        else
        {
            if (bounce)
            {
                Vector3 velocity = this.gameObject.GetComponent<Rigidbody>().velocity;
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, -velocity.y, velocity.z);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        //explosionAudio.Play();  //生成爆炸特效时直接产生音效，故不再次播放
    }

    public void SetBounce(bool boo)
    {
        bounce = boo;
    }
}