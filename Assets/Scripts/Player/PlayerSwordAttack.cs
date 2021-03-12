using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttack : MonoBehaviour
{
    public int damageDealt = 3;
    public GameObject collisionExplosion;
    private GameObject player;
    private Animator anim;

    [SerializeField]
    private bool canSlash = true;

    [SerializeField]
    private AudioSource impactAudioSource;
    [SerializeField]
    private AudioClip[] clips;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSlash)
        {
            anim.SetBool("isSlashing", true);
            StartCoroutine(SlashTimer());
        }
        else
        {
            anim.SetBool("isSlashing", false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        EnemyHealth health = col.gameObject.GetComponent<EnemyHealth>();

        if (health != null)
        {
            health.TakeDamage(damageDealt);
            ImpactSoundEffects();
            GameObject explosion = (GameObject)Instantiate(collisionExplosion, transform.position, transform.rotation);
            Destroy(explosion, 3f);
            Debug.Log("Sword is dealing damage");
        }
    }

    // Explosion Animation

    private void OnCollisionEnter(Collision collision) // however, here this requires trigger to be off, so it collides for explosion to play.
    {
        GameObject explosion = (GameObject)Instantiate(collisionExplosion, transform.position, transform.rotation);
        Destroy(explosion, 3f);
        return;
    }

    private IEnumerator SlashTimer()
    {
        canSlash = false;
        yield return new WaitForSeconds(1);
        canSlash = true;
    }

    public void ImpactSoundEffects()
    {
        AudioClip clip = GetRandomClip();
        impactAudioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

}
