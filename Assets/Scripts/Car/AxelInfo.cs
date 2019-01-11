using System;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

namespace UNIRacer.Car {
	[Serializable]
	public class AxelInfo {
		public WheelCollider leftWheel;
		public WheelCollider rightWheel;
		public bool motor;
		public bool steering;
	}
}
