using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSLib.Models;

namespace POSForm
{
    internal class ReceiptPrinter
    {
        private List<ICartItem> cartItems = [];

        public void PrintReceipt(List<ICartItem> cartItems)
        {
            this.cartItems = cartItems;

            PrintDocument printDocument = new();
            printDocument.PrintPage += PrintDocument_PrintPage;

            // Show the PrintDialog to allow printer selection
            using (PrintDialog printDialog = new())
            {
                printDialog.Document = printDocument; // Link the PrintDocument to the dialog

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    // Apply the selected printer settings
                    printDocument.PrinterSettings = printDialog.PrinterSettings;

                    // Print the document
                    printDocument.Print();
                }
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new("Arial", 12);
            float lineHeight = font.GetHeight();
            float x = 50; // Left margin
            float y = 50; // Top margin

            // Load and draw the logo
            try
            {
                Image logo = Image.FromFile("logo.png");
                graphics.DrawImage(logo, x, y, 100, 100); // Draw logo at the top-left corner
                y += 60; // Adjust y position to leave space for the logo
            }
            catch (Exception ex)
            {
                // Handle cases where the logo file is missing or cannot be loaded
                graphics.DrawString("Logo not found", font, Brushes.Red, x, y);
                y += lineHeight;
            }

            // Print header
            y += lineHeight * 2;

            // Print column headers
            graphics.DrawString("Barcode", font, Brushes.Black, x, y);
            graphics.DrawString("Product", font, Brushes.Black, x + 100, y);
            graphics.DrawString("Qty", font, Brushes.Black, x + 300, y);
            graphics.DrawString("Price", font, Brushes.Black, x + 350, y);
            graphics.DrawString("Total", font, Brushes.Black, x + 450, y);
            y += lineHeight;

            // Print each order
            foreach (var order in cartItems)
            {
                graphics.DrawString(order.Barcode.ToString(), font, Brushes.Black, x, y);
                graphics.DrawString(order.Name, font, Brushes.Black, x + 100, y);
                graphics.DrawString(order.Quantity.ToString(), font, Brushes.Black, x + 300, y);
                graphics.DrawString(order.Price.ToString("C"), font, Brushes.Black, x + 350, y);
                graphics.DrawString(order.Total.ToString("C"), font, Brushes.Black, x + 450, y);
                y += lineHeight;
            }

            // Print footer
            y += lineHeight;
            double grandTotal = cartItems.Sum(o => o.Total);
            graphics.DrawString(
                $"Grand Total: {grandTotal:C}",
                new Font("Arial", 14, FontStyle.Bold),
                Brushes.Black,
                x,
                y
            );
        }
    }
}
