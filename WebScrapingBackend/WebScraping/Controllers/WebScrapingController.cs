using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebScraping.Dto;
using WebScraping.Entities;
using WebScraping.Services;
using System.Net;
using Google.Protobuf;
using Spire.Pdf;
using System.Text;


namespace WebScraping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebScrapingController : ControllerBase
    {
        private readonly YayinService _yayinService;
        private readonly Services.IElasticsearchService<Yayin> _elasticsearchService;
        private readonly IHttpClientFactory _httpClientFactory;
        public WebScrapingController(YayinService yayinService, IElasticsearchService<Yayin> elasticsearchService, IHttpClientFactory httpClientFactory)
        {
            _yayinService = yayinService;
            this._elasticsearchService = elasticsearchService;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("scholar")]
        public async Task<IActionResult> Post([FromBody] SearchRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.SearchText))
            {
                return BadRequest(new { proccess = false, message = "SearchText boş olamaz." });
            }

            try
            {
                string searchUrl = $"https://scholar.google.com/scholar?q={request.SearchText}";
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = await web.LoadFromWebAsync(searchUrl);

                if (doc == null)
                {
                    return BadRequest(new { proccess = false, message = "Hata oluştu urlde." });
                }

                var titles = doc.DocumentNode.SelectNodes("//h3[@class='gs_rt']/a")
                    .Select(node => new SearchResult
                    {
                        Title = node.InnerText.Trim(),
                        Url = node.Attributes["href"].Value.Trim()
                    }).ToList();

                List<Yayin> yayinListesi = new List<Yayin>();

                foreach (var item in titles)
                {
                    Yayin yayin = new Yayin();
                    if (item.Url.Contains(".pdf"))
                    {

                    }
                    else
                    {

                        try
                        {

                            //Ozeti alıyor
                            HtmlDocument doc2 = await web.LoadFromWebAsync(item.Url);

                            var metaTaglar = doc2.DocumentNode.SelectNodes("//meta");

                            if (metaTaglar != null)
                            {
                                foreach (var metaTagEtiket in metaTaglar)
                                {

                                    var name = metaTagEtiket.Attributes["name"];
                                    var property = metaTagEtiket.Attributes["property"];

                                    if ((name != null && (name.Value.Contains("author", StringComparison.OrdinalIgnoreCase) || name.Value.Contains("creator", StringComparison.OrdinalIgnoreCase))) ||
                                        (property != null && (property.Value.Contains("author", StringComparison.OrdinalIgnoreCase) || property.Value.Contains("creator", StringComparison.OrdinalIgnoreCase))))
                                    {

                                        string metin = metaTagEtiket.Attributes["content"]?.Value;
                                        if (!string.IsNullOrEmpty(metin))
                                        {
                                            if (string.IsNullOrEmpty(yayin.Yazarlar))
                                            {
                                                yayin.Yazarlar = metin;
                                            }
                                            else
                                            {
                                                yayin.Yazarlar = yayin.Yazarlar + " , " + metin;
                                            }
                                        }
                                    }

                                    if ((name != null && (name.Value.Contains("publisher", StringComparison.OrdinalIgnoreCase) || name.Value.Contains("Publisher", StringComparison.OrdinalIgnoreCase))) ||
                                        (property != null && (property.Value.Contains("publisher", StringComparison.OrdinalIgnoreCase) || property.Value.Contains("Publisher", StringComparison.OrdinalIgnoreCase))))
                                    {
                                        string metin = metaTagEtiket.Attributes["content"]?.Value;
                                        if (!string.IsNullOrEmpty(metin))
                                        {
                                            yayin.YayinciAdi = metin;
                                        }
                                    }

                                    if ((name != null && (name.Value.Contains("keywords", StringComparison.OrdinalIgnoreCase) || name.Value.Contains("Keywords", StringComparison.OrdinalIgnoreCase))) ||
                                        (property != null && (property.Value.Contains("keywords", StringComparison.OrdinalIgnoreCase) || property.Value.Contains("Keywords", StringComparison.OrdinalIgnoreCase))))
                                    {
                                        string metin = metaTagEtiket.Attributes["content"]?.Value;
                                        if (!string.IsNullOrEmpty(metin))
                                        {
                                            yayin.AnahtarKelimelerMakaleyeAit = metin;
                                        }
                                    }


                                    if ((name != null && (name.Value.Contains("type", StringComparison.OrdinalIgnoreCase) || name.Value.Contains("Type", StringComparison.OrdinalIgnoreCase))) ||
                                       (property != null && (property.Value.Contains("type", StringComparison.OrdinalIgnoreCase) || property.Value.Contains("Type", StringComparison.OrdinalIgnoreCase))))
                                    {
                                        string metin = metaTagEtiket.Attributes["content"]?.Value;
                                        if (!string.IsNullOrEmpty(metin))
                                        {
                                            yayin.Tur = metin;
                                        }
                                    }

                                    if (
                                        (name != null && (
                                        (name.Value.Contains("citation", StringComparison.OrdinalIgnoreCase) && name.Value.Contains("volume", StringComparison.OrdinalIgnoreCase)) ||
                                        (name.Value.Contains("Citation", StringComparison.OrdinalIgnoreCase) && name.Value.Contains("volume", StringComparison.OrdinalIgnoreCase)) ||
                                        (name.Value.Contains("citation", StringComparison.OrdinalIgnoreCase) && name.Value.Contains("Volume", StringComparison.OrdinalIgnoreCase)) ||
                                        (name.Value.Contains("Citation", StringComparison.OrdinalIgnoreCase) && name.Value.Contains("Volume", StringComparison.OrdinalIgnoreCase))
                                        )) ||
                                        (property != null && (
                                        (property.Value.Contains("citation", StringComparison.OrdinalIgnoreCase) && property.Value.Contains("volume", StringComparison.OrdinalIgnoreCase)) ||
                                        (property.Value.Contains("Citation", StringComparison.OrdinalIgnoreCase) && property.Value.Contains("volume", StringComparison.OrdinalIgnoreCase)) ||
                                        (property.Value.Contains("citation", StringComparison.OrdinalIgnoreCase) && property.Value.Contains("Volume", StringComparison.OrdinalIgnoreCase)) ||
                                        (property.Value.Contains("Citation", StringComparison.OrdinalIgnoreCase) && property.Value.Contains("Volume", StringComparison.OrdinalIgnoreCase))
                                        ))
                                    )
                                    {
                                        string metin = metaTagEtiket.Attributes["content"]?.Value;
                                        if (!string.IsNullOrEmpty(metin))
                                        {
                                            yayin.AlintiSayisi = int.Parse(metin);
                                        }
                                    }


                                    if (
                                        (name != null && (
                                        (name.Value.Contains("publication", StringComparison.OrdinalIgnoreCase) && name.Value.Contains("date", StringComparison.OrdinalIgnoreCase)) ||
                                        (name.Value.Contains("Publication", StringComparison.OrdinalIgnoreCase) && name.Value.Contains("date", StringComparison.OrdinalIgnoreCase)) ||
                                        (name.Value.Contains("Publication", StringComparison.OrdinalIgnoreCase) && name.Value.Contains("Date", StringComparison.OrdinalIgnoreCase)) ||
                                        (name.Value.Contains("publication", StringComparison.OrdinalIgnoreCase) && name.Value.Contains("Date", StringComparison.OrdinalIgnoreCase))
                                        )) ||
                                        (property != null && (
                                        (property.Value.Contains("publication", StringComparison.OrdinalIgnoreCase) && property.Value.Contains("date", StringComparison.OrdinalIgnoreCase)) ||
                                        (property.Value.Contains("Publication", StringComparison.OrdinalIgnoreCase) && property.Value.Contains("date", StringComparison.OrdinalIgnoreCase)) ||
                                        (property.Value.Contains("Publication", StringComparison.OrdinalIgnoreCase) && property.Value.Contains("Date", StringComparison.OrdinalIgnoreCase)) ||
                                        (property.Value.Contains("publication", StringComparison.OrdinalIgnoreCase) && property.Value.Contains("Date", StringComparison.OrdinalIgnoreCase))
                                        ))
                                    )
                                    {
                                        string metin = metaTagEtiket.Attributes["content"]?.Value;
                                        if (!string.IsNullOrEmpty(metin) && DateTime.TryParse(metin, out DateTime tarih))
                                        {
                                            yayin.YayinlanmaTarihi = tarih;
                                        }

                                    }


                                }
                            }

                            var absMetin = doc2.DocumentNode.SelectNodes(
                                   "//*[" +
                                   "contains(translate(@class, 'ABS', 'abs'), 'abs') or " +
                                   "contains(translate(@id, 'ABS', 'abs'), 'abs') or " +
                                   "contains(translate(@class, 'ABSTRACT', 'abstract'), 'abstract') or " +
                                   "contains(translate(@id, 'ABSTRACT', 'abstract'), 'abstract')]" +
                                   "//text()");

                            if (absMetin != null)
                            {
                                string yeniMetin = string.Join(" ", absMetin.Select(node => node.InnerText.Trim()));
                                string yeniMetin2 = Regex.Replace(yeniMetin, @"<[^>]+>|&nbsp;", "").Trim();
                                yayin.Ozet = yeniMetin2;
                            }


                            var aTaglar = doc2.DocumentNode.SelectNodes("//*[@href]");
                            if (aTaglar != null)
                            {
                                foreach (var aTag in aTaglar)
                                {
                                    var href = aTag.GetAttributeValue("href", "");
                                    if (href.Contains("doi.org"))
                                    {
                                        yayin.DoiNumarasi = href;
                                    }
                                }
                            }



                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Hata: {ex.Message}");
                            continue;
                        }
                    }



                    yayin.AnahtarKelimelerAramaMotoru = request.SearchText;
                    yayin.Ad = item.Title;
                    yayin.Url = item.Url;
                    yayin.Tur = "Araştırma Makalesi";

                    await _yayinService.CreateAsync(yayin);
                    //await _elasticsearchService.CreateDocumentAsync(yayin);
                    yayinListesi.Add(yayin);
                }

                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }


        }

        [HttpPost("dergipark")]
        public async Task<IActionResult> PostDergiPark([FromBody] SearchRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.SearchText))
            {
                return BadRequest(new { proccess = false, message = "SearchText boş olamaz." });
            }

            try
            {
                string searchUrl = $"https://dergipark.org.tr/tr/search?q={request.SearchText}&section=articles";
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = await web.LoadFromWebAsync(searchUrl);

                if (doc == null)
                {
                    return BadRequest(new { proccess = false, message = "Hata oluştu urlde" });
                }

                var titles = doc.DocumentNode.SelectNodes("//h5[@class='card-title']/a")
                    .Select(node => new SearchResult
                    {
                        Title = node.InnerText.Trim(),
                        Url = node.Attributes["href"].Value.Trim()
                    }).ToList();

                List<Yayin> yayinListesi = new List<Yayin>();
                List<Yayin> yayinListesiresponse = new List<Yayin>();

                foreach (var item in titles)
                {
                    Yayin yayin = new Yayin();
                    //yayin.Id = ObjectId.GenerateNewId();
                    HtmlDocument doc2 = await web.LoadFromWebAsync(item.Url);


                    var tabPaneDiv = doc2.DocumentNode.SelectSingleNode("//div[contains(@class, 'tab-pane') and contains(@class, 'active')]");
                    if (tabPaneDiv != null)
                    {
                        var abstractMetin = tabPaneDiv.SelectSingleNode(".//div[@class='article-abstract data-section']");
                        if (abstractMetin != null)
                        {
                            string yeniMetin = Regex.Replace(abstractMetin.InnerText.Trim(), @"<[^>]+>|&nbsp;", "").Trim();
                            yayin.Ozet = yeniMetin;
                        }
                    }




                    var pdfLinkNode = doc2.DocumentNode.SelectSingleNode("//a[@class='btn btn-sm float-left article-tool pdf d-flex align-items-center']");
                    if (pdfLinkNode != null)
                    {
                        var link= pdfLinkNode.GetAttributeValue("href", "");
                        yayin.PdfLinki = "https://dergipark.org.tr"+link;
                        //await DownloadPdf(link, link);
                    }

                    var yazarP = doc2.DocumentNode.SelectSingleNode("//p[@class='article-authors']");
                    if (yazarP != null)
                    {
                        string authorsText = yazarP.InnerText.Trim();
                        yayin.Yazarlar = authorsText;
                        yayin.YayinciAdi = authorsText;
                    }



                    var aTaglar = doc2.DocumentNode.SelectNodes("//*[@href]");
                    if (aTaglar != null)
                    {
                        foreach (var aTag in aTaglar)
                        {
                            var href = aTag.GetAttributeValue("href", "");
                            if (href.Contains("doi.org"))
                            {
                                yayin.DoiNumarasi = href;
                            }
                        }
                    }


                    HtmlNodeCollection keywordDugumleri = doc2.DocumentNode.SelectNodes("//div[@class='article-keywords data-section']//a");
                    List<string> keywordler = new List<string>();
                    if (keywordDugumleri != null)
                    {
                        foreach (HtmlNode dugum in keywordDugumleri)
                        {
                            string keyword = dugum.InnerText.Trim();
                            keywordler.Add(keyword);
                        }
                    }
                    yayin.AnahtarKelimelerMakaleyeAit = string.Join(",", keywordler);


                    HtmlNode yayinlanmaTarihiDugum = doc2.DocumentNode.SelectSingleNode("//th[text()='Yayımlanma Tarihi']/following-sibling::td");
                    if (yayinlanmaTarihiDugum != null)
                    {
                        if (DateTime.TryParse(yayinlanmaTarihiDugum.InnerText.Trim(), out DateTime tarih))
                        {
                            yayin.YayinlanmaTarihi = tarih;
                        }
                    }

                    yayin.Tur = "Araştırma Makalesi";
                    yayin.AnahtarKelimelerAramaMotoru = request.SearchText;
                    yayin.Ad = item.Title;
                    yayin.Url = item.Url;
                    //yayin.Id = ObjectId.GenerateNewId();
                    yayinListesi.Add(yayin);
                    await _yayinService.CreateAsync(yayin);
                }
                yayinListesiresponse = await _yayinService.GetAsync();
                return Ok(new { proccess = true, yayinListesi = yayinListesiresponse });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }

        [HttpGet("/detay/{id}")]
        public async Task<IActionResult> detay(string id)
        {
            if (id == null)
            {
                return BadRequest(new { proccess = false, message = "Id boş olamaz." });
            }

            try
            {
                Yayin yayin = await _yayinService.GetAsync(id);
                return Ok(new { proccess = true, yayin });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }

        [HttpGet("/yayinlar")]
        public async Task<IActionResult> yayinlar()
        {
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {
                yayinListesi = await _yayinService.GetAsync();
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        [HttpGet("Filtrele/YayinAdi/{ad}")]
        public async Task<IActionResult> filtreleYayinAdi(string ad)
        {
            if (ad == null)
            {
                return BadRequest(new { proccess = false, message = "filtrele yayinAdi boş olamaz." });
            }
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {

                yayinListesi=await _yayinService.GetContainsYayinAdiAsync(ad);
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }


        [HttpGet("Filtrele/YayinlanmaTarihiEnSon")]
        public async Task<IActionResult> YayinlanmaTarihiEnSon()
        {
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {
                yayinListesi = await _yayinService.GetYayinlanmaTarihiEnSonAsync();
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }

        [HttpGet("Filtrele/YayinlanmaTarihiEnOnce")]
        public async Task<IActionResult> YayinlanmaTarihiEnOnce()
        {
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {
                yayinListesi = await _yayinService.GetYayinlanmaTarihiEnOnceAsync();
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }



        [HttpGet("Filtrele/YayinciAdi/{text}")]
        public async Task<IActionResult> GetContainsYayinciAdi(string text)
        {
            if (text == null)
            {
                return BadRequest(new { proccess = false, message = "filtrele YayinciAdi boş olamaz." });
            }
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {
                yayinListesi = await _yayinService.GetContainsYayinciAdiAsync(text);
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }

        [HttpGet("Filtrele/AnahtarKelime/{text}")]
        public async Task<IActionResult> GetContainsAnahtarKelime(string text)
        {
            if (text == null)
            {
                return BadRequest(new { proccess = false, message = "filtrele AnahtarKelime boş olamaz." });
            }
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {
                yayinListesi = await _yayinService.GetContainsAnahtarKelimeAsync(text);
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }

        [HttpGet("Filtrele/Ozet/{text}")]
        public async Task<IActionResult> GetContainsOzet(string text)
        {
            if (text == null)
            {
                return BadRequest(new { proccess = false, message = "filtrele Ozet boş olamaz." });
            }
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {
                yayinListesi = await _yayinService.GetContainsOzetAsync(text);
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }

        [HttpGet("Filtrele/AlintiSayisi/{alintiSayisi}")]
        public async Task<IActionResult> GetAlintiSayisiAsync(int alintiSayisi)
        {
            if (alintiSayisi == null)
            {
                return BadRequest(new { proccess = false, message = "AlintiSayisi boş olamaz." });
            }
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {
                yayinListesi = await _yayinService.GetAlintiSayisiAsync(alintiSayisi);
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }

        [HttpGet("Filtrele/DoiNumarasi/{doiNumarasi}")]
        public async Task<IActionResult> GetDoiNumarasiAsync(string doiNumarasi)
        {
            if (doiNumarasi == null)
            {
                return BadRequest(new { proccess = false, message = "Filtrele doiNumarasi boş olamaz." });
            }
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {
                yayinListesi = await _yayinService.GetDoiNumarasiAsync(doiNumarasi);
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }

        [HttpGet("Filtrele/Url/{text}")]
        public async Task<IActionResult> GetUrlAsync(string text)
        {
            if (text == null)
            { 
                return BadRequest(new { proccess = false, message = "Filtrele url boş olamaz." });
            }
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {
                yayinListesi = await _yayinService.GetUrlAsync(text);
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }

        [HttpGet("Filtrele/Yazarlar/{text}")]
        public async Task<IActionResult> GetYazarlarAsync(string text)
        {
            if (text == null)
            {
                return BadRequest(new { proccess = false, message = "Filtrele yazarlar boş olamaz." });
            }
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {
                yayinListesi = await _yayinService.GetContainsYazarlarAsync(text);
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }


        [HttpGet("Filtrele/Tur/{text}")]
        public async Task<IActionResult> GetTurAsync(string text)
        {
            if (text == null)
            {
                return BadRequest(new { proccess = false, message = "Filtrele tur boş olamaz." });
            }
            List<Yayin> yayinListesi = new List<Yayin>();
            try
            {
                yayinListesi = await _yayinService.GetContainsTurAsync(text);
                return Ok(new { proccess = true, yayinListesi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + ex.Message });

            }

        }

        [HttpGet("pdfIndir/{text}")]
        public async Task<IActionResult> pdfIndir(string text)
        {
            if (text == null)
            {
                return BadRequest(new { proccess = false, message = "Id boş olamaz." });
            }


            try
            {

                var yayin = await _yayinService.GetAsync(text);

                Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                WebClient webClient = new WebClient();

                using (MemoryStream ms = new MemoryStream(webClient.DownloadData(yayin.PdfLinki)))
                {
                    doc.LoadFromStream(ms);
                }
                doc.SaveToFile("result"+text+".pdf", FileFormat.PDF);
                return Ok(new { proccess = true, message = "Pdf indirildi" });
            }
            catch (Exception e)
            {
                return BadRequest(new { proccess = false, message = "Hata oluştu: " + e.Message });
            }


        }


    }
}
