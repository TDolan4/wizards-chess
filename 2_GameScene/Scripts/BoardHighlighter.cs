using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlighter : MonoBehaviour
{
    public static BoardHighlighter Instance{set; get;}
    
    public GameObject highlightPrefab;
    private List<GameObject> highlights;

    private float TILE_SIZE;
    private float TILE_OFFSET;

    private float miniHalfWidth;

    // Start is called before the first frame update
    private void Start() {
        Instance = this;
        highlights = new List<GameObject>();

        TILE_SIZE = BoardManager.MINI_TILE_SIZE;//BoardManager.TILE_SIZE;
        TILE_OFFSET = TILE_SIZE / 2;//BoardManager.TILE_OFFSET;
        miniHalfWidth = BoardManager.miniHalfWidth;
    }

    private GameObject GetHighlightObject(){
        GameObject go = highlights.Find(g => !g.activeSelf);

        if (go == null){
            go = Instantiate (highlightPrefab);
            go.transform.localScale = new Vector3(0.1f * TILE_SIZE, 1.0f, 0.1f * TILE_SIZE);
            highlights.Add (go);
        }
        return go;
    }

    public void HighlightAllowedMoves(bool[,] moves){
        Transform miniWorldPos = GameObject.Find("WorldInMini").transform;
        float mini_x;
        float mini_y;
        float mini_height = miniWorldPos.position.y + .035f;

        for (int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){
                if(moves[i,j]){
                    GameObject go = GetHighlightObject();
                    go.SetActive(true);
                    mini_x = miniWorldPos.position.x - miniHalfWidth;
                    mini_y = miniWorldPos.position.z - miniHalfWidth;
                    Vector3 origin = new Vector3(mini_x, mini_height, mini_y);
                    origin.x += (TILE_SIZE * i) + TILE_OFFSET;
                    origin.z += (TILE_SIZE * j) + TILE_OFFSET;
                    go.transform.position = origin;
                }
            }
        }
    }

    public void Hidehighlights(){
        foreach(GameObject go in highlights){
            go.SetActive(false);
        }
    }
}
