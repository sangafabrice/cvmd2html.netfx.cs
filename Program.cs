/// <version>0.0.1.26</version>

using System;
using System.Reflection;

namespace cvmd2html
{
  internal static class Program
  {
    [STAThread]
    static void Main()
    {
      /** The application execution. */
      if (!string.IsNullOrEmpty(Parameters.Markdown))
      {
        Converter.ConvertFrom(Parameters.Markdown);
        Quit(0);
      }

      /** Configuration and settings. */
      if (Parameters.Set || Parameters.Unset)
      {
        if (Parameters.Set)
        {
          Setup.Set();
          if ((bool)Parameters.NoIcon)
          {
            Setup.RemoveIcon();
          }
          else
          {
            Setup.AddIcon();
          }
        }
        else if (Parameters.Unset)
        {
          Setup.Unset();
        }
        Quit(0);
      }

      Quit(1);
    }

    /// <summary>The path to the application.</summary>
    internal static readonly string Path = Assembly.GetExecutingAssembly().Location;

    /// <summary>Clean up and quit.</summary>
    /// <param name="exitCode">The exit code.</param>
    internal static void Quit(int exitCode)
    {
      GC.Collect();
      Environment.Exit(exitCode);
    }
  }
}