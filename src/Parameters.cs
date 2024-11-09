/// <summary>The parsed parameters.</summary>
/// <version>0.0.1.7</version>

using System;
using System.Dynamic;
using System.Text;

namespace cvmd2html
{
  static partial class Program
  {
    /// <summary>The parameter object.</summary>
    private static readonly dynamic _param = ParseCommandLine(Environment.GetCommandLineArgs());

    /// <summary>The input markdown path.</summary>
    static readonly string MarkdownPathParam = _param.Markdown;

    /// <summary>Specify to configure the shortcut in the registry.</summary>
    static readonly bool SetConfigParam = _param.Set;

    /// <summary>Specify to remove the shortcut menu.</summary>
    static readonly bool UnsetConfigParam = _param.Unset;

    /// <summary>Specify to configure the shortcut without the icon.</summary>
    static readonly Nullable<bool> NoIconConfigParam = _param.NoIcon;

    private static dynamic ParseCommandLine(string[] args)
    {
      dynamic paramExpando = new ExpandoObject();
      paramExpando.Markdown = null;
      paramExpando.Set = false;
      paramExpando.NoIcon = null;
      paramExpando.Unset = false;
      if (args.Length == 2)
      {
        string arg = args[1];
        string[] paramNameValue = arg.Split(new char[]{':'}, 2);
        string paramMarkdown;
        if (paramNameValue.Length == 2 && string.Compare(paramNameValue[0], "/Markdown", true) == 0 && (paramMarkdown = paramNameValue[1]).Length > 0)
        {
          paramExpando.Markdown = paramMarkdown;
          return paramExpando;
        }
        switch (arg.ToLower())
        {
          case "/set":
            paramExpando.Set = true;
            paramExpando.NoIcon = false;
            return paramExpando;
          case "/set:noicon":
            paramExpando.Set = true;
            paramExpando.NoIcon = true;
            return paramExpando;
          case "/unset":
            paramExpando.Unset = true;
            return paramExpando;
          default:
            paramExpando.Markdown = arg;
            return paramExpando;
        }
      }
      else if (args.Length == 1)
      {
        paramExpando.Set = true;
        paramExpando.NoIcon = false;
        return paramExpando;
      }
      ShowHelp();
      return null;
    }

    private static void ShowHelp()
    {
      var helpTextBuilder = new StringBuilder();
      helpTextBuilder.AppendLine("The MarkdownToHtml shortcut launcher.");
      helpTextBuilder.AppendLine("It starts the shortcut menu target script in a hidden window.");
      helpTextBuilder.AppendLine();
      helpTextBuilder.AppendLine("Syntax:");
      helpTextBuilder.AppendLine("  Convert-MarkdownToHtml /Markdown:<markdown file path>");
      helpTextBuilder.AppendLine("  Convert-MarkdownToHtml [/Set[:NoIcon]]");
      helpTextBuilder.AppendLine("  Convert-MarkdownToHtml /Unset");
      helpTextBuilder.AppendLine("  Convert-MarkdownToHtml /Help");
      helpTextBuilder.AppendLine();
      helpTextBuilder.AppendLine("<markdown file path>  The selected markdown's file path.");
      helpTextBuilder.AppendLine("                 Set  Configure the shortcut menu in the registry.");
      helpTextBuilder.AppendLine("              NoIcon  Specifies that the icon is not configured.");
      helpTextBuilder.AppendLine("               Unset  Removes the shortcut menu.");
      helpTextBuilder.AppendLine("                Help  Show the help doc.");
      Popup(helpTextBuilder.ToString(), HELP);
    }
  }
}