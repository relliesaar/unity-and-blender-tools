using UnityEditor;

public class SceneViewCameraSetup
{
    public static void SetCameraSpeed(float speedValue)
    {
        SceneView.CameraSettings settings = new SceneView.CameraSettings();

        settings.speed = speedValue;
        settings.accelerationEnabled = false;
        settings.easingEnabled = false;

        SceneView sceneView = SceneView.lastActiveSceneView;
        sceneView.cameraSettings = settings;
    }
}
