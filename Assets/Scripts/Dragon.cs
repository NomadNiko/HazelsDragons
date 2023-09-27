using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using UnityEditor;

public class Dragon : MonoBehaviour
{
    private Rigidbody2D dragonRigidbody2D;
    private const float JUMP_FORCE = 90;

    public static Dragon instance;

    public static Dragon GetInstance() { return instance; }

    public event EventHandler OnStartedPlaying;
    public event EventHandler OnDied;

    private void Awake() {
        instance = this;
        dragonRigidbody2D = GetComponent<Rigidbody2D>();
        dragonRigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    private State state;

    private enum State {
        WaitingToStart,
        Playing,
        Dead,
    }


    private void Update() {
        switch (state) {
            default:
                case State.WaitingToStart:

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                    state = State.Playing;
                    dragonRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
                   Jump();
                }
                break;
                case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                    Jump();
                }
                break;
                case State.Dead:
                if (OnDied != null) OnDied(this, EventArgs.Empty);
                dragonRigidbody2D.bodyType = RigidbodyType2D.Static;
                break;


        }

    }

    private void Jump() {
        dragonRigidbody2D.velocity = Vector3.up * JUMP_FORCE;
        SoundManager.PlaySound(SoundManager.Sound.DragonJump, .3f);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        state = State.Dead;
        SoundManager.PlaySound(SoundManager.Sound.Lose, 1f);
    }
}
