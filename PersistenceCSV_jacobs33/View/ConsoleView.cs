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
        /// Reset and clear console
        /// </summary>
        public void ResetConsole()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetCursorPosition(1, 1);
        }

        /// <summary>
        /// Display menu
        /// </summary>
        /// <param name="title">Menu title</param>
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
            Console.WriteLine("Goodbye...");
        }
        #endregion
    }
}
