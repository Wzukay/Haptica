using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform checkpoint;
    public float timeToStart = 1.0f;
    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (checkpoint && timer > timeToStart)
        {
            if (checkpoint.childCount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, checkpoint.GetChild(0).position + new Vector3(0, 1, 0), Time.deltaTime * 3);
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, new Quaternion(0, 0, 0, 0), Time.deltaTime * 3);
            }
        }
    }
}
