using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   // Enables the loading & reloading of scenes"
using UnityEngine.UI;
using TMPro;

public class Main : MonoBehaviour
{
    static private Main S;                        // A private singleton for Main
 
     [Header("Inscribed")]
     public GameObject[]  prefabEnemies;               // Array of Enemy prefabs
     public float         enemySpawnPerSecond = 0.5f;  // # Enemies spawned/second
     public float         enemyInsetDefault = 1.5f;    // Inset from the sides
     public float         gameRestartDelay = 2; 
     
     [Header("Score UI")]
     public GameObject    scoreCanvas;                  // Canvas for score UI
     public TextMeshProUGUI scoreText;                 // Text element for score display
     public GameObject     gameOverPanel;              // Panel for game over screen
     public TextMeshProUGUI finalScoreText;            // Text to display final score
 
     private BoundsCheck  bndCheck;
    
    
    void Awake() {
         S = this;
         // Set bndCheck to reference the BoundsCheck component on this 
         // GameObject
         bndCheck = GetComponent<BoundsCheck>();
 
         // Initialize score UI if not already set up
         if (scoreCanvas == null) {
             InitializeScoreUI();
         }
         
         // Invoke SpawnEnemy() once (in 2 seconds, based on default values)
         Invoke( nameof(SpawnEnemy), 1f/enemySpawnPerSecond );                // a
     }
     
     void InitializeScoreUI() {
         // Create Canvas for UI
         GameObject canvasGO = new GameObject("ScoreCanvas");
         Canvas canvas = canvasGO.AddComponent<Canvas>();
         canvas.renderMode = RenderMode.ScreenSpaceOverlay;
         canvasGO.AddComponent<CanvasScaler>();
         canvasGO.AddComponent<GraphicRaycaster>();
         
         // Create Text element for score
         GameObject textGO = new GameObject("ScoreText");
         textGO.transform.SetParent(canvasGO.transform, false);
         scoreText = textGO.AddComponent<TextMeshProUGUI>();
         scoreText.fontSize = 24;
         scoreText.text = "Score: 0";
         
         // Position the text in the top-left corner
         RectTransform textRT = textGO.GetComponent<RectTransform>();
         textRT.anchorMin = new Vector2(0, 1);
         textRT.anchorMax = new Vector2(0, 1);
         textRT.pivot = new Vector2(0, 1);
         textRT.anchoredPosition = new Vector2(10, -10);
         textRT.sizeDelta = new Vector2(200, 50);
         
         // Create Game Over panel
         GameObject gameOverGO = new GameObject("GameOverPanel");
         gameOverGO.transform.SetParent(canvasGO.transform, false);
         Image gameOverImage = gameOverGO.AddComponent<Image>();
         gameOverImage.color = new Color(0, 0, 0, 0.8f);
         gameOverPanel = gameOverGO;
         
         // Size the panel to cover the entire screen
         RectTransform gameOverRT = gameOverGO.GetComponent<RectTransform>();
         gameOverRT.anchorMin = Vector2.zero;
         gameOverRT.anchorMax = Vector2.one;
         gameOverRT.offsetMin = Vector2.zero;
         gameOverRT.offsetMax = Vector2.zero;
         
         // Create Game Over text
         GameObject gameOverTextGO = new GameObject("GameOverText");
         gameOverTextGO.transform.SetParent(gameOverGO.transform, false);
         TextMeshProUGUI gameOverText = gameOverTextGO.AddComponent<TextMeshProUGUI>();
         gameOverText.text = "GAME OVER";
         gameOverText.fontSize = 64;
         gameOverText.color = Color.red;
         gameOverText.alignment = TextAlignmentOptions.Center;
         
         // Position the Game Over text
         RectTransform gameOverTextRT = gameOverTextGO.GetComponent<RectTransform>();
         gameOverTextRT.anchorMin = new Vector2(0.5f, 0.6f);
         gameOverTextRT.anchorMax = new Vector2(0.5f, 0.6f);
         gameOverTextRT.pivot = new Vector2(0.5f, 0.5f);
         gameOverTextRT.sizeDelta = new Vector2(400, 100);
         
         // Create Final Score text
         GameObject finalScoreGO = new GameObject("FinalScoreText");
         finalScoreGO.transform.SetParent(gameOverGO.transform, false);
         finalScoreText = finalScoreGO.AddComponent<TextMeshProUGUI>();
         finalScoreText.text = "Final Score: 0";
         finalScoreText.fontSize = 36;
         finalScoreText.color = Color.white;
         finalScoreText.alignment = TextAlignmentOptions.Center;
         
         // Position the Final Score text
         RectTransform finalScoreRT = finalScoreGO.GetComponent<RectTransform>();
         finalScoreRT.anchorMin = new Vector2(0.5f, 0.4f);
         finalScoreRT.anchorMax = new Vector2(0.5f, 0.4f);
         finalScoreRT.pivot = new Vector2(0.5f, 0.5f);
         finalScoreRT.sizeDelta = new Vector2(400, 60);
         
         // Add ScoreManager component to this GameObject
         ScoreManager scoreManager = gameObject.AddComponent<ScoreManager>();
         scoreManager.scoreText = scoreText;
         
         // Save reference to canvas
         scoreCanvas = canvasGO;
         
         // Hide game over panel initially
         gameOverPanel.gameObject.SetActive(false);
     }
    
   public void SpawnEnemy() {
         // Pick a random Enemy prefab to instantiate
         int ndx = Random.Range(0, prefabEnemies.Length);                     // b
         GameObject go = Instantiate<GameObject>( prefabEnemies[ ndx ] );     // c
 
         // Position the Enemy above the screen with a random x position
         float enemyInset = enemyInsetDefault;                                // d
         if (go.GetComponent<BoundsCheck>() != null) {                        // e
             enemyInset = Mathf.Abs( go.GetComponent<BoundsCheck>().radius );
         }
 
         // Set the initial position for the spawned Enemy                    // f
         Vector3 pos = Vector3.zero;                      
         float xMin = -bndCheck.camWidth + enemyInset;
         float xMax =  bndCheck.camWidth - enemyInset;
         pos.x = Random.Range( xMin, xMax );
         pos.y = bndCheck.camHeight + enemyInset;
         go.transform.position = pos;
 
         // Invoke SpawnEnemy() again
         Invoke( nameof(SpawnEnemy), 1f/enemySpawnPerSecond );                // g
     }

          void DelayedRestart() {                                                   // c
         // Show game over panel with final score
         gameOverPanel.gameObject.SetActive(true);
         
         // Update final score text
         if (ScoreManager.S != null && finalScoreText != null) {
             finalScoreText.text = "Final Score: " + ScoreManager.S.score.ToString();
         }
         
         // Invoke the Restart() method in gameRestartDelay seconds
         Invoke( nameof(Restart), gameRestartDelay );                    
     }
 
     void Restart() {
         // Reload __Scene_0 to restart the game
         // "__Scene_0" below starts with 2 underscores and ends with a zero.
         SceneManager.LoadScene( "__Scene_0" );                               // d
     }
 
     static public void HERO_DIED() {
         S.DelayedRestart();                                                  // b
     }
 }
