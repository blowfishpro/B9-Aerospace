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
using UnityEngine;

/// <summary>
/// Value is the speed of the air flowing 
/// into the ModuleResourceIntake on our part.
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Value is the speed of the air flowing " +
"\n//into the ModuleResourceIntake on our part.")]
[KRequiresModule(typeof(ModuleResourceIntake))]
public class KModuleAnimateAirSpeed : KModuleAnimateValue
{
    #region KSPFields

    [KPartModuleFieldConfigurationDocumentation(
        "True",
        "When True, the animation will only be evaluated when the Intake on this part is open." +
        "\n\t//When open and the player closes the intake it will be interpolated to 0 value." +
        "\n\t//If set to False, animation will be evaluated regardless of intake state.")]
    [KSPFieldDebug("IntakeMustBeOpen")]
    public bool IntakeMustBeOpen = true;

    [KPartModuleFieldConfigurationDocumentation(
        "True",
        "When True, the Normalized Animation Time will interpolate back to 0 when there is no" +
        "\n\t//atmosphere flowing through the intake; otherwise will update as normal.")]
    [KSPFieldDebug("RequireAtmosphere")]
    public bool RequireAtmosphericFlow = true;

    [KPartModuleFieldConfigurationDocumentation(
        "0",
        "The flow value at which the Normalized Animation time will round off to zero and " +
        "\n\t//the intakes will close.")]
    [KSPFieldDebug("FlowThreshold")]
    public float FlowThreshold = 0;

    #endregion

    #region Fields

    /// <summary>
    /// ModuleResourceIntake.airSpeedGui
    /// </summary>
    private FieldInfo _AirSpeed;

    /// <summary>
    /// ModuleRecoucrceIntake.airFlow
    /// </summary>
    private FieldInfo _AirFlow;

    /// <summary>
    /// ModuleResourceIntake.intakeEnabled
    /// </summary>
    private FieldInfo _IntakeOpen;

    /// <summary>
    /// The ModuleResourceIntake that is also within
    /// our part.
    /// </summary>
    private ModuleResourceIntake _Intake;

    /// <summary>
    /// The air speed of the ModuleResourceIntake 
    /// that is also a module within this part.
    /// </summary>
    private double AirSpeed
    {
        get
        {
            return _AirSpeed != null && _Intake != null ?
                (float)_AirSpeed.GetValue(_Intake) :
                0;
        }
    }

    #endregion

    #region Logic

    protected override bool SetupModule()
    {
        if(!base.SetupModule())
            return false;

        foreach(PartModule current in part.Modules)
        {
            if(current.GetType().UnderlyingSystemType != typeof(ModuleResourceIntake))
                continue;

            _Intake = current as ModuleResourceIntake;
            _AirSpeed = _Intake.GetType().GetField("airSpeedGui");
            _AirFlow = _Intake.GetType().GetField("airFlow");
            _IntakeOpen = _Intake.GetType().GetField("intakeEnabled");

            break;
        }

        return _Intake != null
            && _AirSpeed != null
            && _IntakeOpen != null;
    }

    protected override float SolveNormalTime()
    {
        float result;

        if (AirSpeed < MinValue)
        {
            result = 0f;
        }
        else if (AirSpeed > MaxValue)
        {
            result = 1f;
        }
        else
        {
            result = (float)((AirSpeed - MinValue) / _Denominator);
        }

        result = Mathf.Sign(_AnimationRange) == 1 ?
            result * _AnimationRange + MinFrame :
            result * _AnimationRange + MinFrame;

        if(result == 0f)
            return result;

        bool intakeOpen = ((bool)_IntakeOpen.GetValue(_Intake));
        bool airIsFlowing = ((float)_AirFlow.GetValue(_Intake)) > FlowThreshold;

        

        if(IntakeMustBeOpen)
        {
            if(RequireAtmosphericFlow)
            {
                return intakeOpen && airIsFlowing ? result : 0;
            }
            else
            {
                return intakeOpen ? result : 0;
            }
        }
        else if(RequireAtmosphericFlow)
        {
            return airIsFlowing ? result : 0;
        }

        return result;
    }

    #endregion
}
