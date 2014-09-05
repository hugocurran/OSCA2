using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.ManagementConsole;

namespace OSCASnapin
{
    public class AdminProperties : FormView
    {
        private SelectionControl selectionControl = null;

        public AdminProperties()
        {
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="status"></param>
        protected override void OnInitialize(AsyncStatus status)
        {
            // Call the parent method.
            base.OnInitialize(status);

            // Get a typed reference to the hosted control
            // that is set up by the form view description.
            selectionControl = (SelectionControl)this.Control;
            Refresh();
        }

        /// <summary>
        /// Load the data.
        /// </summary>
        protected void Refresh()
        {
            // Populate the lists with fictitious data.
            string[][] users = { new string[] {"Karen", "February 14th"},
                                        new string[] {"Sue", "May 5th"},
                                        new string[] {"Tina", "April 15th"},
                                        new string[] {"Lisa", "March 27th"},
                                        new string[] {"Tom", "December 25th"},
                                        new string[] {"John", "January 1st"},
                                        new string[] {"Harry", "October 31st"},
                                        new string[] {"Bob", "July 4th"}
                                    };

            selectionControl.RefreshData(users);            
        }
        /// <summary>
        /// Handle the selected action.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="status"></param>
        protected override void OnSelectionAction(Microsoft.ManagementConsole.Action action, AsyncStatus status)
        {
            switch ((string)action.Tag)
            {
                case "ShowSelection":
                    {
                        selectionControl.ShowSelection();
                        break;
                    }
            }
        }
    }
}
