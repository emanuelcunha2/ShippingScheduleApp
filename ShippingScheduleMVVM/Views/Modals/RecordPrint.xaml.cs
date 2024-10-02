using ShippingScheduleMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ShippingScheduleMVVM.Views.Modals
{
    /// <summary>
    /// Interaction logic for RecordPrint.xaml
    /// </summary>
    public partial class RecordPrint : Window
    {

        // Timer for printing
        DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        private void dispatcherTimer_Tick(object? sender, EventArgs e)
        {
            // Update the Label that displays the current second
            PrintBorder();

            // Force the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
            dispatcherTimer.Stop();
        }

        private UIElement CloneElement(UIElement element)
        {
            // Serialize the original border element into XAML code
            string originalXaml = XamlWriter.Save(element);

            // Deserialize the XAML code into a new Border element
            UIElement clonedBorder = (UIElement)XamlReader.Parse(originalXaml);

            return clonedBorder;
        }

        private void PrintBorder()
        {
            PrintDialog printDialog = new PrintDialog();
            
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
                // Create a FlowDocument for printing
                FlowDocument doc = new FlowDocument();
                doc.PageWidth = printDialog.PrintableAreaWidth;
                doc.PageHeight = printDialog.PrintableAreaHeight;
                doc.PagePadding = new Thickness(20);
                doc.ColumnGap = 0;
                doc.ColumnWidth = (doc.PageWidth - doc.ColumnGap - doc.PagePadding.Left - doc.PagePadding.Right);

                Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
                doc.PageWidth = pageSize.Width;
                doc.PageHeight = pageSize.Height;

                // For each page that needs to be printed
                foreach (Border b in bordersToPrint)
                {
                    // Create a new section and add the border to it
                    Section section1 = new Section();
                    section1.Blocks.Add(new BlockUIContainer(b));
                    doc.Blocks.Add(section1);
                }

                // Set the document paginator source
                IDocumentPaginatorSource paginator = doc;
                paginator.DocumentPaginator.PageSize = pageSize;

                // Print the document
                printDialog.PrintDocument(paginator.DocumentPaginator, "Print Job");
                this.Close();
            }
            else
            {
                this.Close();
            }
        }

        List<Part> billShipList = new List<Part>();
        List<Border> bordersToPrint = new List<Border>();
        Record sRecord = new Record();
        int totalPart = 0;
        int currentListPosition = 0;
        public RecordPrint(ObservableCollection<Part> BillShipList, Record SRecord)
        {
            InitializeComponent();

            // Copy the parts from the input list to a new list
            foreach (Part b in BillShipList.ToList())
            {
                billShipList.Add(new Part
                {
                    ShipTo = b.ShipTo,
                    Designation = b.Designation,
                    ShipToName = b.ShipToName,
                    APN = b.APN,
                    CPN = b.CPN,
                    UnloadingPoint = b.UnloadingPoint,
                    ExpectedQuantity = b.ExpectedQuantity,
                    FinalQuantity = b.FinalQuantity,
                    DeliveryNote = b.DeliveryNote,
                    TransportNumber = b.TransportNumber,
                });
            }

            // Copy other input values
            sRecord = SRecord;
            totalPart = billShipList.Count();

            // Set values for UI elements
            ShipmentRecordLabel.Text += " #" + SRecord.Id;
            TodayLabel.Text = sRecord.Carrier;
            TShypment.Text = sRecord.ShipmentType;
            TMode.Text = sRecord.TransportMode;
            TCarrier.Text = sRecord.Carrier;
            TPlate.Text = sRecord.Plate;
            TBID.Text = sRecord.BID;
            TPTA.Text = sRecord.PTA;
            TLeaving.Text = sRecord.Day.Substring(0, 10) + " " + sRecord.Time;
            TDelivery.Text = sRecord.DeliveryDate.ToString();
            TUid.Text = sRecord.UnloadingId;
            TComments.Text = sRecord.Comment;

            while (currentListPosition < totalPart)
            {
                // Removes the 18 last items and adds the next 18 or <
                AddPart();

                // Clone the printingBorder element and add it to the list of borders to print
                Border borderToPrint = new Border();
                borderToPrint = (Border)CloneElement(printingBorder);
                bordersToPrint.Add(borderToPrint);
            }

            // Start the dispatcher timer
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void AddPart()
        {
            // Remove any items marked with "PartToDelete" from the billShipList
            var itemsToRemove = billShipList.Where(item => item.Comment == "PartToDelete").ToList();
            foreach (var item in itemsToRemove)
            {
                billShipList.Remove(item);
            }

            // Remove all existing rows in the BillDataList grid except the title row
            for (int i = BillDataList.Children.Count - 1; i >= 0; i--)
            {
                Border? item = BillDataList.Children[i] as Border;
                string tag = item?.Tag?.ToString() ?? "empty";
                if (tag != "Title")
                {
                    BillDataList.Children.RemoveAt(i);
                }
            }

            int countRows = 1;
            // Iterate through the parts in the billShipList and create UI elements for each part
            foreach (Part b in billShipList)
            {
                if (countRows < 19)
                {
                    // Mark the current part as "PartToDelete" for removal in the next iteration
                    b.Comment = "PartToDelete";

                    // Create UI elements for each part attribute
                    Border newBorderShipTo = new() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(0, 0, 1, 1), Padding = new Thickness(1) };
                    Viewbox newViewboxShipTo = new() { StretchDirection = StretchDirection.DownOnly, MaxHeight = 20 };
                    TextBlock textBlockShipTo = new() { Text = b.ShipTo, FontFamily = new FontFamily("Arial"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };

                    Border newBorderName = new() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(0, 0, 1, 1), Padding = new Thickness(1) };
                    Viewbox newViewboxName = new() { StretchDirection = StretchDirection.DownOnly, MaxHeight = 20 };
                    TextBlock textBlockName = new() { Text = b.ShipToName, FontFamily = new FontFamily("Arial"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };

                    Border newBorderAPN = new() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(0, 0, 1, 1), Padding = new Thickness(1) };
                    Viewbox newViewboxAPN = new() { StretchDirection = StretchDirection.DownOnly, MaxHeight = 20 };
                    TextBlock textBlockAPN = new() { Text = b.APN, FontFamily = new FontFamily("Arial"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };

                    Border newBorderCPN = new() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(0, 0, 1, 1), Padding = new Thickness(1) };
                    Viewbox newViewboxCPN = new() { StretchDirection = StretchDirection.DownOnly, MaxHeight = 20 };
                    TextBlock textBlockCPN = new() { Text = b.CPN, FontFamily = new FontFamily("Arial"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };

                    Border newBorderDescription = new() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(0, 0, 1, 1), Padding = new Thickness(1) };
                    Viewbox newViewboxDescription = new() { StretchDirection = StretchDirection.DownOnly, MaxHeight = 20 }; ;
                    TextBlock textBlockDescription = new() { Text = b.Designation, FontFamily = new FontFamily("Arial"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };

                    Border newBorderUP = new() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(0, 0, 1, 1), Padding = new Thickness(1) };
                    Viewbox newViewboxUP = new() { StretchDirection = StretchDirection.DownOnly, MaxHeight = 20 };
                    TextBlock textBlockUP = new() { Text = b.UnloadingPoint, FontFamily = new FontFamily("Arial"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };

                    Border newBorderPrevQTY = new() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(0, 0, 1, 1), Padding = new Thickness(1) };
                    Viewbox newViewboxPrevQTY = new() { StretchDirection = StretchDirection.DownOnly, MaxHeight = 20 };
                    TextBlock textBlockPrevQTY = new() { Text = b.ExpectedQuantity.ToString(), FontFamily = new FontFamily("Arial"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };

                    Border newBorderQTY = new() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(0, 0, 1, 1), Padding = new Thickness(1) };
                    Viewbox newViewboxQTY = new() { StretchDirection = StretchDirection.DownOnly, MaxHeight = 20 };
                    TextBlock textBlockQTY = new() { Text = b.FinalQuantity.ToString(), FontFamily = new FontFamily("Arial"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };

                    Border newBorderDN = new() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(0, 0, 1, 1), Padding = new Thickness(1) };
                    Viewbox newViewboxDN = new() { StretchDirection = StretchDirection.DownOnly, MaxHeight = 20 };
                    TextBlock textBlockDN = new() { Text = b.DeliveryNote, FontFamily = new FontFamily("Arial"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };

                    Border newBorderTransNum = new() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(0, 0, 1, 1), Padding = new Thickness(1) };
                    Viewbox newViewboxTransNum = new() { StretchDirection = StretchDirection.DownOnly, MaxHeight = 20 };
                    TextBlock textBlockTransNum = new() { Text = b.TransportNumber, FontFamily = new FontFamily("Arial"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 11 };

                    Border newBorderEmpty = new() { BorderBrush = new SolidColorBrush(Colors.Black), BorderThickness = new Thickness(0, 0, 1, 1), Padding = new Thickness(1) };
                    newBorderEmpty.SetValue(Grid.RowProperty, countRows);
                    newBorderEmpty.SetValue(Grid.ColumnProperty, 7);

                    // Set the grid row and column positions for each UI element
                    newBorderShipTo.SetValue(Grid.RowProperty, countRows);
                    newBorderShipTo.SetValue(Grid.ColumnProperty, 0);
                    newViewboxShipTo.Child = textBlockShipTo;
                    newBorderShipTo.Child = newViewboxShipTo;

                    newBorderName.SetValue(Grid.RowProperty, countRows);
                    newBorderName.SetValue(Grid.ColumnProperty, 1);
                    newViewboxName.Child = textBlockName;
                    newBorderName.Child = newViewboxName;

                    newBorderAPN.SetValue(Grid.RowProperty, countRows);
                    newBorderAPN.SetValue(Grid.ColumnProperty, 2);
                    newViewboxAPN.Child = textBlockAPN;
                    newBorderAPN.Child = newViewboxAPN;

                    newBorderCPN.SetValue(Grid.RowProperty, countRows);
                    newBorderCPN.SetValue(Grid.ColumnProperty, 3);
                    newViewboxCPN.Child = textBlockCPN;
                    newBorderCPN.Child = newViewboxCPN;

                    newBorderDescription.SetValue(Grid.RowProperty, countRows);
                    newBorderDescription.SetValue(Grid.ColumnProperty, 4);
                    newViewboxDescription.Child = textBlockDescription;
                    newBorderDescription.Child = newViewboxDescription;

                    newBorderUP.SetValue(Grid.RowProperty, countRows);
                    newBorderUP.SetValue(Grid.ColumnProperty, 5);
                    newViewboxUP.Child = textBlockUP;
                    newBorderUP.Child = newViewboxUP;

                    newBorderPrevQTY.SetValue(Grid.RowProperty, countRows);
                    newBorderPrevQTY.SetValue(Grid.ColumnProperty, 6);
                    newViewboxPrevQTY.Child = textBlockPrevQTY;
                    newBorderPrevQTY.Child = newViewboxPrevQTY;

                    newBorderQTY.SetValue(Grid.RowProperty, countRows);
                    newBorderQTY.SetValue(Grid.ColumnProperty, 7);
                    newViewboxQTY.Child = textBlockQTY;
                    newBorderQTY.Child = newViewboxQTY;

                    newBorderDN.SetValue(Grid.RowProperty, countRows);
                    newBorderDN.SetValue(Grid.ColumnProperty, 8);
                    newViewboxDN.Child = textBlockDN;
                    newBorderDN.Child = newViewboxDN;

                    newBorderTransNum.SetValue(Grid.RowProperty, countRows);
                    newBorderTransNum.SetValue(Grid.ColumnProperty, 9);
                    newViewboxTransNum.Child = textBlockTransNum;
                    newBorderTransNum.Child = newViewboxTransNum;

                    // Add the UI elements to the BillDataList grid
                    BillDataList.Children.Add(newBorderShipTo);
                    BillDataList.Children.Add(newBorderName);
                    BillDataList.Children.Add(newBorderAPN);
                    BillDataList.Children.Add(newBorderCPN);
                    BillDataList.Children.Add(newBorderDescription);
                    BillDataList.Children.Add(newBorderUP);
                    BillDataList.Children.Add(newBorderPrevQTY);
                    BillDataList.Children.Add(newBorderQTY);
                    BillDataList.Children.Add(newBorderDN);
                    BillDataList.Children.Add(newBorderTransNum);
                    countRows++;
                }
                else
                {
                    break;
                }
            }
            // Update the current list position based on the number of added rows
            currentListPosition += (countRows - 1);
        }
    }
}
