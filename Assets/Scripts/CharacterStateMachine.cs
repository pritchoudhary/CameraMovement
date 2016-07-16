using UnityEngine;
using System.Collections;

public class CharacterStateMachine : MonoBehaviour
{

    public enum PlayerState
    {
        psWalking,
        psCover,

        psNumStates
    }

    public PlayerState currentState;

	// Use this for initialization
	void Start ()
    {
        currentState = PlayerState.psWalking;
	}

    void Update()
    {
    }
	
    void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.tag == "Cover")
        {
            Debug.Log("Player is in cover");
            currentState = PlayerState.psCover;
        }
    }
}
