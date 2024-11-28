using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class PicusController : ControllerBase
{
    // Sınıf seviyesinde alanları tanımlayın
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string _tableName;
    private readonly Table _table;

    // Constructor ile alanları başlatın
    public PicusController(IAmazonDynamoDB dynamoDb, IConfiguration configuration)
    {
        _dynamoDb = dynamoDb; // DynamoDB client
        _tableName = configuration["AWS:TableName"]; // appsettings.json'dan tablo adı
        _table = Table.LoadTable(_dynamoDb, _tableName); // DynamoDB tablosunu yükle
    }

    // Health Check metodu
    [Route("picus/health")]
    [HttpGet]
    public async Task<IActionResult> HealthCheck()
    {
        try
        {
            // DynamoDB tablosunun durumunu kontrol et
            var response = await _dynamoDb.DescribeTableAsync(_tableName);

            return Ok(new
            {
                status = "Healthy",
                tableStatus = response.Table.TableStatus,
                tableName = _tableName,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "Unhealthy",
                error = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    // Tüm verileri listeleme
    [Route("picus/list")]
    [HttpGet]
    public async Task<IActionResult> ListItems()
    {
        var items = await _table.Scan(new ScanOperationConfig()).GetRemainingAsync();

    // Document nesnesini temiz bir JSON nesnesine dönüştür
    var formattedItems = items.Select(item =>
        System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(item.ToJson())
    );

    return Ok(formattedItems);
    }

    // Yeni veri ekleme
    [Route("picus/put")]
    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] Dictionary<string, object> item)
    {
        item["id"] = Guid.NewGuid().ToString(); // Benzersiz bir ID oluştur
        var document = Document.FromJson(System.Text.Json.JsonSerializer.Serialize(item));
        await _table.PutItemAsync(document);
        return Ok(new { id = item["id"] });
    }

    // Belirli bir veriyi getirme
    [Route("picus/get/{id}")]
    [HttpGet]
    public async Task<IActionResult> GetItem(string id)
    {
        var document = await _table.GetItemAsync(id);
        if (document == null) return NotFound();
        return Ok(document.ToJson());
    }
}
