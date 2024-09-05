using UnityEngine;
using UnityEditor;

public class AnimationPreview : EditorWindow
{
    private GameObject selectedObject;
    private AnimationClip clipToPlay;
    private AnimationClip[] clips;
    private string[] clipTitles;
    private float time = 0.0f;
    private int selectedIndex = 0;
    private int lastIndex = 0;

    [MenuItem("Tools/Animator Preview", false, 2000)]
    public static void DoWindow()
    {
        var window = GetWindowWithRect<AnimationPreview>(new Rect(0, 0, 300, 80));
        window.Show(); 
    }

    public void OnSelectionChange()
    {
        selectedObject = Selection.activeGameObject;
        if (selectedObject.GetComponent<Animator>())
            {
                clips = selectedObject.GetComponent<Animator>().runtimeAnimatorController.animationClips;
                clipTitles = new string[clips.Length];
                for (int i = 0; i < clips.Length; i++)
                {
                    clipTitles[i] = clips[i].name;
                }
                AnimationMode.InAnimationMode();
            }
            else
            {
                clips = new AnimationClip[0];
                clipTitles = new string[0];
                selectedIndex = 0;
                lastIndex = 0;
                AnimationMode.StopAnimationMode();
            }
            Repaint();
    }

    public void OnGUI()
    {
        if (selectedObject == null)
        {
            return;
        }

        if (selectedObject.GetComponent<Animator>() == null)
        {
            EditorGUILayout.HelpBox("Please select a GameObject with an Animator component", MessageType.Info);
            return;
        }

        if (clips.Length < 0)
        {
            return;
        }

        EditorGUILayout.BeginVertical();

        selectedIndex = EditorGUILayout.Popup("Animation Clips", selectedIndex, clipTitles);

        if (selectedIndex != lastIndex)
        { 
            time = 0.0f;
            lastIndex = selectedIndex;
        }

        clipToPlay = clips[selectedIndex];

        if (clipToPlay != null)
        {
            float startTime = 0.0f;
            float stopTime = clipToPlay.length;
            time = EditorGUILayout.Slider(time, startTime, stopTime);
        }
        else if (AnimationMode.InAnimationMode())
        {
            AnimationMode.StopAnimationMode();
        }
        EditorGUILayout.EndVertical();
    }

    void Update()
    {
        if (selectedObject == null)
            return;

        if (clipToPlay == null)
            return;

        if (!EditorApplication.isPlaying && AnimationMode.InAnimationMode())
        {
            AnimationMode.BeginSampling();
            AnimationMode.SampleAnimationClip(selectedObject, clipToPlay, time);
            AnimationMode.EndSampling();

            SceneView.RepaintAll();
        }
    }
}
