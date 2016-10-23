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
        private string _FILEPATH;
        private List<TVShow> _fileData;
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
        /// <summary>
        /// Controller object
        /// </summary>
        public Controller()
        {
            InitializeController();

            //LoadData();

            MainMenu();
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Instantiate view, set default variables
        /// </summary>
        private void InitializeController()
        {
            _view = new ConsoleView();
            _inMenu = true;
            _FILEPATH = "Data\\data.txt";
        }

        /// <summary>
        /// Instantiate data
        /// </summary>
        private void LoadData()
        {
            _fileData = ReadFile();
        }

        /// <summary>
        /// Main menu
        /// </summary>
        private void MainMenu()
        {
            ConsoleKey selection;
            int index;

            while (_inMenu)
            {                
                _view.DisplayMenu("Main Menu");
                selection = _view.GetKeyPress();
                _status = MenuItems.None;

                switch (selection)
                {
                    case ConsoleKey.D1:
                        _status = MenuItems.DisplayAllRecords;
                        DisplayAllRecords();
                        break;
                    case ConsoleKey.D2:
                        _status = MenuItems.AddRecord;
                        AddRecord();
                        break;
                    case ConsoleKey.D3:
                        _status = MenuItems.DeleteRecord;
                        index = SelectRecord("Choose Record to Delete");
                        DeleteRecord(index);
                        break;
                    case ConsoleKey.D4:
                        _status = MenuItems.UpdateRecord;
                        index = SelectRecord("Choose Record to Update");
                        UpdateRecord(index);
                        break;
                    case ConsoleKey.D5:
                        _status = MenuItems.Exit;
                        _inMenu = false;
                        break;
                    default:
                        break;
                }
            }
            ExitProgram();
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

        /// <summary>
        /// Display all records
        /// </summary>
        private void DisplayAllRecords()
        {
            //reset view
            _view.ResetConsole();

            //update data
            _fileData = ReadFile();

            //display data
            _view.DisplayAllRecords(_fileData);
        }

        /// <summary>
        /// Select a record
        /// </summary>
        /// <param name="title">Title for display</param>
        /// <returns>selected index</returns>
        private int SelectRecord(string title)
        {
            bool selected = false;
            int record = 0;

            //reset view
            _view.ResetConsole();

            //update data
            _fileData = ReadFile();

            //display data with selection option
            while (!selected)
            {
                _view.SelectRecord(_fileData, record, title);

                switch (_view.GetKeyPress())
                {
                    case ConsoleKey.UpArrow:
                        record--;
                        break;
                    case ConsoleKey.DownArrow:
                        record++;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        selected = true;
                        break;
                    default:
                        break;
                }

                //handle out of bounds
                record = record <= 0 ? 0 : record;
                record = record >= _fileData.Count - 1 ? _fileData.Count - 1 : record;
            }

            //return selected index
            return record;
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        /// <param name="index">Index of TVShow in data list</param>
        private void DeleteRecord(int index)
        {
            try
            {
                if (PromptForChange())
                {
                    _fileData.RemoveAt(index);
                    WriteToFile();
                }                
            }
            catch (Exception e)
            {
                _view.DisplayError("No more data to remove.");
            }
        }

        /// <summary>
        /// Update a record
        /// </summary>
        /// <param name="index">Index of TVShow in data list</param>
        private void UpdateRecord(int index)
        {
            //update data
            _fileData = ReadFile();

            //get user input
            TVShow show = _view.DisplayAddUpdateRecord("Update a Record");

            //update properties of selected record
            _fileData[index].Name = show.Name;
            _fileData[index].Running = show.Running;
            _fileData[index].Rating = show.Rating;
            _fileData[index].Network = show.Network;

            //write data
            WriteToFile();
        }

        /// <summary>
        /// Add a record to the data
        /// </summary>
        private void AddRecord()
        {
            //update data
            _fileData = ReadFile();

            //get user input
            TVShow show = _view.DisplayAddUpdateRecord("Add a Record");

            //add data and write
            _fileData.Add(show);
            WriteToFile();            
        }

        /// <summary>
        /// Reset file data and write to file
        /// </summary>
        private void ClearAllRecords()
        {
            _fileData = new List<TVShow>();

            WriteToFile();
        }

        /// <summary>
        /// Prompt user for confirmation
        /// </summary>
        /// <returns>Boolean</returns>
        private bool PromptForChange()
        {
            //display confirm message and get result
            return _view.ConfirmChanges();
        }
        #endregion

        #region FILE HANDLING
        /// <summary>
        /// Read data file
        /// </summary>
        /// <returns>List of TVShow</returns>
        private List<TVShow> ReadFile()
        {
            List<TVShow> data = new List<TVShow>();

            try
            {
                StreamReader sr = new StreamReader(_FILEPATH);

                using (sr)
                {
                    bool corrupt = false;
                    TVShow.TVNetwork network;

                    while (!sr.EndOfStream)
                    {
                        //read line
                        string[] line = sr.ReadLine().Split('|');

                        //continue if array not correct length
                        if (line.Length < 3)
                        {
                            corrupt = true;
                            continue;
                        }

                        string name = line[0].Trim();
                        double rating = Double.Parse(line[1].Trim());
                        bool running = Boolean.Parse(line[2].Trim());

                        try { network = (TVShow.TVNetwork)Enum.Parse(typeof(TVShow.TVNetwork), line[3].Trim(), true); }
                        catch (Exception) { network = TVShow.TVNetwork.Other; }

                        data.Add(new TVShow(name, running, rating, network));
                    }
                    if (corrupt) DataCorrupted(false);
                }
            }
            catch (FileNotFoundException e)
            {
                //create new blank file
                WriteToFile();
            }
            catch (UnauthorizedAccessException e)
            {
                _view.DisplayError(e.Message);
                ExitProgram();
            }
            catch (Exception e)
            {
                _view.DisplayError(e.Message);
                DataCorrupted(true);
            }

            return data;
        }

        /// <summary>
        /// Write fileData to file
        /// </summary>
        private void WriteToFile()
        {
            try
            {
                StreamWriter sw = new StreamWriter(_FILEPATH);

                using (sw)
                {
                    //write to file
                    foreach (TVShow item in _fileData)
                    {
                        sw.WriteLine($"{item.Name}| {String.Format("{0:f1}", item.Rating)}| {item.Running.ToString()}| {item.Network.ToString()}");
                    }
                }
            }
            catch (Exception e)
            {
                _view.DisplayError("Something went wrong.");
                ExitProgram();
            }
            
        }

        /// <summary>
        /// Handle data corruption
        /// </summary>
        /// <param name="erase">Erase the data file</param>
        private void DataCorrupted(bool erase)
        {
            //display error message
            if (erase)
            {
                _view.DisplayError("File data corrupted.");
                ClearAllRecords();
            }
            else
            {
                _view.DisplayError("Some data corrupted. Not all lines displayed.");
            }
        }
        #endregion
    }
}
