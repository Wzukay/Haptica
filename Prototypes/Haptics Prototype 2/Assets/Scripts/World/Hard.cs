using UnityEngine;
using System.Collections;

public class Hard : MonoBehaviour
{
    bool started = false;
    bool onb = false;
    bool offb = false;

    public Transform on;
    public Transform off;
    public Transform blocks;
    public Transform[] checkpoints;

    void OnEnable()
    {
        if (!started)
            StartCoroutine(OnCoroutine());
    }

    void Update()
    {
        if (onb)
        {
            for (int i = 0; i < blocks.childCount; i++)
            {
                blocks.GetChild(i).GetComponent<Collider>().enabled = true;
                blocks.GetChild(i).transform.localPosition = Vector3.Lerp(blocks.GetChild(i).transform.localPosition, on.GetChild(i).transform.localPosition, Time.deltaTime * 3);
            }
        }

        if (offb)
        {
            for (int i = 0; i < blocks.childCount; i++)
            {
                blocks.GetChild(i).GetComponent<Collider>().enabled = false;
                blocks.GetChild(i).transform.localPosition = Vector3.Lerp(blocks.GetChild(i).transform.localPosition, off.GetChild(i).transform.localPosition, Time.deltaTime * 3);
            }
        }
    }

    public void Disable()
    {
        if (!started)
            StartCoroutine(DisableAfterAnimation());
    }

    IEnumerator DisableAfterAnimation()
    {
        yield return StartCoroutine(OffCoroutine());  // Wait for off animation to finish
        enabled = false;                             // Now disable — allows OnEnable to be triggered next time
    }

    IEnumerator OnCoroutine()
    {
        onb = true;
        started = true;

        float elapsed = 0f;
        float duration = 1.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        print("On Coroutine Easy");

        onb = false;
        started = false;
    }

    IEnumerator OffCoroutine()
    {
        offb = true;
        started = true;

        float elapsed = 0f;
        float duration = 1.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        print("Off Coroutine Easy");

        offb = false;
        started = false;
    }

}
