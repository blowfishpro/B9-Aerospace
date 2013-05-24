/// <summary>
/// Animates base on: 'this.vessel.geeForce'
/// Value is the part's current vessel's G-force.
/// </summary>
[KPartModuleConfigurationDocumentation(
"\n//Animates base on: 'this.vessel.geeForce'" +
"\n//Value is the part's current vessel's G-force.")]
public class KModuleAnimateGeeForce : KModuleAnimateValue
{
    protected override float SolveNormalTime()
    {
        return (float)(this.vessel.geeForce - MinValue) / _Denominator;
    }
}
