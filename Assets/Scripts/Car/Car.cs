using System;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace UNIRacer.Car {
	public class Car : MonoBehaviour {
		#region Input keys

		[SerializeField] private KeyCode accelerationKey;
		[SerializeField] private KeyCode decelerationKey;
		[SerializeField] private KeyCode leftKey;
		[SerializeField] private KeyCode rightKey;
		[SerializeField] private KeyCode shootKey;

		#endregion

		#region Car parameters

		[SerializeField] private List<AxelInfo> axleInfos;
		[SerializeField] private AnimationCurve engineTourqe;
		[SerializeField] private float breakTorque;
		[SerializeField] private AnimationCurve gearRatios;
		[SerializeField] private float finalDriveRatio;
		[SerializeField] private float maxSteeringAngle;

		[SerializeField] private float downwardForceFactor;

		#endregion

		#region private variables

		private bool _accelerate;
		private bool _decelerate;
		private bool _left;
		private bool _right;

		private float _engineRpm;
		private int _gearIndex;
		private float _minRpm;
		private float _maxRpm;

		private float _minGear = 1;
		private float _maxGear;

		private float _speed;
		private Rigidbody _rigidbody;
		
		#endregion

		#region

		public float Speed {
			get { return _speed; }
		}
		public Waypoint LastWaypoint { get; set; }
		public int Lap { get; set; }

		#endregion
		

		private void Awake() {
			_rigidbody = this.GetComponent<Rigidbody>();
		}

		void Start() {
			_engineRpm = engineTourqe.keys[0].time;
			_minRpm = engineTourqe.keys[0].time;
			_maxRpm = engineTourqe.keys[engineTourqe.keys.Length - 1].time;
			_gearIndex = 1;
			_maxGear = gearRatios.keys[gearRatios.keys.Length - 1].time;
			LastWaypoint = null;
		}

		void Update() {
			HandleInput();
			if (_rigidbody)
				_speed = _rigidbody.velocity.magnitude * 3.6f;
		}

		private void FixedUpdate() {
			float avgWheelRpm = GetAvgWheelRpm();
			_engineRpm = Math.Min(_minRpm +
			                      (avgWheelRpm * finalDriveRatio * gearRatios.Evaluate(_gearIndex)), _maxRpm);
			SwitchGear();

			foreach (AxelInfo i in axleInfos) {
				RotateWheel(i.leftWheel);
				RotateWheel(i.rightWheel);
				if (i.motor && _accelerate) {
					float torque = engineTourqe.Evaluate(_engineRpm) * gearRatios.Evaluate(_gearIndex)
					                                                 * finalDriveRatio;
					i.leftWheel.motorTorque = torque;
					i.rightWheel.motorTorque = torque;
				}
				else {
					i.leftWheel.motorTorque = 0;
					i.rightWheel.motorTorque = 0;
				}

				if (_decelerate) {
					i.leftWheel.brakeTorque = breakTorque;
					i.rightWheel.brakeTorque = breakTorque;
				}
				else {
					i.leftWheel.brakeTorque = 0;
					i.rightWheel.brakeTorque = 0;
				}

				if (i.steering) {
					float steerAngle = maxSteeringAngle;
					if (_left)
						steerAngle *= -1;
					else if (_right)
						steerAngle *= 1;
					else
						steerAngle *= 0;
					i.leftWheel.steerAngle = steerAngle;
					i.rightWheel.steerAngle = steerAngle;
				}
			}

			Vector3 force = Vector3.down * _speed * downwardForceFactor;
			_rigidbody.AddForce(force);
		}

		void RotateWheel(WheelCollider wheel) {
			Vector3 position;
			Quaternion rotation;
			wheel.GetWorldPose(out position, out rotation);
			Transform t = wheel.transform;
			t.rotation = rotation;
			//t.position = position;
		}

		float GetAvgWheelRpm() {
			float total = 0;
			int count = 0;
			foreach (AxelInfo i in axleInfos) {
				if (i.motor) {
					total = i.leftWheel.rpm + i.rightWheel.rpm;
					count += 2;
				}
			}

			return total / count;
		}

		void HandleInput() {
			_accelerate = Input.GetKey(accelerationKey);
			_decelerate = Input.GetKey(decelerationKey);
			_left = Input.GetKey(leftKey);
			_right = Input.GetKey(rightKey);
		}

		private float p = 0;
		void SwitchGear() {
			p = (_engineRpm - _minRpm) / (_maxRpm - _minRpm);
			if (p < 0.1f && _gearIndex > _minGear)
				_gearIndex--;
			if (p > 0.9f && _gearIndex < _maxGear)
				_gearIndex++;
		}
	}
}