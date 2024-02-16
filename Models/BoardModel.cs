using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PenteGame.Models
{



    public class BoardModel
    {

        public PieceModel[,] board { get; set; } = new PieceModel[19, 19];
        public List<PlayerModel> players = new List<PlayerModel> { };

        public BoardModel()
        {
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    board[i, j] = new PieceModel();
                    board[i, j].x = i;
                    board[i, j].y = j;
                }
            }
           
        }

        public BoardModel(PieceModel[,] board)
        {
            this.board = board;
        }

        public PieceModel findPieceById(int id)
        {
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (board[i, j].id == id)
                    {
                        return board[i, j];
                    }
                }
            }
            return null;
        }

        public PieceModel findPieceByCoords(int x, int y)
        {
            return board[x, y];
        }
    }
}