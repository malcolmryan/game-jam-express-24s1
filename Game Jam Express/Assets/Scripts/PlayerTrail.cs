using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    [SerializeField] private float maxDuration = 30;
    [SerializeField] private FollowerMove followerPrefab;

    [SerializeField] private float firstFollower = 0.5f;
    [SerializeField] private float followerIncrement = 0.2f;

    private List<FollowerMove> followers = new List<FollowerMove>();

    class Waypoint {
        public float time;
        public Vector3 position;        

        public Waypoint(float t, Vector3 p)
        {
            time = t;
            position = p;
        }
    }

    private List<Waypoint> waypoints = new List<Waypoint>();

    void Update()
    {
        Waypoint w = new Waypoint(Time.time, transform.position);
        waypoints.Add(w);

        // prune any points that are too old
        while (waypoints[0].time < Time.time - maxDuration) {
            waypoints.RemoveAt(0);
        }
    }

    public Vector3? GetPointAt(float pastTime)
    {
        if (pastTime == 0) {
            return transform.position;
        }

        pastTime = Time.time-pastTime;

        if (pastTime < waypoints[0].time) {
            // too long ago
            return null;
        }

        int i = 1;
        while (waypoints[i].time <= pastTime) {
            i++;
        }
        
        float t = (pastTime - waypoints[i-1].time)/(waypoints[i].time - waypoints[i-1].time);
        Vector3 point = Vector3.Lerp(waypoints[i-1].position, waypoints[i].position, t);
        return point;
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        GameObject g = collider.gameObject;
        Collectable c = g.GetComponent<Collectable>();
        if (c != null) {
            FollowerMove f = Instantiate(followerPrefab);            
            f.playerTrail = this;
            f.delay = firstFollower + followerIncrement * followers.Count;
            f.order = -(followers.Count + 1);
            followers.Add(f);
        }
    }


}
