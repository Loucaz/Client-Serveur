using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class BoardGame_ViewModel : INotifyPropertyChanged
    {
        private readonly Connexion connexion = new();

        #region Button
        private bool canClose;
        public bool CanClose
        {
            get { return canClose; }
            set
            {
                canClose = value;
                OnPropertyChanged();
            }
        }

        public async Task ClickCommandAsync()
        {

            TextServer = "Attente du serveur....";
            //CanClose = false;
            _ = await connexion.setMessageAsync(Text);
            Text = "Client->" + Text;
        }


        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => MyAction(), () => CanExecute));
            }
        }
        public bool CanExecute
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }

        public void MyAction()
        {
            _ = ClickCommandAsync();
        }
        #endregion

        #region Board
        private List<Cards> board;
        public List<Cards> Board
        {
            get { return board; }
            set
            {
                board = value;
                OnPropertyChanged();
            }
        }

        public BoardGame_ViewModel(string nameP)
        {
            Score = "0";
            GenereBoard();
            TextServer = "Attente du Serveur";
            MessageServ = new List<string>
            {
                "...",
                "...",
                "En attente du serveur",
                "Il arrive promis",
                "..."
            };
            namePlayer = nameP;
            Text = "JOIN:" + namePlayer;

            Connexion.MainConnexion();

            _ = ClickCommandAsync();

            _ = Boucle();
        }

        //Pour des tests em local
        private void GenereBoard()
        {
            List<Cards> newBoard = new List<Cards>();
            List<Card> newHand = new List<Card>();

            for (int y = 0; y < 4; y++)
            {
                Cards cards = new Cards();

                for (int x = 0; x < 5; x++)
                {
                    Card c = new()
                    {
                        Num = new Random().Next(0)
                    };
                    cards.Line.Add(c);
                    if (x < 1)
                    {
                        newHand.Add(c);
                    }
                }
                cards.Index = y;
                newBoard.Add(cards);

                Board = newBoard;
                Hand = newHand;
            }
        }

        //Met a jour le board
        private void UpdateBoard(string message)
        {
            string[] stringLine = message.Split(';');

            List<Cards> newBoard = new();

            int index = 0;
            foreach (string line in stringLine)
            {
                index++;
                if (string.IsNullOrEmpty(line))
                    continue;
                Cards cards = new();
                string[] stringLineCards = line.Split(',');
                foreach (string num in stringLineCards)
                {
                    if (string.IsNullOrEmpty(num))
                    {
                        continue;
                    }
                    cards.Line.Add(new Card()
                    {
                        Num = int.Parse(num),
                        Malus = Cards.getPoints(int.Parse(num))
                    });
                }
                cards.Index = index;
                newBoard.Add(cards);
            }
            Board = newBoard;
        }
        #endregion

        #region Hand

        private List<Card> hand;
        public List<Card> Hand
        {
            get { return hand; }
            set
            {
                hand = value;
                OnPropertyChanged();
            }
        }

        //Met a jour la main du joueur
        private void UpdateHand(string message)
        {

            string[] stringHand = message.Split(',');

            List<Card> newHand = new();

            foreach (string num in stringHand)
            {
                if (string.IsNullOrEmpty(num))
                {
                    continue;
                }
                newHand.Add(new Card()
                {
                    Num = int.Parse(num),
                    Malus = Cards.getPoints(int.Parse(num))
                });
            }


            Hand = newHand;
        }
        #endregion

        #region Score
        private string score;

        public string Score
        {
            get { return score; }
            set
            {
                score = value;
                OnPropertyChanged();
            }
        }

        //Met a jour le Score
        private void UpdateScore(string message)
        {
            Score = message;
        }

        //Message Final des scores
        private string EndMessage(string message)
        {
            List<Player> list = new();
            string[] stringLine = message.Split(';');
            foreach (string rep in stringLine)
            {
                if (string.IsNullOrEmpty(rep))
                    continue;
                string[] p = rep.Split(',');
                list.Add(new Player(p[0], p[1]));
            }
            list.Sort((x, y) => x.points.CompareTo(y.points));

            string result = "";
            for (int x = 0; x < list.Count; x++)
            {
                result += list[x].name + " est numero: " + (x + 1) + " avec un score de: " + list[x].points + "\n";
            }
            return result;
        }

        #endregion

        #region Text
        public event PropertyChangedEventHandler PropertyChanged;


        // Logs
        private List<string> messageServ;

        public List<string> MessageServ
        {
            get { return messageServ; }
            set
            {
                messageServ = value;
                OnPropertyChanged();
            }
        }
        public string namePlayer
        {
            get;
            set;
        }
        private string textServer;

        public string TextServer
        {
            get { return textServer; }
            set
            {
                textServer = value;
                OnPropertyChanged();
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                MessageServ = new List<string>
                {
                    MessageServ[1],
                    MessageServ[2],
                    MessageServ[3],
                    MessageServ[4],
                    value
                };
                text = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Ecoute en boucle le serveur
        private async Task Boucle()
        {
            while (true)
            {
                string reponse = await Task.Run(() => connexion.getMessageAsync());
                Text = "Server->" + reponse;

                Lire(reponse);
                _ = connexion.setMessageAsync("OK");
            }
        }

        //Lit le message recu par le serveur
        private void Lire(string message)
        {
            string[] code = message.Split(':');
            switch (code[0])
            {
                case "CARDS":
                    UpdateHand(code[1]);
                    break;
                case "BOARD":
                    UpdateBoard(code[1]);
                    TextServer = "Choisis une carte et envoye la au serveur !";
                    break;
                case "LINE":
                    TextServer = "Choisis une ligne et envoye la au serveur !";
                    break;
                case "SCORE":
                    UpdateScore(code[1]);
                    break;
                case "NO":
                    TextServer = "Le serveur n'as pas compris ta commande !";
                    break;
                case "END":
                    TextServer = EndMessage(code[1]);
                    break;
                default:
                    break;
            }
            CanClose = true;
        }
        #endregion
    }
}
