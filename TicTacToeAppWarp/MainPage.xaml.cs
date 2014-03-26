using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text;
using com.shephertz.app42.gaming.multiplayer.client;

namespace TicTacToeAppWarp
{
    public partial class MainPage : BasePage
    {
        // Cached brushes for use by the textblocks
        private readonly Brush _greenBrush = new SolidColorBrush(Colors.Green);
        private readonly Brush _whiteBrush = new SolidColorBrush(Colors.White);

        private int moveCount = 0;

        // The gamepieces
        internal const string GAMEPIECE_X = "X";
        internal const string GAMEPIECE_0 = "O";

        // Store gamepiece and its current value ("X", "0" or "" (empty))
        internal Dictionary<string, string> _pieceMap;

        // ProgressIndicator that is shown during communication
        private ProgressIndicator _progressIndicator;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Create and add listener objects to receive notification events from the joined game room
            GlobalContext.notificationListenerObj = new NotificationListener(this);
            GlobalContext.warpClient.AddNotificationListener(GlobalContext.notificationListenerObj);

            // Create a ProgressIndicator and add it to the status bar (SystemTray)
            _progressIndicator = new ProgressIndicator();
            _progressIndicator.IsVisible = false;
            _progressIndicator.IsIndeterminate = true;
            SystemTray.SetProgressIndicator(this, _progressIndicator);

            LockGameboard();
            if (!GlobalContext.PlayerIsFirst)
                tbStatus.Text = "Wait for opponet to move. You are assigned O.";
            else
                tbStatus.Text = "Wait for Second user to join. You are assigned X";
            
            PlayerName.Text = GlobalContext.localUsername;
        }

        public void showResult(String result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Console.WriteLine("RESULT in settings " + result);
            });
        }
        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        private void appbarNewGame_Click(object sender, EventArgs e)
        {
            // Update peer to start a new game.
            GlobalContext.warpClient.SendUpdatePeers(MoveMessage.buildNewGameMessageBytes());
        }

        private void appbarEndGame_Click(object sender, EventArgs e)
        {
            // Clean up listeners and unsubscribe to stop receiving further events
            GlobalContext.warpClient.UnsubscribeRoom(GlobalContext.GameRoomId);
            GlobalContext.warpClient.LeaveRoom(GlobalContext.GameRoomId);
            GlobalContext.warpClient.RemoveNotificationListener(GlobalContext.notificationListenerObj);
            GlobalContext.warpClient.RemoveRoomRequestListener(GlobalContext.roomReqListenerObj);
            GlobalContext.warpClient.RemoveConnectionRequestListener(GlobalContext.conListenObj);
            GlobalContext.warpClient.Disconnect();
            NavigationService.Navigate(new Uri("/JoinPage.xaml", UriKind.RelativeOrAbsolute));
        }
        
        /// <summary>
        /// Refreshes the game board and informs the user what gamepiece they are playing
        /// </summary>
        internal void NewGame()
        {
            if (GlobalContext.PlayerIsFirst)
            {
                UpdateStatus(String.Format("Playing as '{0}'", GAMEPIECE_X));
            }
            else
            {
                UpdateStatus(String.Format("Playing as '{0}'", GAMEPIECE_0));
            }

            // The board may be "dirty" from a previous game, so wipe it clean
            InitializeBoard();

        }

        #region Update Status
        /// <summary>
        /// Helper method to write a simple status update to a TextBlock on the page.
        /// Uses the Dispatcher object if currently not on UI Thread.
        /// </summary>
        /// <param name="status">The status message to write</param>
        internal void UpdateStatus(string status)
        {

            if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                // Since I want to call the same logic here and in the Dispatcher case, 
                // I encapsulate it in a method they can both call.
                _updateStatus(status);
            }
            else
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _updateStatus(status);

                });
            }

        }

        private void _updateStatus(string status)
        {
            // If the ProgressIndicator is visible, hide it
            SystemTray.ProgressIndicator.IsVisible = false;
            tbStatus.Text = status;
        }
        #endregion

        /// <summary>
        /// Clear all board pieces and reset their text color. 
        /// Also resets the mapping of gamepiece to value
        /// </summary>
        internal void InitializeBoard()
        {
            if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                // Since I want to call the same logic here and in the Dispatcher case, 
                // I encapsulate it in a method they can both call.
                _initBoard();
            }
            else
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _initBoard();

                });
            }

        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
           // WarpClient.GetInstance().Disconnect();
            base.OnNavigatedFrom(e);
        }
        /// <summary>
        /// Clear all text on the board, reset font color and clear out internal gamepiece map
        /// </summary>
        private void _initBoard()
        {
            _pieceMap = new Dictionary<string, string>();
            moveCount = 0;

            // Each gamepiece is represented as a TextBlock inside the "gBoard"  
            // grid on the on the page
            foreach (TextBlock textBlock in gBoard.Children.OfType<TextBlock>())
            {
                _pieceMap.Add(textBlock.Name, string.Empty);
                textBlock.Text = string.Empty;
                textBlock.Foreground = _whiteBrush;
            }

            // The user can start to tap on the grid and the pieces
            gBoard.IsHitTestVisible = true;

        }

        /// <summary>
        /// Event handler for the Tap event on each gamepiece
        /// </summary>
        /// <param name="sender">The gamepiece that fired the event</param>
        /// <param name="e">The GestureEventArgs</param>
        private void tb_Tap(object sender, GestureEventArgs e)
        {
            if ((moveCount % 2 == 0 && !GlobalContext.PlayerIsFirst) || (moveCount % 2 != 0 && GlobalContext.PlayerIsFirst))
            {
                UpdateStatus("Waiting on Opponent!");
            }
            else
            {
                TextBlock tapped = (TextBlock)sender;

                // Check whether the TextBlock (gamepiece) being tapped is playable
                if (!String.IsNullOrWhiteSpace(tapped.Text))
                {
                    UpdateStatus(String.Format("Oops! That square is taken.", tapped.Name, tapped.Text));
                }
                else
                {
                    UpdateStatus(String.Empty);
                    // Send Update to all users in the room about the move
                    String myPiece = GlobalContext.PlayerIsFirst ? GAMEPIECE_X : GAMEPIECE_0;
                    WarpClient.GetInstance().SendUpdatePeers(MoveMessage.buildMessageBytes(tapped, myPiece));
                    LockGameboard();
                }
            }

        }

        /// <summary>
        /// Play the selected piece.
        /// </summary>
        /// <param name="tbTapped">The (TextBlock) that was played</param>
        private void Play(TextBlock tbTapped, MoveMessage msg)
        {
            // Make sure the status text is clear, because if we update the status message we want the
            // user to notice it
            tbStatus.Text = String.Empty;

            // The user can tap on the gameboard without clicking "New Game". In this case we may 
            // need to initialize the board if it has not been initialized already.
            // Initialization is signified by the _pieceMap not being null.
            if (_pieceMap == null)
                InitializeBoard();

            // Set the square to the user's gamepiece
            tbTapped.Text = msg.piece;

            // Record the updated value for that square
            _pieceMap[tbTapped.Name] = tbTapped.Text;

            // Based on the above move, it is possible that the game has been won
            if (SomebodyWon)
            {
                GameOver();
            }
            else
            {
                /// Is there still a piece for the Opponent to play?
                if (PiecesAvailable())
                {
                    if (msg.sender != GlobalContext.localUsername)
                    {
                        UpdateStatus("Your turn!");
                    }
                }
                else
                {
                    // Nobody wins - end the game
                    GameOver();
                    UpdateStatus("Nobody Won!");
                }

            }
        }

        /// <summary>
        /// Check to see if somebody has won the game based on the current state of the board
        /// </summary>
        internal bool SomebodyWon
        {
            get
            {
                // Get the player who won or else string.empty if nobody has won yet
                string winner = DidSomeoneWin();
                if (!String.IsNullOrEmpty(winner))
                {
                    if (GlobalContext.PlayerIsFirst && winner.ToLower() == GAMEPIECE_X.ToLower() || (!GlobalContext.PlayerIsFirst && winner.ToLower() == GAMEPIECE_0.ToLower()))
                    {
                        UpdateStatus("You Won!");
                        return true;
                    }
                    else
                    {
                        UpdateStatus("Opponent Won!");
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Check to see if there are any squares on the board still in play
        /// </summary>
        /// <returns>True if there is at least one square that is playable, False otherwise</returns>
        private bool PiecesAvailable()
        {
            bool piecesAvailable = false;
            foreach (string value in _pieceMap.Values)
            {
                if (String.IsNullOrEmpty(value))
                {
                    piecesAvailable = true;
                    break;
                }
            }

            return piecesAvailable;
        }


        /// <summary>
        /// Hard-coded game logic to keep things simple.
        /// There are 8 possible ways someone can get a winning line. 
        /// I check for each one here.
        /// </summary>
        /// <returns>Indicates where 'X' won or 'O' won</returns>
        private string DidSomeoneWin()
        {
            // Top Horizontal line
            if (!String.IsNullOrWhiteSpace(tb_00.Text) && tb_00.Text == tb_01.Text && tb_01.Text == tb_02.Text)
            {
                ShowWinningLine(tb_00, tb_01, tb_02);
                return tb_00.Text;
            }

            // Left Vertical Line
            if (!String.IsNullOrWhiteSpace(tb_00.Text) && tb_00.Text == tb_10.Text && tb_10.Text == tb_20.Text)
            {
                ShowWinningLine(tb_00, tb_10, tb_20);
                return tb_00.Text;
            }

            // Top Left to Bottom Right Line
            if (!String.IsNullOrWhiteSpace(tb_00.Text) && tb_00.Text == tb_11.Text && tb_11.Text == tb_22.Text)
            {
                ShowWinningLine(tb_00, tb_11, tb_22);
                return tb_00.Text;
            }

            // Bottom Left to Top Right Line
            if (!String.IsNullOrWhiteSpace(tb_02.Text) && tb_02.Text == tb_11.Text && tb_11.Text == tb_20.Text)
            {
                ShowWinningLine(tb_02, tb_11, tb_20);
                return tb_02.Text;
            }

            // Middle Vertical Line
            if (!String.IsNullOrWhiteSpace(tb_01.Text) && tb_01.Text == tb_11.Text && tb_11.Text == tb_21.Text)
            {
                ShowWinningLine(tb_01, tb_11, tb_21);
                return tb_01.Text;
            }

            // Right Vertical Line
            if (!String.IsNullOrWhiteSpace(tb_02.Text) && tb_02.Text == tb_12.Text && tb_12.Text == tb_22.Text)
            {
                ShowWinningLine(tb_02, tb_12, tb_22);
                return tb_02.Text;
            }

            // Middle Horizontal
            if (!String.IsNullOrWhiteSpace(tb_10.Text) && tb_10.Text == tb_11.Text && tb_11.Text == tb_12.Text)
            {
                ShowWinningLine(tb_10, tb_11, tb_12);
                return tb_10.Text;
            }

            // Bottom Horizontal
            if (!String.IsNullOrWhiteSpace(tb_20.Text) && tb_20.Text == tb_21.Text && tb_21.Text == tb_22.Text)
            {
                ShowWinningLine(tb_20, tb_21, tb_22);
                return tb_20.Text;
            }

            // Nobody won
            return string.Empty;

        }

        internal void ClearStatusText()
        {
            UpdateStatus(String.Empty);
        }

        /// <summary>
        /// Add a little bling to the gameboard by showing the winning line in a different color
        /// </summary>
        /// <param name="w1">Winning piece 1</param>
        /// <param name="w2">Winning piece 2</param>
        /// <param name="w3">Winning piece 3</param>
        private void ShowWinningLine(TextBlock w1, TextBlock w2, TextBlock w3)
        {
            w1.Foreground = _greenBrush;
            w2.Foreground = _greenBrush;
            w3.Foreground = _greenBrush;
        }

        /// <summary>
        /// Lock the gameboard when the game is over
        /// </summary>
        internal void GameOver()
        {
            LockGameboard();
        }

        internal void LockGameboard()
        {
            gBoard.IsHitTestVisible = false;
        }

        internal void UnLockGameboard()
        {
            gBoard.IsHitTestVisible = true;
        }

        private void appbarHideDiag_Click(object sender, EventArgs e)
        {
            ApplicationBarMenuItem toggle = (ApplicationBarMenuItem)sender;

            spDiagnostics.Visibility = (spDiagnostics.Visibility == System.Windows.Visibility.Visible) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            toggle.Text = (spDiagnostics.Visibility == System.Windows.Visibility.Visible) ? "Hide Diagnostics" : "Show Diagnostics";
        }

        internal void UpdateUiFromMove(MoveMessage msg)
        {
            TextBlock tb = (TextBlock)gBoard.FindName(msg.TextBoxName);
            Play(tb, msg);
            moveCount++;
            UnLockGameboard();
        }
    }
}