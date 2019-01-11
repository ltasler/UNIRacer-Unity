using UnityEngine;

public class Car : MonoBehaviour {

	[SerializeField] private KeyCode acceleration;
	[SerializeField] private KeyCode deceleration;
	[SerializeField] private KeyCode left;
	[SerializeField] private KeyCode right;
	[SerializeField] private KeyCode shoot;
	
	[SerializeField] private AnimationCurve engineTourqe;
	[SerializeField] private AnimationCurve gearRatios;
	[SerializeField] private float finalDriveRatio;

	private Vector2 _inputDirecton;
	private bool _shoot;

	private WheelCollider _frontLeft;
	private const string FRONT_LEFT_WHEEL = "FrontLeft";
	private WheelCollider _frontRight;
	private const string FRONT_RIGHT_WHEEL = "FrontRight";
	private WheelCollider _backLeft;
	private const string BACK_LEFT_WHEEL = "BackLeft";
	private WheelCollider _backRight;
	private const string BACK_RIGHT_WHEEL = "FrontLeft";
	
	
	private void Awake() {
		for (int i = 0; i < this.transform.childCount; i++) {
			
		}
	}

	void Update() {
		HandleInput();
	}

	private void FixedUpdate() {
		throw new System.NotImplementedException();
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
}
