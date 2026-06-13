using UnityEngine;
using UnityEngine.InputSystem;

public class Quitgame : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.isPressed)
        {
            Application.Quit();
            Debug.Log("Game is exiting");
        }
        
    }
}
