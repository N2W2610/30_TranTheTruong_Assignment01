using _30_TranTheTruong_Assignment01_FrontEnd.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace _30_TranTheTruong_Assignment01_FrontEnd.Services
{
    public class ApiClient
    {
        // OData deserialisation on the server is case sensitive -> keep PascalCase when sending.
        private static readonly JsonSerializerOptions Json = new()
        {
            PropertyNamingPolicy = null,
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _context;

        public ApiClient(HttpClient http, IHttpContextAccessor context)
        {
            _http = http;
            _context = context;
        }

        private class ODataList<T>
        {
            [JsonPropertyName("value")]
            public List<T> Value { get; set; } = new();
        }

        // ------------------------------------------------------------ auth
        public Task<ApiResult<LoginResponseVM>> LoginAsync(LoginVM model) =>
            SendAsync<LoginResponseVM>(HttpMethod.Post, "api/auth/login", new { model.Email, model.Password }, throwOn401: false);

        // ------------------------------------------------------------ accounts (Admin)
        public Task<ApiResult<List<AccountVM>>> GetAccountsAsync(string? search)
        {
            var url = "odata/SystemAccounts?$orderby=" + Uri.EscapeDataString("AccountId");
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = Lit(search);
                var filter = $"contains(tolower(AccountName),'{s}') or contains(tolower(AccountEmail),'{s}')";
                url += "&$filter=" + Uri.EscapeDataString(filter);
            }
            return GetListAsync<AccountVM>(url);
        }

        public Task<ApiResult<object>> CreateAccountAsync(AccountVM m) =>
            SendAsync<object>(HttpMethod.Post, "odata/SystemAccounts", new
            {
                m.AccountName,
                m.AccountEmail,
                m.AccountRole,
                m.AccountPassword
            });

        public Task<ApiResult<object>> UpdateAccountAsync(AccountVM m) =>
            SendAsync<object>(HttpMethod.Put, $"odata/SystemAccounts({m.AccountId})", new
            {
                m.AccountId,
                m.AccountName,
                m.AccountEmail,
                m.AccountRole,
                AccountPassword = string.IsNullOrWhiteSpace(m.AccountPassword) ? null : m.AccountPassword
            });

        public Task<ApiResult<object>> DeleteAccountAsync(short id) =>
            SendAsync<object>(HttpMethod.Delete, $"odata/SystemAccounts({id})");

        // ------------------------------------------------------------ categories (Staff)
        public Task<ApiResult<List<CategoryVM>>> GetCategoriesAsync(string? search)
        {
            var url = "odata/Categories?$orderby=" + Uri.EscapeDataString("CategoryId");
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = Lit(search);
                var filter = $"contains(tolower(CategoryName),'{s}') or contains(tolower(CategoryDesciption),'{s}')";
                url += "&$filter=" + Uri.EscapeDataString(filter);
            }
            return GetListAsync<CategoryVM>(url);
        }

        public Task<ApiResult<object>> CreateCategoryAsync(CategoryVM m) =>
            SendAsync<object>(HttpMethod.Post, "odata/Categories", new
            {
                m.CategoryName,
                m.CategoryDesciption,
                m.ParentCategoryId,
                m.IsActive
            });

        public Task<ApiResult<object>> UpdateCategoryAsync(CategoryVM m) =>
            SendAsync<object>(HttpMethod.Put, $"odata/Categories({m.CategoryId})", new
            {
                m.CategoryId,
                m.CategoryName,
                m.CategoryDesciption,
                m.ParentCategoryId,
                m.IsActive
            });

        public Task<ApiResult<object>> DeleteCategoryAsync(short id) =>
            SendAsync<object>(HttpMethod.Delete, $"odata/Categories({id})");

        // ------------------------------------------------------------ tags
        public Task<ApiResult<List<TagVM>>> GetTagsAsync() => GetListAsync<TagVM>("odata/Tags?$orderby=TagName");

        // ------------------------------------------------------------ news
        public Task<ApiResult<List<NewsVM>>> GetNewsAsync(string? search, short? createdById, bool onlyActive)
        {
            var filters = new List<string>();
            if (onlyActive) filters.Add("NewsStatus eq true");
            if (createdById.HasValue) filters.Add($"CreatedById eq {createdById.Value}");
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = Lit(search);
                filters.Add($"(contains(tolower(NewsTitle),'{s}') or contains(tolower(Headline),'{s}') or contains(tolower(NewsContent),'{s}'))");
            }

            var url = "odata/NewsArticles?$expand=" + Uri.EscapeDataString("Category,CreatedBy,Tags")
                      + "&$orderby=" + Uri.EscapeDataString("CreatedDate desc");
            if (filters.Count > 0) url += "&$filter=" + Uri.EscapeDataString(string.Join(" and ", filters));

            return GetListAsync<NewsVM>(url);
        }

        public Task<ApiResult<NewsVM>> GetNewsByIdAsync(string id) =>
            SendAsync<NewsVM>(HttpMethod.Get,
                $"odata/NewsArticles('{Lit(id)}')?$expand=" + Uri.EscapeDataString("Category,CreatedBy,Tags"));

        public Task<ApiResult<object>> CreateNewsAsync(NewsFormVM m) =>
            SendAsync<object>(HttpMethod.Post, "api/news", ToNewsBody(m));

        public Task<ApiResult<object>> UpdateNewsAsync(NewsFormVM m) =>
            SendAsync<object>(HttpMethod.Put, $"api/news/{Uri.EscapeDataString(m.NewsArticleId ?? "")}", ToNewsBody(m));

        public Task<ApiResult<object>> DeleteNewsAsync(string id) =>
            SendAsync<object>(HttpMethod.Delete, $"api/news/{Uri.EscapeDataString(id)}");

        private static object ToNewsBody(NewsFormVM m) => new
        {
            m.NewsTitle,
            m.Headline,
            m.NewsContent,
            m.NewsSource,
            m.CategoryId,
            m.NewsStatus,
            m.TagIds
        };

        // ------------------------------------------------------------ profile (Staff)
        public Task<ApiResult<ProfileVM>> GetProfileAsync() => SendAsync<ProfileVM>(HttpMethod.Get, "api/profile");

        public Task<ApiResult<object>> UpdateProfileAsync(ProfileVM m) =>
            SendAsync<object>(HttpMethod.Put, "api/profile", new { m.AccountName, m.AccountEmail, m.NewPassword });

        // ------------------------------------------------------------ report (Admin)
        public Task<ApiResult<ReportVM>> GetReportAsync(DateTime start, DateTime end) =>
            SendAsync<ReportVM>(HttpMethod.Get, $"api/report?startDate={start:yyyy-MM-dd}&endDate={end:yyyy-MM-dd}");

        // ------------------------------------------------------------ plumbing
        // Escapes a value for use inside an OData string literal ('...')
        private static string Lit(string value) => value.Trim().ToLowerInvariant().Replace("'", "''");

        private async Task<ApiResult<List<T>>> GetListAsync<T>(string url)
        {
            var result = await SendAsync<ODataList<T>>(HttpMethod.Get, url);
            return result.Success
                ? ApiResult<List<T>>.Ok(result.Data?.Value ?? new List<T>(), result.Status)
                : ApiResult<List<T>>.Fail(result.Error ?? "Request failed.", result.Status);
        }

        private async Task<ApiResult<T>> SendAsync<T>(HttpMethod method, string url, object? body = null, bool throwOn401 = true)
        {
            using var request = new HttpRequestMessage(method, url);

            var token = _context.HttpContext?.Session.GetString("token");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            if (body != null)
            {
                request.Content = JsonContent.Create(body, body.GetType(), null, Json);
            }

            try
            {
                using var response = await _http.SendAsync(request);
                var text = await response.Content.ReadAsStringAsync();
                var status = (int)response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    T? data = default;
                    if (!string.IsNullOrWhiteSpace(text) && typeof(T) != typeof(object))
                    {
                        data = JsonSerializer.Deserialize<T>(text, Json);
                    }
                    return ApiResult<T>.Ok(data, status);
                }

                if (status == 401 && throwOn401 && !string.IsNullOrEmpty(token))
                {
                    throw new ApiUnauthorizedException();
                }

                return ApiResult<T>.Fail(ExtractError(status, text), status);
            }
            catch (HttpRequestException ex)
            {
                return ApiResult<T>.Fail("Cannot reach the API server: " + ex.Message, 503);
            }
            catch (JsonException ex)
            {
                return ApiResult<T>.Fail("Unexpected response from the API: " + ex.Message, 500);
            }
        }

        private static string ExtractError(int status, string body)
        {
            if (status == 401) return "Invalid credentials or your session has expired.";
            if (status == 403) return "You do not have permission to perform this action.";

            try
            {
                using var doc = JsonDocument.Parse(body);
                var root = doc.RootElement;
                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("error", out var error))
                    {
                        if (error.ValueKind == JsonValueKind.String) return error.GetString() ?? "Request failed.";
                        if (error.ValueKind == JsonValueKind.Object && error.TryGetProperty("message", out var msg))
                            return msg.GetString() ?? "Request failed.";
                    }

                    if (root.TryGetProperty("errors", out var errors) && errors.ValueKind == JsonValueKind.Object)
                    {
                        var messages = new List<string>();
                        foreach (var property in errors.EnumerateObject())
                        {
                            if (property.Value.ValueKind != JsonValueKind.Array) continue;
                            foreach (var item in property.Value.EnumerateArray())
                            {
                                var m = item.GetString();
                                if (!string.IsNullOrWhiteSpace(m)) messages.Add(m);
                            }
                        }
                        if (messages.Count > 0) return string.Join(" ", messages);
                    }

                    if (root.TryGetProperty("title", out var title) && title.ValueKind == JsonValueKind.String)
                        return title.GetString() ?? "Request failed.";
                }
            }
            catch (JsonException)
            {
                // body is not JSON - fall through
            }

            return status == 404 ? "The requested item was not found." : $"Request failed (HTTP {status}).";
        }
    }

}
