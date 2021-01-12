using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnClick : MonoBehaviour
{
    public void RetryOrResumeGame()
    {
        
        if (GameControl.Instance.gameState == GameControl.GameStatus.ENDED)
        {
            Debug.Log("Clicked Play Button");
            GameControl.Instance.RetryGame();
            
        }
        if (GameControl.Instance.gameState == GameControl.GameStatus.STARTED || GameControl.Instance.gameState == GameControl.GameStatus.PAUSED)
        {
            Debug.Log("Clicked Resume Button");
            GameControl.Instance.ResumeGame();
        }
    }

    public void ExitGame()
    {
        GameControl.Instance.ExitGame();
    }
}
