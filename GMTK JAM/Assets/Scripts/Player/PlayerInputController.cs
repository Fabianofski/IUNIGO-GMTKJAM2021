using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputController : MonoBehaviour
{
    [Header("Input Device")]
    [SerializeField] BoolReference IsUsingGamepad;

    [Header("Game Mechanics")]
    [SerializeField] BoolEventReference FireEvent;
    [SerializeField] BoolEventReference SplitEvent;
    [SerializeField] BoolReference LevelIsArranged;
    [SerializeField] BoolReference GameEnded;
    [SerializeField] BoolReference GamePaused;
    bool canSplit = true;
    public void OnSplit()
    {
        if (!canSplit || GameEnded.Value || GamePaused.Value) return;

        if(SplitEvent.Event)
            SplitEvent.Event.Raise(!LevelIsArranged.Value);

        canSplit = false;
        Invoke("ResetSplit", 0.5f);
    }

    void ResetSplit()
    {
        canSplit = true;
    }

    public void OnPause()
    {
        if(!GameEnded.Value)
            GamePaused.Value = !GamePaused.Value;
    }

    public void OnFire(InputValue _value)
    {
        if (GameEnded.Value || GamePaused.Value) return;

        if (FireEvent.Event)
            FireEvent.Event.Raise(_value.isPressed);
    }

    public void OnRestart(InputValue _value)
    {
        if (_value.isPressed)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnControlsChanged(PlayerInput _input)
    {
        IsUsingGamepad.Value = _input.currentControlScheme == "Gamepad";
    }
}
