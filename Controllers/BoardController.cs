using PenteGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Drawing;
using System.Web.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace PenteGame.Controllers
{
    public enum gameEvents
    {
        win = 0,
        placePiece = 1,
        capture = 2,
        tria = 3,
        tessera = 4
    }



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
            //would be easier: board.players.Add(PlayerModel(P1Name,P1colorValue));

            PlayerModel player = new PlayerModel(P1Name, P1colorValue.ToLower() == "black" ? Color.Black : Color.White);//should return Color.White
            board.players.Add(player);

            player = new PlayerModel(P2Name, P2colorValue.ToLower() == "black" ? Color.Black : Color.White); //should return Color.Black
            board.players.Add(player);

            board.currentPlayer = board.players[0];

            return RedirectToAction("Board", board);
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
                piece.image = board.currentPlayer.pieceSprite;
                piece.colorValue = board.currentPlayer.colorValue;
                piece.ownedPlayer = board.currentPlayer;
            }
            else
            {
                return RedirectToAction("Board", board);//piece is already taken
            }

            // UpdateBoard(piece);
            board.lastPiece = piece;
            board.board[x, y] = piece;
            newPiece = board.lastPiece;
            CheckBoard();
            ChangeTurn();
            return RedirectToAction("Board", board);
        }

        void ChangeTurn()
        {
            if (board.players[0] == board.currentPlayer)
            {
                board.currentPlayer = board.players[1];
            }
            else
            {
                board.currentPlayer = board.players[0];
            }
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
            if (newPiece.x > 3)
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
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x - 2, newPiece.y]);
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x - 1, newPiece.y]);
                                EventDeploy(gameEvents.capture, board.currentPlayer.Name);
                                board.board[newPiece.x - 2, newPiece.y] = new PieceModel();
                                board.board[newPiece.x - 1, newPiece.y] = new PieceModel();
                            }
                        }
                    }
                }
            }
            //right
            if (newPiece.x < 15)
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
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x + 2, newPiece.y]);
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x + 1, newPiece.y]);
                                EventDeploy(gameEvents.capture, board.currentPlayer.Name);
                                board.board[newPiece.x + 2, newPiece.y] = new PieceModel();
                                board.board[newPiece.x + 1, newPiece.y] = new PieceModel();
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
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y + 2]);
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y + 1]);
                                EventDeploy(gameEvents.capture, board.currentPlayer.Name);
                                board.board[newPiece.x, newPiece.y + 2] = new PieceModel();
                                board.board[newPiece.x, newPiece.y + 1] = new PieceModel();
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
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y - 2]);
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y - 1]);
                                EventDeploy(gameEvents.capture, board.currentPlayer.Name);
                                board.board[newPiece.x, newPiece.y - 2] = new PieceModel();
                                board.board[newPiece.x, newPiece.y - 1] = new PieceModel();
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
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y - 2]);
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y - 1]);
                                EventDeploy(gameEvents.capture, board.currentPlayer.Name);
                                board.board[newPiece.x - 2, newPiece.y + 2] = new PieceModel();
                                board.board[newPiece.x - 1, newPiece.y + 1] = new PieceModel();
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
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y - 2]);
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y - 1]);
                                EventDeploy(gameEvents.capture, board.currentPlayer.Name);
                                board.board[newPiece.x + 2, newPiece.y + 2] = new PieceModel();
                                board.board[newPiece.x + 1, newPiece.y + 1] = new PieceModel();
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
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y - 2]);
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y - 1]);
                                EventDeploy(gameEvents.capture, board.currentPlayer.Name);
                                board.board[newPiece.x + 2, newPiece.y - 2] = new PieceModel();
                                board.board[newPiece.x + 1, newPiece.y - 1] = new PieceModel();
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
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y - 2]);
                                newPiece.ownedPlayer.capturedPieces.Add(board.board[newPiece.x, newPiece.y - 1]);
                                EventDeploy(gameEvents.capture, board.currentPlayer.Name);
                                board.board[newPiece.x - 2, newPiece.y - 2] = new PieceModel();
                                board.board[newPiece.x - 1, newPiece.y - 1] = new PieceModel();
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


        public void EventDeploy(gameEvents eventID, string actingPlayerName)
        {
            //check for win
            switch (eventID)
            {

                case gameEvents.win:

                    board.latestEvent = actingPlayerName + " has won the game!";
                    board.isGameOver = true;
                    break;
                case gameEvents.placePiece:
                    board.latestEvent = actingPlayerName + " has placed a piece at x,y";
                    break;
                case gameEvents.capture:
                    board.latestEvent = actingPlayerName + " has captured a pair!";
                    break;
                case gameEvents.tria:
                    board.latestEvent = actingPlayerName + " has formed a tria!";
                    break;
                case gameEvents.tessera:
                    board.latestEvent = actingPlayerName + " has formed a tessera!";
                    break;
            }

            //check if captured


            //check for tria
            //check for tessera
            //if none of the above, return "@player has placed a piece at x,y"



        }


        void CheckForEvents()
        {
           switch(CheckRowEvent())
            {
                case 3:
                    EventDeploy(gameEvents.tria, board.currentPlayer.Name);
                    break;
                case 4:
                    EventDeploy(gameEvents.tessera, board.currentPlayer.Name);
                    break;
                case 5:
                    EventDeploy(gameEvents.win, board.currentPlayer.Name);
                    break;
                default:
                    EventDeploy(gameEvents.placePiece, board.currentPlayer.Name);
                    break;
            }
        }


        int CheckRowEvent()
        {   int ret = 0;
            int horiz = CountSideways(newPiece);
            ret = horiz;
            int vert = CountVertical(newPiece);
            ret = vert > ret ? vert : ret;
            int diag = CountDiagonal(newPiece);
            ret = diag > ret ? diag : ret;

            return ret;
        }




        int CountSideways(PieceModel piece)
        {
            int count =0;
            //loop back to start of board and count matching pieces
          
            for(int i = piece.x; i < 19; i++)
            {

                if (board.board[i, piece.y].colorValue == piece.colorValue)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            for (int i = piece.x; i > 0; i--)
            {
                if (board.board[i, piece.y].colorValue == piece.colorValue)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }


            return count;

        }

        int CountVertical(PieceModel piece)
        {
            int count = 0;
            //loop back to start of board and count matching pieces

            for (int i = piece.y; i < 19; i++)
            {

                if (board.board[piece.x, i].colorValue == piece.colorValue)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            for (int i = piece.y; i > 0; i--)
            {
                if (board.board[piece.x, i].colorValue == piece.colorValue)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        int CountDiagonal(PieceModel piece)
        {
            int count = 0;
            //loop back to start of board and count matching pieces

            for (int i = piece.x; i < 19; i++)
            {
                for (int j = piece.y; j < 19; j++)
                {
                    if (board.board[i, j].colorValue == piece.colorValue)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int i = piece.x; i > 0; i--)
            {
                for (int j = piece.y; j > 0; j--)
                {
                    if (board.board[i, j].colorValue == piece.colorValue)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

                
            return count;
        }




        //check the board
        // check for win
        // check if stolen 
        // check if stolen == to 10


    }
}