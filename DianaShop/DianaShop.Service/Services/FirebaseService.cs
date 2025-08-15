using DianaShop.Service.ReponseModel;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

public interface IFirebaseService
{
    Task<FirebaseRespondModel> GetSensorData(String deviceId);
}

public class FirebaseService : IFirebaseService
{
    private readonly FirebaseClient _firebaseClient;

    public FirebaseService(IConfiguration configuration)
    {
        var dbUrl = configuration["Firebase:DatabaseUrl"];
        var secret = configuration["Firebase:Secret"];

        _firebaseClient = new FirebaseClient(
            dbUrl,
            new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(secret)
            });
    }

    public async Task<FirebaseRespondModel> GetSensorData(String deviceId)
    {
        var data = await _firebaseClient
            .Child(deviceId)
            .OnceSingleAsync<FirebaseRoot>();

        var parsedTime = DateTime.ParseExact(
        data.time,
        "dd/MM/yyyy - HH:mm:ss",
        CultureInfo.InvariantCulture
    );
        return new FirebaseRespondModel
        {
            ImageBase64 = data.camera.image.data,
            Temperature = data.sensor.temp.data,
            CreatedDate = parsedTime
        };

        //if (data == null || data.Count == 0)
        //    return null;

        //var firstDevice = data.First().Value;

        //// Parse time "14/08/2025 - 23:01:13"
        //DateTime createdDate;
        //if (!DateTime.TryParseExact(firstDevice.time?.data,
        //    "dd/MM/yyyy - HH:mm:ss",
        //    CultureInfo.InvariantCulture,
        //    DateTimeStyles.None,
        //    out createdDate))
        //{
        //    createdDate = DateTime.MinValue;
        //}

        //return new FirebaseRespondModel
        //{
        //    ImageBase64 = firstDevice.camera?.image?.data ?? "",
        //    Temperature = firstDevice.sensor?.temp?.data ?? 0,
        //    CreatedDate = createdDate
        //};
    }
}