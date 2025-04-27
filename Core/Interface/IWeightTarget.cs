using Core;

namespace ServiceLayer
{
    public interface IWeightTarget
    {
         Dictionary<WeightCategory, (float TargetBMI, float TargetWeightChange, float TargetFatPercentageChange)> Targets();
        (float TargetBMI, float TargetWeightChange, float TargetFatPercentageChange) GetTargetForCategory(WeightCategory category);

    }
}