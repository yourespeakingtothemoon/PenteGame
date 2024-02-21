using PenteGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Drawing;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;

namespace PenteGame.Controllers
{
    public class BoardController : Controller
    {
        static public BoardModel board = new BoardModel();
        private PieceModel newPiece;

        int turn;

        // fun html stuff
        public ActionResult Index()
        {
            return View();
        }

        #region Index_NON_WORKING
        //[HttpPost]
        //public ActionResult Index(PlayerModel player)
        //{
        //    board.players.Add(player);
        //    return RedirectToAction("board", "board");
        //}

        //[HttpPost]
        //public ActionResult Index(PlayerModel player, PlayerModel player2)
        //{
        //    board.players.Add(player);
        //    board.players.Add(player2);
        //    return RedirectToAction("board", "board");
        //}
        #endregion

        [HttpPost]
        public ActionResult Index(string P1Name, string P1colorValue, string P2Name, string P2colorValue)
        {
            if (P1colorValue != null && P2colorValue != null)
            {
                if (P1colorValue != P2colorValue)
                {
                    PlayerModel player = new PlayerModel(P1Name, GetColorValue(P1colorValue));
                    board.players.Add(player);

                    player = new PlayerModel(P2Name, GetColorValue(P2colorValue));
                    board.players.Add(player);

                    return RedirectToAction("Board", board);
                }
                return RedirectToAction("Index");
            }
            else 
            {
                return RedirectToAction("Index");
            }
        }


        public ActionResult StartGame(int numOfPlayers)
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                PlayerModel player = new PlayerModel();
                board.players.Add(player);
            }
            return View("board", "board");

        }

        public ActionResult Board()
        {
            return View(board);
        }

        public ActionResult UpdateBoard(PieceModel piece)
        {
            board.board[piece.x, piece.y] = piece;
            newPiece = piece;
            return View("BoardView", board);
        }

        public ActionResult DeleteBoard()
        {
            board = null;
            return RedirectToAction("board", "board");
        }

        [HttpGet]
        public ActionResult AddBoard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBoard(BoardModel boardboy)
        {
            board = boardboy;
            return RedirectToAction("board", "board");
        }

        public ActionResult GetBoard()
        {
            return View("BoardView", board);
        }

        [HttpPost]
       public ActionResult pieceClicked(Object sender, EventArgs e)
        {

            var x = Convert.ToInt32(Request.Form["pieceX"]);
            var y = Convert.ToInt32(Request.Form["pieceY"]);
            var piece = board.findPieceByCoords(x, y);

           // PieceModel piece = new PieceModel(x,y);
           // for now we will just turn the piece red
           if (piece.image == "/Content/Images/empty.png")
               {
                if (board.lastPiece != null)
                {

                    if (board.lastPiece.image == "/Content/Images/red.png")
                    {
                        piece.image = "/Content/Images/blue.png";
                    }
                    else
                    {
                        piece.image = "/Content/Images/red.png";

                    }
                }
                else
                {
                        piece.image = "/Content/Images/red.png";
                }
               }
             else
               {
             return RedirectToAction("Board", board);//piece is already taken
               }

           // UpdateBoard(piece);
          board.lastPiece = piece;
            board.board[x, y] = piece;
           return RedirectToAction("Board", board);
        }


            // fun logic stuff

            public void CheckBoard()
        {

            if (CheckCaptures() || CheckFiveinRow())
            {
                //win   


                foreach (var player in board.players)
                {
                    if (player.hasWon)
                    {
                        //win game

                    }
                }
            }

        }

        public bool CheckCaptures()
        {
            //horizontal and vertical
            //left
            if (newPiece.x < 3)
            {
                if (board.board[newPiece.x - 3, newPiece.y] != null)
                {
                    if (board.board[newPiece.x - 3, newPiece.y].colorValue == newPiece.colorValue)
                    {
                        if (board.board[newPiece.x - 2, newPiece.y] != null && board.board[newPiece.x - 1, newPiece.y] != null)
                        {
                            if (board.board[newPiece.x - 2, newPiece.y].colorValue != newPiece.colorValue || board.board[newPiece.x - 1, newPiece.y].colorValue != newPiece.colorValue)
                            {
                                newPiece.ownedPlayer.captured += 2;
                                board.board[newPiece.x - 2, newPiece.y] = null;
                                board.board[newPiece.x - 1, newPiece.y] = null;
                            }
                        }
                    }
                }
            }
            //right
            if (newPiece.x > 15)
            {
                if (board.board[newPiece.x + 3, newPiece.y] != null)
                {
                    if (board.board[newPiece.x + 3, newPiece.y].colorValue == newPiece.colorValue)
                    {
                        if (board.board[newPiece.x + 2, newPiece.y] != null && board.board[newPiece.x + 1, newPiece.y] != null)
                        {
                            if (board.board[newPiece.x + 2, newPiece.y].colorValue != newPiece.colorValue || board.board[newPiece.x + 1, newPiece.y].colorValue != newPiece.colorValue)
                            {
                                newPiece.ownedPlayer.captured += 2;
                                board.board[newPiece.x + 2, newPiece.y] = null;
                                board.board[newPiece.x + 1, newPiece.y] = null;
                            }
                        }
                    }
                }
            }
            //up
            if (newPiece.y < 3)
            {
                if (board.board[newPiece.x, newPiece.y + 3] != null)
                {
                    if (board.board[newPiece.x, newPiece.y + 3].colorValue == newPiece.colorValue)
                    {
                        if (board.board[newPiece.x, newPiece.y + 2] != null && board.board[newPiece.x, newPiece.y + 1] != null)
                        {
                            if (board.board[newPiece.x, newPiece.y + 2].colorValue != newPiece.colorValue || board.board[newPiece.x, newPiece.y + 1].colorValue != newPiece.colorValue)
                            {
                                newPiece.ownedPlayer.captured += 2;
                                board.board[newPiece.x, newPiece.y + 2] = null;
                                board.board[newPiece.x, newPiece.y + 1] = null;
                            }
                        }
                    }
                }
            }
            //down
            if (newPiece.y > 15)
            {
                if (board.board[newPiece.x, newPiece.y - 3] != null)
                {
                    if (board.board[newPiece.x, newPiece.y - 3].colorValue == newPiece.colorValue)
                    {
                        if (board.board[newPiece.x, newPiece.y - 2] != null && board.board[newPiece.x, newPiece.y - 1] != null)
                        {
                            if (board.board[newPiece.x, newPiece.y - 2].colorValue != newPiece.colorValue || board.board[newPiece.x, newPiece.y - 1].colorValue != newPiece.colorValue)
                            {
                                newPiece.ownedPlayer.captured += 2;
                                board.board[newPiece.x, newPiece.y - 2] = null;
                                board.board[newPiece.x, newPiece.y - 1] = null;
                            }
                        }
                    }
                }
            }

            //dig
            //left up
            if (newPiece.x < 3 && newPiece.y < 3)
            {
                if (board.board[newPiece.x - 3, newPiece.y + 3] != null)
                {
                    if (board.board[newPiece.x - 3, newPiece.y + 3].colorValue == newPiece.colorValue)
                    {
                        if (board.board[newPiece.x - 2, newPiece.y + 2] != null && board.board[newPiece.x - 1, newPiece.y + 1] != null)
                        {
                            if (board.board[newPiece.x - 2, newPiece.y + 2].colorValue != newPiece.colorValue || board.board[newPiece.x - 1, newPiece.y + 1].colorValue != newPiece.colorValue)
                            {
                                newPiece.ownedPlayer.captured += 2;
                                board.board[newPiece.x - 2, newPiece.y + 2] = null;
                                board.board[newPiece.x - 1, newPiece.y + 1] = null;
                            }
                        }
                    }
                }
            }
            //right up
            if (newPiece.x > 15 && newPiece.y < 3)
            {
                if (board.board[newPiece.x + 3, newPiece.y + 3] != null)
                {
                    if (board.board[newPiece.x + 3, newPiece.y + 3].colorValue == newPiece.colorValue)
                    {
                        if (board.board[newPiece.x + 2, newPiece.y + 2] != null && board.board[newPiece.x + 1, newPiece.y + 1] != null)
                        {
                            if (board.board[newPiece.x + 2, newPiece.y + 2].colorValue != newPiece.colorValue || board.board[newPiece.x + 1, newPiece.y + 1].colorValue != newPiece.colorValue)
                            {
                                newPiece.ownedPlayer.captured += 2;
                                board.board[newPiece.x + 2, newPiece.y + 2] = null;
                                board.board[newPiece.x + 1, newPiece.y + 1] = null;
                            }
                        }
                    }
                }
            }
            //right down
            if (newPiece.x > 15 && newPiece.y > 15)
            {
                if (board.board[newPiece.x + 3, newPiece.y - 3] != null)
                {
                    if (board.board[newPiece.x + 3, newPiece.y - 3].colorValue == newPiece.colorValue)
                    {
                        if (board.board[newPiece.x + 2, newPiece.y - 2] != null && board.board[newPiece.x + 1, newPiece.y - 1] != null)
                        {
                            if (board.board[newPiece.x + 2, newPiece.y - 2].colorValue != newPiece.colorValue || board.board[newPiece.x + 1, newPiece.y - 1].colorValue != newPiece.colorValue)
                            {
                                newPiece.ownedPlayer.captured += 2;
                                board.board[newPiece.x + 2, newPiece.y - 2] = null;
                                board.board[newPiece.x + 1, newPiece.y - 1] = null;
                            }
                        }
                    }
                }
            }
            //left down
            if (newPiece.x < 3 && newPiece.y > 15)
            {
                if (board.board[newPiece.x - 3, newPiece.y - 3] != null)
                {
                    if (board.board[newPiece.x - 3, newPiece.y - 3].colorValue == newPiece.colorValue)
                    {
                        if (board.board[newPiece.x - 2, newPiece.y - 2] != null && board.board[newPiece.x - 1, newPiece.y - 1] != null)
                        {
                            if (board.board[newPiece.x - 2, newPiece.y - 2].colorValue != newPiece.colorValue || board.board[newPiece.x - 1, newPiece.y - 1].colorValue != newPiece.colorValue)
                            {
                                newPiece.ownedPlayer.captured += 2;
                                board.board[newPiece.x - 2, newPiece.y - 2] = null;
                                board.board[newPiece.x - 1, newPiece.y - 1] = null;
                            }
                        }
                    }
                }
            }

            if (newPiece.ownedPlayer.captured == 10)
            {
                newPiece.ownedPlayer.hasWon = true;
            }

            //to do
            return false;
        }

        public bool CheckFiveinRow()
        {
            if (CheckFiveHorizontal() || CheckFiveVertical() || CheckFiveFrontSlash() || CheckFiveBackSlash())
            {
                return true;
            }
            return false;
        }

        public bool CheckFiveHorizontal()
        {
            int count = 0;
            for (int i = newPiece.x; i < 4; i++)
            {
                if (newPiece.x - i < 0) break;
                if (board.board[newPiece.x - i, newPiece.y].colorValue != newPiece.colorValue) break;
                count++;
            }
            for (int i = newPiece.x; i > -4; i--)
            {
                if (newPiece.x - i > 15) break;
                if (board.board[newPiece.x - i, newPiece.y].colorValue != newPiece.colorValue) break;
                count++;
            }
            if (count >= 5) return true;
            return false;
        }

        public bool CheckFiveVertical()
        {
            int count = 0;
            for (int i = newPiece.y; i > -4; i--)
            {
                if (newPiece.x - i < 0) break;
                if (board.board[newPiece.x, newPiece.y - i].colorValue != newPiece.colorValue) break;
                count++;
            }
            for (int i = newPiece.y; i < 4; i++)
            {
                if (newPiece.x - i > 15) break;
                if (board.board[newPiece.x, newPiece.y - i].colorValue != newPiece.colorValue) break;
                count++;
            }
            if (count >= 5) return true;
            return false;
        }

        public bool CheckFiveFrontSlash()
        {
            int count = 0;
            for (int i = newPiece.x; i < 4; i++)
            {

                if (newPiece.x - i < 0 || newPiece.x + i > 15) break;
                if (board.board[newPiece.x - i, newPiece.y + i].colorValue != newPiece.colorValue) break;
                count++;


            }
            for (int i = newPiece.x; i > -4; i--)
            {

                if (newPiece.x - i > 15 || newPiece.x + i < 0) break;
                if (board.board[newPiece.x - i, newPiece.y + i].colorValue != newPiece.colorValue) break;
                count++;

            }
            if (count >= 5) return true;
            return false;
        }

        public bool CheckFiveBackSlash()
        {
            int count = 0;
            for (int i = newPiece.x; i < 4; i++)
            {
                if (i < 0) break;
                if (board.board[newPiece.x - i, newPiece.y - i].colorValue != newPiece.colorValue) break;
                count++;
            }
            for (int i = newPiece.x; i > -4; i--)
            {
                if (i > 15) break;
                if (board.board[newPiece.x - i, newPiece.y - i].colorValue != newPiece.colorValue) break;
                count++;
            }
            if (count >= 5) return true;
            return false;
        }



        //check the board
        // check for win
        // check if stolen 
        // check if stolen == to 10

        //Check the color and Get it to what it needs
        public Color GetColorValue(string colorValue)
        {
            Color selected = new Color();

            switch (colorValue.ToLower())
            {
                case "black":
                    selected = Color.Black;
                    break;
                case "white":
                    selected = Color.White;
                    break;
                case "red":
                    selected = Color.Red;
                    break;
                case "blue":
                    selected = Color.Blue;
                    break;
                case "yellow":
                    selected = Color.Yellow;
                    break;
                case "orange":
                    selected = Color.Orange;
                    break;
                case "green":
                    selected = Color.Green;
                    break;
                case "purple":
                    selected = Color.Purple;
                    break;
            }

            //if (colorValue == "black") selected = Color.Black;
            //else if (colorValue == "white") selected = Color.White;
            //else if (colorValue == "red") selected = Color.Red;
            //else if (colorValue == "blue") selected = Color.Blue;
            //else if (colorValue == "yellow") selected = Color.Yellow;
            //else if (colorValue == "orange") selected = Color.Orange;
            //else if (colorValue == "green") selected = Color.Green;
            //else if (colorValue == "purple") selected = Color.Purple;

            return selected;
        }
    }
}