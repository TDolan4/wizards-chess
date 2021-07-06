using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chesspiece
{
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[8, 8];
        Chesspiece c;

        //right
        for (int i = CurrentX + 1; i <= 7; i++) {
            c = BoardManager.Instance.Chesspieces[i, CurrentY];
            if (c == null) {
                r[i, CurrentY] = true;
            }
            else {
                if (c.isWhite != isWhite) {
                    r[i, CurrentY] = true;
                }
                break;
            }
        }

        //left
        for (int i = CurrentX - 1; i >= 0; i--) {
            c = BoardManager.Instance.Chesspieces[i, CurrentY];
            if (c == null) {
                r[i, CurrentY] = true;
            }
            else {
                if (c.isWhite != isWhite) {
                    r[i, CurrentY] = true;
                }
                break;
            }
        }

        //up
        for (int i = CurrentY + 1; i <= 7; i++) {
            c = BoardManager.Instance.Chesspieces[CurrentX, i];
            if (c == null) {
                r[CurrentX, i] = true;
            }
            else {
                if (c.isWhite != isWhite) {
                    r[CurrentX, i] = true;
                }
                break;
            }
        }

        //down
        for (int i = CurrentY - 1; i >= 0; i--) {
            c = BoardManager.Instance.Chesspieces[CurrentX, i];
            if (c == null) {
                r[CurrentX, i] = true;
            }
            else {
                if (c.isWhite != isWhite) {
                    r[CurrentX, i] = true;
                }
                break;
            }
        }

        //forward right
        for (int i = CurrentX + 1, j = CurrentY + 1; i <= 7 && j <= 7; i++, j++) {
            c = BoardManager.Instance.Chesspieces[i, j];
            if (c == null) {
                r[i, j] = true;
            }
            else {
                if (isWhite != c.isWhite) {
                    r[i, j] = true;
                }
                break;
            }
        }

        //forward left
        for (int i = CurrentX - 1, j = CurrentY + 1; i >= 0 && j <= 7; i--, j++) {
            c = BoardManager.Instance.Chesspieces[i, j];
            if (c == null) {
                r[i, j] = true;
            }
            else {
                if (isWhite != c.isWhite) {
                    r[i, j] = true;
                }
                break;
            }
        }

        //back left
        for (int i = CurrentX - 1, j = CurrentY - 1; i >= 0 && j >= 0; i--, j--) {
            c = BoardManager.Instance.Chesspieces[i, j];
            if (c == null) {
                r[i, j] = true;
            }
            else {
                if (isWhite != c.isWhite) {
                    r[i, j] = true;
                }
                break;
            }
        }

        //back right
        for (int i = CurrentX + 1, j = CurrentY - 1; i <= 7 && j >= 0; i++, j--) {
            c = BoardManager.Instance.Chesspieces[i, j];
            if (c == null) {
                r[i, j] = true;
            }
            else {
                if (isWhite != c.isWhite) {
                    r[i, j] = true;
                }
                break;
            }
        }

        return r;
    }
}
