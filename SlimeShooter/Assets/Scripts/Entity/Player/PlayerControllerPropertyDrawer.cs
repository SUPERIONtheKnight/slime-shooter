using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerPropertyDrawer : Editor {
	void OnSceneGUI() {
		PlayerController physics = target as PlayerController;
		Handles.DrawLine(physics.floorDetectPosition1, physics.floorDetectPosition3);
		Handles.DrawLine(physics.floorDetectPosition1, physics.floorDetectPosition4);
		Handles.DrawLine(physics.floorDetectPosition2, physics.floorDetectPosition3);
		Handles.DrawLine(physics.floorDetectPosition2, physics.floorDetectPosition4);
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		if (GUILayout.Button("Test")) {
			Debug.Log("Test");
		}
	}
}