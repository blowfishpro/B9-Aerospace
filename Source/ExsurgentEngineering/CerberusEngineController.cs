//using UnityEngine;
//
//namespace ExsurgentEngineering
//{
//	[KSPAddon (KSPAddon.Startup.EditorAny, false)]
//	public class CerberusEngineController : MonoBehaviour
//	{
//		Vessel currentVessel;
//
//		public void Awake ()
//		{
//			Debug.Log ("CerberusEngineController.Awake");
//
//		}
//
//		public void Start ()
//		{
//			Debug.Log ("CerberusEngineController.Start");
//			GameEvents.onVesselWasModified.Add (OnVesselWasModified);
//			GameEvents.onVesselChange.Add (OnVesselChange);
//
//			currentVessel = FlightGlobals.ActiveVessel;
//			Debug.Log ("currentVessel.name: " + currentVessel.name);
//		}
//
//		public void Update ()
//		{
//			Debug.Log ("CerberusEngineController.Update");
//		}
//
//		public void LateUpdate ()
//		{
//			Debug.Log ("CerberusEngineController.LateUpdate");
//		}
//
//		public void FixedUpdate ()
//		{
//			Debug.Log ("CerberusEngineController.FixedUpdate");
//		}
//
//		public void OnVesselWasModified (Vessel data)
//		{
//			Debug.Log ("CerberusEngineController.OnVesselWasModified: " + data);
//		}
//
//		public void OnVesselChange (Vessel data)
//		{
//			Debug.Log ("CerberusEngineController.OnVesselChange: " + data);
//		}
//	}
//}
