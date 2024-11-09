/// <version>0.0.1.27</version>

using System;
using System.Reflection;

namespace cvmd2html
{
  static partial class Program
  {
    [STAThread]
    private static void Main()
    {
      /** The application execution. */
      if (!string.IsNullOrEmpty(MarkdownPathParam))
      {
        ConvertFrom(MarkdownPathParam);
        Quit(0);
      }

      /** Configuration and settings. */
      if (SetConfigParam || UnsetConfigParam)
      {
        if (SetConfigParam)
        {
          SetShortcut();
          if ((bool)NoIconConfigParam)
          {
            RemoveIcon();
          }
          else
          {
            AddIcon();
          }
        }
        else if (UnsetConfigParam)
        {
          UnsetShortcut();
        }
        Quit(0);
      }

      Quit(1);
    }

    /// <summary>The path to the application.</summary>
    static readonly string AssemblyLocation = Assembly.GetExecutingAssembly().Location;

    /// <summary>Clean up and quit.</summary>
    /// <param name="exitCode">The exit code.</param>
    static void Quit(int exitCode)
    {
      GC.Collect();
      Environment.Exit(exitCode);
    }
  }
}