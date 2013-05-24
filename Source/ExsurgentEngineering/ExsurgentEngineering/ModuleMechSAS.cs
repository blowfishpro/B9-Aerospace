// Original version from Luke321 posted to 
// http://forum.kerbalspaceprogram.com/showthread.php/22543-Plugin-Part-0-17-MechSAS-controllable-SAS-for-Mechjeb


using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MechSAS
{
    public class MechSAS : PartModule
    {
		
        [KSPField(guiName="Current Torque")]
        public float
            torqueFactor;

        [KSPField(guiName="Max Torque")] 
        public float
            maxTorque;
		
        [KSPField(guiName="Active")] 
        public bool
            running;


        public static Dictionary<Vessel,List<MechSAS>> instances;

        public List<MechSAS> ActiveSet
        {
            get
            {
                List<MechSAS> list = null;
                instances.TryGetValue(vessel, out list);

                if (list == null)
                    instances.Add(vessel, new List<MechSAS>());

                return list;
            }
        }

        // primary for the active vessel
        public MechSAS Primary
        {
            get
            {
                return ActiveSet.FirstOrDefault(inst => inst.running);
            }
        }



        [KSPEvent(guiActive = true, guiName = "Activate MechSAS")]
        public void Activate()
        {
            
            Events ["Deactivate"].active = true;
            Events ["Activate"].active = false;
            running = true;
        }
		
        [KSPEvent(guiActive = true, guiName = "Deactivate MechSAS", active = false)]
        public void Deactivate()
        {
            Events ["Deactivate"].active = true;
            Events ["Activate"].active = false;
            running = true;
        }


        [KSPAction("Toggle MechSAS")]
        public void ToggleAction(KSPActionParam param)
        {
            if (running)
                Deactivate();
            else
                Activate();
        }

        public override void OnAwake()
        {
//            Debug.Log("OnAwake: " + vessel);
            if (instances == null)
                instances = new Dictionary<Vessel, List<MechSAS>>();
        }

        public override void OnStart(StartState state)
        {
//            Debug.Log(String.Format("OnStart: {0}", state));

            if (state == StartState.Editor)
                return;
            running = true;
            part.force_activate();
        }

        public override void OnActive()
        {
//            var instance = ActiveSet.FirstOrDefault(inst => inst == this);
//            if (instance == null)
//                ActiveSet.Add(this);
        }

        public override void OnInactive()
        {
//            var instance = ActiveSet.FirstOrDefault(inst => inst == this);
//            if (instance)
//                ActiveSet.Add(this);
        }

        public float MaxTorque
        {
            get
            {
                return ActiveSet.Sum(ms => ms.maxTorque);
            }
        }

        public override void OnFixedUpdate()
        {
//            if (HighLogic.LoadedSceneIsEditor)
//                return;
//            if (FlightGlobals.ActiveVessel != vessel)
//                return;
            if (this != Primary)
                return;


            var pitch = FlightInputHandler.state.pitch;
            var roll = FlightInputHandler.state.roll;
            var yaw = FlightInputHandler.state.yaw;

            var torque = new Vector3(-pitch, -roll, -yaw) * MaxTorque;
            vessel.rigidbody.AddRelativeTorque(torque);

        }
	

        [KSPEvent(guiActive=false, active=true)]
        public void MechJebVesselStateUpdated()
        {
//            if (this != Primary)
//                return;
//            Debug.Log("handling MechJebVesselStateUpdated");
            BaseEventData ed = new BaseEventData(BaseEventData.Sender.USER); 

            var script = String.Format(@"
				torqueRAvailable = torqueRAvailable + {0}
				torquePYAvailable = torquePYAvailable + {0}", maxTorque);
            ed.Set("script", script); 
            vessel.rootPart.SendEvent("MechJebLua", ed);
        }

		
    }
}
