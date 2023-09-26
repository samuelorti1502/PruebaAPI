namespace PruebaAPI.Conexion
{
    public class ConexionDB
    {
        private String CadenaConexion = string.Empty;

        public ConexionDB()
        {

            var constructor = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            CadenaConexion = constructor.GetSection("ConnectionStrings:Conexion").Value;

        }

        public string GetConexion()
        {
            return CadenaConexion;
        }
    }

}
