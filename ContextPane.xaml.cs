using Quokka;
using Quokka.ListItems;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PluginTypedText
{

  /// <summary>
  /// The context pane for TypedTextItems.
  /// </summary>
  public partial class ContextPane : ItemContextPane
  {

    private readonly ListItem? Item;

    /// <summary>
    /// Grabs details about the selected item
    /// </summary>
    public ContextPane()
    {
      InitializeComponent();

      Item = ((SearchWindow)Application.Current.MainWindow).SelectedItem!;

      DetailsImage.Source = Item.Icon;
      TextTyped.Text = Item.Name;
    }

    /// <summary>
    /// <inheritdoc/><br />
    /// Up and down keys select list items and the enter key executes the item's action
    /// </summary>
    /// <param name="sender"><inheritdoc/></param>
    /// <param name="e"><inheritdoc/></param>
    protected override void PageKeyDown(object sender, KeyEventArgs e)
    {
      if (e != null)
      {
        ButtonsListView.Focus();
        switch (e.Key)
        {
          case Key.Enter:
            if (ButtonsListView.SelectedIndex == -1)
            {
              ButtonsListView.SelectedIndex = 0;
            }
            Grid CurrentItem = (Grid)ButtonsListView.SelectedItem;
            Button CurrentButton = (Button)((Grid)CurrentItem.Children[1]).Children[0];
            CurrentButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            break;
          case Key.Down:
            if (ButtonsListView.SelectedIndex == -1)
            {
              ButtonsListView.SelectedIndex = 1;
            }
            else if (ButtonsListView.SelectedIndex == ButtonsListView.Items.Count - 1)
            {
              ButtonsListView.SelectedIndex = 0;
            }
            else
            {
              ButtonsListView.SelectedIndex++;
            }
            ButtonsListView.ScrollIntoView(ButtonsListView.SelectedItem);
            break;
          case Key.Up:
            if (ButtonsListView.SelectedIndex is -1 or 0)
            {
              ButtonsListView.SelectedIndex = ButtonsListView.Items.Count - 1;
            }
            else
            {
              ButtonsListView.SelectedIndex--;
            }
            ButtonsListView.ScrollIntoView(ButtonsListView.SelectedItem);
            break;
          case var value when value == (Key)Application.Current.Resources["ContextPaneKey"]:
            ReturnToSearch();
            break;
          default:
            return;
        }
        e.Handled = true;
      }
    }

    private void CopyText(object sender, RoutedEventArgs e)
    {
      Item!.Execute();
    }

  }
}
