using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UNIRacer.Car;

public class Waypoint : MonoBehaviour {

    [SerializeField] private Waypoint previousWaypoint;
    [SerializeField] private bool firstWaypoint;

    private Waypoint _nextWaypoint;


    private void Awake() {
        if (!previousWaypoint) {
            Debug.LogError($"Error: Previous Waypoint is not defined. Lap is most likely not defined");
            return;
        }
//        TODO: Nek pregled waypointov?
//        int countFirstWaypoints = 0;
//        Waypoint w = this;
//        while (w.previousWaypoint != this) {
//            if (firstWaypoint)
//                countFirstWaypoints++;
//            w = previousWaypoint;
//            if (w == null)
//                return;
//        }
//        _nextWaypoint = w;
//        if (countFirstWaypoints != 1) {
//            Debug.LogWarning($"There are {countFirstWaypoints} in scene. Passing trought First " +
//                             $"Waypoint counts a lap. Are you sure that there should be this many in 1 scene?");
//        }
    }


    private void Start() {
        if (firstWaypoint) {
            Debug.Log($"Waypoint {this.name} is First Waypoint. Make sure that it is only one.");
        }
    }

    private void OnTriggerEnter(Collider other) {
        Car car = other.GetComponentInParent<Car>();
        if (!car.LastWaypoint || previousWaypoint == car.LastWaypoint) {
            Debug.Log($"Player {car.name} has reached waypoint {this.name}.");
            car.LastWaypoint = this;
            if (firstWaypoint ) {
                car.Lap++;
                Debug.Log($"Player {car.name} made a lap. It is his {car.Lap}");
            }
        }
        else {
            Debug.Log($"Player {car.name} has reached wrong waypoint {this.name}");
        }
    }
}
