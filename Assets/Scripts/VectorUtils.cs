using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class VectorUtils
{
    public static Vector3 RandomVectorInsideSphere(Vector3 center, float radius)
    {
        Vector3 output;
        float x = Random.Range(-1f * radius, radius);
        float y = Random.Range(-1f * Mathf.Sqrt((radius * radius) - (x * x)), Mathf.Sqrt((radius * radius) - (x * x)));
        float z = Random.Range(-1f * Mathf.Sqrt((radius * radius) - (x * x) - (y * y)), Mathf.Sqrt((radius * radius) - (x * x) - (y * y)));
        output = new Vector3(x, y, z) + center;
        return output;
    }

    public static Vector3 RandomVectorOnSphere(Vector3 center, float radius)
    {
        Vector3 output;
        float x = Random.Range(-1f * Mathf.Abs(radius), Mathf.Abs(radius));
        float y = Random.Range(-1f * Mathf.Sqrt((Mathf.Abs(radius) * Mathf.Abs(radius)) - (x * x)), Mathf.Sqrt((Mathf.Abs(radius) * Mathf.Abs(radius)) - (x * x)));
        float z = ((Random.Range(-1f, 1f) < 0f)?-1f:1f) * Mathf.Sqrt((radius * radius) - (x * x) - (y * y));
        if (float.IsNaN(z))
        {
            z = 0;
        }
        if (float.IsNaN(y))
        {
            y = 0;
        }
        if (float.IsNaN(x))
        {
            x = 0;
        }
        output = new Vector3(x, y, z) + center;
        return output;
    }

    public static Vector3 RandomRotation()
    {
        return new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
    }

    public static Vector3 ConvertToYPlane(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }
    
    public static Vector3 ScreenPositionToAnchoredPosition(Vector2 screenPosition, Canvas canvas)
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        return (screenPosition - screenSize / 2) /
               screenSize *
               canvas.GetComponent<RectTransform>().sizeDelta;
    }
    
    public static Vector3 GetCenter(Vector3[] points)
    {
        var center = Vector3.zero;
        for (int i = 0; i < points.Length; i++)
            center += points[i] / points.Length;
        return center;
    }
    
    public static Vector2 FindClosestPoint(List<Vector2> pointsList, Vector2 desiredPoint)
    {
        if (pointsList.Count > 0)
        {
            Vector2 closestPoint = Vector3.zero;
            float closestDistance = Mathf.Infinity;

            foreach (Vector2 point in pointsList)
            {
                float distance = Vector3.Distance(point, desiredPoint);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = point;
                }
            }

            return closestPoint;
        }

        return desiredPoint;
    }
    
    public static List<Vector2> GetValidPositions(List<Vector2> occupiedPointsList, Vector2 offset, float width, float height, float minDistance)
    {
        List<Vector2> positionsList = new List<Vector2>();
        bool validPosition = false;
        float xMin = -width / 2f;
        float xMax = width / 2f;
        float zMin = -height / 2f;
        float zMax = height / 2f;
        int iterationCount = 0;

        while (iterationCount < 10)
        {
            iterationCount++;
            Vector2 position = new Vector2(Random.Range(xMin, xMax), Random.Range(zMin, zMax)) + offset;
            validPosition = true;
            foreach (Vector2 point in occupiedPointsList)
            {
                if (Vector2.Distance(position, point) < minDistance)
                {
                    validPosition = false;
                    break;
                }
            }
            if (validPosition)
            {
                foreach (Vector2 pos in positionsList)
                {
                    if (Vector2.Distance(position, pos) < minDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }
                if (validPosition)
                {
                    positionsList.Add(position);
                }
            }
        }
        return positionsList;
    }
}
