using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PreApplicationStartSupport.Core;

namespace PreApplicationStart
{
	public partial class _default : System.Web.UI.Page
	{
		protected void Page_Load (object sender, EventArgs e)
		{
			EventLog.DataSource = MessageContainer.Instance.Messages;
			EventLog.DataBind ();
		}
	}
}