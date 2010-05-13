using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreApplicationStartSupport.Core
{
	public interface ISetupPlugin : IPlugin
	{
		bool PerformSetupSteps (string rootDir);
	}
}
