
using Newtonsoft.Json;
using Quokka.ListItems;
using Quokka.PluginArch;
using System.IO;
using System.Windows;
using WinCopies.Util;

namespace Plugin_TypedText {
  /// <summary>
  /// The TypedText Plugin
  /// </summary>
  public partial class TypedText : Plugin {

    private static PluginSettings pluginSettings = new();
    internal static PluginSettings PluginSettings { get => pluginSettings; set => pluginSettings = value; }

    /// <summary>
    /// Loads plugin settings
    /// </summary>
    public TypedText() {
      string fileName = Environment.CurrentDirectory + "\\PlugBoard\\Plugin_TypedText\\Plugin\\settings.json";
      PluginSettings = JsonConvert.DeserializeObject<PluginSettings>(File.ReadAllText(fileName))!;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override string PluggerName { get; set; } = "TypedText";

    private List<ListItem> ProduceItems(string query, int number) {
      bool differentDesc = false;
      if (query.Contains(PluginSettings.ShowDifferentDescriptionFlag)) {
        query = query.Replace(PluginSettings.ShowDifferentDescriptionFlag, "");
        differentDesc = true;
      }
      //
      List<ListItem> items = new();
      for (int i = 0; i < number; i++) {
        items.Add(new TypedTextItem(query, differentDesc));
      }
      //
      if (
            "otherTypedTextItem".Contains(query, StringComparison.OrdinalIgnoreCase)
            || ( FuzzySearch.LD("otherTypedTextItem", query) < PluginSettings.FuzzySearchThreshold )
      ) {
        items.Add(new OtherTypedTextItem(query));
      }
      return items;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="query"><inheritdoc/></param>
    /// <returns>
    /// A single TypedText item that shows you the query you typed in
    /// </returns>
    public override List<ListItem> OnQueryChange(string query) {
      return ProduceItems(query, 1);
    }

    /// <summary>
    /// <inheritdoc/><br />
    /// Displays a message-box to the user, telling them that Quokka is about to shutdown
    /// </summary>
    public override void OnAppShutdown() {
      System.Windows.MessageBox.Show(
        "Quokka is about to shutdown",
        "Message from the TypedText plugin",
        MessageBoxButton.OK,
        MessageBoxImage.Information
      );
    }

    /// <summary>
    /// <inheritdoc/><br />
    /// Displays a message-box to the user, telling them that Quokka is initializing
    /// </summary>
    public override void OnAppStartup() {
      System.Windows.MessageBox.Show(
        "Quokka is Initializing",
        "Message from the TypedText plugin",
        MessageBoxButton.OK,
        MessageBoxImage.Information
      );
    }

    /// <summary>
    /// <inheritdoc/><br />
    /// Displays a message-box to the user, telling them that they have launched the Search Window
    /// </summary>
    public override void OnSearchWindowStartup() {
      System.Windows.MessageBox.Show(
        "The Search Window has been launched",
        "Message from the TypedText plugin",
        MessageBoxButton.OK,
        MessageBoxImage.Information
      );
    }

    /// <summary>
    /// Provides the correct number of TypedTextItems for the SpecialCommand given
    /// </summary>
    /// <param name="command"><inheritdoc/></param>
    /// <returns>The respective amount of TypedTextItems</returns>
    public override List<ListItem> OnSpecialCommand(string command) {
      switch (command) {
        case var value when value == PluginSettings.Show2ItemsSpecialCommand: {
          return ProduceItems(command, 2);
        }
        case var value when value == PluginSettings.Show3ItemsSpecialCommand: {
          return ProduceItems(command, 3);
        }
        default: {
          return ProduceItems(command, 4);
        }
      }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>All of the SpecialCommands in the plugin settings</returns>
    public override List<string> SpecialCommands() {
      return new List<string>() {
      PluginSettings.Show2ItemsSpecialCommand, PluginSettings.Show3ItemsSpecialCommand, PluginSettings.Show4ItemsSpecialCommand };
    }

    /// <summary>
    /// <inheritdoc/><br />
    /// Provides a single TypedTextItem, like OnQueryChange<br />
    /// The signifier is only used to remove items from other plugins from the results list
    /// </summary>
    /// <param name="command"><inheritdoc/></param>
    /// <returns>A single TypedTextItem, like OnQueryChange</returns>
    public override List<ListItem> OnSignifier(string command) {
      return ProduceItems(command.Substring(PluginSettings.ItemSignifier.Length), 1);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>ItemSignifier in the plugin settings</returns>
    public override List<string> CommandSignifiers() { return new List<string>() { PluginSettings.ItemSignifier }; }
  }
}