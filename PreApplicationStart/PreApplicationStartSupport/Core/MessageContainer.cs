using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PreApplicationStartSupport.Core
{
	public class MessageContainer
	{
		List <string> messages;

		public static MessageContainer Instance { get; private set; }

		public ReadOnlyCollection<string> Messages
		{
			get { return messages.AsReadOnly (); }
		}

		static MessageContainer ()
		{
			Instance = new MessageContainer ();
		}

		MessageContainer ()
		{ 
			messages = new List<string> ();
		}

		public void Clear ()
		{
			messages.Clear ();
		}

		public void Add (string message, uint indent = 0, bool timeStamp = true)
		{
			var sb = new StringBuilder ();
			if (indent > 0)
				sb.Append ('\t', (int)indent);
			if (timeStamp)
				sb.AppendFormat ("[{0}] ", DateTime.Now);
			sb.Append (message);

			messages.Add (sb.ToString ());
		}

		public void Add (string format, params object [] args)
		{
			Add (0, true, format, args);
		}

		public void Add (uint indent, string format, params object [] args)
		{
			Add (indent, true, format, args);
		}

		public void Add (uint indent, bool timeStamp, string format, params object [] args)
		{
			if (args == null || args.Length == 0) {
				Add (format, indent);
				return;
			}

			Add (String.Format (format, args), indent, timeStamp);
		}
	}
}
