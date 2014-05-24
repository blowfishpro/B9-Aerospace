/*
 * Author: dtobi
 * This work is shared under CC BY-NC-ND 3.0 license.
 * Non commercial, no derivatives, attribution if shared unmodified.
 * You may distribute this code and the compiled .dll as is.
 * 
 * Exception from the no-deriviates clause in case of KSP updates:
 * In case of an update of KSP that breaks the code, you may change
 * this code to make it work again and redistribute it under a different
 * class name until the author publishes an updated version. After a
 * release by the author, the right to redistribute the changed code
 * vanishes.
 * 
 * You must keep this boilerplate in the file and give credit to the author
 * in the download file as well as on the webiste that links to the file.
 * 
 * Should you wish to change things in the code, contact me via the KSP forum.
 * Patches are welcome.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace km_Lib
{
	public class KM_Gimbal : PartModule
	{
		[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "Transform")]
		public string gimbalTransformName = "gimbal"; 

		[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "Yaw", guiUnits = "°")]//, UI_FloatRange(minValue = 0f, maxValue = 25.0f, stepIncrement = 1.0f)]
		public float yawGimbalRange = 1f; 

		[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "Pitch", guiUnits = "°")]//, UI_FloatRange(minValue = 0f, maxValue = 25.0f, stepIncrement = 1.0f)]
		public float pitchGimbalRange = 1f;

		[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "Debug")]//, UI_Toggle(disabledText = "Disabled", enabledText = "Enabled")]
		public bool debug = false;

		[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = false, guiName = "Roll"),
			UI_Toggle(disabledText = "Disabled", enabledText = "Enabled")]
		public bool enableRoll = false;

		[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "Pitch-Trim") , UI_FloatRange(minValue = -14f, maxValue = 14f, stepIncrement = 1f)]
        public float trimX      = 0;
        public float lastTrimX  = 0; // remember the last value to know when to update the editor

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "Trim"),
            UI_Toggle(disabledText="Disabled", enabledText="Enabled")]
        public bool  enableTrim = true;

		[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "DebugLevel")]//,UI_FloatRange(minValue = 0f, maxValue = 5f, stepIncrement = 1f)]
		public float debugLevel = 0;

		[KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "CurrentYaw")]
		private float currentYaw = 0;

		[KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "CurrentPitch")]
		private float currentPitch = 0;

		[KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false, guiName = "CurrentRoll")]
		private float currentRoll = 0;

        [KSPField(isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "Gimbal"),
			UI_Toggle(disabledText="Disabled", enabledText="Enabled")]
		public bool enableGimbal = true;

		[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "Method"),
			UI_Toggle(disabledText="Precise", enabledText="Smooth")]
		public bool enableSmoothGimbal  = false;

		[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "Speed"),
			UI_FloatRange(minValue =1f, maxValue = 100.0f, stepIncrement = 1f)]
		public float responseSpeed = 60;
		public float maxResponseSpeed = 100f;

        [KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false)]
        public bool invertPitch = false;

        [KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false)]
        public bool invertYaw = false;

        [KSPField(isPersistant = false, guiActive = false, guiActiveEditor = false)]
        public bool invertRoll = false;

        bool isRunning = false;

        public List<UnityEngine.Transform>  gimbalTransforms;
		public List<UnityEngine.Quaternion> initRots;

        private ModuleEngines engine;
        private ModuleEnginesFX engineFX;
                		  
		private void printd(int debugPriority, string text){
			if (debug && debugPriority <= debugLevel)
				print ("d"+debugPriority+" "+text);
		}

		[KSPEvent(guiName = "Toggle Debug", guiActive = false)]
		public void dbg_run (){
			debugLevel = 3;
		}

		[KSPAction("Toggle Gimbal")]
		public void toggleGimbal (KSPActionParam param){
			enableGimbal = !enableGimbal;
            resetTransform ();  
		}

        [KSPAction("Trim +")]
        public void plusTrim (KSPActionParam param){
            trimX=trimX+1;
        }
        [KSPAction("Trim -")]
        public void minusTrim (KSPActionParam param){
            trimX=trimX-1;
        }

        [KSPAction("Trim +5")]
        public void plus5Trim (KSPActionParam param){
            trimX=trimX+5;
        }
        [KSPAction("Trim -5")]
        public void minus5Trim (KSPActionParam param){
            trimX=trimX-5;
        }

        [KSPAction("Toggle Trim")]
        public void toggletTrimX (KSPActionParam param){
            enableTrim = !enableTrim;
		}

        private void resetTransform() {
            for (int i = 0; i < gimbalTransforms.Count; i++) {
                gimbalTransforms [i].localRotation = initRots [i];
            }
        }

		public override string GetInfo ()
		{
			return "Yaw Gimbal\n"+yawGimbalRange+"\n" +
				"Pitch Gimbal\n"+pitchGimbalRange+"\n"+
                "KM Gimbal plugin by dtobi";
		}

		public override void OnStart (StartState state)
		{
			foreach(UnityEngine.Transform transform in part.FindModelTransforms(gimbalTransformName))
			{
				gimbalTransforms.Add (transform);
				printd(0, "Adding transform:"+transform);
				printd (0, "Rots:"+transform.localRotation);
				initRots.Add(transform.localRotation);
			}

            if (state == StartState.Editor) {
                this.part.OnEditorAttach += OnEditorAttach;
                this.part.OnEditorDetach += OnEditorDetach;
                this.part.OnEditorDestroy += OnEditorDestroy;
                OnEditorAttach ();
            } else {
                engine = this.part.GetComponentInChildren <ModuleEngines> ();
                if (engine == null)
                    print ("Gimbal ERROR: ModuleEngines not found!");
                engineFX = this.part.GetComponentInChildren <ModuleEnginesFX> ();
                if (engineFX == null)
                    print ("Gimbal ERROR: ModuleEngineFX not found!");

            }
			base.OnStart (state);
		}

		/// <summary>
		/// Determine the signed angle between two vectors, with normal 'n'
		/// as the rotation axis.
		/// Code by Tinus: http://forum.unity3d.com/threads/51092-need-Vector3-Angle-to-return-a-negtive-or-relative-value
		/// </summary>
		public static double AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
		{
			return Mathf.Atan2(
				Vector3.Dot(n, Vector3.Cross(v1, v2)),
				Vector3.Dot(v1, v2));
		}

		public static double doublePrecision(double number, int digits){
			var scale = Math.Pow (10, digits);
			return Math.Round (number * scale) / scale;
		}

		private void OnEditorAttach()
		{
			RenderingManager.AddToPostDrawQueue(99, updateEditor);
		}

		private void OnEditorDetach()
		{

			RenderingManager.RemoveFromPostDrawQueue(99, updateEditor);
			print("KM_Gimbal: OnEditorDetach");
        }

		private void OnEditorDestroy()
		{
			RenderingManager.RemoveFromPostDrawQueue(99, updateEditor);
			print("KM_Gimbal: OnEditorDestroy");

		}

        public override void OnUpdate ()
        {
            // enable activation on action group withou staging
            if (((engine != null && engine.getIgnitionState)
                ||  (engineFX != null && engineFX.getIgnitionState)) && !isRunning) {
                this.part.force_activate ();
            }
        }

		public override void OnFixedUpdate(){
			if (HighLogic.LoadedSceneIsEditor)
				return;
            //if(FlightGlobals.ActiveVessel != vessel)
            //	return;
            updateFlight ();
		}

		private void updateEditor(){
			if (trimX != lastTrimX) {
				Vector3 rotVec = new Vector3 (trimX, 0f, 0f);
				for (int i = 0; i < gimbalTransforms.Count; i++) {
					gimbalTransforms [i].localRotation = initRots [i];
					gimbalTransforms [i].Rotate (rotVec);
				}
			}
			lastTrimX = trimX;
		}

		private void updateFlight(){
            if (!enableGimbal || (engine != null && !engine.EngineIgnited) || 
                (engineFX != null && !engineFX.EngineIgnited)) {
                if (isRunning) {
                    resetTransform ();
                    isRunning = false;
                }
                return;           
            }
			    
            isRunning = true;

			// this needs to be here because these varaibles will be changed
            currentPitch = vessel.ctrlState.pitch * (invertPitch?-1:1);
            currentYaw   = vessel.ctrlState.yaw   * (invertYaw?-1:1);
            currentRoll  = vessel.ctrlState.roll  * (invertRoll?-1:1);

			for (int i = 0; i < gimbalTransforms.Count; i++) {
				// save the current gimbal rotation and restore defaults
				UnityEngine.Quaternion savedRot = gimbalTransforms [i].localRotation;
				gimbalTransforms [i].localRotation = initRots [i];


				if (debug) {
					printd (2, "Engine right:" + gimbalTransforms [i].right);
					printd (2, "Vessel right:" + vessel.ReferenceTransform.right);

					printd (2, "Engine forward:" + gimbalTransforms [i].forward);
					printd (2, "Vessel forward:" + vessel.ReferenceTransform.forward);

					printd (2, "Engine up:" + gimbalTransforms [i].up);
					printd (2, "Vessel up:" + vessel.ReferenceTransform.up);
				}

				// find the vector between engine and vessel
				Vector3 center = vessel.findWorldCenterOfMass () - part.rigidbody.worldCenterOfMass;

                // Determine the contribution of the engine's x and y rotation on the yaw, pitch, and roll
				double yawAngleX = AngleSigned (gimbalTransforms [i].right, vessel.ReferenceTransform.right, gimbalTransforms [i].forward) * -1;
				double pitchAngleX = AngleSigned (gimbalTransforms [i].right, vessel.ReferenceTransform.forward, gimbalTransforms [i].forward);
				double rollAngleX = (enableRoll ? AngleSigned (gimbalTransforms [i].up, center, gimbalTransforms [i].forward) : 0);

				double yawAngleY = AngleSigned (gimbalTransforms [i].up, vessel.ReferenceTransform.right, gimbalTransforms [i].forward);
				double pitchAngleY = AngleSigned (gimbalTransforms [i].up, vessel.ReferenceTransform.forward, gimbalTransforms [i].forward) * -1;
				double rollAngleY = (enableRoll ? AngleSigned (gimbalTransforms [i].right, center, gimbalTransforms [i].forward) : 0);

				double yawContributionX = Math.Sin (yawAngleX);
				double pitchContributionX = Math.Sin (pitchAngleX);
				double rollContributionX = (enableRoll ? Math.Sin (rollAngleX) : 0);

				double yawContributionY = Math.Sin (yawAngleY);
				double pitchContributionY = Math.Sin (pitchAngleY);
				double rollContributionY = (enableRoll ? Math.Sin (rollAngleY) : 0);

				// determine if we are in front or behind the CoM tgo flip axis (Goddard style rockets)
				var pitchYawSign = Math.Sign (Vector3.Dot (vessel.findWorldCenterOfMass () - part.rigidbody.worldCenterOfMass, vessel.transform.up));

                var rotX = Mathf.Clamp((enableTrim?trimX:0) + (float)(currentYaw * yawContributionX * pitchYawSign + currentPitch * pitchContributionX * pitchYawSign + currentRoll * rollContributionX) * pitchGimbalRange, -pitchGimbalRange, pitchGimbalRange);
                var rotY = Mathf.Clamp ((float)(currentYaw * yawContributionY * pitchYawSign + currentPitch * pitchContributionY * pitchYawSign + currentRoll * rollContributionY) * yawGimbalRange * -1, -yawGimbalRange, yawGimbalRange);

				Vector3 rotVec = new Vector3 ((float)rotX, (float)rotY, 0f);

				if (enableSmoothGimbal) {
					printd (3, "Animated Gimbal");
					gimbalTransforms [i].Rotate (rotVec);
					gimbalTransforms [i].localRotation = Quaternion.RotateTowards (savedRot, gimbalTransforms [i].localRotation, responseSpeed * TimeWarp.fixedDeltaTime);
				} else {
					printd (3, "Lerp Gimbal");
					gimbalTransforms [i].Rotate (rotVec);
					gimbalTransforms [i].localRotation = Quaternion.Lerp (savedRot, gimbalTransforms [i].localRotation, responseSpeed / maxResponseSpeed);
				}

				if (debug) {
					printd (1, "rotX:" + rotX);
					printd (1, "rotY:" + rotY);

					printd (1, "pitchYawSign:" + pitchYawSign);
					printd (2, "yawAngleX:" + doublePrecision (Mathf.Rad2Deg * yawAngleX, 2));
					printd (2, "pitchAngleX:" + doublePrecision (Mathf.Rad2Deg * pitchAngleX, 2));
					printd (2, "rollAngleX:" + doublePrecision (Mathf.Rad2Deg * rollAngleX, 2));

					printd (2, "yawAngleY:" + doublePrecision (Mathf.Rad2Deg * yawAngleY, 2));
					printd (2, "pitchAngleY:" + doublePrecision (Mathf.Rad2Deg * pitchAngleY, 2));
					printd (2, "rollAngleY:" + doublePrecision (Mathf.Rad2Deg * rollAngleY, 2));

					printd (2, "ycX:" + yawContributionX + " pcX:" + pitchContributionX + " rcX:" + rollContributionX);
					printd (2, "ycY:" + yawContributionY + " pcY:" + pitchContributionY + " rcX:" + rollContributionY);
					printd (2, "vessel.ReferenceTransform.right:" + vessel.ReferenceTransform.right);
					printd (2, "vessel.ReferenceTransform.forward:" + vessel.ReferenceTransform.forward);
					printd (2, "vessel.ReferenceTransform.up:" + vessel.ReferenceTransform.up);
					printd (1, "vessel.ctrlState.pitch" + vessel.ctrlState.pitch);
					printd (1, "vessel.ctrlState.yaw" + vessel.ctrlState.yaw);
					printd (1, "vessel.ctrlState.roll" + vessel.ctrlState.roll);
				}
			}
		}
	}
}


