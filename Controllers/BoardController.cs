using PenteGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace PenteGame.Controllers
{
    public class BoardController : Controller
    {
        public BoardModel board = new BoardModel();
        public List<PlayerModel> players = new List<PlayerModel> { };
        private PieceModel newPiece;

        int turn;

        // fun html stuff
        public ActionResult Index()
        {
            return View(board);
        }

        public ActionResult StartGame(int numOfPlayers)
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                PlayerModel player = new PlayerModel();
                players.Add(player);
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
            return RedirectToAction("board", "board");
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


        // fun logic stuff

        public void CheckBoard()
        {

            if (CheckCaptures() || CheckFiveinRow())
            {
                //win   


                foreach (var player in players)
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