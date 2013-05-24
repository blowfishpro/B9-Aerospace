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

//[KRequiresModule(typeof(ModuleDeployableSolarPanel))]
//public class KModuleAnimateSolarCharge : KModuleAnimateValue
//{
//    private ModuleDeployableSolarPanel _SolarPanel;
//    private FieldInfo _FlowRate;

//    private float FlowRate
//    {
//        get
//        {
//            return _FlowRate != null && _SolarPanel != null ?
//                (float)_FlowRate.GetValue(_SolarPanel) :
//                0;
//        }
//    }

//    protected override bool  SetupModule()
//    {
//        if(!base.SetupModule())
//            return false;

//        foreach(PartModule current in part.Modules)
//        {
//            if(current.GetType() != typeof(ModuleDeployableSolarPanel))
//                continue;

//            _SolarPanel = current as ModuleDeployableSolarPanel;
//            break;
//        }

//        if (_SolarPanel != null)
//        {
//            _FlowRate = _SolarPanel.GetType()
//                .GetField("flowRate", BindingFlags.Instance | BindingFlags.Public);
//        }
//        return _FlowRate != null && _SolarPanel != null;
//    }

//    protected override float SolveNormalTime()
//    {
//        LastNormalTime =
//            Mathf.Lerp(
//                LastNormalTime,
//                Mathf.Clamp01((FlowRate - MinValue) / _Denominator),
//                Time.deltaTime * LerpDampening);
//        return LastNormalTime;
//    }

//}

