using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PenteGame.Models
{



    public class BoardModel
    {

        public PieceModel[,] board { get; set; } = new PieceModel[19, 19];

        public BoardModel()
        {
           
        }

        public BoardModel(PieceModel[,] board)
        {
            this.board = board;
        }
    }
}