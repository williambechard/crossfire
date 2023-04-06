using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public string lastLoadedScene = "";
    [Header("Loading Assets")]
    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private GameObject _TransitionCanvas;
    [SerializeField] private Image _progressBar;
    private float _target;

    [SerializeField] Animator anim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //show Transition canvas
            _TransitionCanvas.SetActive(true);

            //Init fade in of the scene (this is needed if it is the first scene
            // and therefore not a loaded one
            anim.Play("FadeIn", 0, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnloadScene(string name)
    {
        lastLoadedScene = name;
        SceneManager.UnloadSceneAsync(name);
    }

    //Load Scene either as a replacement for current scene, or as an additive scene (so current scene isnt unloaded)
    public void LoadScene(string sceneName)
    {
        lastLoadedScene = sceneName;
        lScene(SceneManager.LoadSceneAsync(sceneName));
    }
    public void LoadSceneAdditive(string sceneName)
    {
        lastLoadedScene = sceneName;
        lScene(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
    }

    public void LoadSceneAdditiveInstantly(string sceneName)
    {
        lastLoadedScene = sceneName;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    AsyncOperation nextScene;

    //load scene helper
    public void lScene(AsyncOperation scene)
    {

        Debug.Log("Loading scene : " + scene);
        //reset fill bar & target
        _target = 0;
        _progressBar.fillAmount = 0;

        nextScene = scene;
        nextScene.allowSceneActivation = false; //dont show the new scene (until its loaded)

        //show Transition canvas
        _TransitionCanvas.SetActive(true);

        //anim.SetBool("Intro", true);
        anim.Play("FadeOut", 0, 0);
    }


    public void MiddleTransition()
    {
        _loadingCanvas.SetActive(true); //show loading object
                                        //now show the new scene
        do
        {
            //artificial wait time
            _target = nextScene.progress;
        } while (nextScene.progress < 0.9f); //scene is loaded at 90% :)
        nextScene.allowSceneActivation = true;
        _loadingCanvas.SetActive(false); //hide the loading object
    }

    public void EndTransition() => _TransitionCanvas.SetActive(false);


    public void Update() => _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);

}
