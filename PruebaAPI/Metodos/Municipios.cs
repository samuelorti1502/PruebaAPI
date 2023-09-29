using PruebaAPI.Conexion;
using PruebaAPI.Models;
using System.Data.SqlClient;

namespace PruebaAPI.Metodos
{
    public class Metodo_Municipios
    {
        ConexionDB conexion = new ConexionDB();

        private void AgregarParametro(SqlCommand cmd, string nombre, object valor)
        {
            if (valor == null)
            {
                cmd.Parameters.AddWithValue(nombre, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(nombre, valor);
            }
        }

        private async Task<List<MunicipiosModel>> EjecutarSP(int accion, int? id_municipio, string nombre, int id_departamento)
        {
            var lista = new List<MunicipiosModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDMunicipio", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id_municipio", id_municipio);
                AgregarParametro(cmd, "@nombre", nombre);
                AgregarParametro(cmd, "@id_departamento", id_departamento);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Municipio = new MunicipiosModel
                        {
                            id_municipio = (int)leer["id_municipio"],
                            nombre = (string)leer["nombre"],
                            id_departamento = (int)leer["id_departamento"]
                        };

                        lista.Add(M_Municipio);
                    }
                }
            }

            return lista;
        }

        public async Task<List<MunicipiosModel>> MostrarMunicipios()
        {
            return await EjecutarSP(5, 0, "", 0);
        }

        public async Task<List<MunicipiosModel>> MostrarMunicipio_id(int id_municipio)
        {
            return await EjecutarSP(4, id_municipio, "", 0);
        }


        public async Task InsertarMunicipio(MunicipiosModel parametros)
        {

            await EjecutarSP(1, 0, parametros.nombre, parametros.id_departamento);
        }

        public async Task ModificarMunicipio(MunicipiosModel parametros)
        {
            await EjecutarSP(2, parametros.id_municipio, parametros.nombre, parametros.id_departamento);
        }

        public async Task EliminarMunicipio(int id_municipio)
        {
            await EjecutarSP(3, id_municipio, "", 0);
        }
    }
}
