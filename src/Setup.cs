/// <summary>The methods for managing the shortcut menu option: install and uninstall.</summary>
/// <version>0.0.1.16</version>

using Microsoft.Win32;

namespace cvmd2html
{
  static partial class Program
  {
    private static readonly RegistryKey HKCU = Registry.CurrentUser;
    const string SHELL_SUBKEY = @"SOFTWARE\Classes\SystemFileAssociations\.md\shell";
    const string VERB = "cthtml";
    private static readonly string VERB_SUBKEY = string.Format(@"{0}\{1}", SHELL_SUBKEY, VERB);
    private static readonly string VERB_KEY = string.Format(@"{0}\{1}", HKCU, VERB_SUBKEY);
    private static readonly string ICON_VALUENAME = "Icon";

    /// <summary>Configure the shortcut menu in the registry.</summary>
    static void SetShortcut()
    {
      string COMMAND_KEY = VERB_KEY + @"\command";
      var command = string.Format(@"""{0}"" /Markdown:""%1""", AssemblyLocation);
      Registry.SetValue(COMMAND_KEY, null, command);
      Registry.SetValue(VERB_KEY, null, "Convert to &HTML");
    }

    /// <summary>Add an icon to the shortcut menu in the registry.</summary>
    static void AddIcon()
    {
      Registry.SetValue(VERB_KEY, ICON_VALUENAME, AssemblyLocation);
    }

    /// <summary>Remove the shortcut icon menu.</summary>
    static void RemoveIcon()
    {
      RegistryKey VERB_KEY_OBJ = HKCU.CreateSubKey(VERB_SUBKEY);
      if (VERB_KEY_OBJ != null)
      {
        VERB_KEY_OBJ.DeleteValue(ICON_VALUENAME, false);
        VERB_KEY_OBJ.Close();
      }
    }

    /// <summary>Remove the shortcut menu by removing the verb key and subkeys.</summary>
    static void UnsetShortcut()
    {
      RegistryKey SHELL_KEY_OBJ = HKCU.CreateSubKey(SHELL_SUBKEY);
      if (SHELL_KEY_OBJ != null)
      {
        SHELL_KEY_OBJ.DeleteSubKeyTree(VERB, false);
        SHELL_KEY_OBJ.Close();
      }
    }
  }
}