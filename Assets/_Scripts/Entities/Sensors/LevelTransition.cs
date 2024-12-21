using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string ToScene;
    public string ToGateID;
    public string FromScene;
    public string FromGateID;

    public bool isActive = true;


    private void OnTriggerEnter2D(Collider2D collision) {
        if(!isActive) {
            return;
        }
        GameManager.Instance.LevelTransition(FromGateID);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        isActive = true;
    }
}
