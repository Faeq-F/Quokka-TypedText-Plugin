
using Newtonsoft.Json;
using Quokka.ListItems;
using Quokka.PluginArch;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace PluginTypedText
{
  /// <summary>
  /// The TypedText Plugin
  /// </summary>
  public partial class TypedText : Plugin
  {

    private static PluginSettings pluginSettings = new();
    internal static PluginSettings PluginSettings { get => pluginSettings; set => pluginSettings = value; }

    /// <summary>
    /// Loads plugin settings
    /// </summary>
    public TypedText()
    {
      string fileName = Environment.CurrentDirectory + "\\PlugBoard\\PluginTypedText\\Plugin\\settings.json";
      PluginSettings = JsonConvert.DeserializeObject<PluginSettings>(File.ReadAllText(fileName))!;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override string PluginName { get; set; } = "TypedText";

    private static Collection<ListItem> ProduceItems(string query, int number)
    {
      Collection<ListItem> items = new();
      if (!pluginSettings.UsePlugin) return items;

      bool differentDesc = false;
      if (query.Contains(PluginSettings.ShowDifferentDescriptionFlag))
      {
        query = query.Replace(PluginSettings.ShowDifferentDescriptionFlag, "");
        differentDesc = true;
      }

      for (int i = 0; i < number; i++)
      {
        items.Add(new TypedTextItem(query, differentDesc));
      }

      FuzzySearch.SearchAll(query, new Collection<string>() { "otherTypedTextItem" }, PluginSettings.FuzzySearchThreshold)
        .ToList().ForEach(x => items.Add(new OtherTypedTextItem(query)));

      return items;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="query"><inheritdoc/></param>
    /// <returns>
    /// If usePlugin is false, an empty collection otherwise a single TypedText item that shows you the query you typed in
    /// </returns>
    public override Collection<ListItem> OnQueryChange(string query)
    {
      query ??= "";
      if (!pluginSettings.UsePlugin) return new Collection<ListItem>();
      return ProduceItems(query, 1);
    }

    /// <summary>
    /// <inheritdoc/><br />
    /// Displays a message-box to the user, telling them that Quokka is about to shutdown (if usePlugin is true)
    /// </summary>
    public override void OnAppShutdown()
    {
      if (pluginSettings.UsePlugin)
        System.Windows.MessageBox.Show(
          "Quokka is about to shutdown",
          "Message from the TypedText plugin",
          MessageBoxButton.OK,
          MessageBoxImage.Information
        );
    }

    /// <summary>
    /// <inheritdoc/><br />
    /// Displays a message-box to the user, telling them that Quokka is initializing (if usePlugin is true)
    /// </summary>
    public override void OnAppStartup()
    {
      if (pluginSettings.UsePlugin)
        System.Windows.MessageBox.Show(
          "Quokka is Initializing",
          "Message from the TypedText plugin",
          MessageBoxButton.OK,
          MessageBoxImage.Information
        );
    }

    /// <summary>
    /// <inheritdoc/><br />
    /// Displays a message-box to the user, telling them that they have launched the Search Window (if usePlugin is true)
    /// </summary>
    public override void OnSearchWindowStartup()
    {
      if (pluginSettings.UsePlugin)
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
    public override Collection<ListItem> OnSpecialCommand(string command)
    {
      command ??= "";
      switch (command)
      {
        case var value when value == PluginSettings.Show2ItemsSpecialCommand:
          {
            return ProduceItems(command, 2);
          }
        case var value when value == PluginSettings.Show3ItemsSpecialCommand:
          {
            return ProduceItems(command, 3);
          }
        default:
          {
            return ProduceItems(command, 4);
          }
      }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>An empty collection if usePlugin is false, otherwise all of the SpecialCommands in the plugin settings</returns>
    public override Collection<string> SpecialCommands()
    {
      if (!pluginSettings.UsePlugin) return new Collection<string>();
      return new Collection<string>() {
      PluginSettings.Show2ItemsSpecialCommand, PluginSettings.Show3ItemsSpecialCommand, PluginSettings.Show4ItemsSpecialCommand };
    }

    /// <summary>
    /// <inheritdoc/><br />
    /// Provides a single TypedTextItem, like OnQueryChange<br />
    /// The signifier is only used to remove items from other plugins from the results list
    /// </summary>
    /// <param name="command"><inheritdoc/></param>
    /// <returns>A single TypedTextItem, like OnQueryChange</returns>
    public override Collection<ListItem> OnSignifier(string command)
    {
      command ??= "";
      return ProduceItems(command.Substring(PluginSettings.ItemSignifier.Length), 1);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>An empty collection if usePlugin is false, otherwise the ItemSignifier in the plugin settings</returns>
    public override Collection<string> CommandSignifiers()
    {
      if (!pluginSettings.UsePlugin) return new Collection<string>();
      return new Collection<string>() { PluginSettings.ItemSignifier };
    }
  }
}
