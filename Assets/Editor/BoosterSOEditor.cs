using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoosterSO))]
public class BoosterSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BoosterSO so = (BoosterSO)target;

        BoosterDriveName currentOptions = so.driveNames;

        so.driveNames = (BoosterDriveName)EditorGUILayout.MaskField(
            "此推動器支援的模式",
        (int)currentOptions,
            System.Enum.GetNames(typeof(BoosterDriveName))
        );
        so.data.BoosterName = EditorGUILayout.TextField("推進器名稱", so.data.BoosterName);
        so.data.KeyString = EditorGUILayout.TextField("KeyString", so.data.KeyString);
        so.data.DeltaSpeedPower = EditorGUILayout.FloatField("推進器每秒的加速度", so.data.DeltaSpeedPower);
        so.data.DeltaAngularPower = EditorGUILayout.FloatField("推進器每秒的角加速度", so.data.DeltaAngularPower);
        so.data.MaxSpeed = EditorGUILayout.FloatField("推進器最大速度", so.data.MaxSpeed);
        so.data.MaxAngularSpeed = EditorGUILayout.FloatField("推進器最大角速度", so.data.MaxAngularSpeed);
        //so.data.ImplusForce = EditorGUILayout.FloatField("推進器的瞬間力", so.data.ImplusForce);
        //so.data.ImplusTourque = EditorGUILayout.FloatField("推進器的瞬間扭矩", so.data.ImplusTourque);

        if (so.data.Accelerationcurve == null)
        {
            so.data.Accelerationcurve = new AnimationCurve(); // 初始化一個空的 AnimationCurve
        }
        so.data.Accelerationcurve = EditorGUILayout.CurveField("加速度曲線(注意模式是否可以支援)", so.data.Accelerationcurve);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}