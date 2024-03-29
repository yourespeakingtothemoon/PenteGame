﻿using System;
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

        public PieceModel(int x, int y)
        {
           this.x = x;
           this.y = y;

        }

        public PieceModel(PieceModel piece)
        {
            this.id = piece.id;
            this.image = piece.image;
            this.x = piece.x;
            this.y = piece.y;
            this.colorValue = piece.colorValue;
            this.ownedPlayer = piece.ownedPlayer;
            this.isActive = piece.isActive;
        }

      
    }
}