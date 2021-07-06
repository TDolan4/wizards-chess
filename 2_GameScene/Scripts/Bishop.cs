using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Chesspiece
{
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[8, 8];
        Chesspiece c;

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
        for (int i = CurrentX - 1, j = CurrentY + 1; i >= 0 && j<= 7; i--, j++) {
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
