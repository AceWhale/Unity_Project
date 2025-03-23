using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private GameObject content;
    private static MenuScript previousInstance = null;

    void Start()
    {
        if(previousInstance == null)
        {
            previousInstance = this;
        }
        else
        {
            // Наявні два об'єкти типу MenuScript - швидше за все це копія меню на ігровій сцені
            // один з них потрібно прибрати. 
            Destroy(this.gameObject);

            // Destroy(previousInstance.gameObject);
            // previousInstance = this;
        }
        content = transform.Find("Content").gameObject;
        //DontDestroyOnLoad

        Time.timeScale = this.gameObject.activeInHierarchy ? 0.0f : 1.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            content.SetActive(!content.activeInHierarchy);
			Time.timeScale = 1.0f - Time.timeScale;
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(2);
        }
    }
}
/* UI. Перехід між сценами. Меню налаштувань.
 * Завдання: реалізувати доступ до одного і того ж меню з різних сцен проєкту.
 *  + налаштування, зроблені попередньо, мають залишатись актуальними.
 *  
 * Інструментарій: метод DontDestroyOnLoad, який дозволяє зберегти об'єкт при 
 * завантаженні нової сцени. DontDestroyOnLoad подається один раз (для одного об'єкта),
 * подальші зміни сцен не вимагають повтора інструкції.
 * 
 * Проблеми: контроль того, що об'єкти не створюються повторно 
 * 
 * Схема: створюється окрема сцена, на якій розміщується лише меню.
 * При завантаженні сцени меню помічається як DontDestroyOnLoad і одразу
 * завантажується інша (ігрова) сцена. Для контролю загальної роботи створюємо
 * третю сцену і моніторимо життєвий цикл меню при переході між сценами.
 */
