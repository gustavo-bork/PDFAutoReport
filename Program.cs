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
Color lightGray = new DeviceRgb(224, 224, 224);
document.SetMargins(45, 45, 45, 45);

#region Página 1
    var canvas = new PdfCanvas(pdf.AddNewPage());
    canvas.MakeRectangle(x: 35, y: ps.GetHeight() - 80, width: 520, height: 40, color: ColorConstants.DARK_GRAY);

    Paragraph paragraph = new Paragraph("AutoReport II")
        .SetFontSize(14)
        .SetFontColor(ColorConstants.WHITE)
        .SetTextAlignment(TextAlignment.CENTER);
    document.Add(paragraph);

    canvas.MakeRectangle(x: 35, y: 551, width: 520, height: 203, color: lightGray);
    paragraph = new Paragraph("\nA AUTOREPORT busca informações em inúmeras fontes públicas e " +
        "privadas, minimizando amplamente o risco nas operações de compra e venda de veículos." +
        "Porém, é importante alertar que não é possível que não é possível garantir 100% das " +
        "informações sobre os veículos, uma vez que algumas informações podem não constar na base " +
        "de dados das fontes utilizadas. Assim, a AUTOREPORT não se responsabiliza pela ausência de " +
        "alguma informação no momento da pesquisa.\n\n").SetFontSize(10);
    document.Add(paragraph);

    #region Detalhes consulta
    paragraph = new Paragraph(new Text("DETALHES ").SetBold())
        .Add("DA CONSULTA")
        .SetFontColor(ColorConstants.RED);
    document.Add(paragraph);
    Table table = new Table(new[] { 200f, 400f });
    new List<(string cabecalho, string valor)>() {
            ("Data/Hora", DateTime.Now.ToString()),
            ("Código", string.Empty),
            ("Status", string.Empty),
            ("Dados informados", string.Empty) 
        }.ForEach(item => {
            bool hasBackground = item.cabecalho.IsOneOf("Data/Hora", "Status");
            table.AddHeader(item.cabecalho, fontSize: 9, hasBackground);
            table.AddCell(item.valor, fontSize: 9, hasBackground);
        });
    document.Add(table);
    #endregion

    #region Resumo consulta
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

    new List<(Image sinal, string texto, Color cor, bool hasBackground)>() {
        (sinalCerto, "Não possui alerta de Roubo/Furto", ColorConstants.BLACK, true),
        (sinalCerto, "Não possui histórico de Roubo/Furto", ColorConstants.BLACK, true),
        (sinalAlerta, "Veículo com débito no valor de R$ 5,23", ColorConstants.BLUE, false),
        (sinalCerto, "Não possui restrição RENAJUD", ColorConstants.BLACK, false),
        (sinalCerto, "Não possui restrição judicial", ColorConstants.BLACK, true),
        (sinalAlerta, "Possui GRAVAME", ColorConstants.BLUE, true),
        (sinalCerto, "Não possui restrição administrativa", ColorConstants.BLACK, false),
        (sinalCerto, "Veículo EM CIRCULAÇÃO", ColorConstants.BLACK, false),
        (sinalCerto, "Não possui registros de acidentes", ColorConstants.BLACK, true),
        (sinalCerto, "Não possui registros de ocorrências policiais", ColorConstants.BLACK, true),
        (sinalCerto, "Não possui registros de flagrantes de trânsito", ColorConstants.BLACK, false),
        (sinalAlerta, "Possui histórico de leilão BASE 1", ColorConstants.BLUE, false),
        (sinalErro, "Possui indício de sinistro", ColorConstants.RED, true),
        (sinalAlerta, "Possui histórico de leilão BASE 2", ColorConstants.BLUE, true),
    }.ForEach(item => {
        table.AddImage(item.sinal, item.hasBackground);
        table.AddCell(item.texto, fontSize: 9, item.hasBackground, false, item.cor);
    });
    document.Add(table);
    #endregion

    #region Info veículo
    canvas.MakeRectangle(x: 35, y: document.GetBottomMargin() + 285, width: 520, height: 40, color: ColorConstants.RED);
    Image carro = new Image(ImageDataFactory.Create("carro.png"));
    paragraph = new Paragraph("INFORMAÇÕES SOBRE O VEÍCULO")
        .SetTextAlignment(TextAlignment.CENTER)
        .SetVerticalAlignment(VerticalAlignment.MIDDLE)
        .SetFontColor(ColorConstants.WHITE)
        .SetFontSize(14)
        .SetFixedPosition(ps.GetWidth() - 640, document.GetBottomMargin() + 295, 505);
    document.Add(paragraph);
    document.Add(
        carro.SetHeight(30)
            .SetWidth(30)
            .SetFixedPosition(document.GetLeftMargin() + 5, document.GetBottomMargin() + 290)
    );

    paragraph = new Paragraph("\n\n\nNome do carro").SetBold().SetFontSize(13);
    canvas.MakeRectangle(x: 35, y: 195, width: 515, height: 130, color: lightGray);
    document.Add(paragraph); 

    table = new Table(new[] { 110F, 150F, 110F, 150F });
    new List<string>() { "Placa", "Ano", "Cor", "Combustível", "Chassi", "Renavam", "Município", "N Motor", "Marca", "Modelo" }
        .ForEach(item => {
            bool hasBackground = item.IsOneOf("Placa", "Ano", "Chassi", "Renavam", "Marca", "Modelo");
            table.AddHeader(item, 10, hasBackground);
            table.AddCell("", 10, hasBackground);
        });
    document.Add(table);
    #endregion

    var pagina = pdf.GetNumberOfPages();
    paragraph = new Paragraph($"Página {pagina} de 4")
        .SetFixedPosition(document.GetLeftMargin() + 200, ps.GetBottom() + 10, 505);

    document.Add(paragraph);
    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
#endregion

#region Página 2
    canvas = new PdfCanvas(pdf.GetPage(2));

    #region Info técnica
    canvas.MakeRectangle(x: 35, y: 680, width: 520, height: 120, color: lightGray);
    paragraph = new Paragraph("INFORMAÇÕES ").SetFontColor(ColorConstants.RED);
    document.Add(paragraph.Add(new Text("TÉCNICAS").SetBold()));

    table = new Table(new[] { 150F, 100F, 150F, 100F });
    new List<string>() {
        "Espécie", "Procedência", "Tipo", "Potência",
        "Cilindrada", "Peso bruto", "Capacidade de passageiros", "Capacidade de carga" 
    }
    .ForEach(item => {
        bool hasBackground = item.IsOneOf("Espécie", "Procedência", "Cilindrada", "Peso bruto");
        table.AddCell(item, 10, hasBackground, true);
        table.AddCell("", 10, hasBackground);
    });
    document.Add(table);
    #endregion

    #region Dados proprietário
    canvas.MakeRectangle(x: 35, y: 535, width: 520, height: 140, color: lightGray);
    paragraph = new Paragraph("\nDADOS ").SetFontColor(ColorConstants.RED);
    document.Add(paragraph.Add(new Text("PROPRIETÁRIO").SetBold()));

    table = new Table(new[] { 70F, 90F, 110F, 150F, 90F });
    new List<string>() { "Tipo", "Documento", "Nome", "Endereço", "Cidade-UF" }.ForEach(item => table.AddHeader(item, 10));
    table.AddHeaderLarge("ATUAL", true); 
    table.AddCell("", 10, true); table.AddCell("", 10, true); table.AddCell("", 10, true); table.AddCell("", 10, true);
    table.AddHeaderLarge("ANTERIOR", false);
    table.AddCell(""); table.AddCell(""); table.AddCell(""); table.AddCell("");
    document.Add(table);
    #endregion

    // Icon by Freepik
    Image info = new Image(ImageDataFactory.Create("information.png"))
        .SetHeight(30)
        .SetWidth(30)
        .SetFixedPosition(document.GetLeftMargin() + 5, document.GetBottomMargin() + 440);
    canvas.MakeRectangle(x: 35, y: document.GetBottomMargin() + 435, width: 520, height: 40, color: ColorConstants.RED);
    paragraph = new Paragraph("OCORRÊNCIAS IMPORTANTES")
        .SetFontSize(14)
        .SetFontColor(ColorConstants.WHITE)
        .SetFixedPosition(document.GetLeftMargin() + 50, document.GetBottomMargin() + 445, 505);
    document.Add(paragraph);
    document.Add(info);

    #region Tabela indício sinistro
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
    #endregion

    #region Histórico roubo furto
    canvas.MakeRectangle(x: 35, y: 315, width: 520, height: 60, color: lightGray);
    paragraph = new Paragraph("\n\nHISTÓRICO DE ").Add(new Text("ROUBO E FURTO").SetBold()).SetFontColor(ColorConstants.RED);
    document.Add(paragraph);
    table = new Table(new[] { 505f });
    table.AddCell("NENHUM REGISTRO ENCONTRADO NA BASE DE DADOS", 10, true);
    document.Add(table);
    #endregion

    pagina = pdf.GetNumberOfPages();
    paragraph = new Paragraph($"Página {pagina} de 4")
        .SetFixedPosition(document.GetLeftMargin() + 200, ps.GetBottom() + 10, 505);
    document.Add(paragraph);
    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
#endregion

#region Página 3
    canvas = new PdfCanvas(pdf.GetPage(3));

    #region Tabela registro débitos
    canvas.MakeRectangle(x: 35, y: 585, width: 520, height: 215, color: lightGray);
    paragraph = new Paragraph("REGISTRO DE ")
        .Add(new Text("DÉBITOS").SetBold())
        .SetFontColor(ColorConstants.RED);
    document.Add(paragraph);

    table = new Table(2, true);
    table.AddCell("Tipo", 10, false, true);
    table.AddCell("Valor", 10, false, true);
    new List<(string tipo, double valor)> { 
        ("Detran", 0), 
        ("IPVA", 0), 
        ("Licenciamento", 0), 
        ("DPVAT", 5.23), 
        ("DER", 0), 
        ("Renainf", 0), 
        ("Municipais", 0), 
        ("PRF", 0) 
    }.ForEach(item => {
        var hasBackground = item.tipo.IsOneOf("Detran", "Licenciamento", "DER", "Municipais");
        table.AddCell(item.tipo.ToUpper(), 10, hasBackground);
        table.AddCell(item.valor.ToString("C", CultureInfo.CurrentCulture), 10, hasBackground, bold: item.Item2 == 5.23);
    });
    document.Add(table);
    table.Complete();
    #endregion

    #region Tabela restrições
    canvas.MakeRectangle(x: 35, y: 420, width: 520, height: 160, color: lightGray);
    paragraph = new Paragraph("\nRestrições".ToUpper()).SetFontColor(ColorConstants.RED);
    document.Add(paragraph);

    table = new Table(2, true);
    table.AddCell("Tipo", fontSize: 10, background: false, bold: true);
    table.AddCell("Mensagem base estadual", fontSize: 10, background: false, bold: true);
    new List<string>() {
        "Restrições Renajud",
        "Restrições Judiciais",
        "Restrições Financeiras",
        "Restrições Roubo e Furto",
        "Restrições Administrativas" 
    }.ForEach(item => {
        string text = !item.Contains("Financeiras") ? "Não constam registros" : "Alienação fiduciária";
        Color color = !item.Contains("Financeiras") ? new DeviceRgb(0, 120, 0) : ColorConstants.YELLOW;
        bool hasBackground = item.IsOneOf("Restrições Renajud", "Restrições Financeiras", "Restrições Administrativas");

        table.AddCell(item, 10, hasBackground, false);
        table.AddCell(text, 10, hasBackground, false, color);
    });
    document.Add(table);
    table.Complete();
    #endregion

    #region Ocorrencias transito
    canvas.MakeRectangle(x: 35, y: 355, width: 520, height: 40, color: ColorConstants.RED);
    // Image by itim2101
    Image crashingCar = new Image(ImageDataFactory.Create("carro_batendo.png"));
    // Image by bqlqn
    Image dollar = new Image(ImageDataFactory.Create("dollar.png"));
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
    #endregion

    #region Acidentes
    canvas.MakeRectangle(x: 35, y: 285, width: 520, height: 65, color: lightGray);
    paragraph = new Paragraph("\n\n\n\n\nAcidentes".ToUpper())
        .SetFontColor(ColorConstants.RED);
    document.Add(paragraph);

    table = new Table(new[] { 505f });
    table.AddCell("Nenhum registro encontrado na base de dados".ToUpper(), 10, true);
    document.Add(table);
    #endregion

    #region Ocorrencias policiais
    canvas.MakeRectangle(x: 35, y: 215, width: 520, height: 65, color: lightGray);
    paragraph = new Paragraph("\nOcorrências policiais".ToUpper())
        .SetFontColor(ColorConstants.RED);
    document.Add(paragraph);

    table = new Table(new[] { 505f });
    table.AddCell("Nenhum registro encontrado na base de dados".ToUpper(), 10, true);
    document.Add(table);
    #endregion

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
    #endregion

    pagina = pdf.GetNumberOfPages();
    paragraph = new Paragraph($"Página {pagina} de 4")
        .SetFixedPosition(document.GetLeftMargin() + 200, ps.GetBottom() + 10, 505);
    document.Add(paragraph);
#endregion

document.Close();