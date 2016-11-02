using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skype2BlinkGui.Models;

namespace Skype2BlinkGui.ViewModels
{
    class SkypeStateViewModel
    {
        /// <summary>
        /// Init a new instance of the SkypeStateViewModel class
        /// </summary>
        public SkypeStateViewModel()
        {
            SkypeState = new SkypeState();
        }

        private SkypeState _SkypeState;

        /// <summary>
        /// Gets the SkypeState instance
        /// </summary>
        public SkypeState SkypeState
        {
            get { return _SkypeState; }
            set { _SkypeState = value; }
        }

    }
}
