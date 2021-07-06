using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chesspiece
{
    public override bool[,] PossibleMove(){
        bool[,] r = new bool[8,8];
        Chesspiece c, c2;

        //White team move
        if (isWhite) {
            //Diagonal Left
            if(CurrentX != 0 && CurrentY != 7)            {
                c = BoardManager.Instance.Chesspieces[CurrentX - 1, CurrentY + 1];
                if(c != null && !c.isWhite)                {
                    r[CurrentX - 1, CurrentY + 1] = true;
                }
            }
            //Diagonal Right
            if (CurrentX != 7 && CurrentY != 7)
            {
                c = BoardManager.Instance.Chesspieces[CurrentX + 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                {
                    r[CurrentX + 1, CurrentY + 1] = true;
                }
            }
            //Forward
            if(CurrentY != 7) {
                c = BoardManager.Instance.Chesspieces[CurrentX, CurrentY + 1];
                if (c == null){
                    r[CurrentX, CurrentY + 1] = true;
                }
            }
            //Forward 2
            if (CurrentY == 1){
                c = BoardManager.Instance.Chesspieces[CurrentX, CurrentY + 1];
                c2 = BoardManager.Instance.Chesspieces[CurrentX, CurrentY + 2];
                if (c == null && c2 == null) {
                    r[CurrentX, CurrentY + 2] = true;
                }
            }
            
        }
        else {
            //Diagonal Left
            if (CurrentX != 0 && CurrentY != 0)
            {
                c = BoardManager.Instance.Chesspieces[CurrentX - 1, CurrentY - 1];
                if (c != null && c.isWhite)
                {
                    r[CurrentX - 1, CurrentY - 1] = true;
                }
            }
            //Diagonal Right
            if (CurrentX != 7 && CurrentY != 0)
            {
                c = BoardManager.Instance.Chesspieces[CurrentX + 1, CurrentY - 1];
                if (c != null && c.isWhite)
                {
                    r[CurrentX + 1, CurrentY - 1] = true;
                }
            }
            //Forward
            if (CurrentY != 0)
            {
                c = BoardManager.Instance.Chesspieces[CurrentX, CurrentY - 1];
                if (c == null)
                {
                    r[CurrentX, CurrentY - 1] = true;
                }
            }
            //Forward 2
            if (CurrentY == 6)
            {
                c = BoardManager.Instance.Chesspieces[CurrentX, CurrentY - 1];
                c2 = BoardManager.Instance.Chesspieces[CurrentX, CurrentY - 2];
                if (c == null && c2 == null)
                {
                    r[CurrentX, CurrentY - 2] = true;
                }
            }
        }
        return r;
    }

}
