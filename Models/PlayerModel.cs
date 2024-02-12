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

        public Color colorValue;

        public bool hasWon { get; set; } = false;

        public bool isTurn { get; set; }

        public PlayerModel()
        {
        }

        public PlayerModel(string name, Color value, bool hasWon, bool isTurn)
        {
            Name = name;
            this.value = value;
            this.hasWon = hasWon;
            this.isTurn = isTurn;
        }
    }
}