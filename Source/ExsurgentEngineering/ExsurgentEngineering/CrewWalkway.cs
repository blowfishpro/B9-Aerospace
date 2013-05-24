using UnityEngine;
using System;
using System.Collections.Generic;

namespace ExsurgentEngineering
{

    public class CrewWalkway : PartModule
    {
        [KSPEvent(guiActive=true, guiName="Add Crew")]
        public void AddCrew()
        {
            for (int i = 0; i < part.parent.CrewCapacity; i++)
            {
                KerbalCrewRoster.AssignNextAvailableCrewMemberTo(part.parent);    
            }

        }

        [KSPAction("Add Crew")]
        public void AddCrewAction(KSPActionParam actionParam)
        {
            AddCrew();
        }
        

//        [KSPEvent(guiActive=true, guiName="Remove One")]
        public void RemoveOne()
        {
////            var crew = vessel.GetVesselCrew().FindAll(protoman => protoman.seat.part == part);
//
            var roster = KerbalCrewRoster.CrewRoster;

            //var crewbal = roster.FindAll(protoman => protoman.seat != null).Find(protoman => protoman.seat.part == part.parent);
            var crewbal = roster.Find(protoman => protoman.seat && protoman.seat.part == part.parent);
            Debug.Log(String.Format("crewbal: {0}", crewbal.name));


            var msg = "";

            foreach (var guy in roster)
            {
                msg += String.Format("name: {0}, status: {1}\n", guy.name, guy.rosterStatus);
            }
            Debug.Log(msg);

            var kerbal = crewbal.KerbalRef;
//            FlightEVA.SpawnEVA(kerbal);
            kerbal.die();

            //

            //            crewbal.rosterStatus = ProtoCrewMember.RosterStatus.RESPAWN;
//            var kerbal = crewbal.KerbalRef;
//            if (kerbal != null)
//            {
//
//                KerbalGUIManager.RemoveActiveCrew(kerbal);
//            }
//            crewbal.seat.DespawnCrew();
//            crewbal.seat = null;

        }

        //crewbal.Die();
            
//            Debug.Log("part.partName: " + part.partName);
//            Debug.Log("roster: " + roster);
//            Debug.Log("part.parent.partName: " + part.parent.partName);
//            foreach (var protoman in roster)
//            {
//
//                Debug.Log(String.Format("protoman: {0}, name; {1}", protoman, protoman.name));
//                Debug.Log(String.Format("\t protoman.rosterstatus: {0}", protoman.rosterStatus));
//                if (protoman.seat != null)
//                {
//                    Debug.Log(String.Format("\t seat: {0}", protoman.seat));
//                    Debug.Log(String.Format("\t seat.part: {0}, seat.part.partName", protoman.seat.part, protoman.seat.part.partName));
//                }
//
//            }
//

        //            foreach (var crew in vessel.GetVesselCrew())
//            {
//                Debug.Log("crew: " + crew);
//        }
          

//            var seat = part.internalModel.seats.Find(s => s.taken);
//            seat.DespawnCrew();
//        }

    }

}

