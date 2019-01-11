using System;
using UnityEngine;

public class Car : MonoBehaviour {

	[SerializeField] private KeyCode acceleration;
	[SerializeField] private KeyCode deceleration;
	[SerializeField] private KeyCode left;
	[SerializeField] private KeyCode right;
	[SerializeField] private KeyCode shoot;

	[SerializeField] private AnimationCurve engineTourqe;
	[SerializeField] private float breakTorque;
	[SerializeField] private AnimationCurve gearRatios;
	[SerializeField] private float finalDriveRatio;

	[SerializeField] private float maxSteer;
	//[SerializeField] private float timeToMaxSteer;

	private Vector2 _inputDirecton;
	private bool _shoot;

	private int _gearIndex;

	private WheelCollider _frontLeft;
	private const string FRONT_LEFT_WHEEL = "FrontLeft";
	private WheelCollider _frontRight;
	private const string FRONT_RIGHT_WHEEL = "FrontRight";
	private WheelCollider _backLeft;
	private const string BACK_LEFT_WHEEL = "BackLeft";
	private WheelCollider _backRight;
	private const string BACK_RIGHT_WHEEL = "BackRight";

	private const int WHEEL_COUNT = 4;
	
	
	private void Awake() {
		int wheelCount = 0;
		for (int i = 0; i < this.transform.childCount; i++) {
			Transform t = this.transform.GetChild(i);
			WheelCollider wheel = t.GetComponent<WheelCollider>();
			if (wheel != null) {
				wheelCount++;
				switch (wheel.name) {
					case FRONT_LEFT_WHEEL:
						_frontLeft = wheel;
						break;
					case FRONT_RIGHT_WHEEL:
						_frontRight = wheel;
						break;
					case BACK_LEFT_WHEEL:
						_backLeft = wheel;
						break;
					case BACK_RIGHT_WHEEL:
						_backRight = wheel;
						break;
					default:
						Debug.LogWarningFormat("Wheel {0} is not recognized on object {1}"
							, wheel.name, this.name);
						break;
				}
			}
		}
		if (wheelCount != 4)
			Debug.LogWarningFormat("Unexpected wheel count for {0}. Expected {1}, but recived {2}"
				, this.name, WHEEL_COUNT, wheelCount);
	}

	void Start() {
		//TODO: Mehanizem za menjavanje prestav
		_gearIndex = 1;
	}

	void Update() {
		HandleInput();
		RotateWheel(_frontLeft);
		RotateWheel(_frontRight);
		RotateWheel(_backLeft);
		RotateWheel(_backRight);
	}

	private void FixedUpdate() {
		if (_inputDirecton.y < 0) {
			SetBraking(breakTorque);
		}
		else {
			SetBraking(0);
		}
		if (_inputDirecton.y > 0) {
			float currentRpm = ((_backLeft.rpm + _backRight.rpm) / 2) 
			                   * finalDriveRatio * gearRatios.Evaluate(_gearIndex);
			float motorTorque = engineTourqe.Evaluate(currentRpm) * gearRatios.Evaluate(_gearIndex)
			                                                      * finalDriveRatio;
		
			//BWD
			_backLeft.motorTorque = motorTorque / 2;
			_backRight.motorTorque = motorTorque / 2;
		}

		//float curentSteerAngle = (_frontLeft.steerAngle + _frontRight.steerAngle) / 2;
		float destSteerAngle = maxSteer * _inputDirecton.x;
		//float actualSteerAngle = Math.Min(curentSteerAngle + destSteerAngle * Time.fixedDeltaTime * timeToMaxSteer, destSteerAngle);
		_frontLeft.steerAngle = destSteerAngle;
		_frontRight.steerAngle = destSteerAngle;
	}

	void SetBraking(float braking) {
		_backLeft.brakeTorque = braking;
		_backRight.brakeTorque = braking;
		_frontLeft.brakeTorque = braking;
		_frontRight.brakeTorque = braking;
	}

	void HandleInput() {
		_inputDirecton = new Vector2(0.0f, 0.0f);
		if (Input.GetKey(acceleration))
			_inputDirecton.y += 1;
		if (Input.GetKey(deceleration))
			_inputDirecton.y -= 1;
		if (Input.GetKey(left))
			_inputDirecton.x -= 1;
		if (Input.GetKey(right))
			_inputDirecton.x += 1;
		if (Input.GetKey(shoot))
			_shoot = true;
		else
			_shoot = false;
	}

	void RotateWheel(WheelCollider wheel) {
		Vector3 position;
		Quaternion rotation;
		wheel.GetWorldPose(out position, out rotation);
		Transform t = wheel.transform;
		t.rotation = rotation;
	}
}
