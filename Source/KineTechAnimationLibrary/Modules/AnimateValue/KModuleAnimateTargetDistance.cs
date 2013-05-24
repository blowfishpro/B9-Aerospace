/* * * * * * * * * * * * * * * */
/* Author: Dustin 'Kine' Judson
/*
/* This work is licensed under the 
/* Creative Commons Attribution-ShareAlike 3.0 Unported License. 
/*
/* To view a copy of this license, visit: 
/* http://creativecommons.org/licenses/by-sa/3.0/
/* 
/* A the following must be included with 
/* any and all formats using this work or any
/* derivatives of this work:
/* 
/* "Kine-Tech Animation Library by Dustin 'Kine' Judson is viewable at: https://github.com/KineMorto/KineTechAnimationLibrary"
/*
/* * * * * * * * * * * * * * * */
using UnityEngine;

/// <summary>
/// Value is the current distance from the vessel's
/// transform to it target. Target is only stored for
/// the active vessel, so when the vessel is not active
/// this is not updated.
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Value is the current distance from the vessel's" +
"\n//transform to it target. Target is only stored for" +
"\n//the active vessel, so when the vessel is not active" +
"\n//this is not updated.")]
public class KModuleAnimateTargetDistance : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        if(FlightGlobals.fetch == null)
            return 0f;

        if(this.vessel != FlightGlobals.ActiveVessel)
            return LastNormalTime;

        
        if(FlightGlobals.fetch.VesselTarget == null)
            return 0f;

        Vector3 working = FlightGlobals.fetch.VesselTarget.GetTransform().localPosition;
        working -= this.vessel.transform.position;

        return (float)((working.magnitude - MinValue) / _Denominator);
    }

    protected override void UpdateModule()
    {
        base.UpdateModule();

        this.IsLocked = this.vessel != FlightGlobals.ActiveVessel;  
    }
}
