using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ManagementConsole;
using OSCASnapin.ConfigManager;

namespace OSCASnapin
{
    class AdminNode : ScopeNode
    {
        private Configuration mgrConfig = Configuration.Instance;

        public AdminNode()
        {
            this.ImageIndex = 0;
            this.SelectedImageIndex = 0;
            this.EnabledStandardVerbs = StandardVerbs.Refresh;
            this.DisplayName = "Admin Properties";

            // Create a form view for the AdminNode.
            FormViewDescription fvd = new FormViewDescription();
            fvd.DisplayName = "Users (FormView)";
            fvd.ViewType = typeof(AdminProperties);
            fvd.ControlType = typeof(SelectionControl);

            // Attach the view to the root node.
            this.ViewDescriptions.Add(fvd);
            this.ViewDescriptions.DefaultIndex = 0;

        }

    }


}
