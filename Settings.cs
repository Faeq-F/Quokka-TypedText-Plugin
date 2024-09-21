/// <summary>
/// All plugin specific settings
/// </summary>
public class PluginSettings {
  /// <summary>
  /// The Special Command to show 2 TypedTextItems. Defaults to "2TypedText"
  /// </summary>
  public string Show2ItemsSpecialCommand { get; set; } = "2TypedText";
  /// <summary>
  /// The Special Command to show 3 TypedTextItems. Defaults to "3TypedText"
  /// </summary>
  public string Show3ItemsSpecialCommand { get; set; } = "3TypedText";
  /// <summary>
  /// The Special Command to show 4 TypedTextItems. Defaults to "4TypedText"
  /// </summary>
  public string Show4ItemsSpecialCommand { get; set; } = "4TypedText";
  /// <summary>
  /// The command signifier used for this plugin (defaults to "TT ")<br />
  /// </summary>
  public string ItemSignifier { get; set; } = "TT ";
  /// <summary>
  /// If this text is included in the query, the description of the TypedTextItem produced will be different. Defaults to " --Desc"
  /// </summary>
  public string ShowDifferentDescriptionFlag { get; set; } = " --Desc";
  /// <summary>
  ///   The threshold for when to consider the query
  ///   is similar enough to "otherTypedTextItem" for it to be
  ///   displayed (defaults to 5).<br />
  ///   Currently uses the Levenshtein distance; the larger 
  ///   the number, the bigger the difference.
  /// </summary>
  public int FuzzySearchThreshold { get; set; } = 5;
}

