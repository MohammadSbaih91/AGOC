using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using AGOC.Models;

namespace AGOC.Services
{
    public class HrService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HrService> _logger;

        public HrService(HttpClient httpClient, ILogger<HrService> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://api.hr.apps.openshift-poc.pp.gov.qa/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Api-Key", "F1396576-1929-44BF-9AE5-E166B2FC974C");
            _logger = logger;
        }

        [HttpGet]
        public async Task<EmployeeInfo?> GetEmployeeByCodeAsync(int? empNumber)
        {
            try
            {
                if (empNumber == null)
                {
                    _logger.LogWarning("GetEmployeeByCodeAsync was called with a null employee number.");
                    return null;
                }

                _logger.LogInformation("Fetching employee data for employee number {EmpNumber}.", empNumber);

                HttpResponseMessage response = await _httpClient.GetAsync($"api/Employee/code/{empNumber}");

                if (response.IsSuccessStatusCode)
                {
                    var empJson = await response.Content.ReadAsStringAsync();
                    var employee = JsonConvert.DeserializeObject<EmployeeInfo>(empJson);

                    _logger.LogInformation("Employee data successfully retrieved for employee number {EmpNumber}.", empNumber);
                    return employee;
                }
                else
                {
                    _logger.LogWarning("Error fetching employee data: {StatusCode} - {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Request error while fetching employee data for employee number {EmpNumber}.", empNumber);
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error while processing employee data for employee number {EmpNumber}.", empNumber);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching employee data for employee number {EmpNumber}.", empNumber);
                return null;
            }
        }
    }
}