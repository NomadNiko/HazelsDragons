using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene {
        GameScene,
        Loading,
        MainMenu,
    }

    public static Scene targetScene;
    public static void Load(Scene scene) {

        SceneManager.LoadScene(Scene.Loading.ToString());
        targetScene = scene;
        SceneManager.LoadScene(scene.ToString());
    }

    public static void LoadTargetScene() {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
