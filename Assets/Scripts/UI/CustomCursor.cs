using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    // Script that displays a custom cursor texture 

    public static CustomCursor customCursor; 
    public Texture2D cursor; // The texture of the cursor

    public bool cursorEnabled; // Whether cursor is enabled or not


    private void Awake()
    {
        customCursor = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SetCursor", 0f); // Set the cursor as soon as scene loads
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDisable()
    {
        // Resets the cursor to the default  
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        // Set the cursorEnabled to false  
        this.cursorEnabled = false;
    }


    public void SetCursor()
    {
        Cursor.SetCursor(this.cursor, Vector2.zero, CursorMode.Auto);
        Debug.Log("Custom cursor has been set.");
        //Set the cursorEnabled to true  
        this.cursorEnabled = true;
    }
}
