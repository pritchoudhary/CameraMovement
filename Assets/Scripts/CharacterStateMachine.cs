using UnityEngine;
using System.Collections;

public class CharacterStateMachine : MonoBehaviour
{

    enum PlayerState
    {
        psWalking,
        psCover,

        psNumStates
    }

    PlayerState currentState;

	// Use this for initialization
	void Start ()
    {
        currentState = PlayerState.psWalking;
	}
	
    void OnCollisionEnter (Collision other)
    {
        Debug.Log("Collision");

        if (other.gameObject.tag == "Cover")
        {
            Debug.Log("Player is in cover");
            currentState = PlayerState.psCover;
        }
    }
}
