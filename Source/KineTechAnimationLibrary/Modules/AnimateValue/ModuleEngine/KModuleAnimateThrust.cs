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
using System.Reflection;

/// <summary>
/// Value is the amount of thrust currently
/// being produced by the ModuleEngines that
/// is attached to our part.
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Value is the amount of thrust currently" +
"\n//being produced by the ModuleEngines that" +
"\n//is attached to our part.")]
[KRequiresModule(typeof(ModuleEngines))]
public class KModuleAnimateThrust : KModuleAnimateValue
{
    [KPartModuleFieldConfigurationDocumentation("True", "When True, the maximum thrust of the engine will be used as MaxValue; otherwise False.")]
    [KSPFieldDebug("UseMaxThrust", false, isPersistant=true)]
    public bool UseMaxThrust = true;
    
    private ModuleEngines _Engine;
    private FieldInfo _Thrust;

    private float _CurrentThrust { get { return _Thrust != null && _Engine != null ? (float)_Thrust.GetValue(_Engine) : 0f; } }

    protected override bool SetupModule()
    {
        foreach (PartModule cModule in part.Modules)
        {
            if (cModule.GetType() != typeof(ModuleEngines))
                continue;

            _Engine = cModule as ModuleEngines;
            break;
        }

        if (_Engine != null)
        {
            _Thrust = _Engine.GetType().GetField("finalThrust");

            if(UseMaxThrust)
                this.MaxValue = _Engine.maxThrust;
        }

        return base.SetupModule();
    }

    protected override float SolveNormalTime()
    {
        return (float)(_CurrentThrust - MinValue) / _Denominator;
    }
}
