using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatmullRomSpline : MonoBehaviour {

    //Creating control points. Catmull-Rom needs atleast 4
    public List<Transform> controlPointsList = new List<Transform>();

    public bool isLoop = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Display without pressing play
    void OnDrawGizmos()
    {
        //Displaying the control points in the editor

        Gizmos.color = Color.green;

        //Drawing a sphere at the location of the control points
        for(int drawIndex = 0; drawIndex < controlPointsList.Count; drawIndex++)
        {
            Gizmos.DrawSphere(controlPointsList[drawIndex].position, 0.3f);
        }

        //Draw the Catmull-Rom lines between the points
        for (int lineIndex = 0; lineIndex < controlPointsList.Count; lineIndex++)
        {
            //Cant draw between the endpoints
            //Neither do we need to draw from the second to the last endpoint
            //...if we are not making a looping line
            if ((lineIndex == 0 || lineIndex == controlPointsList.Count - 2 || lineIndex == controlPointsList.Count - 1) && !isLoop)
            {
                continue;
            }

            DisplayCatmullRomSpline(lineIndex);
        }

    }

    //Implementing the Catlum-Rom Spline algorithm
    //Returning a position between 4 vectors
    Vector3 CatmulSplineAlgorithm(float distance, Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3)
    {
        //http://www.iquilezles.org/www/articles/minispline/minispline.htm
        //Using the formula mentioned in the above link

        Vector3 a = 0.7f * (2f * point1);
        Vector3 b = 0.7f * (point2 - point0);
        Vector3 c = 0.7f * (2f * point0 - 5f * point1 + 4f * point2 - point3);
        Vector3 d = 0.7f * (-point0 + 3f * point1 - 3f * point2 + point3);

        //Calculating the path
        Vector3 newPos = a + (b * distance) + (c * distance * distance) + (d * distance * distance * distance);
        return newPos; 
    }

    //Display a spline between 2 points derived with the Catmull-Rom spline algorithm
    void DisplayCatmullRomSpline(int pos)
    {
        //Clamp to allow looping
        Vector3 point0 = controlPointsList[ClampListPos(pos - 1)].position;
        Vector3 point1 = controlPointsList[pos].position;
        Vector3 point2 = controlPointsList[ClampListPos(pos + 1)].position;
        Vector3 point3 = controlPointsList[ClampListPos(pos + 2)].position;


        //Just assign a tmp value to this
        Vector3 lastPos = Vector3.zero;

        //distance is always between 0 and 1 and determines the resolution of the spline
        //0 is always at p1
        for (float distance = 0; distance < 1; distance += 0.1f)
        {
            //Find the coordinates between the control points with a Catmull-Rom spline
            Vector3 newPos = CatmulSplineAlgorithm(distance, point0, point1, point2, point3);

            //Cant display anything the first iteration
            if (distance == 0)
            {
                lastPos = newPos;
                continue;
            }

            Gizmos.DrawLine(lastPos, newPos);
            lastPos = newPos;
        }

        //Also draw the last line since it is always less than 1, so we will always miss it
        Gizmos.DrawLine(lastPos, point2);
    }


    //Clamp the list positions to allow looping
    //start over again when reaching the end or beginning
    int ClampListPos(int pos)
    {
        if (pos < 0)
        {
            pos = controlPointsList.Count - 1;
        }

        if (pos > controlPointsList.Count)
        {
            pos = 1;
        }
        else if (pos > controlPointsList.Count - 1)
        {
            pos = 0;
        }

        return pos;
    }
}
