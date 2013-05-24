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

/// <summary>
/// Animates based on: 'KSP.FlightGlobals.ship_tgtSpeed'. 
/// If target is lost/deselected, and interpolation is 
/// not active the value will jump back to 0.0f.
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Animates based on: 'KSP.FlightGlobals.ship_tgtSpeed'. " +
"\n//If target is lost/deselected, and interpolation is " +
"\n//not active the value will jump back to 0.0f.")]
public class KModuleAnimateTargetSpeed : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        if(FlightGlobals.fetch == null)
            return 0f;

        if(this.vessel != FlightGlobals.ActiveVessel)
            return LastNormalTime;

        if(FlightGlobals.fetch.VesselTarget == null)
            return 0f;

        return (float)((FlightGlobals.ship_tgtSpeed - MinValue) / _Denominator);
    }

    protected override void UpdateModule()
    {
        base.UpdateModule();

        this.IsLocked = this.vessel != FlightGlobals.ActiveVessel;
    }
}
