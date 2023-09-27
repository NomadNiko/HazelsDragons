using CodeMonkey;
using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Level : MonoBehaviour {
    private const float CAMERA_ORTH_SIZE = 50f;
    private const float PIPE_WIDTH = 4.2f;
    private const float CLIFF_HEIGHT = 5.2f;
    private const float CLIFF_MOVE_SPEED = 40f;
    private const float CLIFF_DESTROY_X_POSITION = -120f;
    private const float CLIFF_SPAWN_X_POSITION = +100f;
    private const float DRAGON_X_POSITION = -30;

 

    private const int cliffsPerRound = 4;

    private List<Cliff> cliffList;

    private static Level instance;

    public static Level GetInstance() {
        return instance;
    }

    private int cliffsPassedCount;
    private int cliffsSpawnedCount;
    private float cliffSpawnTimer;
    private float cliffSpawnTimerMax;
    private float gapSize;
    private State state;


    public enum Difficulty {
        Level1, 
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
    }

    private enum State {
        WaitingToStart,
        Playing,
        DragonDead,
    }

    private void Awake() {
        instance = this;
        cliffList = new List<Cliff>();
        cliffSpawnTimerMax = 3f;
        SetDifficulty(Difficulty.Level1);
        state = State.WaitingToStart;
        SoundManager.PlaySound(SoundManager.Sound.GameMusic, .13f);
    }

    private void Start() {
        Dragon.GetInstance().OnStartedPlaying += Dragon_OnStartedPlaying;
        Dragon.GetInstance().OnDied += Dragon_OnDied;
    }

    private void Dragon_OnStartedPlaying(object sender, EventArgs e) {
        state = State.Playing;
    }

    private void Dragon_OnDied(object sender, EventArgs e) {
        state = State.DragonDead;

    }

    private void Update() {
        if (state == State.Playing) {
            HandleCliffSpawning();
            HandleCliffMovement();
        }
    }

    private void HandleCliffSpawning() {
        cliffSpawnTimer -= Time.deltaTime;
        if (cliffSpawnTimer < 0 ) {
            // Spawn Cliff
            cliffSpawnTimer += cliffSpawnTimerMax;


            float heightEdgeLimit = 2f;
            float minHeight = gapSize * 0.5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTH_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

            float height = UnityEngine.Random.Range(minHeight, maxHeight);
            CreateGapCliffs(height, gapSize, CLIFF_SPAWN_X_POSITION);
            
        }
    }   


    private void HandleCliffMovement() {

        for (int i = 0; i < cliffList.Count; i++) {
            Cliff cliff = cliffList[i];
            bool hasNotPassedDragon = cliff.GetXPosition() > DRAGON_X_POSITION;

            cliff.Move();
            if (hasNotPassedDragon && cliff.GetXPosition() <= DRAGON_X_POSITION && cliff.IsBottom()) {
                // Cliff passed bird
                cliffsPassedCount++;
                SoundManager.PlaySound(SoundManager.Sound.Score, .3f);
            }
            if (cliff.GetXPosition() < CLIFF_DESTROY_X_POSITION) {
                // Destroy Cliff
                cliff.DestroySelf();
                cliffList.Remove(cliff);
                i--; 
            }

        }
    }

    private void SetDifficulty(Difficulty difficulty) {
        switch (difficulty) {
            case Difficulty.Level1:
                gapSize = 55f;
                cliffSpawnTimerMax = 2.2f;
                break;
            case Difficulty.Level2:
                gapSize = 50f;
                cliffSpawnTimerMax = 2f;
                break;
            case Difficulty.Level3:
                cliffSpawnTimerMax = 1.7f;
                gapSize = 45f;
                break;
            case Difficulty.Level4:
                cliffSpawnTimerMax = 1.4f;
                gapSize = 40f;
                break;
            case Difficulty.Level5:
                cliffSpawnTimerMax = 1.1f;
                gapSize = 35f;
                break;
            case Difficulty.Level6:
                cliffSpawnTimerMax = 0.8f;
                gapSize = 32f;
                break;
        }
    }

    private Difficulty GetDifficulty() {
        if (cliffsSpawnedCount >= 65) return Difficulty.Level6;
        if (cliffsSpawnedCount >= 45) return Difficulty.Level5;
        if (cliffsSpawnedCount >= 25) return Difficulty.Level4;
        if (cliffsSpawnedCount >= 15) return Difficulty.Level3;
        if (cliffsSpawnedCount >= 4) return Difficulty.Level2;
        return Difficulty.Level1;
    }


    private void CreateGapCliffs(float gapY, float gapSize, float xPosition) {
        CreateCliff(gapY - gapSize * .5f, xPosition, true);
        CreateCliff(CAMERA_ORTH_SIZE * 2f - gapY - gapSize * 0.5f, xPosition, false);
        cliffsSpawnedCount++;
        SetDifficulty(GetDifficulty());
    }

    private void CreateCliff(float height, float xPosition, bool createOnBottom) {

        // Set up CliffTop

        Transform cliffTop = Instantiate(GameAssets.GetInstance().cliffTop);
        float cliffTopYPosition;
        if (createOnBottom) {
            cliffTopYPosition = -CAMERA_ORTH_SIZE + height - CLIFF_HEIGHT * 0.5f;
        } else {
            cliffTop.localScale = new Vector3(1, -1, 1);
            cliffTopYPosition = +CAMERA_ORTH_SIZE - height + CLIFF_HEIGHT * 0.5f;
        }
        cliffTop.position = new Vector3(xPosition, cliffTopYPosition);


        // Set up CliffPipe

        Transform cliffPipe = Instantiate(GameAssets.GetInstance().cliffPipe);
        float cliffPipeYPosition;
        cliffPipe.position = new Vector3(xPosition, -CAMERA_ORTH_SIZE);
        if (createOnBottom) {
            cliffPipeYPosition = -CAMERA_ORTH_SIZE;
        } else {
            cliffPipeYPosition = +CAMERA_ORTH_SIZE;
            cliffPipe.localScale = new Vector3(1, -1, 1);
        }
        cliffPipe.position = new Vector3(xPosition, cliffPipeYPosition);

        SpriteRenderer cliffPipeSpriteRenderer = cliffPipe.GetComponent<SpriteRenderer>();
        cliffPipeSpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D cliffBoxCollider2D = cliffPipe.GetComponent<BoxCollider2D>();
        cliffBoxCollider2D.size = new Vector2(PIPE_WIDTH, height);
        cliffBoxCollider2D.offset = new Vector3(0f, height * 0.5f);

        Cliff cliff = new Cliff(cliffTop, cliffPipe, createOnBottom);
        cliffList.Add(cliff);
    }

    public int GetCliffsSpawned() {
        return cliffsSpawnedCount;
    }
    public int GetCliffsPassed() {
        return cliffsPassedCount;
    }

    private class Cliff {
        private Transform cliffTopTransform;
        private Transform cliffPipeTransform;
        private float cliffMoveSpeed;
        private bool isBottom;

        public Cliff(Transform cliffTopTransform, Transform cliffPipeTransform, bool isBottom) { 
            this.cliffTopTransform = cliffTopTransform;
            this.cliffPipeTransform = cliffPipeTransform;
            this.isBottom = isBottom;
        }

        public void Move() {
            cliffTopTransform.position += new Vector3(-1, 0, 0) * CLIFF_MOVE_SPEED * Time.deltaTime;
            cliffPipeTransform.position += new Vector3(-1, 0, 0) * CLIFF_MOVE_SPEED * Time.deltaTime;
        }

        public bool IsBottom() {
            return isBottom;
        }

        public float GetXPosition() {
            return cliffTopTransform.position.x;
        }

        public void DestroySelf() {
            Destroy(cliffTopTransform.gameObject);
            Destroy(cliffPipeTransform.gameObject);
        }
    
    }

    //Arch


    private class Arch {
        private Transform archTopTransform;
        private Transform archPipeTransform;
        private float archMoveSpeed;
        private bool isBottom;

        public Arch(Transform cliffTopTransform, Transform cliffPipeTransform, bool isBottom) {
            this.archTopTransform = cliffTopTransform;
            this.archPipeTransform = cliffPipeTransform;
            this.isBottom = isBottom;
        }

        public void Move() {
            archTopTransform.position += new Vector3(-1, 0, 0) * CLIFF_MOVE_SPEED * Time.deltaTime;
            archPipeTransform.position += new Vector3(-1, 0, 0) * CLIFF_MOVE_SPEED * Time.deltaTime;
        }

        public bool IsBottom() {
            return isBottom;
        }

        public float GetXPosition() {
            return archTopTransform.position.x;
        }

        public void DestroySelf() {
            Destroy(archTopTransform.gameObject);
            Destroy(archPipeTransform.gameObject);
        }

    }

}
