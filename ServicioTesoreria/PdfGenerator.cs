using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
public static class PdfGenerator
{
    public static PdfDocument CreatePdf(string author)
    {
        var document = new Document();
        var sec = document.Sections.AddSection();
        sec.AddParagraph("Author:" + author);
        return RenderDocument(document);

        Paragraph paragraph = document.LastSection.AddParagraph("Table Overview", "Heading1");
        paragraph.AddBookmark("Tables");

        document.LastSection.AddParagraph("Simple Tables", "Heading2");

        Table table = new Table();
        table.Borders.Width = 0.75;

        Column column = table.AddColumn(Unit.FromCentimeter(2));
        column.Format.Alignment = ParagraphAlignment.Center;

        table.AddColumn(Unit.FromCentimeter(5));

        Row row = table.AddRow();
        row.Shading.Color = Colors.PaleGoldenrod;
        Cell cell = row.Cells[0];
        cell.AddParagraph("Itemus");
        cell = row.Cells[1];
        cell.AddParagraph("Descriptum");

        row = table.AddRow();
        cell = row.Cells[0];
        cell.AddParagraph("1");
        cell = row.Cells[1];
        cell.AddParagraph("lalaala");

        row = table.AddRow();
        cell = row.Cells[0];
        cell.AddParagraph("2");
        cell = row.Cells[1];
        cell.AddParagraph("lololo");

        table.SetEdge(0, 0, 2, 3, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

        document.LastSection.Add(table);
    }

    private static PdfDocument RenderDocument(Document document)
    {
        var rend = new PdfDocumentRenderer { Document = document };
        rend.RenderDocument();
        return rend.PdfDocument;
    }
}