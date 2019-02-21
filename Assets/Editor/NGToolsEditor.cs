// /!\ Save this script in an Editor folder /!\
// Open the window at Tools/NG Scene Camera to enable & configure the speeds.
// Use the Scroll Wheel to change speed on the fly.
// Press Ctrl to use the sub-speed.
// Use Ctrl + Scroll Wheel to change the sub-speed.

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NGToolsEditor
{
  [InitializeOnLoad]
  public class NGSceneCameraWindow : EditorWindow
  {
    public const string Title = "NG Scene Camera";

    private static FieldInfo  s_FlySpeed;

    private int           currentSpeed;
    private List<float>       speed;
    private int           currentSubSpeed;
    private List<float>       subSpeed;

    private GUIStyle  rightToLeftLabel;
    private Vector2   scrollPosition;

    static  NGSceneCameraWindow()
    {
      Type  SceneViewMotion = typeof(Editor).Assembly.GetType("UnityEditor.SceneViewMotion");

      if (SceneViewMotion != null)
        NGSceneCameraWindow.s_FlySpeed = SceneViewMotion.GetField("s_FlySpeed", BindingFlags.NonPublic | BindingFlags.Static);
    }

    [MenuItem("Tools/" + NGSceneCameraWindow.Title)]
    public static void  Open()
    {
      EditorWindow.GetWindow<NGSceneCameraWindow>(NGSceneCameraWindow.Title);
    }

    protected virtual void  OnEnable()
    {
      if (NGSceneCameraWindow.s_FlySpeed != null)
      {
        int n = EditorPrefs.GetInt(NGSceneCameraWindow.Title + ".speed", -1);
        if (n > 0)
        {
          this.speed = new List<float>(n);

          for (int i = 0; i < n; i++)
            this.speed.Add(EditorPrefs.GetFloat(NGSceneCameraWindow.Title + ".speed." + i));
        }

        if (this.speed == null || this.speed.Count == 0)
          this.speed = new List<float>() { 5F, 10F, 20F, 40F, 100F };

        this.currentSpeed = Mathf.Clamp(this.currentSpeed, 0, this.speed.Count - 1);

        n = EditorPrefs.GetInt(NGSceneCameraWindow.Title + ".subSpeed", -1);
        if (n > 0)
        {
          this.subSpeed = new List<float>(n);

          for (int i = 0; i < n; i++)
            this.subSpeed.Add(EditorPrefs.GetFloat(NGSceneCameraWindow.Title + ".subSpeed." + i));
        }

        if (this.subSpeed == null || this.subSpeed.Count == 0)
          this.subSpeed = new List<float>() { .1F, .5F, 1F, 3F };

        this.currentSubSpeed = Mathf.Clamp(this.currentSubSpeed, 0, this.subSpeed.Count - 1);

        SceneView.onSceneGUIDelegate += this.OnSceneDelegate;
      }
    }

    protected virtual void  OnDestroy()
    {
      if (NGSceneCameraWindow.s_FlySpeed != null)
      {
        SceneView.onSceneGUIDelegate -= this.OnSceneDelegate;

        EditorPrefs.SetInt(NGSceneCameraWindow.Title + ".speed", this.speed.Count);
        for (int i = 0; i < this.speed.Count; i++)
          EditorPrefs.SetFloat(NGSceneCameraWindow.Title + ".speed." + i, this.speed[i]);

        EditorPrefs.SetInt(NGSceneCameraWindow.Title + ".subSpeed", this.subSpeed.Count);
        for (int i = 0; i < this.subSpeed.Count; i++)
          EditorPrefs.SetFloat(NGSceneCameraWindow.Title + ".subSpeed." + i, this.subSpeed[i]);
      }
    }

    protected virtual void  OnGUI()
    {
      this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);
      {
        this.DrawSpeedList("Speed", this.speed);
        this.DrawSpeedList("Sub Speed", this.subSpeed);
      }
      EditorGUILayout.EndScrollView();
    }

    private void  DrawSpeedList(string label, List<float> list)
    {
      for (int i = 0; i < list.Count; i++)
      {
        EditorGUILayout.BeginHorizontal();
        {
          list[i] = EditorGUILayout.FloatField(label + " " + (i + 1), list[i]);

          if (list.Count > 1 && GUILayout.Button("X", GUILayout.Width(30F)) == true)
          {
            list.RemoveAt(i);
            return;
          }
        }
        EditorGUILayout.EndHorizontal();
      }

      EditorGUILayout.BeginHorizontal();
      {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add " + label, GUILayout.Width(100F)) == true)
          list.Add(0F);
      }
      EditorGUILayout.EndHorizontal();
    }

    private void  OnSceneDelegate(SceneView sceneView)
    {
      if (this.rightToLeftLabel == null)
      {
        this.rightToLeftLabel = new GUIStyle(GUI.skin.label);
        this.rightToLeftLabel.alignment = TextAnchor.MiddleRight;
      }

      Handles.BeginGUI();
      {
        Rect  r = sceneView.position;

        r.x = r.width - 150F + 50F; // Hide text field.
        r.y = 120F;
        r.width = 150F;
        r.height = 20F;

        if (Event.current.type == EventType.Repaint && Event.current.control == false && Tools.viewTool == ViewTool.FPS)
        {
          Rect  r2 = r;
          r2.xMax -= 55F;
          r2.yMin += 4F;
          r2.yMax -= 6F;
          EditorGUI.DrawRect(r2, Color.cyan);
        }

        this.currentSpeed = (int)EditorGUI.Slider(r, this.currentSpeed, 0, this.speed.Count - 1);
        r.x -= r.width;
        GUI.Label(r, this.speed[this.currentSpeed].ToString(), this.rightToLeftLabel);
        r.x += r.width;

        r.y += r.height;

        if (Event.current.type == EventType.Repaint && Event.current.control == true && Tools.viewTool == ViewTool.FPS)
        {
          Rect  r2 = r;
          r2.xMax -= 55F;
          r2.yMin += 4F;
          r2.yMax -= 6F;
          EditorGUI.DrawRect(r2, Color.cyan);
        }

        this.currentSubSpeed = EditorGUI.IntSlider(r, this.currentSubSpeed, 0, this.subSpeed.Count - 1);

        r.x -= r.width;
        GUI.Label(r, this.subSpeed[this.currentSubSpeed].ToString(), this.rightToLeftLabel);
      }
      Handles.EndGUI();

      if (Tools.viewTool == ViewTool.FPS)
      {
        if (Event.current.type == EventType.KeyDown)
        {
          if (Event.current.keyCode == KeyCode.LeftControl ||
            Event.current.keyCode == KeyCode.RightControl)
          {
            Event.current.Use();
          }
        }
        else if (Event.current.type == EventType.ScrollWheel)
        {
          if (Event.current.control == false)
            this.currentSpeed = Mathf.Clamp(this.currentSpeed + (Event.current.delta.y < 0F ? 1 : -1), 0, this.speed.Count - 1);
          else
            this.currentSubSpeed = Mathf.Clamp(this.currentSubSpeed + (Event.current.delta.y < 0F ? 1 : -1), 0, this.subSpeed.Count - 1);
          
          Event.current.Use();
        }

        if (Event.current.control == false)
          NGSceneCameraWindow.s_FlySpeed.SetValue(null, this.speed[this.currentSpeed]);
        else
          NGSceneCameraWindow.s_FlySpeed.SetValue(null, this.subSpeed[this.currentSubSpeed]);
      }
    }
  }
}