using PenteGame.Models;
using System;
using System.Collections.Generic;

using System.Drawing;
using System.Web.Mvc;


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
        bool usingTimer = true;
        int turn;

        // fun html stuff
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Start(string timerUse, int timerLength, string P1Name, string P1colorValue, string P2Name, string P2colorValue)
        {
            if (P1Name == string.Empty || P1Name == null) P1Name = "Player 1";
            if (P2Name == string.Empty || P2Name == null) P2Name = "Player 2";

            if (P1colorValue != null && P2colorValue != null)
            {
                if (P1colorValue != P2colorValue)
                {
                    PlayerModel player = new PlayerModel(P1Name, GetColorValue(P1colorValue));
                    board.players.Add(player);

                    player = new PlayerModel(P2Name, GetColorValue(P2colorValue));
                    board.players.Add(player);

                    board.currentPlayer = board.players[0];

                    board.timerUsed = timerUse != null;
                    board.timerLength = timerLength;

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
            board.currentPlayer = board.players[0];
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
            piece.colorValue = board.currentPlayer.colorValue;
            piece.ownedPlayer = board.currentPlayer;
          board.lastPiece = piece;
           // board.lastPiece.ownedPlayer = board.currentPlayer;
            board.board[x, y] = piece;
            newPiece = piece;
            newPiece.ownedPlayer = board.currentPlayer;
            CheckGame();
            ChangeTurn();
           return RedirectToAction("Board", board);
        }


        public ActionResult SkipTurn()
        {   ChangeTurn();
            return RedirectToAction("Board", board);
               }

        public ActionResult ResetGame1()
        {
            ResetBoard();
            ResetPlayers();
            board.isGameOver = false;
            return RedirectToAction("Board", board);
        }

        public ActionResult ResetGame2()
        {
            board = new BoardModel();
            return RedirectToAction("Index");
        }

        public ActionResult ResetBoard()
        {
        for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    board.board[i, j].image = "/Content/Images/empty.png";
                    board.board[i, j].colorValue = Color.Empty;
                    board.board[i, j].ownedPlayer = null;
                }

            }
        
            return RedirectToAction("Board", board);
        }
        public ActionResult ResetPlayers()
        {
            foreach (var player in board.players)
            {
                player.captured = 0;
                player.hasWon = false;
                player.capturedPieces.Clear();
            }
            return RedirectToAction("Board", board);
        }

        

        // fun logic stuff

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

        public void CheckGame()
        {
             CheckForEvents();
             CaptureLogic();
             CheckCaptureWin();
            
        }
     
        void CheckCaptureWin()
        {
            if (newPiece.ownedPlayer.captured == 10)
            {
                newPiece.ownedPlayer.hasWon = true;
                EventDeploy(gameEvents.win, newPiece.ownedPlayer.Name);
              //  return true;
            }
            
        }
        //Depreciated code - CheckGame() is now used
        //    public void CheckBoard()
        //{

        //    if (CheckCaptures() || CheckFiveinRow())
        //    {
        //        //win   


        //        foreach (var player in board.players)
        //        {
        //            if (player.hasWon)
        //            {
        //                //win game

        //            }
        //        }
        //    }
        //}

        public void CaptureLogic()
        {
            CaptureLogicHoriz();
            CaptureLogicVert();
            CaptureLogicDiag();
        }

        //Depreciated code - CaptureLogic() is now used
        //public bool CheckCaptures()
        //{
        //    //horizontal and vertical
        //    //left
        //    if (newPiece.x < 3)
        //    {
        //        if (board.board[newPiece.x - 3, newPiece.y] != null)
        //        {
        //            if (board.board[newPiece.x - 3, newPiece.y].colorValue == newPiece.colorValue)
        //            {
        //                if (board.board[newPiece.x - 2, newPiece.y] != null && board.board[newPiece.x - 1, newPiece.y] != null)
        //                {
        //                    if (board.board[newPiece.x - 2, newPiece.y].colorValue != newPiece.colorValue || board.board[newPiece.x - 1, newPiece.y].colorValue != newPiece.colorValue)
        //                    {
        //                        newPiece.ownedPlayer.captured += 2;
        //                        board.board[newPiece.x - 2, newPiece.y] = null;
        //                        board.board[newPiece.x - 1, newPiece.y] = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //right
        //    if (newPiece.x > 15)
        //    {
        //        if (board.board[newPiece.x + 3, newPiece.y] != null)
        //        {
        //            if (board.board[newPiece.x + 3, newPiece.y].colorValue == newPiece.colorValue)
        //            {
        //                if (board.board[newPiece.x + 2, newPiece.y] != null && board.board[newPiece.x + 1, newPiece.y] != null)
        //                {
        //                    if (board.board[newPiece.x + 2, newPiece.y].colorValue != newPiece.colorValue || board.board[newPiece.x + 1, newPiece.y].colorValue != newPiece.colorValue)
        //                    {
        //                        newPiece.ownedPlayer.captured += 2;
        //                        board.board[newPiece.x + 2, newPiece.y] = null;
        //                        board.board[newPiece.x + 1, newPiece.y] = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //up
        //    if (newPiece.y < 3)
        //    {
        //        if (board.board[newPiece.x, newPiece.y + 3] != null)
        //        {
        //            if (board.board[newPiece.x, newPiece.y + 3].colorValue == newPiece.colorValue)
        //            {
        //                if (board.board[newPiece.x, newPiece.y + 2] != null && board.board[newPiece.x, newPiece.y + 1] != null)
        //                {
        //                    if (board.board[newPiece.x, newPiece.y + 2].colorValue != newPiece.colorValue || board.board[newPiece.x, newPiece.y + 1].colorValue != newPiece.colorValue)
        //                    {
        //                        newPiece.ownedPlayer.captured += 2;
        //                        board.board[newPiece.x, newPiece.y + 2] = null;
        //                        board.board[newPiece.x, newPiece.y + 1] = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //down
        //    if (newPiece.y > 15)
        //    {
        //        if (board.board[newPiece.x, newPiece.y - 3] != null)
        //        {
        //            if (board.board[newPiece.x, newPiece.y - 3].colorValue == newPiece.colorValue)
        //            {
        //                if (board.board[newPiece.x, newPiece.y - 2] != null && board.board[newPiece.x, newPiece.y - 1] != null)
        //                {
        //                    if (board.board[newPiece.x, newPiece.y - 2].colorValue != newPiece.colorValue || board.board[newPiece.x, newPiece.y - 1].colorValue != newPiece.colorValue)
        //                    {
        //                        newPiece.ownedPlayer.captured += 2;
        //                        board.board[newPiece.x, newPiece.y - 2] = null;
        //                        board.board[newPiece.x, newPiece.y - 1] = null;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    //dig
        //    //left up
        //    if (newPiece.x < 3 && newPiece.y < 3)
        //    {
        //        if (board.board[newPiece.x - 3, newPiece.y + 3] != null)
        //        {
        //            if (board.board[newPiece.x - 3, newPiece.y + 3].colorValue == newPiece.colorValue)
        //            {
        //                if (board.board[newPiece.x - 2, newPiece.y + 2] != null && board.board[newPiece.x - 1, newPiece.y + 1] != null)
        //                {
        //                    if (board.board[newPiece.x - 2, newPiece.y + 2].colorValue != newPiece.colorValue || board.board[newPiece.x - 1, newPiece.y + 1].colorValue != newPiece.colorValue)
        //                    {
        //                        newPiece.ownedPlayer.captured += 2;
        //                        board.board[newPiece.x - 2, newPiece.y + 2] = null;
        //                        board.board[newPiece.x - 1, newPiece.y + 1] = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //right up
        //    if (newPiece.x > 15 && newPiece.y < 3)
        //    {
        //        if (board.board[newPiece.x + 3, newPiece.y + 3] != null)
        //        {
        //            if (board.board[newPiece.x + 3, newPiece.y + 3].colorValue == newPiece.colorValue)
        //            {
        //                if (board.board[newPiece.x + 2, newPiece.y + 2] != null && board.board[newPiece.x + 1, newPiece.y + 1] != null)
        //                {
        //                    if (board.board[newPiece.x + 2, newPiece.y + 2].colorValue != newPiece.colorValue || board.board[newPiece.x + 1, newPiece.y + 1].colorValue != newPiece.colorValue)
        //                    {
        //                        newPiece.ownedPlayer.captured += 2;
        //                        board.board[newPiece.x + 2, newPiece.y + 2] = null;
        //                        board.board[newPiece.x + 1, newPiece.y + 1] = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //right down
        //    if (newPiece.x > 15 && newPiece.y > 15)
        //    {
        //        if (board.board[newPiece.x + 3, newPiece.y - 3] != null)
        //        {
        //            if (board.board[newPiece.x + 3, newPiece.y - 3].colorValue == newPiece.colorValue)
        //            {
        //                if (board.board[newPiece.x + 2, newPiece.y - 2] != null && board.board[newPiece.x + 1, newPiece.y - 1] != null)
        //                {
        //                    if (board.board[newPiece.x + 2, newPiece.y - 2].colorValue != newPiece.colorValue || board.board[newPiece.x + 1, newPiece.y - 1].colorValue != newPiece.colorValue)
        //                    {
        //                        newPiece.ownedPlayer.captured += 2;
        //                        board.board[newPiece.x + 2, newPiece.y - 2] = null;
        //                        board.board[newPiece.x + 1, newPiece.y - 1] = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //left down
        //    if (newPiece.x < 3 && newPiece.y > 15)
        //    {
        //        if (board.board[newPiece.x - 3, newPiece.y - 3] != null)
        //        {
        //            if (board.board[newPiece.x - 3, newPiece.y - 3].colorValue == newPiece.colorValue)
        //            {
        //                if (board.board[newPiece.x - 2, newPiece.y - 2] != null && board.board[newPiece.x - 1, newPiece.y - 1] != null)
        //                {
        //                    if (board.board[newPiece.x - 2, newPiece.y - 2].colorValue != newPiece.colorValue || board.board[newPiece.x - 1, newPiece.y - 1].colorValue != newPiece.colorValue)
        //                    {
        //                        newPiece.ownedPlayer.captured += 2;
        //                        board.board[newPiece.x - 2, newPiece.y - 2] = null;
        //                        board.board[newPiece.x - 1, newPiece.y - 1] = null;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (newPiece.ownedPlayer.captured == 10)
        //    {
        //        newPiece.ownedPlayer.hasWon = true;
        //    }

        //    //to do
        //    return false;
        //}

        public void CaptureLogicHoriz()
        {
            if (newPiece.x > 3 && board.board[newPiece.x - 3, newPiece.y].image == newPiece.image && newPiece.image != board.board[newPiece.x-1,newPiece.y].image)
            {
                if (board.board[newPiece.x - 1, newPiece.y].image == board.board[newPiece.x - 1, newPiece.y].image && board.board[newPiece.x-1,newPiece.y].image!="/Content/Images/empty.png")
                {
                    CapturePieces(new List<PieceModel> { board.board[newPiece.x - 1, newPiece.y], board.board[newPiece.x - 2, newPiece.y] });
                }
                
            }

            if (newPiece.x < 15 && board.board[newPiece.x + 3, newPiece.y].image == newPiece.image && newPiece.image != board.board[newPiece.x + 1, newPiece.y].image)
            {
                if (board.board[newPiece.x + 1, newPiece.y].image == board.board[newPiece.x + 1, newPiece.y].image && board.board[newPiece.x + 1, newPiece.y].image != "/Content/Images/empty.png")
                {
                    CapturePieces(new List<PieceModel> { board.board[newPiece.x + 1, newPiece.y], board.board[newPiece.x + 2, newPiece.y] });
                }
          
            }

        }

        public void CaptureLogicVert()
        {
if (newPiece.y > 3 && board.board[newPiece.x, newPiece.y - 3].image == newPiece.image && newPiece.image != board.board[newPiece.x, newPiece.y - 1].image)
            {
                if (board.board[newPiece.x, newPiece.y - 1].image == board.board[newPiece.x, newPiece.y - 1].image && board.board[newPiece.x, newPiece.y - 1].image != "/Content/Images/empty.png")
                {
                    CapturePieces(new List<PieceModel> { board.board[newPiece.x, newPiece.y - 1], board.board[newPiece.x, newPiece.y - 2] });
                }
            }

            if (newPiece.y < 15 && board.board[newPiece.x, newPiece.y + 3].image == newPiece.image && newPiece.image != board.board[newPiece.x, newPiece.y + 1].image)
            {
                if (board.board[newPiece.x, newPiece.y + 1].image == board.board[newPiece.x, newPiece.y + 1].image && board.board[newPiece.x, newPiece.y + 1].image != "/Content/Images/empty.png")
                {
                    CapturePieces(new List<PieceModel> { board.board[newPiece.x, newPiece.y + 1], board.board[newPiece.x, newPiece.y + 2] });
                }
            }
           
            
        }

        void CaptureLogicDiag()
        {
            if (newPiece.x > 3 && newPiece.y > 3 && board.board[newPiece.x - 3, newPiece.y - 3].image == newPiece.image && newPiece.image != board.board[newPiece.x - 1, newPiece.y - 1].image)
            {
                if (board.board[newPiece.x - 1, newPiece.y - 1].image == board.board[newPiece.x - 1, newPiece.y - 1].image && board.board[newPiece.x - 1, newPiece.y - 1].image != "/Content/Images/empty.png")
                {
                    CapturePieces(new List<PieceModel> { board.board[newPiece.x - 1, newPiece.y - 1], board.board[newPiece.x - 2, newPiece.y - 2] });
                }
            }

            if (newPiece.x < 15 && newPiece.y < 15 && board.board[newPiece.x + 3, newPiece.y + 3].image == newPiece.image && newPiece.image != board.board[newPiece.x + 1, newPiece.y + 1].image)
            {
                if (board.board[newPiece.x + 1, newPiece.y + 1].image == board.board[newPiece.x + 1, newPiece.y + 1].image && board.board[newPiece.x + 1, newPiece.y + 1].image != "/Content/Images/empty.png")
                {
                    CapturePieces(new List<PieceModel> { board.board[newPiece.x + 1, newPiece.y + 1], board.board[newPiece.x + 2, newPiece.y + 2] });
                }
            }

            if (newPiece.x > 3 && newPiece.y < 15 && board.board[newPiece.x - 3, newPiece.y + 3].image == newPiece.image && newPiece.image != board.board[newPiece.x - 1, newPiece.y + 1].image)
            {
                if (board.board[newPiece.x - 1, newPiece.y + 1].image == board.board[newPiece.x - 1, newPiece.y + 1].image && board.board[newPiece.x - 1, newPiece.y + 1].image != "/Content/Images/empty.png")
                {
                    CapturePieces(new List<PieceModel> { board.board[newPiece.x - 1, newPiece.y + 1], board.board[newPiece.x - 2, newPiece.y + 2] });

                }
            }

            if (newPiece.x < 15 && newPiece.y > 3 && board.board[newPiece.x + 3, newPiece.y - 3].image == newPiece.image && newPiece.image != board.board[newPiece.x + 1, newPiece.y - 1].image)
            {
                if (board.board[newPiece.x + 1, newPiece.y - 1].image == board.board[newPiece.x + 1, newPiece.y - 1].image && board.board[newPiece.x + 1, newPiece.y - 1].image != "/Content/Images/empty.png")
                {
                    CapturePieces(new List<PieceModel> { board.board[newPiece.x + 1, newPiece.y - 1], board.board[newPiece.x + 2, newPiece.y - 2] });
                }
            }

        }

        

        void CapturePieces(List<PieceModel> pieces)
        {
            newPiece.ownedPlayer.captured += pieces.Count;

            foreach (var piece in pieces)
            {
                PieceModel cappedPiece = new PieceModel(piece);
                newPiece.ownedPlayer.capturedPieces.Add(cappedPiece);
                piece.colorValue = Color.Empty;
                piece.image = "/Content/Images/empty.png";
            }
            EventDeploy(gameEvents.capture, board.currentPlayer.Name);
        }

        //depreciated code - Event System is now used
        //public bool CheckFiveinRow()
        //{
        //    if (CheckFiveHorizontal() || CheckFiveVertical() || CheckFiveFrontSlash() || CheckFiveBackSlash())
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public bool CheckFiveHorizontal()
        //{
        //    int count = 0;
        //    for (int i = newPiece.x; i < 4; i++)
        //    {
        //        if (newPiece.x - i < 0) break;
        //        if (board.board[newPiece.x - i, newPiece.y].colorValue != newPiece.colorValue) break;
        //        count++;
        //    }
        //    for (int i = newPiece.x; i > -4; i--)
        //    {
        //        if (newPiece.x - i > 15) break;
        //        if (board.board[newPiece.x - i, newPiece.y].colorValue != newPiece.colorValue) break;
        //        count++;
        //    }
        //    if (count >= 5) return true;
        //    return false;
        //}

        //public bool CheckFiveVertical()
        //{
        //    int count = 0;
        //    for (int i = newPiece.y; i > -4; i--)
        //    {
        //        if (newPiece.x - i < 0) break;
        //        if (board.board[newPiece.x, newPiece.y - i].colorValue != newPiece.colorValue) break;
        //        count++;
        //    }
        //    for (int i = newPiece.y; i < 4; i++)
        //    {
        //        if (newPiece.x - i > 15) break;
        //        if (board.board[newPiece.x, newPiece.y - i].colorValue != newPiece.colorValue) break;
        //        count++;
        //    }
        //    if (count >= 5) return true;
        //    return false;
        //}

        //public bool CheckFiveFrontSlash()
        //{
        //    int count = 0;
        //    for (int i = newPiece.x; i < 4; i++)
        //    {

        //        if (newPiece.x - i < 0 || newPiece.x + i > 15) break;
        //        if (board.board[newPiece.x - i, newPiece.y + i].colorValue != newPiece.colorValue) break;
        //        count++;


        //    }
        //    for (int i = newPiece.x; i > -4; i--)
        //    {

        //        if (newPiece.x - i > 15 || newPiece.x + i < 0) break;
        //        if (board.board[newPiece.x - i, newPiece.y + i].colorValue != newPiece.colorValue) break;
        //        count++;

        //    }
        //    if (count >= 5) return true;
        //    return false;
        //}

        //public bool CheckFiveBackSlash()
        //{
        //    int count = 0;
        //    for (int i = newPiece.x; i < 4; i++)
        //    {
        //        if (i < 0) break;
        //        if (board.board[newPiece.x - i, newPiece.y - i].colorValue != newPiece.colorValue) break;
        //        count++;
        //    }
        //    for (int i = newPiece.x; i > -4; i--)
        //    {
        //        if (i > 15) break;
        //        if (board.board[newPiece.x - i, newPiece.y - i].colorValue != newPiece.colorValue) break;
        //        count++;
        //    }
        //    if (count >= 5) return true;
        //    return false;
        //}



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
                default:
                    break;
            }
            return selected;
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
                    board.latestEvent = actingPlayerName + " has placed a piece at "+ newPiece.x.ToString() +","+newPiece.y.ToString();
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
            switch (CheckRowEvent())
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
        {
            int ret = 0;
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
            int countLeft = 0;
            int countRight = 0;
            //loop back to start of board and count matching pieces

            for (int i = piece.x; i < 19; i++)
            {

                if (board.board[i, piece.y].image == piece.image)
                {
                    countLeft++;
                }
                else
                {
                    break;
                }
            }
            for (int i = piece.x; i > 0; i--)
            {
                if (board.board[i, piece.y].image == piece.image)
                {
                    countRight++;
                }
                else
                {
                    break;
                }
            }


            return countLeft > countRight ? countLeft : countRight;

        }

        int CountVertical(PieceModel piece)
        {
            int countUp = 0;
            int countDown = 0;
            //loop back to start of board and count matching pieces

            for (int i = piece.y; i < 19; i++)
            {

                if (board.board[piece.x, i].image == piece.image)
                {
                    countUp++;
                }
                else
                {
                    break;
                }
            }
            for (int i = piece.y; i > 0; i--)
            {
                if (board.board[piece.x, i].image == piece.image)
                {
                    countDown++;
                }
                else
                {
                    break;
                }
            }
            return countUp > countDown ? countUp : countDown;
        }

        int CountDiagonal(PieceModel piece)
        {
            int countXY = 0;
            int countxy = 0;
            int countXy = 0;
            int countxY = 0;
            //loop back to start of board and count matching pieces

            for (int i = piece.x; i < 19; i++)
            {
                for (int j = piece.y; j < 19; j++)
                {
                    if (board.board[i, j].image == piece.image)
                    {
                        countXY++;
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
                    if (board.board[i, j].image == piece.image)
                    {
                        countxy++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int i = piece.x; i < 19; i++)
            {
                for (int j = piece.y; j > 0; j--)
                {
                    if (board.board[i, j].image == piece.image)
                    {
                        countXy++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int i = piece.x; i > 0; i--)
            {
                for (int j = piece.y; j < 19; j++)
                {
                    if (board.board[i, j].image == piece.image)
                    {
                        countxY++;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            int count = countXY > countxy ? countXY : countxy;
            count = count > countXy ? count : countXy;
            count = count > countxY ? count : countxY;
            return count;
        }


        //timer code
    

    }


}