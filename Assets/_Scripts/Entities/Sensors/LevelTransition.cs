using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string nextScene;
    public int id;
    public bool isActive = true;


    private void OnTriggerEnter2D(Collider2D collision) {
        if(!isActive) {
            return;
        }
        GameManager.Instance.LevelTransition(id);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        isActive = true;
    }
}
