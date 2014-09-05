using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ManagementConsole;

namespace OSCASnapin.ConfigManager
{
    public class CAPropertyPage : PropertyPage
    {
		private CAPropertiesControl caPropertiesControl = null;
        private CANode caNode = null;

		/// <summary>
		/// Constructor for the page.
		/// </summary>
        public CAPropertyPage(CANode parentNode)
		{
            // save scope node parent
            this.caNode = parentNode;

            // setup property page container stuff
            this.Title = "CA Properties";

            // setup contained control and hand it a reference to its parent (This propertypage)
            caPropertiesControl = new CAPropertiesControl(this);
            this.Control = caPropertiesControl;
        }

		/// <summary>
		/// Initialize notification for the page. Default implementation is empty.
		/// </summary>
		protected override void OnInitialize()
		{
            base.OnInitialize();

			// populate the contained control 
            caPropertiesControl.RefreshData(caNode);
		}

		/// <summary>
		/// Sent to every page in the property sheet to indicate that the user has clicked 
		/// the Apply button and wants all changes to take effect.
		/// </summary>
		protected override bool OnApply()
		{
			// does the control say the values are valid?
			if (this.Dirty)
			{
				if (caPropertiesControl.CanApplyChanges())
				{
					// save changes
                    caPropertiesControl.UpdateData(caNode);
				}
				else
				{
					// something invalid was entered 
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Sent to every page in the property sheet to indicate that the user has clicked the OK 
		/// or Close button and wants all changes to take effect.
		/// </summary>
		protected override bool OnOK()
		{
			return this.OnApply();
		}

		/// <summary>
		/// Indicates that the user wants to cancel the property sheet.
		/// Default implementation allows cancel operation.
		/// </summary>
		protected override bool QueryCancel()
		{
			return true;
		}

		/// <summary>
		/// Indicates that the user has canceled and the property sheet is about to be destroyed.
		/// All changes made since the last PSN_APPLY notification are canceled
		/// </summary>
		protected override void OnCancel()
		{
            caPropertiesControl.RefreshData(caNode);
		}
		
		/// <summary>
		/// Notifies a page that the property sheet is getting destoyed. 
		/// Use this notification message as an opportunity to perform cleanup operations.
		/// </summary>
		protected override void OnDestroy()
		{
		}

	}
}
