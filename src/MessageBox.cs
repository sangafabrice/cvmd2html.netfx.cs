/// <summary>The markdown conversion message box.</summary>
/// <version>0.0.1.16</version>

using System;
using System.Windows;

namespace cvmd2html
{
  internal static class MessageBox
  {
    /// <summary>The warning message type.</summary>
    internal const MessageBoxImage WARNING = MessageBoxImage.Exclamation;

    /// <summary>The help message type.</summary>
    internal const MessageBoxImage HELP = MessageBoxImage.None;

    /// <summary>Show a warning message or an error message box.</summary>
    /// <param name="message">The message text.</param>
    /// <param name="messageType">The message box type.</param>
    internal static void Show(string message, MessageBoxImage messageType = MessageBoxImage.Error)
    {
      if (Array.IndexOf(EXPECTED_MESSAGETYPE, messageType) < 0)
      {
        messageType = CRITICAL;
      }
      // The error message box shows the OK button alone.
      // The warning message box shows the alternative Yes or No buttons.
      MessageBoxResult msgBoxResult = System.Windows.MessageBox.Show(message, MESSAGE_BOX_TITLE, messageType == CRITICAL || messageType == HELP ? MessageBoxButton.OK:MessageBoxButton.YesNo, messageType);
      Predicate<MessageBoxResult> predicate = (MessageBoxResult result) => result == msgBoxResult;
      if (Array.Exists(EXPECTED_DIALOGRESULT, predicate))
      {
        Program.Quit(1);
      }
    }

    /// <summary>The uniform message box title.</summary>
    const string MESSAGE_BOX_TITLE = "Convert to HTML";

    /// <summary>The list of Dialog Results that lead to quit the application.</summary>
    static readonly MessageBoxResult[] EXPECTED_DIALOGRESULT = { MessageBoxResult.OK, MessageBoxResult.No };

    /// <summary>The list of expected message types.</summary>
    static readonly MessageBoxImage[] EXPECTED_MESSAGETYPE = { CRITICAL, WARNING, HELP };

    /// <summary>The error message type.</summary>
    const MessageBoxImage CRITICAL = MessageBoxImage.Error;
  }
}