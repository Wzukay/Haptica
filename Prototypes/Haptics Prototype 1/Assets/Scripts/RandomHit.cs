using System.Collections;
using UnityEngine;

public class RandomHit : MonoBehaviour
{
    public Transform player;
    public Transform sword;
    public bool canHit = true;

    public void Hit()
    {
        if (canHit)
        {
            StartCoroutine(Cor());
            canHit = false;
        }
    }

    IEnumerator Cor()
    {
        Random.InitState(Time.renderedFrameCount);

        yield return new WaitForSeconds(0);
        sword.localPosition = new Vector3(3, .5f, 3);
        yield return new WaitForSeconds(1);
        sword.localPosition = player.transform.localPosition + new Vector3(
                    Random.Range(-.25f, 0.25f), Random.Range(0.1f, 0.5f), Random.Range(-.25f, 0.25f));
        yield return new WaitForSeconds(1);
        sword.localPosition = new Vector3(3, .5f, 0);

        canHit = true;
    }
}
