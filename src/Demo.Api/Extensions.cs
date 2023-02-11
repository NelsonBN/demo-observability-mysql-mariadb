namespace Demo.Api;

public static class Extensions
{
    public static string ToDatabaseName(this string name)
        => (name?.ToLower()) switch
        {
            "mysql" => "MySQL",
            "mariadb" => "MariaDB",
            _ => null
        };
}
