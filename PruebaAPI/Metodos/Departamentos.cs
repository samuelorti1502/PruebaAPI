using PruebaAPI.Conexion;
using PruebaAPI.Models;
using System.Data.SqlClient;

namespace PruebaAPI.Metodos
{
    public class Metodo_Departamento
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

        private async Task<List<DepartamentosModel>> EjecutarSP(int accion, int? id, string nombre)
        {
            var lista = new List<DepartamentosModel>();

            using (var sql = new SqlConnection(conexion.GetConexion()))
            using (var cmd = new SqlCommand("sp_CRUDDepartamento", sql))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                AgregarParametro(cmd, "@operacion", accion);
                AgregarParametro(cmd, "@id_departamento", id);
                AgregarParametro(cmd, "@nombre", nombre);

                await sql.OpenAsync();

                using (var leer = await cmd.ExecuteReaderAsync())
                {
                    while (await leer.ReadAsync())
                    {
                        var M_Departamento = new DepartamentosModel
                        {
                            operacion = accion,
                            id_departamento = (int)leer["id_departamento"],
                            nombre = (string)leer["nombre"]
                        };

                        lista.Add(M_Departamento);
                    }
                }
            }

            return lista;
        }

        public async Task<List<DepartamentosModel>> MostrarDepartamentos()
        {
            return await EjecutarSP(5, 0, "");
        }
        public async Task<List<DepartamentosModel>> MostrarDepartamento_id(int id)
        {
            return await EjecutarSP(4, id, "");
        }

        public async Task insertarDepartamento(DepartamentosModel parametros)
        {
            await EjecutarSP(1, null, parametros.nombre);
        }

        public async Task actualizarDepartamento(DepartamentosModel parametros)
        {
            await EjecutarSP(2, parametros.id_departamento, parametros.nombre);
        }
        public async Task eliminarDepartamento(DepartamentosModel parametros)
        {
            await EjecutarSP(3, parametros.id_departamento, "");
        }
    }
}
