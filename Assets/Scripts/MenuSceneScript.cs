using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneScript : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    void Start()
    {
        DontDestroyOnLoad(menu);
        SceneManager.LoadScene(1);
    }
}
/* Контролер самої сцени "Меню"
 * його задача помітити MenuCanvas як такий, що не руйнується
 * та перейти до ігрової сцени.
 */