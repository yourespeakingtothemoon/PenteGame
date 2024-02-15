using PenteGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Drawing;
using System.Web.Mvc;

namespace PenteGame.Controllers
{
    public class BoardController : Controller
    {
        public BoardModel board = new BoardModel();
        private PieceModel newPiece;

        int turn;

        // fun html stuff
        public ActionResult Index()
        {
            return View(board);
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
        public ActionResult Index(string P1Name, string P1colorValue, string P2Name,string P2colorValue)
        {
            PlayerModel player = new PlayerModel();
            player.Name = P1Name;
            player.colorValue = P1colorValue == "Black" ? Color.Black : Color.White; //should return Color.Black
            board.players.Add(player);

            player.Name = P2Name;
            player.colorValue = P2colorValue == "Black" ? Color.Black : Color.White; //should return Color.White
            board.players.Add(player);

            return RedirectToAction("board", "board");
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

        public ActionResult Board(BoardModel board)
        {
            return View(board);
        }

        public ActionResult UpdateBoard(PieceModel piece)
        {
            board.board[piece.x, piece.y] = piece;
            newPiece = piece;
            return RedirectToAction("Board", "Board");
        }

        public ActionResult DeleteBoard()
        {
            board = null;
            return RedirectToAction("board", "board");
        }

        public ActionResult AddBoard()
        {
            return View();
        }

        public ActionResult AddBoard(BoardModel board)
        {
            this.board = board;
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


           // for now we will just turn the piece red
            if (piece.image == "/Content/Images/empty.png")
                {
                if (newPiece == null)
                {
                    piece.image = "/Content/Images/red.png";
                }
                else
                {
                    if (newPiece.image == "/Content/Images/red.png")
                    {
                        piece.image = "/Content/Images/blue.png";
                    }
                    else
                    {
                        piece.image = "/Content/Images/red.png";

                    }
                }
                }
                else
                {                 //piece is already taken
                }

           // UpdateBoard(piece);
           newPiece = piece;
            return View("BoardView", board);
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


    }
}