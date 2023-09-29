using PruebaAPI.Conexion;
using PruebaAPI.Models;
using System.Data.SqlClient;

namespace PruebaAPI.Metodos
{
    public class MetodosTipoPersona
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

        private async Task<List<ModeloTipoPersona>> EjecutarSP(int accion, int? id, string tipoPersona, int idEstatus, string usuarioCreacion)
        {
            var lista = new List<ModeloTipoPersona>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDTipoPersona", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id_tipo_persona", id);
                AgregarParametro(cmd, "@tipo_persona", tipoPersona);
                AgregarParametro(cmd, "@id_estatus", idEstatus);
                AgregarParametro(cmd, "@usuario_creacion", usuarioCreacion);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_TipoPersona = new ModeloTipoPersona
                        {
                            opcion = accion,
                            id_tipo_persona = (int)leer["id_tipo_persona"],
                            tipo_persona = (String)leer["tipo_persona"],
                            id_estatus = (int)leer["id_estatus"],
                            usuario_creacion = (string)leer["usuario_creacion"]
                        };

                        lista.Add(M_TipoPersona);
                    }
                }
            }

            return lista;
        }

        public async Task<List<ModeloTipoPersona>> MostrarTipoPersona()
        {
            return await EjecutarSP(5, 0, "", 0, "");
        }

        public async Task<List<ModeloTipoPersona>> MostrarTipoPersonaId(int id)
        {

            return await EjecutarSP(4, id, "", 0, "");
        }

        public async Task<List<ModeloTipoPersona>> InsertarTipoPersona(ModeloTipoPersona parametros)
        {
            return await EjecutarSP(1, 0, parametros.tipo_persona, parametros.id_estatus, parametros.usuario_creacion);
        }

        public async Task<List<ModeloTipoPersona>> ActualizarTipoPersona(ModeloTipoPersona parametros)
        {
            return await EjecutarSP(2, parametros.id_tipo_persona, parametros.tipo_persona, parametros.id_estatus, parametros.usuario_creacion);
        }

        public async Task<List<ModeloTipoPersona>> EliminarTipoPerona(ModeloTipoPersona parametros)
        {
            return await EjecutarSP(3, parametros.id_tipo_persona, "", 0, parametros.usuario_creacion);
        }
    }
}
