using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace LeasingOffers.Models
{
    public class PDFGenerator
    {
        public static string Comment { get; set; } = string.Empty;
        public void GeneratePDF(string appPhysicalPath)
        {
            
            // Saving
            SaveFileDialog save = new SaveFileDialog()
            {
                Title = "Документ",
                Filter = "PDF | *.pdf",
                FileName = "",
            };
            
            // Create document
            save.ShowDialog();

            string savePath = save.FileName;


            iTextSharp.text.Document doc1 = new iTextSharp.text.Document();
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            if (string.IsNullOrEmpty(savePath)) return;

            var streamObj = new System.IO.FileStream(savePath, System.IO.FileMode.CreateNew);
            PdfWriter writer = PdfWriter.GetInstance(doc1, streamObj);
            doc1.Open();


            // Logo
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(@"..\..\..\Images\logo.png");
            logo.ScaleToFit(new iTextSharp.text.Rectangle(doc1.PageSize.Width * 0.8f, 200));
            logo.SetAbsolutePosition(50, 713);
            doc1.Add(logo);

            PdfContentByte cb = writer.DirectContent;

            EncodingProvider encodingProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(encodingProvider);

            // Header
            CreateRectWithText("ПРОСТОЕ РЕШЕНИЕ СЛОЖНЫХ ЗАДАЧ!", 0, 665, doc1.PageSize.Width, 40, "arialbd.ttf", BaseColor.WHITE, new BaseColor(0xE4, 0xAC, 0x3B), 17, cb);
            string subjectName = OfferManager.SelectedItems.FirstOrDefault().Subject;
            CreateRectWithText(subjectName, 10, 627, 113, 27, "arialbd.ttf", BaseColor.WHITE, new BaseColor(0x57, 0x36, 0x31), 9f, cb); // 573631

            // Table
            AddText("СВОДНАЯ ТАБЛИЦА ЛИЗИНГОВЫХ РАСЧЁТОВ", "arialbd.ttf", 16f, new BaseColor(0x58, 0x5C, 0x68), 150, 635, cb);
            CreateTable(cb, doc1);

            // Portrait
            iTextSharp.text.Image photo = iTextSharp.text.Image.GetInstance(@"..\..\..\Images\michail_photo.png");
            photo.ScalePercent(9.43f);
            photo.SetAbsolutePosition(10, 197);
            doc1.Add(photo);

            // Portrait description
            float descriptionXPos = 130;
            float descriptionYPos = 337;
            AddText("С уважением,", "arial.ttf", 10.5f, BaseColor.DARK_GRAY, descriptionXPos, descriptionYPos, cb);
            descriptionYPos -= 13;
            AddText("Пудовкин Михаил Валерьевич", "arial.ttf", 10.5f, BaseColor.DARK_GRAY, descriptionXPos, descriptionYPos, cb);
            descriptionYPos -= 13;
            AddText("Руководитель отдела по вопросам", "arial.ttf", 10.5f, BaseColor.DARK_GRAY, descriptionXPos, descriptionYPos, cb);
            descriptionYPos -= 13;
            AddText("лизинга и страхования", "arial.ttf", 10.5f, BaseColor.DARK_GRAY, descriptionXPos, descriptionYPos, cb);
            descriptionYPos -= 23;
            AddText("+79061220289 (Mail, WhatsApp, Telegram)", "arial.ttf", 10f, BaseColor.DARK_GRAY, descriptionXPos, descriptionYPos, cb);
            descriptionYPos -= 13;
            AddText("pmv@leas-brokers.ru - Email", "arial.ttf", 10f, BaseColor.DARK_GRAY, descriptionXPos, descriptionYPos, cb);
            descriptionYPos -= 13;
            AddText("leas-brokers.ru", "arial.ttf", 10f, BaseColor.DARK_GRAY, descriptionXPos, descriptionYPos, cb);
            descriptionYPos -= 13;
            AddText("Группа компаний \"БРОКЕРС\"", "arial.ttf", 10f, BaseColor.DARK_GRAY, descriptionXPos, descriptionYPos, cb);

            CreateFrame(255, 150, 330, 195, BaseColor.LIGHT_GRAY, cb);
            AddText("Комментарий эксперта:", "arial.ttf", 10f, BaseColor.DARK_GRAY, 335, 330, cb);

            AddExpertComment(Comment, cb);


            AddText("НАШИ ПАРТНЁРЫ", "arialbd.ttf", 16f, BaseColor.DARK_GRAY, 240, 160, cb);

            float startPos = 30f;
            float columnSpacing = 32f;
            float rowSpacing = 50f;
            float rowYPos = 124;
            float imageWidth = 80f;
            float offset = imageWidth + columnSpacing;

            // First row
            AddImage("alfa.png",            startPos, rowYPos, 80, 130, doc1, cb);
            AddImage("vtb.png",             columnSpacing += offset, rowYPos, 80, 130, doc1, cb);
            AddImage("sber.png",            columnSpacing += offset, rowYPos, 80, 130, doc1, cb);
            AddImage("gasprom.png",         columnSpacing += offset, rowYPos, 80, 130, doc1, cb);
            AddImage("sovkom.png",          columnSpacing += offset, rowYPos, 80, 130, doc1, cb);

            columnSpacing = 32f;

            // Second row
            AddImage("evroplan.png",         startPos, rowYPos -= rowSpacing, 80, 130, doc1, cb);
            AddImage("carcade.png",          columnSpacing += offset, rowYPos, 80, 130, doc1, cb);
            AddImage("leasing-trade.png",    columnSpacing += offset, rowYPos, 80, 130, doc1, cb);
            AddImage("reso.png",             columnSpacing += offset, rowYPos, 80, 130, doc1, cb);
            AddImage("kurskpromleasing.png", columnSpacing += offset, rowYPos, 80, 130, doc1, cb);

            doc1.Close();

            // Launch document
            Process.Start(new ProcessStartInfo(save.FileName) { UseShellExecute = true });
        }

        private void AddExpertComment(string comment, PdfContentByte cb)
        {
            string currentLine = "";
            int maxLineLength = 47;
            float linePos = 315;

            string[] words = comment.Split(' ');

            foreach (string word in words)
            {
                if (currentLine.Length + word.Length + 1 <= maxLineLength)
                {
                    currentLine += word + ' ';
                } 
                else {
                    AddText(currentLine, "arial.ttf", 10f, BaseColor.DARK_GRAY, 335, linePos, cb);
                    linePos -= 15f;
                    currentLine = word + ' ';
                }
            }
            AddText(currentLine, "arial.ttf", 10f, BaseColor.DARK_GRAY, 335, linePos, cb);
        }

        private void AddText(string text, string fontName, float fontSize, BaseColor textColor, float x, float y, PdfContentByte cb)
        {
            string fontPath = @"..\..\..\Fonts\" + fontName;
            BaseFont fontBold = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            cb.BeginText();

            cb.SetFontAndSize(fontBold, fontSize);
            cb.SetColorFill(textColor);

            cb.SetTextMatrix(x, y);
            cb.ShowText(text);
            cb.EndText();
        }

        private void AddImage(string name, float x, float y, float w, float h, Document doc, PdfContentByte cb)
        {
            string path = @"..\..\..\Images\partners\" + name;
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(path);

            image.SetAbsolutePosition(x, y);
            image.ScaleToFit(w, h);

            doc.Add(image);
        }

        private void CreateTable(PdfContentByte cb, iTextSharp.text.Document doc)
        {
            BaseColor mainColor = new BaseColor(0xB3, 0x93, 0x5B);
            BaseColor textColor = BaseColor.DARK_GRAY;

            string fontName = "arial.ttf";
            string fontPath = @"..\..\..\Fonts\" + fontName;
            BaseFont fontArial = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            cb.SetFontAndSize(fontArial, 10);


            float tablePos = 597;
            float currentYPos = tablePos;
            int rectHeight = 25;
            int offset = -rectHeight;
            float fontSize = 9f;

            // FIRST COLUMN

            CreateRectWithText("Лизинговая компания", 10, currentYPos, 113, rectHeight, "arial.ttf", BaseColor.WHITE, mainColor, fontSize, cb);

            currentYPos += offset - 3;
            CreateRectWithTextAndStripLeftAligned("Стоимость ПЛ", 10, currentYPos, 113, rectHeight, "arial.ttf", BaseColor.WHITE,
                mainColor, BaseColor.WHITE, fontSize, cb);

            currentYPos += offset;
            CreateRectWithTextAndStripLeftAligned("Аванс %", 10, currentYPos, 113, rectHeight, "arial.ttf", BaseColor.WHITE,
                mainColor, BaseColor.WHITE, fontSize, cb);

            currentYPos += offset;
            CreateRectWithTextAndStripLeftAligned("Аванс в руб.", 10, currentYPos, 113, rectHeight, "arial.ttf", BaseColor.WHITE,
                mainColor, BaseColor.WHITE, fontSize, cb);

            currentYPos += offset;
            CreateRectWithTextAndStripLeftAligned("Комиссия", 10, currentYPos, 113, rectHeight, "arial.ttf", BaseColor.WHITE,
                mainColor, BaseColor.WHITE, fontSize, cb);

            currentYPos += offset;
            CreateRectWithTextAndStripLeftAligned("Аннуитетный платёж", 10, currentYPos, 113, rectHeight, "arial.ttf", BaseColor.WHITE,
                mainColor, BaseColor.WHITE, fontSize, cb);

            currentYPos += offset;
            CreateRectWithTextAndStripLeftAligned("Количество платежей", 10, currentYPos, 113, rectHeight, "arial.ttf", BaseColor.WHITE,
                mainColor, BaseColor.WHITE, fontSize, cb);

            currentYPos += offset;
            CreateRectWithTextAndStripLeftAligned("Выкупной платёж", 10, currentYPos, 113, rectHeight, "arial.ttf", BaseColor.WHITE,
                mainColor, BaseColor.WHITE, fontSize, cb);

            currentYPos += offset;
            CreateRectWithTextAndStripLeftAligned("Страхование", 10, currentYPos, 113, rectHeight, "arial.ttf", BaseColor.WHITE,
                mainColor, BaseColor.WHITE, fontSize, cb);

            currentYPos += offset - 10f;
            CreateRectTotal("ИТОГО", 10, currentYPos, 113, rectHeight + 10, "arialbd.ttf", BaseColor.WHITE, mainColor, fontSize + 1, cb);

            // SECOND COLUMN

            float xPos = 123;
            currentYPos = tablePos;
            int columns = OfferManager.SelectedItemsAmount;
            BaseColor columnsColor = new BaseColor(0xFF, 0xF6, 0xBA);

            float rectWidth = (doc.PageSize.Width - 10 - 113 - 3 * (columns - 1) - 10 ) / columns;

            BaseColor stripColor = new BaseColor(0x58, 0x5B, 0x6A);
            
            foreach (Offer offer in OfferManager.SelectedItems)
            {
                xPos += 3;
                CreateRectWithText(offer.CompanyName, xPos, currentYPos, rectWidth, rectHeight, "arial.ttf", textColor, columnsColor, fontSize, cb);

                currentYPos += offset - 3;
                CreateRectWithTextAndStrip(FormatCost(offer.PlCost), xPos, currentYPos, rectWidth, rectHeight, "arial.ttf", textColor,
                    columnsColor, stripColor, fontSize, cb);

                currentYPos += offset;
                CreateRectWithTextAndStrip(FormatCost(offer.Advance), xPos, currentYPos, rectWidth, rectHeight, "arial.ttf", textColor,
                    columnsColor, stripColor, fontSize, cb);

                currentYPos += offset;
                CreateRectWithTextAndStrip(FormatCost(offer.AdvanceInRub), xPos, currentYPos, rectWidth, rectHeight, "arial.ttf", textColor,
                    columnsColor, stripColor, fontSize, cb);

                currentYPos += offset;
                CreateRectWithTextAndStrip(FormatCost(offer.Commission), xPos, currentYPos, rectWidth, rectHeight, "arial.ttf", textColor,
                    columnsColor, stripColor, fontSize, cb);

                currentYPos += offset;
                CreateRectWithTextAndStrip(FormatCost(offer.Annuity), xPos, currentYPos, rectWidth, rectHeight, "arial.ttf", textColor,
                    columnsColor, stripColor, fontSize, cb);

                currentYPos += offset;
                CreateRectWithTextAndStrip(FormatCost(offer.PaymentAmount), xPos, currentYPos, rectWidth, rectHeight, "arial.ttf", textColor,
                    columnsColor, stripColor, fontSize, cb);

                currentYPos += offset;
                CreateRectWithTextAndStrip(FormatCost(offer.Redemption), xPos, currentYPos, rectWidth, rectHeight, "arial.ttf", textColor,
                    columnsColor, stripColor, fontSize, cb);

                currentYPos += offset;
                CreateRectWithTextAndStrip(FormatCost(offer.Insurance), xPos, currentYPos, rectWidth, rectHeight, "arial.ttf", textColor,
                    columnsColor, stripColor, fontSize, cb);

                currentYPos += offset - 10f;

                CreateRectWithText(FormatCost(offer.TotalSum), xPos, currentYPos, rectWidth, rectHeight + 10, "arialbd.ttf", textColor, columnsColor, fontSize, cb);

                xPos += rectWidth;
                currentYPos = tablePos;
            }

        }

        private void CreateRectangle(float x, float y, float w, float h, BaseColor color, PdfContentByte cb)
        {
            cb.Rectangle(x, y, w, h);
            cb.SetColorFill(color);
            cb.Fill();
        }

        private void CreateRectWithText(string text, float rectX, float rectY, float width, float height, 
                                        string fontName, BaseColor textColor, BaseColor rectColor, float fontSize, PdfContentByte cb)
        {
            CreateRectangle(rectX, rectY, width, height, rectColor, cb);

            string fontPath = @"..\..\..\Fonts\" + fontName;
            BaseFont font = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            float textWidth = font.GetWidthPoint(text, fontSize);
            float textHeight = font.GetAscentPoint(text, fontSize) - font.GetDescentPoint(text, fontSize);
            float textX = rectX + (width - textWidth) / 2;
            float textY = rectY + (height - textHeight) / 2;


            cb.BeginText();
            cb.SetTextMatrix(textX, textY);
            cb.SetFontAndSize(font, fontSize);
            cb.SetColorFill(textColor);
            cb.ShowText(text);
            cb.EndText();
        }

        private void CreateRectTotal(string text, float rectX, float rectY, float width, float height,
                                        string fontName, BaseColor textColor, BaseColor rectColor, float fontSize, PdfContentByte cb)
        {
            CreateRectangle(rectX, rectY, width, height, rectColor, cb);

            string fontPath = @"..\..\..\Fonts\" + fontName;
            BaseFont font = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            float textWidth = font.GetWidthPoint(text, fontSize);
            float textHeight = font.GetAscentPoint(text, fontSize) - font.GetDescentPoint(text, fontSize);
            float textX = rectX + 10;
            float textY = rectY + (height - textHeight) / 2;


            cb.BeginText();
            cb.SetTextMatrix(textX, textY);
            cb.SetFontAndSize(font, fontSize);
            cb.SetColorFill(textColor);
            cb.ShowText(text);
            cb.EndText();
        }

        private void CreateRectWithTextAndStrip(string text, float rectX, float rectY, float width, float height,
                                        string fontName, BaseColor textColor, BaseColor rectColor, BaseColor stripColor, float fontSize, PdfContentByte cb)
        {
            cb.SetColorFill(rectColor);
            cb.Rectangle(rectX, rectY, width, height);
            cb.Fill();

            float stripWidth = width - 2 * 8;
            float stripHeight = 0.5f;

            cb.SetColorFill(stripColor);
            cb.Rectangle(rectX + 8, rectY + 0.5f, stripWidth, stripHeight);
            cb.Fill();

            string fontPath = @"..\..\..\Fonts\" + fontName;
            BaseFont font = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            float textWidth = font.GetWidthPoint(text, fontSize);
            float textHeight = font.GetAscentPoint(text, fontSize) - font.GetDescentPoint(text, fontSize);
            float textX = rectX + (width - textWidth) / 2;
            float textY = rectY + (height - textHeight) / 2 + 0.5f;

            cb.BeginText();

            cb.SetFontAndSize(font, fontSize);
            cb.SetColorFill(textColor);

            cb.SetTextMatrix(textX, textY);
            cb.ShowText(text);
            cb.EndText();
        }

        private void CreateRectWithTextAndStripLeftAligned(string text, float rectX, float rectY, float width, float height,
                                        string fontName, BaseColor textColor, BaseColor rectColor, BaseColor stripColor, float fontSize, PdfContentByte cb)
        {
            cb.SetColorFill(rectColor);
            cb.Rectangle(rectX, rectY, width, height);
            cb.Fill();

            float stripWidth = width - 2 * 8;
            float stripHeight = 0.5f;

            cb.SetColorFill(stripColor);
            cb.Rectangle(rectX + 8, rectY + 0.5f, stripWidth, stripHeight);
            cb.Fill();

            string fontPath = @"..\..\..\Fonts\" + fontName;
            BaseFont font = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            float textWidth = font.GetWidthPoint(text, fontSize);
            float textHeight = font.GetAscentPoint(text, fontSize) - font.GetDescentPoint(text, fontSize);
            float textX = rectX + 10;
            float textY = rectY + (height - textHeight) / 2 + 0.5f;

            cb.BeginText();

            cb.SetFontAndSize(font, fontSize);
            cb.SetColorFill(textColor);

            cb.SetTextMatrix(textX, textY);
            cb.ShowText(text);
            cb.EndText();
        }

        public static string FormatCost(string cost)
        {
            if (!cost.Contains('.') && !cost.Contains(','))
            {
                cost += ".00";
            }

            if (cost[cost.Length - 2] == '.' || cost[cost.Length - 2] == ',')
            {
                cost += "0";
            }

            cost = cost.Trim();
            cost = cost.Replace(',', '.');

            string formattedCost = "";
            string reverseCost = "";

            int digitCount = 0;
            bool startCount = false;

            for (int i = cost.Length - 1; i >= 0; i--)
            {
                if (startCount)
                {
                    ++digitCount;
                }

                if (cost[i] == '.')
                {
                    startCount = true;
                }

                reverseCost += cost[i];

                if (digitCount == 3 && i != 0)
                {
                    reverseCost += ' ';
                    digitCount = 0;
                }

            }

            for (int i = reverseCost.Length - 1; i >= 0; i--)
            {
                formattedCost += reverseCost[i];
            }

            return formattedCost;
        }

        private void CreateFrame(float w, float h, float x, float y, BaseColor color, PdfContentByte cb)
        {
            cb.SetLineWidth(2f);
            cb.SetColorStroke(color);
            cb.Rectangle(x, y, w, h);
            cb.Stroke();
        }
    }
}
