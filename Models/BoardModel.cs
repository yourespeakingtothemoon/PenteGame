using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PenteGame.Models
{



    public class BoardModel
    {

        Piece[,] board = new Piece[19,19];
        public BoardModel()
        {
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                   
                   //TODO Logic to set the board
                }
            }
           
        }
        public Piece[,] getBoard()
        {
            return board;
        }

        public void setBoard(Piece[,] newBoard)
        {
            board = newBoard;
        }

        public void setBoardPiece(int x, int y, Piece value)
        {
            board[x, y] = value;
        }


    }
}