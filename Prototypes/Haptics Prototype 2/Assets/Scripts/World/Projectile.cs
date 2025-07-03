using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool thrownByPlayer = false;
    public float speed = 20f;
    public float lifeTime = 5f;
    public int damage = 10;
    public GameObject hitEffect;
    Player player;

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
        player = Player.singleton;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

         if (transform.position.x > 100 || transform.position.y > 100 || transform.position.z > 100 ||
                transform.position.x < -100 || transform.position.y < -100 || transform.position.z < -100)
                Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!thrownByPlayer) {
                player.TakeHit(collision.contacts[0].point);
                Destroy(gameObject);
                print("Hit player");
            }
                
            else
                return;
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            print("col");
            if (thrownByPlayer)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            else
                return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
