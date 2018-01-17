using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IntroToWebApps.WebForms
{
    public partial class Default : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // clear the data each time the page loads
            this.outputLiteral.Text = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void processDataButton_Click(object sender, EventArgs e)
        {
            this.outputLiteral.Text = firstNameTextBox.Text + " " + lastNameTextBox.Text;
        }
    }
}