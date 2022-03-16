using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightingLine : MonoBehaviour
{
    
    public Transform startPoint;
    public Transform endPoint;

    public int pointCount; // points that create lighting

    private LineRenderer lineRenderer;

    private List<Vector3> points = new List<Vector3>();
    private List<float> pointX = new List<float>();

    float k = 0;
    float b = 0;
    public float range = 0.6f; //lighting range


    public float f = 0.1f;  //fluctuate time period
    float timer;



    // Start is called before the first frame update
    void Start()
    {
        
        lineRenderer = transform.GetComponent<LineRenderer>();
        // y= kx + b
        // slope k
        k = (endPoint.position.y - startPoint.position.y) / (endPoint.position.x - startPoint.position.x);
        b = startPoint.position.y - (k * startPoint.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > f){
            lineRenderer.positionCount = pointCount + 2;
            lineRenderer.SetPositions(GetPoints());
            timer = 0;
        }
        // lineRenderer.positionCount = pointCount + 2;
        // lineRenderer.SetPositions(GetPoints());
    }

    
    public Vector3[] GetPoints(){
        points.Clear();
        points.Add(startPoint.position);
        points.Add(endPoint.position);

        pointX.Clear();
        pointX.Add(startPoint.position.x);
        pointX.Add(endPoint.position.x);
        
        for(int i=0; i<pointCount; i++){
            pointX.Add(Random.Range(startPoint.position.x, endPoint.position.x));
        }

        pointX.Sort();

        for(int i=0; i<pointX.Count;i++){
            float y = k*pointX[i]+b;
            if(i==0 || i==pointX.Count-1){
                points.Add(new Vector3(pointX[i], y, 0));
            }
            else{
                points.Add(new Vector3(pointX[i], y+Random.Range(-range, range), 0));
            }
        }


        return points.ToArray();
    }
}
