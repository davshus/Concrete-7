using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WayPoint
{
    public Transform transform;
    public double speed;
    public WayPoint(Transform t, double s)
    {
        transform = t;
        speed = s;
    }
}

public class StartUp : MonoBehaviour {
    public GameObject foot1, foot2, thigh1, thigh2;
    public Camera cam;
    public Transform camTarg;
    Transform t;
    public float decayBase;
    public float decayCoefficient;
    public float decayOffset;
    private float lastTime;
    WayPoint[] waypoints;
    public int waypointCount;
    public Transform[] waypointTransforms;
    public double[] waypointSpeeds;
    int currentWaypointTarget;
    public GameObject decayStatus;
	// Use this for initialization
	void Start () {
        Vector3 vel = new Vector3(4, 0, 0);
        ((Rigidbody)foot1.GetComponent<Rigidbody>()).angularVelocity = vel;
        ((Rigidbody)thigh1.GetComponent<Rigidbody>()).angularVelocity = vel;
        ((Rigidbody)foot2.GetComponent<Rigidbody>()).angularVelocity = vel;
        ((Rigidbody)thigh2.GetComponent<Rigidbody>()).angularVelocity = vel;
        t = GetComponent<Transform>();
        waypoints = new WayPoint[waypointCount];
        for (int i = 0; i < waypointCount; i++) {
            waypoints[i] = new WayPoint(waypointTransforms[i], waypointSpeeds[i]);
        }
        lastTime = Time.realtimeSinceStartup;

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            lastTime = Time.realtimeSinceStartup;
        }
        float decay = Mathf.Pow(decayBase, (Mathf.Max(0, (Time.realtimeSinceStartup - lastTime) - decayOffset)) * decayCoefficient);
        decayStatus.transform.localScale = new Vector3(decay * 3, decayStatus.transform.localScale.y, decayStatus.transform.localScale.z);
        t.position = Vector3.MoveTowards(t.position, waypoints[currentWaypointTarget].transform.position, (float)waypoints[currentWaypointTarget].speed * Time.deltaTime * decay);
        if (t.position.Equals(waypoints[currentWaypointTarget].transform.position) && currentWaypointTarget < waypointCount - 1)
        {
            currentWaypointTarget++;
            t.LookAt(waypoints[currentWaypointTarget].transform);
            Debug.Log(waypoints[currentWaypointTarget].transform.position);
        }
    }
    void LateUpdate () {
        cam.transform.position = new Vector3(t.position.x + 4, t.position.y + 3, t.position.z - 4);
        cam.transform.LookAt(camTarg);
	}
}
