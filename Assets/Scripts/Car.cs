using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Car : MonoBehaviour {

    [SerializeField] 
    float topSpeed;

    [SerializeField] 
    float acclearationFactor;

    [SerializeField] 
    float rotationFactor;

    float _speed;
    float _steer;
    Transform _transform;
    Rigidbody _rigidbody;
   
    // Start is called before the first frame update
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _speed = 0.0f;
    }

    // Update is called once per frame
    void Update() {
        HandleInput();
    }

    void FixedUpdate() {
        Vector3 rotation = _transform.eulerAngles;
        rotation.y += _steer * Time.fixedDeltaTime * rotationFactor;
        _transform.localRotation = Quaternion.Euler(rotation);

        Vector3 direction = _transform.forward * _speed * Time.fixedDeltaTime;
        Debug.Log(direction);
        Vector3 position = _transform.position + direction;
        _rigidbody.MovePosition(position);
    }
    void HandleInput() {
        _speed += -Input.GetAxis("Vertical") * acclearationFactor;

        _steer = Input.GetAxis("Horizontal");
    } 
}
