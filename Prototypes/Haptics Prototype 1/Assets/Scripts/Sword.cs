using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Player player;
    public bool hit = false;

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.tag == "Player" && !hit)
        {
            print("Touched Player");
            hit = true;
            player.TakeHit(col.contacts[0].point);
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.collider.gameObject.tag == "Player")
            hit = false;
    }
}
