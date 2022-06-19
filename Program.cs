﻿using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Colors;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using System;
using System.Globalization;
using System.Linq;
using iText.IO.Image;

PdfWriter writer = new PdfWriter("PdfAutoReport.pdf");
PdfDocument pdf = new PdfDocument(writer);
Document document = new Document(pdf);
PageSize ps = pdf.GetDefaultPageSize();
document.SetMargins(45, 45, 45, 45);

/* ↓ Página 1 ↓ */
var canvas = new PdfCanvas(pdf.AddNewPage());
canvas.SetFillColor(ColorConstants.DARK_GRAY);
canvas.Rectangle(ps.GetWidth() - 550, ps.GetHeight() - 80, 505, 40).Fill();
canvas.SetFillColor(ColorConstants.WHITE).Fill();

Paragraph paragraph = new Paragraph("AutoReport II")
    .SetFontSize(14)
    .SetTextAlignment(TextAlignment.CENTER);
document.Add(paragraph);

Color lightGray = new DeviceRgb(224, 224, 224);
canvas.SetFillColor(lightGray);
canvas.Rectangle(x: 39, y: 551, width: 515, height: 203).Fill();
canvas.ResetFillColorRgb(); 
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
        bool background = item.IsOneOf("Data/Hora", "Status");
        table.AddHeaderSmall(item, background);
        table.AddCellSmall("", background);
    });
document.Add(table);

canvas.MakeRectangle(x: 39, y: 378, width: 515, height: 163, color: lightGray);
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

canvas.MakeRectangle(x: document.GetLeftMargin(), y: document.GetBottomMargin() + 285, width: 505, height: 40, color: ColorConstants.RED);
Image carro = new Image(ImageDataFactory.Create("carro.png"));
float tamanho = 30f;
canvas.SetFillColor(ColorConstants.WHITE);
paragraph = new Paragraph("INFORMAÇÕES SOBRE O VEÍCULO")
    .SetTextAlignment(TextAlignment.CENTER)
    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
    .SetFontSize(14)
    .SetFixedPosition(ps.GetWidth() - 640, document.GetBottomMargin() + 295, 505);
document.Add(paragraph);
document.Add(
    carro.SetHeight(tamanho)
        .SetWidth(tamanho)
        .SetFixedPosition(document.GetLeftMargin() + 5, document.GetBottomMargin() + 290)
);
canvas.ResetFillColorRgb();

paragraph = new Paragraph("\n\n\nNome do carro").SetBold().SetFontSize(13);
canvas.MakeRectangle(x: 39, y: 195, width: 515, height: 130, color: lightGray);
document.Add(paragraph); 

table = new Table(new[] { 110F, 150F, 110F, 150F });
new[] { "Placa", "Ano", "Cor", "Combustível", "Chassi", "Renavam", "Município", "N Motor", "Marca", "Modelo" }
    .ToList()
    .ForEach((item) => {
        bool background = item.IsOneOf("Placa", "Ano", "Chassi", "Renavam", "Marca", "Modelo");
        table.AddHeader(item, background);
        table.AddCellSmall("", background);
    });
document.Add(table);

paragraph = new Paragraph($"Página {pdf.GetPageNumber(pdf.GetFirstPage())} de 4")
    .SetFixedPosition(document.GetLeftMargin() + 200, ps.GetBottom() + 10, 505);

document.Add(paragraph);
document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
/* ↑ Fim página 1 ↑ */

/* ↓ Página 2  ↓ */
canvas = new PdfCanvas(pdf.GetPage(2));
canvas.MakeRectangle(x: 35, y: 680, width: 520, height: 120, color: lightGray);
canvas.MakeRectangle(x: 35, y: 535, width: 520, height: 140, color: lightGray); 

paragraph = new Paragraph("INFORMAÇÕES ").SetFontColor(ColorConstants.RED);
document.Add(paragraph.Add(new Text("TÉCNICAS").SetBold()));

table = new Table(new[] { 150F, 100F, 150F, 100F });
new[] {
    "Espécie", "Procedência", "Tipo", "Potência",
    "Cilindrada", "Peso bruto", "Capacidade de passageiros", "Capacidade de carga" 
}.ToList()
.ForEach((item) => {
    bool background = item.IsOneOf("Espécie", "Procedência", "Cilindrada", "Peso bruto");
    table.AddHeader(item, background);
    table.AddCellSmall("", background);
});
document.Add(table);

paragraph = new Paragraph("\nDADOS ").SetFontColor(ColorConstants.RED);
document.Add(paragraph.Add(new Text("PROPRIETÁRIO").SetBold()));

table = new Table(new[] { 70F, 90F, 110F, 150F, 90F });
new[] { "Tipo", "Documento", "Nome", "Endereço", "Cidade-UF" }
    .ToList()
    .ForEach(item => {
        table.AddHeader(item, false);
    });
table.AddHeaderLarge("ATUAL", true); 
table.AddCell("", true); table.AddCell("", true); table.AddCell("", true); table.AddCell("", true);
table.AddHeaderLarge("ANTERIOR", false);
table.AddCell(""); table.AddCell(""); table.AddCell(""); table.AddCell("");
document.Add(table);
/* ↑ Fim página 2 ↑ */

document.Close();