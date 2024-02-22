using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace PenteGame.Models
{
    public class PlayerModel
    {
        private static int nextID = 0;
        public int? id { get; set; } = nextID++;

        public string Name { get; set; }

        public int captured { get; set; } = 0;

        public List<PieceModel> capturedPieces = new List<PieceModel> { };

        public Color colorValue;

        public string pieceSprite { get; set; }

        public bool hasWon { get; set; } = false;

        public PlayerModel()
        {
        }

        public PlayerModel(string name, Color colorValue, int captured = 0, bool hasWon = false)
        {
            Name = name;
            this.captured = captured;
            this.colorValue = colorValue;

            this.pieceSprite = "/Content/Images/" + colorValue.Name + ".png";
            this.hasWon = hasWon;
        }
    }
}