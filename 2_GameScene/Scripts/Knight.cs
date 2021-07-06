using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chesspiece {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[8, 8];
        int[] knightMovesX = new int[] { 1, -1, -2, -2, -1,  1,  2, 2 };
        int[] knightMovesY = new int[] { 2,  2,  1, -1, -2, -2, -1, 1 };

        for (int i = 0; i < 8; i++) {
            KnightMove(CurrentX + knightMovesX[i], CurrentY + knightMovesY[i], ref r);
        }      

        return r;
    }
    public void KnightMove(int x, int y, ref bool[,] r) {
        Chesspiece c;
        if(x >= 0 && x <= 7 && y >= 0 && y <= 7) {
            c = BoardManager.Instance.Chesspieces[x, y];
            if(c == null) {
                r[x, y] = true;
            }
            else if (isWhite != c.isWhite) {
                r[x, y] = true;
            }
        }
    }

}