using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using AGOC.Domain.Interfaces;

public class LdapAuthenticationService
{
    private readonly string? _path;
    private readonly string? _userId;
    private readonly string? _username;
    private readonly IUsersManager _usersManager;

    public LdapAuthenticationService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IUsersManager usersManager)
    {
        _path = configuration["Ldap:Path"];
        _userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _username = httpContextAccessor.HttpContext?.User.Identity?.Name;
        _usersManager = usersManager;
    }

    public async Task<LdapValidationResult> ValidateUserAsync(string username, string password)
    {
        var result = new LdapValidationResult();

        if (string.IsNullOrEmpty(username))
        {
            result.IsValid = false;
            result.ErrorMessage = "يجب إدخال اسم المستخدم";
            LogEvent("Validation Failed", "Username is missing.", EventLogEntryType.Warning);
            return result;
        }

        if (string.IsNullOrEmpty(password))
        {
            result.IsValid = false;
            result.ErrorMessage = "يجب إدخال كلمة السر";
            LogEvent("Validation Failed", "Password is missing.", EventLogEntryType.Warning);
            return result;
        }

        var user = await _usersManager.GetUser(username);
        if (user == null)
        {
            result.IsValid = false;
            result.ErrorMessage = "اسم المستخدم غير موجود";
            LogEvent("Validation Failed", $"User not found: {username}", EventLogEntryType.Warning);
            return result;
        }

        try
        {
            //using (var entry = new DirectoryEntry(_path, username, password))
            //{
            //    object nativeObject = entry.NativeObject;

            //    using (var search = new DirectorySearcher(entry))
            //    {
            //        search.Filter = $"(SAMAccountName={username})";
            //        search.PropertiesToLoad.Add("cn");

            //        var searchResult = search.FindOne();

            //        if (searchResult == null)
            //        {
            //            result.IsValid = false;
            //            result.ErrorMessage = "اسم المستخدم غير موجود";
            //            LogEvent("Validation Failed", $"Search result not found for user: {username}", EventLogEntryType.Warning);
            //            return result;
            //        }
            //    }
            //}

            result.IsValid = AuthenticateUser_Linux("10.10.10.9", username + "@pp.gov.qa", password);
            result.Roles = await _usersManager.GetUserRoles(user.Id);
            LogEvent("Validation Succeeded", $"User {username} authenticated successfully.", EventLogEntryType.Information);
        }
        catch (DirectoryServicesCOMException ex)
        {
            EventLog.WriteEntry("VMS", ex.Source + " - " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Error);
            result.IsValid = false;
            result.ErrorMessage = "اسم المستخدم او كلمة السر غير صحيحة";
            LogEvent("DirectoryServicesCOMException", $"Authentication failed for user {username}.", EventLogEntryType.Error);
        }
        catch (Exception ex)
        {
            EventLog.WriteEntry("VMS", ex.Source + " - " + ex.Message + " - " + ex.StackTrace, EventLogEntryType.Error);
            result.IsValid = false;
            result.ErrorMessage = "حدث خطأ غير متوقع: " + ex.Message;
            await LogErrorAsync(ex);
            LogEvent("Unexpected Error", $"Unexpected error occurred for user {username}. Error: {ex.Message}", EventLogEntryType.Error);
        }

        return result;
    }

    private static bool AuthenticateUser_Linux(string ldapServer, string userDn, string password)
    {
        bool IsValidLogin = false;
        try
        {
            using (LdapConnection connection = new LdapConnection(new LdapDirectoryIdentifier(ldapServer)))
            {
                connection.Credential = new NetworkCredential(userDn, password);
                connection.AuthType = AuthType.Basic;

                connection.Timeout = new TimeSpan(0, 0, 30);

                Console.WriteLine("Attempting to bind to the LDAP server...");
                connection.Bind(); // Attempt to authenticate
                Console.WriteLine("Bind successful.");

                IsValidLogin = true;
            }
        }
        catch (LdapException ldapEx)
        {
            Console.WriteLine($"LDAP Exception during authentication: {ldapEx.Message}");
            Console.WriteLine($"Error Code: {ldapEx.ErrorCode}");
            if (ldapEx.ServerErrorMessage != null)
            {
                Console.WriteLine($"Server Error Message: {ldapEx.ServerErrorMessage}");
            }
            IsValidLogin = false;
        }
        catch (TimeoutException timeoutEx)
        {
            Console.WriteLine($"Timeout Exception: {timeoutEx.Message}");
            IsValidLogin = false;
        }
        catch (AuthenticationException authEx)
        {
            Console.WriteLine($"Authentication Exception: {authEx.Message}");
            IsValidLogin = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred during authentication: {ex.Message}");
            IsValidLogin = false;
        }
        return IsValidLogin;
    }

    //private bool LDapLogin(string username, string password)
    //{
    //    string ldapServer = "10.10.10.9"; // IP address of the LDAP server

    //    return IsValidLogin;

    //}
    private void LogEvent(string title, string message, EventLogEntryType entryType)
    {
        //try
        //{
        //    if (OperatingSystem.IsWindows())
        //    {
        //        const string source = "VMS";
        //        const string log = "Application";

        //        if (!EventLog.SourceExists(source))
        //        {
        //            EventLog.CreateEventSource(source, log);
        //        }

        //        EventLog.WriteEntry(source, $"{title}: {message}", entryType);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    // Handle any exceptions that occur during the logging process
        //    Trace.TraceError($"Failed to log to event viewer: {ex.Message}\n\n{ex.StackTrace}");
        //}
    }

    private async Task LogErrorAsync(Exception ex)
    {
        try
        {
            LogEvent("Error", $"{ex.Message}\n\n{ex.StackTrace}", EventLogEntryType.Error);
        }
        catch (Exception logEx)
        {
            Trace.TraceError($"Failed to log to event viewer: {logEx.Message}\n\n{logEx.StackTrace}");
        }
    }
}

public class LdapValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string>? Roles { get; set; }
}