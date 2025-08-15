using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace DianaShop.Service.Helpers
{
    public class MomoLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new MomoCompare());
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new MomoCompare());

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        // Tạo URL yêu cầu đến MoMo với chữ ký bảo mật
        public string CreateRequestUrl(string baseUrl, string momoHashSecret)
        {
            var data = new StringBuilder();

            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            var querystring = data.ToString();

            // Tạo chữ ký bảo mật từ query string và secret
            var signData = querystring.TrimEnd('&');
            var momoSecureHash = MomoUtils.HmacSHA256(momoHashSecret, signData);

            baseUrl += "?" + querystring;
            baseUrl += "momo_SecureHash=" + momoSecureHash;

            return baseUrl;
        }

        // Xác minh chữ ký phản hồi từ MoMo
        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseData();
            var myChecksum = MomoUtils.HmacSHA256(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseData()
        {
            var data = new StringBuilder();

            if (_responseData.ContainsKey("momo_SecureHash"))
            {
                _responseData.Remove("momo_SecureHash");
            }

            foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1); // Xóa ký tự `&` cuối cùng
            }

            return data.ToString();
        }
    }

    // Lớp hỗ trợ các hàm mã hóa và tiện ích chung
    // Lớp hỗ trợ các hàm mã hóa và tiện ích chung
    public class MomoUtils
    {
        public static string HmacSHA256(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
    }


    // Lớp so sánh thứ tự sắp xếp cho MoMo tương tự Vnpay
    public class MomoCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var compare = CompareInfo.GetCompareInfo("en-US");
            return compare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
