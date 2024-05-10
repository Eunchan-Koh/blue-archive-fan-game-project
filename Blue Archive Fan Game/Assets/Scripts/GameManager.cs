using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   public bool cursorVisible;
    public bool LockCursor;
    public Character[] allCharacters;
    public Character[] selectedCharacters;
    void Update(){
        Cursor.visible= !cursorVisible;
        Cursor.lockState = LockCursor?CursorLockMode.Locked:CursorLockMode.None;
    }

    
}
