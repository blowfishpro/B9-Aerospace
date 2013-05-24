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
/// Animates based on: 'this.vessel.srf_velocity.magnitude'
/// Value is the current speed relative to the part's 
/// vessel's current orbital body's surface. 
/// Also, 's's's's's's's's
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Animates based on: 'this.vessel.srf_velocity.magnitude'" +
"\n//Value is the current speed relative to the part's " +
"\n//vessel's current orbital body's surface. " +
"\n//Also, 's's's's's's's's")]
public class KModuleAnimateSurfaceSpeed : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        return (float)(this.vessel.srf_velocity.magnitude / _Denominator);
    }
}

