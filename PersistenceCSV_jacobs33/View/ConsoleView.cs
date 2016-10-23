using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceCSV_jacobs33
{
    public class ConsoleView
    {
        #region ENUMS
        #endregion

        #region FIELDS
        private const int _HEIGHT = 50;
        private const int _WIDTH = 100;
        private string _title;
        #endregion

        #region PROPERTIES
        public string Title
        {
            get { return _title; }
        }
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// ConsoleView constructor
        /// </summary>
        public ConsoleView()
        {
            InitializeConsole();
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Setup the view
        /// </summary>
        private void InitializeConsole()
        {
            //set height and width
            Console.WindowWidth = _WIDTH;
            Console.BufferWidth = _WIDTH;

            Console.WindowHeight = _HEIGHT;
            Console.BufferHeight = _HEIGHT;

            //change title
            _title = "Persistence by Taylor Jacobson";
            Console.Title = _title;
        }
        
        /// <summary>
        /// Return keypress
        /// </summary>
        /// <returns>ConsoleKey</returns>
        public ConsoleKey GetKeyPress()
        {
            return Console.ReadKey(true).Key;
        }

        /// <summary>
        /// Display menu
        /// </summary>
        /// <param name="title"></param>
        public void DisplayMenu(string title)
        {
            //variables
            int maxWidth = 0, maxHeight = 0;
            int top = 0, left = 0;
            List<string> menu = new List<string>();

            //reset display
            ResetConsole();

            //build readable menu
            foreach (Controller.MenuItems item in Enum.GetValues(typeof(Controller.MenuItems)))
            {
                if (item == Controller.MenuItems.None) continue;
                string str = string.Concat(item.ToString().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
                menu.Add($"{(int)item}. {str}");
            }

            //determine max width for any item in the menu
            foreach (string item in menu)
            {
                maxWidth = maxWidth < item.Length ? item.Length : maxWidth;
            }

            //determine starting coordinates for the TOP/LEFT of menu
            maxHeight = (menu.Count + (title != "" ? 1 : 0)) * 2;
            top = (_HEIGHT - maxHeight) / 4 - 2;
            left = (_WIDTH - maxWidth) / 2 - 2;

            //set position
            Console.SetCursorPosition(left, top);

            //loop for menu building
            if (title != "")
            {
                Console.Write(title);
                top++;
                Console.SetCursorPosition(left, top);
            }

            for (int i = 0; i < maxWidth; i++)
            {
                Console.Write("_");
            }

            foreach (string item in menu)
            {
                top += 2;
                Console.SetCursorPosition(left, top);
                Console.Write(item);
            }
        }

        /// <summary>
        /// Display exit message
        /// </summary>
        public void DisplayExitMessage()
        {
            ResetConsole();
            Console.SetCursorPosition(_WIDTH / 2 - 6, _HEIGHT / 2);
            Console.Write("Goodbye...");
        }

        /// <summary>
        /// Display all records
        /// </summary>
        /// <param name="records">List of TVShow</param>
        public void DisplayAllRecords(List<TVShow> records)
        {
            //header
            HelperHeader("All Records:");
            Console.WriteLine("\t\tTitle                               Network         Rating  Running\n");

            //create lines
            foreach (TVShow item in records)
            {
                Console.Write("\t\t>");
                Console.Write(HelperBuildString(item.Name, 35));
                Console.Write(HelperBuildString(item.Network.ToString(), 16));
                Console.Write(HelperBuildString(String.Format("{0:f1}", item.Rating), 8));
                Console.Write(HelperBuildString(item.Running ? "Yes" : "No", 7));
                Console.Write("\n");
            }

            //pause
            HelperPause();
        }

        /// <summary>
        /// Draw records with selected record in red
        /// </summary>
        /// <param name="records">List of records</param>
        /// <param name="selection">Index of selected item</param>
        /// <param name="title">Title of menu</param>
        public void SelectRecord(List<TVShow> records, int selection, string title)
        {
            //header
            Console.ForegroundColor = ConsoleColor.White;
            HelperHeader(title + ":");
            Console.WriteLine("\t\tTitle                               Network         Rating  Running\n");

            //create lines
            foreach (TVShow item in records)
            {
                if (records.IndexOf(item) == selection)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                Console.Write("\t\t>");
                Console.Write(HelperBuildString(item.Name, 35));
                Console.Write(HelperBuildString(item.Network.ToString(), 16));
                Console.Write(HelperBuildString(String.Format("{0:f1}", item.Rating), 8));
                Console.Write(HelperBuildString(item.Running ? "Yes" : "No", 7));
                Console.Write("\n");
            }
        }

        /// <summary>
        /// Display error message
        /// </summary>
        /// <param name="message">Message</param>
        public void DisplayError(string message)
        {
            int len = message.Length;
            int left = (_WIDTH - len) / 2;

            ResetConsole();

            //display error
            Console.SetCursorPosition(left, _HEIGHT / 2);
            Console.WriteLine(message);

            HelperPause();
        }
        #endregion

        #region #ADD/UPDATE RECORDS

        /// <summary>
        /// Display for Add or Update a Record
        /// </summary>
        /// <param name="title">Title of menu</param>
        /// <returns>TVShow Object</returns>
        public TVShow DisplayAddUpdateRecord(string title)
        {
            ResetConsole();
            Console.CursorVisible = true;

            //display header
            HelperHeader(title);

            //get information
            string name = DisplayAddRecordName();
            double rating = DisplayAddRecordRating();
            bool running = DisplayAddRecordRunning();
            TVShow.TVNetwork network = DisplayAddRecordNetwork();

            return new TVShow(name, running, rating, network);
        }

        /// <summary>
        /// Get name
        /// </summary>
        /// <returns>String</returns>
        private string DisplayAddRecordName()
        {
            Console.Write("\n\t\tTV Show Name: ");            
            return Console.ReadLine();
        }

        /// <summary>
        /// Get rating
        /// </summary>
        /// <returns>Double</returns>
        private double DisplayAddRecordRating()
        {
            double rating = 0.0;
            string response = "";

            Console.Write("\n\t\tRating (out of 10): ");

            //get position for input
            int top = Console.CursorTop;
            int left = Console.CursorLeft;

            response = Console.ReadLine();
            while (!double.TryParse(response, out rating) || rating > 10.0 || rating < 0.0)
            {
                HelperEraseScreen(left, top, response.Length);
                response = Console.ReadLine();
            }

            return rating;
        }

        /// <summary>
        /// Get show status
        /// </summary>
        /// <returns>Boolean</returns>
        private bool DisplayAddRecordRunning()
        {
            bool rated = false;
            bool rating = false;

            Console.Write("\n\t\tRunning? (yes / no): ");

            //get position for input
            int top = Console.CursorTop;
            int left = Console.CursorLeft;

            while (!rated)
            {
                string response = Console.ReadLine().ToLower();

                if (response == "y" || response == "yes" || response == "true")
                {
                    rated = true;
                    rating = true;
                }
                else if (response == "n" || response == "no" || response == "false")
                {
                    rated = true;
                    rating = false;
                }
                else
                {
                    HelperEraseScreen(left, top, response.Length);
                }
            }

            return rating;
        }

        /// <summary>
        /// Get show network
        /// </summary>
        /// <returns>Enum TVShow.TVNetwork</returns>
        private TVShow.TVNetwork DisplayAddRecordNetwork()
        {
            TVShow.TVNetwork network;
            Console.Write("\n\t\tNetwork Name: ");

            string response = Console.ReadLine();

            //Choose other if not in list
            try { network = (TVShow.TVNetwork)Enum.Parse(typeof(TVShow.TVNetwork), response, true); }
            catch (Exception) { network = TVShow.TVNetwork.Other; }

            return network;
        }

        #endregion 

        #region HELPERS

        /// <summary>
        /// Create a header
        /// </summary>
        /// <param name="title">Header Title</param>
        private void HelperHeader(string title)
        {
            Console.SetCursorPosition(1, 4);
            Console.WriteLine("\t\t{0}", title);

            string line = "";
            for (int i = 0; i < _WIDTH - 32; i++)
            {
                line += "_";
            }

            Console.WriteLine($"\t\t{line}\n");

        }

        /// <summary>
        /// Press any key to continue
        /// </summary>
        private void HelperPause()
        {
            Console.SetCursorPosition(33, _HEIGHT - 5);
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Erase everything on screen after current position
        /// </summary>
        private void HelperEraseScreen(int left, int top, int length)
        {
            //set cursor to start of input
            Console.SetCursorPosition(left, top);

            //erase all text
            Console.WriteLine(new string(' ', length));

            //set cursor position again
            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Build a string into another string for display purposes
        /// </summary>
        /// <param name="text">Text that will be displayed</param>
        /// <param name="mask">Size of area string will fit into</param>
        private string HelperBuildString(string text, int maskLength)
        {
            int textLen = text.Length;

            if (textLen >= maskLength)
            {
                text = text.Substring(0, maskLength - 4);
                text += @"... ";
            }
            else
            {
                for (int i = 0; i < maskLength - textLen; i++)
                {
                    text += @" ";
                }
            }

            return text;
        }

        /// <summary>
        /// Confirm changes
        /// </summary>
        /// <returns>Boolean</returns>
        public bool ConfirmChanges()
        {
            Console.SetCursorPosition(15, _HEIGHT - 5);
            Console.Write("Are you sure you wish to complete this action? (YES/NO) : ");
            Console.CursorVisible = true;

            //handle input
            string confirm = Console.ReadLine().ToLower();

            if (confirm == "yes" || confirm == "y")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Reset cursor and clear console
        /// </summary>
        public void ResetConsole()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetCursorPosition(1, 1);
            Console.ResetColor();
        }


        #endregion
    }
}
