namespace Skype2BlinkGui.Models
{
    using System.ComponentModel;
    class SkypeState : INotifyPropertyChanged
    {
        /// <summary>
        /// Create instance of SkypState class
        /// </summary>
        public SkypeState()
        {
            new Test();
        }

        private bool _Running = false;
        /// <summary>
        /// Skype process executed
        /// </summary>
        public bool Running
        {
            get { return _Running; }
            set {
                _Running = value;
                OnPropertyChanged("Running");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
