using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager manager;
    public PlayerController currentPlayer; // Keeps track of current active player when game starts
    public PlayerChanger activePlayerChanger; // The character that is changed to
    // Start is called before the first frame update

    private void Awake()
    {
        manager = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
