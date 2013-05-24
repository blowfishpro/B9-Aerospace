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
/// Animates based on: 'this.vessel.latitude'. 
/// Value is the current latitude of the part's
/// vessel relative to its current mainbody.
/// An interesting part shall use this :)
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Animates based on: 'this.vessel.latitude'. " +
"\n//Value is the current latitude of the part's" +
"\n//vessel relative to its current mainbody." +
"\n//An interesting part shall use this :)")]
public class KModuleAnimateLatitude : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        return (float)(this.vessel.latitude - MinValue) / _Denominator;
    }
}

