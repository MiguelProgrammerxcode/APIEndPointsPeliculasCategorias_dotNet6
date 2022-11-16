namespace PeliculaApi.Utilitarios;

public static class Util
{
    #region "Security"
    
    public static bool VerificaPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
        var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return !hashComputado.Where((t, i) => t != passwordHash[i]).Any();
    }
    
    public static void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }
    
    #endregion
}