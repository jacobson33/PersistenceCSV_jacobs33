using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace PersistenceCSV_jacobs33
{
    public class Controller
    {
        #region ENUMS
        public enum MenuItems
        {
            None,
            DisplayAllRecords,
            AddRecord,
            DeleteRecord,
            UpdateRecord,
            Exit
        }
        #endregion

        #region FIELDS
        private ConsoleView _view;
        private bool _inMenu;
        private MenuItems _status;
        #endregion

        #region PROPERTIES
        public ConsoleView View
        {
            get { return _view; }
        }

        public MenuItems Status
        {
            get { return _status; }
        }
        #endregion

        #region CONSTRUCTOR
        public Controller()
        {
            InitializeController();

            LoadData();

            MainMenu();
        }
        #endregion

        #region METHODS
        private void InitializeController()
        {
            _view = new ConsoleView();
            _inMenu = true;
        }

        private void LoadData()
        {

        }

        private void MainMenu()
        {
            ConsoleKey selection;

            while (_inMenu)
            {
                _view.DisplayMenu("Main Menu");
                _status = MenuItems.None;
                selection = GetMenuSelection();

                switch (selection)
                {
                    case ConsoleKey.D1:
                        _status = MenuItems.DisplayAllRecords;
                        DisplayAllRecords();
                        break;
                    case ConsoleKey.D2:
                        _status = MenuItems.AddRecord;
                        break;
                    case ConsoleKey.D3:
                        _status = MenuItems.DeleteRecord;
                        break;
                    case ConsoleKey.D4:
                        _status = MenuItems.UpdateRecord;
                        break;
                    case ConsoleKey.D5:
                        _status = MenuItems.Exit;
                        ExitProgram();
                        break;
                    default:
                        break;
                }
            }

            ExitProgram();
        }

        private ConsoleKey GetMenuSelection()
        {
            return Console.ReadKey(true).Key;
        }

        /// <summary>
        /// Exit the program
        /// </summary>
        private void ExitProgram()
        {
            _view.DisplayExitMessage();
            Thread.Sleep(2000);
            Environment.Exit(1);
        }

        private void DisplayAllRecords()
        {
            _view.ResetConsole();
            Console.Clear();
            ReadFile();
        }
        #endregion

        #region FILE HANDLING
        private void ReadFile()
        {
            StreamReader sr = new StreamReader("Data\\data.txt");

            using (sr)
            {
                while (!sr.EndOfStream)
                {
                    string text = sr.ReadLine();
                    Console.WriteLine(text);
                }
                
            }

            Console.ReadKey(true);
        }
        #endregion
    }
}
