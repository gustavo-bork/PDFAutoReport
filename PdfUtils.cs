using iText.Kernel.Colors;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using iText.Layout.Properties;

public static class PdfUtils {
    public static Table AddCellSmall(this Table table, string text, bool background = false) {
        return table.AddCell(
            new Cell()
                .SetBackgroundColor(background ? ColorConstants.LIGHT_GRAY : null)
                .SetFontSize(9)
                .Add(new Paragraph(text))
            );
    }
    public static Table AddImageSmall(this Table table, Image image, bool background = false) {
        return table.AddCell(
            new Cell().SetBackgroundColor(background ? ColorConstants.LIGHT_GRAY : null)
                .Add(image
                    .SetHeight(9.5f)
                    .SetWidth(9.5f)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER))
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
        );
    }
    public static Table AddHeader(this Table table, string text, float fontSize, bool background = false) {
        return table.AddCell(
            new Cell().SetBackgroundColor(background ? ColorConstants.LIGHT_GRAY : null)
                .SetFontSize(fontSize).SetBold()
                .Add(new Paragraph(text))
        );
    }
    public static Table AddBlueHeaderSmall(this Table table, string text, bool background = false) {
        return table.AddCell(
            new Cell().SetBackgroundColor(background ? ColorConstants.LIGHT_GRAY : null)
                .SetFontSize(9).SetBold()
                .SetFontColor(ColorConstants.BLUE)
                .Add(new Paragraph(text))
        );
    }
    public static Table AddRedHeaderSmall(this Table table, string text, bool background = false) {
        return table.AddCell(
            new Cell().SetBackgroundColor(background ? ColorConstants.LIGHT_GRAY : null)
                .SetFontSize(9).SetBold()
                .SetFontColor(ColorConstants.RED)
                .Add(new Paragraph(text))
        );
    }
    public static PdfCanvas MakeRectangle(this PdfCanvas canvas, double x, double y, double width, double height, Color color) {
        canvas.SetFillColor(color);
        canvas.Rectangle(x, y, width, height).Fill();
        return canvas.ResetFillColorRgb(); 
    }
    public static Table AddHeaderLarge(this Table table, string text, bool background = false) {
        return table.AddCell(
            new Cell().SetBackgroundColor(background ? ColorConstants.LIGHT_GRAY : null)
                .SetFontSize(10).SetBold().SetHeight(35f).SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .Add(new Paragraph(text))
        );
    }
    public static Table AddCell(this Table table, string text, float fontSize, bool background = false, bool bold = false, Color fontColor = null) {
        var cell = new Cell();
        if (background) cell = cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
        if (bold) cell = cell.SetBold();
        if (fontColor is not null) cell.SetFontColor(fontColor);
        
        return table.AddCell(cell.SetFontSize(fontSize).Add(new Paragraph(text)));
    }
    public static bool IsOneOf(this string item, params string[] options) => options.Contains(item);
}