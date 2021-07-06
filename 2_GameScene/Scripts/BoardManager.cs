using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
  public static BoardManager Instance { set; get; }
  private bool[,] allowedMoves { set; get; }

  public Chesspiece[,] Chesspieces { set; get; }
  private Chesspiece selectedChesspiece;
  public Chesspiece[,] MiniChesspieces { set; get; }
  private Chesspiece miniSelectedChesspiece;

  public const float TILE_SIZE = 2.0f;
  public const float TILE_OFFSET = TILE_SIZE / 2;
  public const float MINI_TILE_SIZE = .075f;

  private int selectionX = -1;
  private int selectionY = -1;

  public List<GameObject> chessPiecePrefabs;
  public List<GameObject> miniChessPiecePrefabs;
  private List<GameObject> activeChessPieces;

  private Quaternion halfRotation = Quaternion.Euler(0, 180, 0);

  public bool isWhiteTurn = true;

  public bool drawChessboard = false;

  public const float miniHalfWidth = 0.3f;

  public enum Pieces { King, Queen, Bishop, Knight, Rook, Pawn };
  public Pieces playerPiece;


  // // Start is called before the first frame update
  void Start()
  {
    transform.localScale = new Vector3(TILE_SIZE, 1.0f, TILE_SIZE);
    Instance = this;
    SpawnAllChessPieces();

    float random = Random.Range(0.0f, 1.0f);
    Chesspiece c;
    int posX = -1;
    int posY = 0;

    GameObject player = GameObject.Find("Player");
    playerPiece = (Pieces)Psys_On.piece_num;
    switch (playerPiece)
    {
      case (Pieces.King):
        posX = 4;
        break;
      case (Pieces.Queen):
        posX = 3;
        break;
      case (Pieces.Bishop):
        player.AddComponent<Bishop>();
        if (random < 0.5f)
          posX = 2;
        else
          posX = 5;
        break;
      case (Pieces.Knight):
        player.AddComponent<Knight>();
        if (random < 0.5f)
          posX = 1;
        else
          posX = 6;
        break;
      case (Pieces.Rook):
        player.AddComponent<Rook>();
        if (random < 0.5f)
          posX = 0;
        else
          posX = 7;
        break;
      case (Pieces.Pawn):
        player.AddComponent<Pawn>();
        posX = (int)(random * 8);
        if (posX == 8)
          posX = 7;
        posY = 1;
        break;
    }
    c = Chesspieces[posX, posY];
    c.SetInvisible();
    player.transform.position = GetTileCenter(posX, posY);
    player.transform.SetParent(c.gameObject.transform);
  }

  // Update is called once per frame
  void Update()
  {
    UpdateSelection();
    if (drawChessboard)
    {
      DrawChessboard();
    }

    if (Input.GetMouseButtonDown(0))
    {
      if (selectionX >= 0 && selectionY >= 0)
      {
        if (selectedChesspiece == null)
        {
          //Select chesspiece
          SelectChesspiece(selectionX, selectionY);
        }
        else
        {
          //Move chesspiece
          MoveChesspiece(selectionX, selectionY);
        }
      }
    }

    AI();
  }

  private void SelectChesspiece(int x, int y)
  {
    if (Chesspieces[x, y] == null)
    {
      return;
    }

    if (Chesspieces[x, y].isWhite != isWhiteTurn)
    {
      return;
    }

    bool hasAtLeastOneMove = false;
    allowedMoves = Chesspieces[x, y].PossibleMove();
    for (int i = 0; i < 8; i++)
    {
      for (int j = 0; j < 8; j++)
      {
        if (allowedMoves[i, j])
        {
          hasAtLeastOneMove = true;
        }
      }
    }

    if (!hasAtLeastOneMove)
    {
      return;
    }

    selectedChesspiece = Chesspieces[x, y];
    miniSelectedChesspiece = MiniChesspieces[x, y];
    BoardHighlighter.Instance.HighlightAllowedMoves(allowedMoves);
  }

  private void MoveChesspiece(int x, int y)
  {
    if (allowedMoves[x, y])
    {
      Chesspiece c = Chesspieces[x, y];
      Chesspiece m = MiniChesspieces[x, y];
      if (c != null && c.isWhite != isWhiteTurn)
      {
        //Capture piece

        //Win if King
        if (c.GetType() == typeof(King))
        {
          EndGame();
          return;
        }

        activeChessPieces.Remove(c.gameObject);
        Destroy(c.gameObject);
        Destroy(m.gameObject);
      }
      if (selectedChesspiece.GetType() == typeof(Pawn))
      {
        if (y == 7)
        {
          activeChessPieces.Remove(selectedChesspiece.gameObject);
          Destroy(selectedChesspiece.gameObject);
          SpawnChessPiece(1, x, y, Quaternion.identity);
          selectedChesspiece = Chesspieces[x, y];
          miniSelectedChesspiece = MiniChesspieces[x, y];
        }
        else if (y == 0)
        {
          activeChessPieces.Remove(selectedChesspiece.gameObject);
          Destroy(selectedChesspiece.gameObject);
          SpawnChessPiece(7, x, y, halfRotation);
          selectedChesspiece = Chesspieces[x, y];
          miniSelectedChesspiece = MiniChesspieces[x, y];
        }

      }

      Chesspieces[selectedChesspiece.CurrentX, selectedChesspiece.CurrentY] = null;
      MiniChesspieces[selectedChesspiece.CurrentX, selectedChesspiece.CurrentY] = null;

      //selectedChesspiece.transform.position = GetTileCenter(x,y);
      miniSelectedChesspiece.transform.position = GetMiniTileCenter(x, y);
      selectedChesspiece.SetDestination(GetTileCenter(x, y));
      //miniSelectedChesspiece.SetDestination(GetMiniTileCenter(x, y));

      selectedChesspiece.SetPosition(x, y);
      Chesspieces[x, y] = selectedChesspiece;
      MiniChesspieces[x, y] = miniSelectedChesspiece;
      isWhiteTurn = !isWhiteTurn;
    }
    BoardHighlighter.Instance.Hidehighlights();
    selectedChesspiece = null;
    miniSelectedChesspiece = null;
  }

  private void UpdateSelection()
  {
    //Camera GameCamera = GameObject.Find("GameCamera").GetComponent<Camera>();
    if (!Camera.main)
    {
      Debug.Log("zoooiiiinks scoobs");
      return;
    }

    RaycastHit hit;

    if (isWhiteTurn && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("ChessPlane")))
    {
      //if(isWhiteTurn && Physics.Raycast(GameCamera.ScreenPointToRay(Input.mousePosition),out hit, 50.0f, LayerMask.GetMask("ChessPlane"))){
      Transform miniWorldPos = GameObject.Find("WorldInMini").transform;
      selectionX = (int)((hit.point.x - (miniWorldPos.position.x - miniHalfWidth)) / MINI_TILE_SIZE);
      selectionY = (int)((hit.point.z - (miniWorldPos.position.z - miniHalfWidth)) / MINI_TILE_SIZE);
      //Debug.Log(hit.point.x);           
    }
    else
    {
      selectionX = -1;
      selectionY = -1;
    }
  }

  private void SpawnChessPiece(int index, int x, int y, Quaternion rotation)
  {
    GameObject go = Instantiate(chessPiecePrefabs[index], GetTileCenter(x, y), rotation) as GameObject;
    go.transform.SetParent(transform);
    Chesspieces[x, y] = go.GetComponent<Chesspiece>();

    GameObject minigo = Instantiate(miniChessPiecePrefabs[index], GetMiniTileCenter(x, y), rotation) as GameObject;
    GameObject player = GameObject.Find("Player");

    minigo.transform.localScale = new Vector3(0.03f, 0.04f, 0.03f);
    MiniChesspieces[x, y] = minigo.GetComponent<Chesspiece>();
    minigo.transform.SetParent(player.transform);
    Chesspieces[x, y].SetPosition(x, y);
    activeChessPieces.Add(go);
  }

  private void SpawnAllChessPieces()
  {
    activeChessPieces = new List<GameObject>();
    Chesspieces = new Chesspiece[8, 8];
    MiniChesspieces = new Chesspiece[8, 8];

    //White

    //King
    SpawnChessPiece(0, 4, 0, Quaternion.identity);

    //Queen
    SpawnChessPiece(1, 3, 0, Quaternion.identity);

    //Bishops
    SpawnChessPiece(2, 2, 0, Quaternion.identity);
    SpawnChessPiece(2, 5, 0, Quaternion.identity);

    //Knights
    SpawnChessPiece(3, 1, 0, Quaternion.identity);
    SpawnChessPiece(3, 6, 0, Quaternion.identity);

    //Rooks
    SpawnChessPiece(4, 0, 0, Quaternion.identity);
    SpawnChessPiece(4, 7, 0, Quaternion.identity);

    //Pawns
    for (int i = 0; i < 8; i++)
    {
      SpawnChessPiece(5, i, 1, Quaternion.identity);
    }

    //Black

    //King
    SpawnChessPiece(6, 4, 7, halfRotation);

    //Queen
    SpawnChessPiece(7, 3, 7, halfRotation);

    //Bishops
    SpawnChessPiece(8, 2, 7, halfRotation);
    SpawnChessPiece(8, 5, 7, halfRotation);

    //Knights
    SpawnChessPiece(9, 1, 7, halfRotation);
    SpawnChessPiece(9, 6, 7, halfRotation);

    //Rooks
    SpawnChessPiece(10, 0, 7, halfRotation);
    SpawnChessPiece(10, 7, 7, halfRotation);

    //Pawns
    for (int i = 0; i < 8; i++)
    {
      SpawnChessPiece(11, i, 6, halfRotation);
    }

  }

  private Vector3 GetTileCenter(int x, int y)
  {
    Vector3 origin = Vector3.zero;
    origin.x += (TILE_SIZE * x) + TILE_OFFSET;
    origin.z += (TILE_SIZE * y) + TILE_OFFSET;
    return origin;
  }

  private Vector3 GetMiniTileCenter(int x, int y)
  {
    Transform miniWorldPos = GameObject.Find("WorldInMini").transform;
    float mini_x = miniWorldPos.position.x - miniHalfWidth;
    float mini_y = miniWorldPos.position.z - miniHalfWidth;
    float mini_height = miniWorldPos.position.y + .035f;
    Vector3 origin = new Vector3(mini_x, mini_height, mini_y);
    origin.x += (MINI_TILE_SIZE * x) + MINI_TILE_SIZE / 2;
    origin.z += (MINI_TILE_SIZE * y) + MINI_TILE_SIZE / 2;
    return origin;
  }

  private void DrawChessboard()
  {
    Vector3 widthLine = Vector3.right * 8 * TILE_SIZE;
    Vector3 heightLine = Vector3.forward * 8 * TILE_SIZE;

    for (int i = 0; i < 9; i++)
    {
      Vector3 start = Vector3.forward * i * TILE_SIZE;
      Debug.DrawLine(start, start + widthLine);
      for (int j = 0; j < 9; j++)
      {
        start = Vector3.right * j * TILE_SIZE;
        Debug.DrawLine(start, start + heightLine);
      }
    }

    // Draw the Selection
    if (selectionX > -1 && selectionY > -1)
    {
      Debug.DrawLine(
          Vector3.forward * TILE_SIZE * selectionY + Vector3.right * TILE_SIZE * selectionX,
          Vector3.forward * TILE_SIZE * (selectionY + 1) + Vector3.right * TILE_SIZE * (selectionX + 1)
      );
      Debug.DrawLine(
          Vector3.forward * TILE_SIZE * (selectionY + 1) + Vector3.right * TILE_SIZE * selectionX,
          Vector3.forward * TILE_SIZE * selectionY + Vector3.right * TILE_SIZE * (selectionX + 1)
      );
    }
  }

  private void EndGame()
  {
    if (isWhiteTurn)
    {
      Debug.Log("White team wins");
    }
    else
    {
      Debug.Log("Black team wins");
    }

    foreach (GameObject go in activeChessPieces)
    {
      Destroy(go);
    }

    isWhiteTurn = true;
    BoardHighlighter.Instance.Hidehighlights();
    SpawnAllChessPieces();
  }

  private void AI()
  {
    if (isWhiteTurn)
    {
      return;
    }

    //list of positions of pieces with possible legal moves
    List<int[]> moveablePieces = new List<int[]>();
    //maps each piece to a list of its available legal moves
    // Dictionary<int[],List<int[]>> allLegalMoves = new Dictionary<int[], List<int[]>>();
    List<List<int[]>> allLegalMoves = new List<List<int[]>>();

    //temporarily holds each pieces legal moves
    bool[,] r;
    List<int[]> rPossibleMoves = new List<int[]>();
    //gather all legal moves
    for (int i = 0; i < 8; i++)
    {
      for (int j = 0; j < 8; j++)
      {
        Chesspiece c = Chesspieces[i, j];
        if (c != null && !c.isWhite)
        {
          r = c.PossibleMove();
          rPossibleMoves = listPossibleMoves(r);
          if (rPossibleMoves.Count > 0)
          {
            int[] moveablePiece = new int[] { i, j };
            moveablePieces.Add(moveablePiece);
            allLegalMoves.Add(rPossibleMoves);
          }
        }
      }
    }

    int x = -1;
    int y = -1;
    List<int[]> legalMoves;

    //Search for a move that takes a piece
    for (int i = 0; i < moveablePieces.Count; i++)
    {
      legalMoves = allLegalMoves[i];
      foreach (int[] move in legalMoves)
      {
        x = move[0];
        y = move[1];
        Chesspiece c = Chesspieces[x, y];
        if (c != null && Chesspieces[x, y].isWhite)
        {
          makeMove(moveablePieces[i], move);
          Debug.Log("Test2");
          return;
        }
      }
    }

    //if no take move exists, then pick a move at random
    int randomIndex = Mathf.RoundToInt(Random.Range(-0.49f, (float)moveablePieces.Count - 0.51f));
    int[] randomPiece = moveablePieces[randomIndex];
    int[] randomMove = allLegalMoves[randomIndex][Mathf.RoundToInt(Random.Range(-0.49f, (float)randomPiece.Length - 0.51f))];

    makeMove(randomPiece, randomMove);
  }

  private List<int[]> listPossibleMoves(bool[,] r)
  {
    List<int[]> rPossibleMoves = new List<int[]>();
    for (int i = 0; i < 8; i++)
    {
      for (int j = 0; j < 8; j++)
      {
        if (r[i, j])
        {
          rPossibleMoves.Add(new int[] { i, j });
        }
      }
    }
    return rPossibleMoves;
  }

  private void makeMove(int[] pieceToMove, int[] moveTo)
  {
    int pieceX = pieceToMove[0];
    int pieceY = pieceToMove[1];
    int moveX = moveTo[0];
    int moveY = moveTo[1];
    //Debug.Log("pieceX: " + pieceX + "  pieceY: " + pieceY);
    //Debug.Log("moveX: " + moveX + "  moveX: " + moveY);
    SelectChesspiece(pieceX, pieceY);
    MoveChesspiece(moveX, moveY);
  }
}
