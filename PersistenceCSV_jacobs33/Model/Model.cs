using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceCSV_jacobs33
{
    /// <summary>
    /// TVShow class that will contain ratings, network information and if the show is still running
    /// </summary>
    public class TVShow
    {
        #region ENUMS
        public enum TVNetwork
        {
            Other, HBO, CBS, PBS, BBC, ITV, CartoonNetwork, NationalGeographic, Nickelodeon, Fox, ABC, AMC
        }
        #endregion

        #region FIELDS
        private TVNetwork _network;
        private bool _running;
        private double _rating;
        private string _name;
        #endregion

        #region PROPERTIES
        public TVNetwork Network
        {
            get { return _network; }
            set { _network = value; }
        }
        public bool Running
        {
            get { return _running; }
            set { _running = value; }
        }
        public double Rating
        {
            get { return _rating; }
            set { _rating = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Constructor
        /// </summary>
        public TVShow()
        {

        }
        /// <summary>
        /// Overload constructor
        /// </summary>
        /// <param name="name">Name of show</param>
        /// <param name="running">Is show running?</param>
        /// <param name="rating">Rating out of 10.0</param>
        /// <param name="network">Network show aired on</param>
        public TVShow(string name, bool running, double rating, TVNetwork network)
        {
            _name = name;
            _running = running;
            _rating = rating;
            _network = network;
        }
        #endregion

        #region METHODS
        #endregion
    }
}
