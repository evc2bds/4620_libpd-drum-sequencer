using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProGenLine : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float radius = 1.0f;

    [SerializeField]
    private Vector2 randomOffset = Vector2.zero;

    [SerializeField]
    [Range(1, 1000)]
    private int howManyPoints = 100;

    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float updateInSeconds = 0.5f;

    private float internalTimer = 0.0f;

    private LineRenderer lineRenderer;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        DrawCircle();

    }

    void DrawCircle() 
    {
        lineRenderer.positionCount = howManyPoints + 1;

        for(int i = 0; i < howManyPoints; i++) 
        {
            //calculate the angle
            float angle = 2f * Mathf.PI / (float)howManyPoints * i;

            float lineXPosition = Mathf.Cos(angle) * radius;
            float lineYPosition = Mathf.Sin(angle) * radius;

            //calculate the offset and apply a randomized distance
            float offsetX = UnityEngine.Random.Range(lineXPosition, lineXPosition + (lineXPosition * randomOffset.x));
            float offsetY = UnityEngine.Random.Range(lineYPosition, lineYPosition + (lineYPosition * randomOffset.y));

            //set position of line render
            lineRenderer.SetPosition(i, new Vector3(lineXPosition + offsetX, lineYPosition + offsetY, 0));
        }

        //calculate the last point position to close the circle 
        lineRenderer.SetPosition(howManyPoints, new Vector3(lineRenderer.GetPosition(0).x, lineRenderer.GetPosition(0).y, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if(internalTimer >= updateInSeconds)
        {
            DrawCircle();
            internalTimer = 0;
        } else {
            internalTimer += Time.deltaTime * 1.0f;
        }
        
    }
}
