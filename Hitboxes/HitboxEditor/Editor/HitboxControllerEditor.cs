using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HitboxController))]
public class HitboxControllerEditor : Editor
{
    HitboxController hitboxController;
    private bool resizeMode = false;

    private void OnEnable()
    {
        hitboxController = (HitboxController)target;
    }

    private void OnSceneGUI()
    {
        InputEvents();
        ShowScaleHandle();
        SceneView.RepaintAll();
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    }

    public void InputEvents()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
        {
            resizeMode = !resizeMode;
        }
    }
    public void ShowScaleHandle()
    {
        if (!resizeMode)
        {
            return;
        }

        Vector3 position = hitboxController.transform.position - (hitboxController.hitBoxSize * 0.5f);
        EditorGUI.BeginChangeCheck();
        Vector3 newScale = Handles.ScaleHandle(hitboxController.hitBoxSize, Tools.handlePosition, Quaternion.Euler(0f, 90f, 0f), HandleUtility.GetHandleSize(position));

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Scale Cube");
            hitboxController.hitBoxSize = newScale;
        }
    }
}
