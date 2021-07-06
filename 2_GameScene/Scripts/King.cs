using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chesspiece {
    public override bool[,] PossibleMove() {
        bool[,] r = new bool[8, 8];
        int[] kingMovesX = new int[] { 1, 0, -1, -1, -1,  0,  1, 1 };
        int[] kingMovesY = new int[] { 1, 1,  1,  0, -1, -1, -1, 0 };

        for (int i = 0; i < 8; i++) {
            KingMove(CurrentX + kingMovesX[i], CurrentY + kingMovesY[i], ref r);
        }

        return r;
    }
    public void KingMove(int x, int y, ref bool[,] r) {
        Chesspiece c;
        if (x >= 0 && x <= 7 && y >= 0 && y <= 7) {
            c = BoardManager.Instance.Chesspieces[x, y];
            if (c == null) {
                r[x, y] = true;
            }
            else if (isWhite != c.isWhite) {
                r[x, y] = true;
            }
        }
    }
}
