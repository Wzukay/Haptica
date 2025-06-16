using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public RandomHit randomHit;
    public List<Transform> hitPositions;
    public Gradient colorGradient;
    private TouchPoint[,] touchPoints;

    public int contactsPerRow = 4;
    public float maximumValue = 1.0f;
    public float valueDivider = 4f;
    public float valueDecreaseRateMultiplier = 2f;

    void Start()
    {
        touchPoints = new TouchPoint[contactsPerRow, contactsPerRow];
        for (int i = 0; i < contactsPerRow; i++)
        {
            for (int j = 0; j < contactsPerRow; j++)
            {
                touchPoints[i, j] = new TouchPoint();
            }
        }
    }

    void Update()
    {
        // Debug key to simulate a random hit
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Vector3 hit = hitPositions[Random.Range(0, hitPositions.Count)].position;
            // TakeHit(hit);

            randomHit.Hit();
        }

        // Decay all touch values over time
        for (int i = 0; i < contactsPerRow; i++)
        {
            for (int j = 0; j < contactsPerRow; j++)
            {
                if (touchPoints[i, j].value > 0)
                {
                    touchPoints[i, j].value -= Time.deltaTime * valueDecreaseRateMultiplier;
                    touchPoints[i, j].value = Mathf.Clamp(touchPoints[i, j].value, 0.0f, maximumValue);
                }
            }
        }
    }

    public void TakeHit(Vector3 hit)
    {
        float minDist = float.MaxValue;
        int minI = -1, minJ = -1;

        for (int i = 0; i < contactsPerRow; i++)
        {
            for (int j = 0; j < contactsPerRow; j++)
            {
                int flatIndex = i * contactsPerRow + j;
                if (flatIndex >= hitPositions.Count)
                    continue;

                float dist = Vector3.Distance(hit, hitPositions[flatIndex].position);
                if (dist < minDist)
                {
                    minDist = dist;
                    minI = i;
                    minJ = j;
                }
            }
        }

        if (minI == -1 || minJ == -1)
        {
            Debug.LogError("TakeHit: Could not find closest point.");
            return;
        }

        ApplyHit(minI, minJ);
    }

    private void ApplyHit(int i, int j)
    {
        // Apply full hit to center
        touchPoints[i, j].value = maximumValue;

        // Apply partial hit to neighbors
        for (int i2 = -1; i2 <= 1; i2++)
        {
            for (int j2 = -1; j2 <= 1; j2++)
            {
                int ni = i + i2;
                int nj = j + j2;

                if (ni >= 0 && ni < contactsPerRow && nj >= 0 && nj < contactsPerRow)
                {
                    if (i2 != 0 || j2 != 0)
                    {
                        touchPoints[ni, nj].value = Mathf.Max(
                            touchPoints[ni, nj].value,
                            maximumValue / valueDivider
                        );
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (hitPositions == null || hitPositions.Count == 0)
            return;

        for (int i = 0; i < contactsPerRow; i++)
        {
            for (int j = 0; j < contactsPerRow; j++)
            {
                int index = i * contactsPerRow + j;
                if (index >= hitPositions.Count)
                    continue;

                var pos = hitPositions[index].position;
                var value = touchPoints != null ? touchPoints[i, j].value : 0f;

                Gizmos.color = colorGradient.Evaluate(value);
                Gizmos.DrawSphere(pos, 0.05f);
            }
        }
    }
}

[System.Serializable]
public class TouchPoint
{
    public float value = 0f;
}
