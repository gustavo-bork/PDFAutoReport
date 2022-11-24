using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Colors;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using System.Globalization;

PdfWriter writer = new PdfWriter("PdfAutoReport.pdf");
PdfDocument pdf = new PdfDocument(writer);
Document document = new Document(pdf);
PageSize ps = pdf.GetDefaultPageSize();
document.SetMargins(45, 45, 45, 45);

#region Página 1
    var canvas = new PdfCanvas(pdf.AddNewPage());
    canvas.MakeRectangle(x: 35, y: ps.GetHeight() - 80, width: 520, height: 40, color: ColorConstants.DARK_GRAY);
    canvas.SetFillColor(ColorConstants.WHITE).Fill();

    Paragraph paragraph = new Paragraph("AutoReport II")
        .SetFontSize(14)
        .SetTextAlignment(TextAlignment.CENTER);
    document.Add(paragraph);

    Color lightGray = new DeviceRgb(224, 224, 224);
    canvas.MakeRectangle(x: 35, y: 551, width: 520, height: 203, color: lightGray);
    document.Add(new Paragraph("\nA AUTOREPORT busca informações em inúmeras fontes públicas e " +
        "privadas, minimizando amplamente o risco nas operações de compra e venda de veículos." +
        "Porém, é importante alertar que não é possível que não é possível garantir 100% das " +
        "informações sobre os veículos, uma vez que algumas informações podem não constar na base " +
        "de dados das fontes utilizadas. Assim, a AUTOREPORT não se responsabiliza pela ausência de " +
        "alguma informação no momento da pesquisa.\n\n").SetFontSize(10));

    paragraph = new Paragraph(new Text("DETALHES ").SetBold())
        .Add("DA CONSULTA")
        .SetFontColor(ColorConstants.RED);
    document.Add(paragraph);
    Table table = new Table(new[] { 200f, 400f });
    new[] { "Data/Hora", "Código", "Status", "Dados informados" }
        .ToList()
        .ForEach((item) => {
            bool hasBackground = item.IsOneOf("Data/Hora", "Status");
            table.AddHeader(item, 9, hasBackground);
            table.AddCell("", 9, hasBackground);
        });
    document.Add(table);

    canvas.MakeRectangle(x: 35, y: 378, width: 520, height: 163, color: lightGray);
    paragraph = new Paragraph(new Text("\nRESUMO ").SetBold())
        .Add("DA CONSULTA")
        .SetFontColor(ColorConstants.RED);
    document.Add(paragraph);

    table = new Table(new[] { 25F, 240F, 25F, 240F });
    // Icons by Freepik, kliwir art, Pixel perfect and flaticon
    Image sinalCerto = new Image(ImageDataFactory.Create("sinal_certo.png")),
        sinalAlerta = new Image(ImageDataFactory.Create("sinal_alerta.png")),
        sinalErro = new Image(ImageDataFactory.Create("sinal_errado.png"));

    table.AddImageSmall(sinalCerto, true); table.AddCellSmall("Não possui alerta de Roubo/Furto", true);
    table.AddImageSmall(sinalCerto, true); table.AddCellSmall("Não possui histórico de Roubo/Furto", true);
    table.AddImageSmall(sinalAlerta, false); table.AddBlueHeaderSmall("Veículo com débito no valor de R$ 5,23", false);
    table.AddImageSmall(sinalCerto, false); table.AddCellSmall("Não possui restrição RENAJUD", false);
    table.AddImageSmall(sinalCerto, true); table.AddCellSmall("Não possui restrição judicial", true);
    table.AddImageSmall(sinalAlerta, true); table.AddBlueHeaderSmall("Possui GRAVAME", true);
    table.AddImageSmall(sinalCerto, false); table.AddCellSmall("Não possui restrição administrativa", false);
    table.AddImageSmall(sinalCerto, false); table.AddCellSmall("Veículo EM CIRULAÇÃO", false);
    table.AddImageSmall(sinalCerto, true); table.AddCellSmall("Não possui registros de acidentes", true);
    table.AddImageSmall(sinalCerto, true); table.AddCellSmall("Não possui registros de ocorrências policiais", true);
    table.AddImageSmall(sinalCerto, false); table.AddCellSmall("Não possui registros de flagrantes de trânsito", false);
    table.AddImageSmall(sinalAlerta, false); table.AddBlueHeaderSmall("Possui histórico de leilão BASE 1", false);
    table.AddImageSmall(sinalErro, true); table.AddRedHeaderSmall("Possui indício de sinistro", true);
    table.AddImageSmall(sinalAlerta, true); table.AddBlueHeaderSmall("Possui histórico de leilão BASE 2", true);
    document.Add(table);

    canvas.MakeRectangle(x: 35, y: document.GetBottomMargin() + 285, width: 520, height: 40, color: ColorConstants.RED);
    Image carro = new Image(ImageDataFactory.Create("carro.png"));
    canvas.SetFillColor(ColorConstants.WHITE);
    paragraph = new Paragraph("INFORMAÇÕES SOBRE O VEÍCULO")
        .SetTextAlignment(TextAlignment.CENTER)
        .SetVerticalAlignment(VerticalAlignment.MIDDLE)
        .SetFontSize(14)
        .SetFixedPosition(ps.GetWidth() - 640, document.GetBottomMargin() + 295, 505);
    document.Add(paragraph);
    document.Add(
        carro.SetHeight(30)
            .SetWidth(30)
            .SetFixedPosition(document.GetLeftMargin() + 5, document.GetBottomMargin() + 290)
    );
    canvas.ResetFillColorRgb();

    paragraph = new Paragraph("\n\n\nNome do carro").SetBold().SetFontSize(13);
    canvas.MakeRectangle(x: 35, y: 195, width: 515, height: 130, color: lightGray);
    document.Add(paragraph); 

    table = new Table(new[] { 110F, 150F, 110F, 150F });
    new[] { "Placa", "Ano", "Cor", "Combustível", "Chassi", "Renavam", "Município", "N Motor", "Marca", "Modelo" }
        .ToList()
        .ForEach(item => {
            bool hasBackground = item.IsOneOf("Placa", "Ano", "Chassi", "Renavam", "Marca", "Modelo");
            table.AddHeader(item, 10, hasBackground);
            table.AddCell("", 10, hasBackground);
        });
    document.Add(table);

    var pagina = pdf.GetNumberOfPages();
    paragraph = new Paragraph($"Página {pagina} de 4")
        .SetFixedPosition(document.GetLeftMargin() + 200, ps.GetBottom() + 10, 505);

    document.Add(paragraph);
    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
#endregion

#region Página 2
    canvas = new PdfCanvas(pdf.GetPage(2));
    canvas.MakeRectangle(x: 35, y: 680, width: 520, height: 120, color: lightGray);
    canvas.MakeRectangle(x: 35, y: 535, width: 520, height: 140, color: lightGray); 

    paragraph = new Paragraph("INFORMAÇÕES ").SetFontColor(ColorConstants.RED);
    document.Add(paragraph.Add(new Text("TÉCNICAS").SetBold()));

    table = new Table(new[] { 150F, 100F, 150F, 100F });
    new[] {
        "Espécie", "Procedência", "Tipo", "Potência",
        "Cilindrada", "Peso bruto", "Capacidade de passageiros", "Capacidade de carga" 
    }
    .ToList()
    .ForEach(item => {
        bool hasBackground = item.IsOneOf("Espécie", "Procedência", "Cilindrada", "Peso bruto");
        table.AddCell(item, 10, hasBackground, true);
        table.AddCell("", 10, hasBackground);
    });
    document.Add(table);

    paragraph = new Paragraph("\nDADOS ").SetFontColor(ColorConstants.RED);
    document.Add(paragraph.Add(new Text("PROPRIETÁRIO").SetBold()));

    table = new Table(new[] { 70F, 90F, 110F, 150F, 90F });
    new[] { "Tipo", "Documento", "Nome", "Endereço", "Cidade-UF" }
        .ToList()
        .ForEach(item => {
            table.AddHeader(item, 10);
        });
    table.AddHeaderLarge("ATUAL", true); 
    table.AddCell("", 10, true); table.AddCell("", 10, true); table.AddCell("", 10, true); table.AddCell("", 10, true);
    table.AddHeaderLarge("ANTERIOR", false);
    table.AddCell(""); table.AddCell(""); table.AddCell(""); table.AddCell("");
    document.Add(table);

    // Icon by Freepik
    Image info = new Image(ImageDataFactory.Create("information.png"));
    canvas.MakeRectangle(x: 35, y: document.GetBottomMargin() + 435, width: 520, height: 40, color: ColorConstants.RED);
    canvas.SetFillColor(ColorConstants.WHITE);
    paragraph = new Paragraph("OCORRÊNCIAS IMPORTANTES")
        .SetFontSize(14)
        .SetFixedPosition(document.GetLeftMargin() + 50, document.GetBottomMargin() + 445, 505);
    document.Add(paragraph);
    document.Add(
        info.SetHeight(30)
            .SetWidth(30)
            .SetFixedPosition(document.GetLeftMargin() + 5, document.GetBottomMargin() + 440)
    );
    canvas.ResetFillColorRgb();

    canvas.MakeRectangle(x: 35, y: 395, width: 520, height: 80, color: lightGray);
    paragraph = new Paragraph("\n\n\n\nINDÍCIO DE ")
        .SetFontColor(ColorConstants.RED)
        .Add(new Text("SINISTRO").SetBold());
    document.Add(paragraph);
    
    table = new Table(new[] { 65f, 300f, 80f, 115f });
    table.AddCell("Data", 10, false, true);
    table.AddCell("Mensagem", 10, false, true);
    table.AddCell("Seguradora", 10, false, true);
    table.AddCell("Número análise", 10, false, true);
    table.AddCell(DateTime.Now.ToString("dd/MM/yyyy"));
    table.AddCell("Consta indício de sinistro nas bases de dados pesquisadas", 9, false);
    table.AddCell("");
    table.AddCell("Não informado".ToUpper());
    document.Add(table);

    canvas.MakeRectangle(x: 35, y: 315, width: 520, height: 60, color: lightGray);
    paragraph = new Paragraph("\n\nHISTÓRICO DE ").Add(new Text("ROUBO E FURTO").SetBold()).SetFontColor(ColorConstants.RED);
    document.Add(paragraph);
    table = new Table(new[] { 505f });
    table.AddCell("NENHUM REGISTRO ENCONTRADO NA BASE DE DADOS", 10, true);
    document.Add(table);

    pagina = pdf.GetNumberOfPages();
    paragraph = new Paragraph($"Página {pagina} de 4")
        .SetFixedPosition(document.GetLeftMargin() + 200, ps.GetBottom() + 10, 505);
    document.Add(paragraph);
    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
#endregion

#region Página 3
    canvas = new PdfCanvas(pdf.GetPage(3));
    canvas.MakeRectangle(x: 35, y: 585, width: 520, height: 215, color: lightGray);

    paragraph = new Paragraph("REGISTRO DE ")
        .Add(new Text("DÉBITOS").SetBold())
        .SetFontColor(ColorConstants.RED);
    document.Add(paragraph);

    table = new Table(2, true);
    table.AddCell("Tipo", 10, false, true);
    table.AddCell("Valor", 10, false, true);
    new[] { ("Detran", 0), ("IPVA", 0), ("Licenciamento", 0), ("DPVAT", 5.23), ("DER", 0), ("Renainf", 0), ("Municipais", 0), ("PRF", 0) }
        .ToList()
        .ForEach(item => {
            var hasBackground = item.Item1.IsOneOf("Detran", "Licenciamento", "DER", "Municipais");
            table.AddCell(item.Item1.ToUpper(), 10, hasBackground);
            table.AddCell(item.Item2.ToString("C", CultureInfo.CurrentCulture), 10, hasBackground, bold: item.Item2 == 5.23);
        });
    document.Add(table);
    table.Complete();

    canvas.MakeRectangle(x: 35, y: 420, width: 520, height: 160, color: lightGray);
    paragraph = new Paragraph("\nRestrições".ToUpper()).SetFontColor(ColorConstants.RED);
    document.Add(paragraph);

    table = new Table(2, true);
    table.AddCell("Tipo", 10, false, true);
    table.AddCell("Mensagem base estadual", 10, false, true);
    new[] { "Restrições Renajud", "Restrições Judiciais", "Restrições Financeiras", "Restrições Roubo e Furto", "Restrições Administrativas" }
        .ToList()
        .ForEach(item => {
            string text = !item.Contains("Financeiras") ? "Não constam registros" : "Alienação fiduciária";
            Color color = !item.Contains("Financeiras") ? new DeviceRgb(0, 120, 0) : ColorConstants.YELLOW;
            bool hasBackground = item.IsOneOf("Restrições Renajud", "Restrições Financeiras", "Restrições Administrativas");

            table.AddCell(item, 10, hasBackground, false);
            table.AddCell(text, 10, hasBackground, false, color);
        });
    document.Add(table);
    table.Complete();

    canvas.MakeRectangle(x: 35, y: 355, width: 520, height: 40, color: ColorConstants.RED);
    // Image by itim2101
    Image crashingCar = new Image(ImageDataFactory.Create("carro_batendo.png"));
    crashingCar = crashingCar
        .SetHeight(30)
        .SetWidth(30)
        .SetFixedPosition(left: 45, bottom: 360);
    paragraph = new Paragraph("Ocorrências de trânsito".ToUpper())
        .SetFixedPosition(left: 80, bottom: 365, width: 520)
        .SetVerticalAlignment(VerticalAlignment.MIDDLE)
        .SetFontSize(14)
        .SetFontColor(ColorConstants.WHITE);
    document.Add(paragraph);
    document.Add(crashingCar);

    canvas.MakeRectangle(x: 35, y: 285, width: 520, height: 65, color: lightGray);
    paragraph = new Paragraph("\n\n\n\n\nAcidentes".ToUpper())
        .SetFontColor(ColorConstants.RED);
    document.Add(paragraph);

    table = new Table(new[] { 505f });
    table.AddCell("Nenhum registro encontrado na base de dados".ToUpper(), 10, true);
    document.Add(table);
    
    canvas.MakeRectangle(x: 35, y: 215, width: 520, height: 65, color: lightGray);
    paragraph = new Paragraph("\nOcorrências policiais".ToUpper())
        .SetFontColor(ColorConstants.RED);
    document.Add(paragraph);

    table = new Table(new[] { 505f });
    table.AddCell("Nenhum registro encontrado na base de dados".ToUpper(), 10, true);
    document.Add(table);

    pagina = pdf.GetNumberOfPages();
    paragraph = new Paragraph($"Página {pagina} de 4")
        .SetFixedPosition(document.GetLeftMargin() + 200, ps.GetBottom() + 10, 505);
    document.Add(paragraph);
    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
#endregion

#region Página 4
    canvas = new PdfCanvas(pdf.GetPage(4));

    #region Flagrantes trânsito
    canvas.MakeRectangle(x: 35, y: 735, width: 520, height: 60, color: lightGray);
    paragraph = new Paragraph("Flagrantes de trânsito".ToUpper()).SetFontColor(ColorConstants.RED);
    document.Add(paragraph);

    table = new Table(new[] { 505f });
    table.AddCell("Nenhum registro encontrado na base de dados".ToUpper(), 10, true);
    document.Add(table);
    #endregion

    #region Base dados 1
    // Image by Freepik
    Image auction = new Image(ImageDataFactory.Create("auction.png"));
    auction = auction
        .SetHeight(30)
        .SetWidth(30)
        .SetFixedPosition(left: 45, bottom: 685);
    canvas.MakeRectangle(x: 35, y: 680, width: 520, height: 40, color: ColorConstants.RED);
    paragraph = new Paragraph("Publicado em edital de notificação e/ou leilão público".ToUpper())
        .SetTextAlignment(TextAlignment.CENTER)
        .SetVerticalAlignment(VerticalAlignment.MIDDLE)
        .SetFontSize(14)
        .SetFixedPosition(left: 45, bottom: 690, width: 520)
        .SetFontColor(ColorConstants.WHITE);
    document.Add(paragraph);
    document.Add(auction);

    canvas.MakeRectangle(x: 35, y: 595, width: 520, height: 80, color: lightGray);
    paragraph = new Paragraph("\n\n\n\nBase de ".ToUpper())
        .Add(new Text("Dados 1".ToUpper()).SetBold())
        .SetFontColor(ColorConstants.RED);
    document.Add(paragraph);

    table = new Table(new[] { 90f, 70f, 120f, 80f, 100f, 60f });
    new List<string>() { 
        "Data",
        "Leiloeiro",
        "Comitente",
        "Condição",
        "Aceitação seguro",
        "% FIPE" 
    }.ForEach(item => table.AddHeader(item, fontSize: 10));
    new List<string>() {
        DateTime.Now.ToShortDateString(),
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty
    }.ForEach(item => table.AddCell(item, fontSize: 10, background: true));
    document.Add(table);
    #endregion

    #region Base dados 2
    canvas.MakeRectangle(x: 35, y: 505, width: 520, height: 80, color: lightGray);
    paragraph = new Paragraph("\n\nBase de ".ToUpper())
        .Add(new Text("Dados 2".ToUpper()).SetBold())
        .SetFontColor(ColorConstants.RED);
    document.Add(paragraph);
    document.Add(table);

    canvas.MakeRectangle(x: 35, y: 430, width: 520, height: 55, color: lightGray);
    var text = new Text(@"Os dados de leilão foram obtidos através de divulgações realizadas pelos leiloeiros oficiais em mídias 
                        impressa e eletrônica. Eventuais inexatidões são de responsabilidade exclusiva dos leiloeiros responsáveis pela divulgação")
                        .SetBold()
                        .SetFontColor(ColorConstants.GRAY)
                        .SetFontSize(9);
    paragraph = new Paragraph("\n\nImportante - ".ToUpper()).Add(text);
    document.Add(paragraph);

    pagina = pdf.GetNumberOfPages();
    paragraph = new Paragraph($"Página {pagina} de 4")
        .SetFixedPosition(document.GetLeftMargin() + 200, ps.GetBottom() + 10, 505);
    document.Add(paragraph);
    #endregion
#endregion

document.Close();