using System;

namespace PreApplicationStartSupport.Core
{
	public interface IPlugin
	{
		string Name { get; }
		Version Version { get; }

		void Initialize ();
	}
}
