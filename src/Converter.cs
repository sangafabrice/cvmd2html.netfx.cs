/// <summary>The method to convert from markdown to html.</summary>
/// <version>0.0.1.51</version>

using System;
using System.IO;
using Markdig;

namespace cvmd2html
{
  static partial class Program
  {
    /// <summary>Convert the content of the markdown file and write it to an html file.</summary>
    /// <param name="markdownPath">The input markdown file path.</param>
    static void ConvertFrom(string markdownPath)
    {
      // Validate the input markdown path string.
      if (string.Compare(Path.GetExtension(markdownPath), ".md", true) != 0)
      {
        Popup(string.Format(@"""{0}"" is not a markdown (.md) file.", markdownPath));
      }
      SetHtmlContent(GetHtmlPath(markdownPath), ConvertToHtml(GetContent(markdownPath)));
    }

    /// <summary>Convert a markdown content to an html document.</summary>
    /// <param name="markdownContent">The content to convert.</param>
    /// <returns>The output html document content.</returns>
    private static string ConvertToHtml(string markdownContent)
    {
      return Markdown.ToHtml(markdownContent);
    }

    /// <summary>Returns the output path when it is unique without prompts or when the user
    /// accepts to overwrite an existing HTML file. Otherwise, it exits the script.</summary>
    /// <param name="markdownPath">The input markdown file path.</param>
    /// <returns>The output html document content.</returns>
    private static string GetHtmlPath(string markdownPath)
    {
      string htmlPath = Path.ChangeExtension(markdownPath, ".html");
      if (File.Exists(htmlPath))
      {
        Popup(string.Format(@"The file ""{0}"" already exists." + "\n\nDo you want to overwrite it?", htmlPath), WARNING);
      }
      else if (Directory.Exists(htmlPath))
      {
        Popup(string.Format(@"""{0}"" cannot be overwritten because it is a directory.", htmlPath));
      }
      return htmlPath;
    }

    /// <summary>Get the content of a file.</summary>
    /// <param name="filePath">The path that is read.</param>
    /// <returns>The content of the file.</returns>
    private static string GetContent(string filePath)
    {
      try
      {
        return File.ReadAllText(filePath);
      }
      catch (UnauthorizedAccessException error)
      {
        if (!Directory.Exists(filePath))
        {
          Popup(error.Message);
        }
        Popup(string.Format("Access to the path '{0}' is denied.", filePath));
      }
      catch (Exception error)
      {
        Popup(error.Message);
      }
      return null;
    }

    /// <summary>Write the html text to the output HTML file.</summary>
    /// <remarks>It notifies the user when the operation did not complete with success.</remarks>
    /// <param name="htmlPath">The output html path.</param>
    /// <param name="htmlContent">The content of the html file.</param>
    /// <returns>The content of the file.</returns>
    private static void SetHtmlContent(string htmlPath, string htmlContent)
    {
      try
      {
        File.WriteAllText(htmlPath, htmlContent);
      }
      catch (Exception error)
      {
        Popup(error.Message);
      }
    }
  }
}