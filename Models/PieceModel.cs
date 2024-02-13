using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PenteGame.Models
{
    public class PieceModel
    {
        private static int nextID = 0;
        public int? id { get; set; } = nextID++;

        public string image ="/Content/Images/empty.png";

        public int x { get; set; }
        public int y { get; set; }

        public Color colorValue;

        public PlayerModel ownedPlayer { get; set; }
        
        public bool isActive { get; set; }

        public PieceModel()
        {

        }

      
    }
}