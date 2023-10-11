using Microsoft.AspNetCore.Mvc;
using RestauranteAPI.Conn;
using RestauranteAPI.Models;
using System.Data.SqlClient;

public class Metodos_Facturas
{
    private ConexionDB conexion = new ConexionDB();

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

    private async Task<List<FacturasModel>> EjecutarSP(int accion, int? id_factura, int? id_restaurante, string? serie, int? numero, string? fecha_fact, string? nit, int? direccion, int? id_cupon, int? id_estatus, int? usuario_creacion)
    {
        var lista = new List<FacturasModel>();

        using (var sql = new SqlConnection(conexion.GetConexion()))
        using (var cmd = new SqlCommand("sp_CRUDFactura", sql))
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            AgregarParametro(cmd, "@operacion", accion);
            AgregarParametro(cmd, "@id_factura", id_factura);
            AgregarParametro(cmd, "@id_restaurante", id_restaurante);
            AgregarParametro(cmd, "@serie", serie);
            AgregarParametro(cmd, "@numero", numero);
            AgregarParametro(cmd, "@fecha_fact", fecha_fact);
            AgregarParametro(cmd, "@nit", nit);
            AgregarParametro(cmd, "@direccion", direccion);
            AgregarParametro(cmd, "@id_cupon", id_cupon);
            AgregarParametro(cmd, "@id_estatus", id_estatus);
            AgregarParametro(cmd, "@usuario_creacion", usuario_creacion);

            await sql.OpenAsync();

            using (var leer = await cmd.ExecuteReaderAsync())
            {
                while (await leer.ReadAsync())
                {
                    var M_Rol = new FacturasModel
                    {
                        id_factura = (int)leer["id"],
                        id_restaurante = (int)leer["id_restaurante"],
                        serie = (string)leer["serie"],
                        numero = (int)leer["numero"],
                        fecha_fact = (string)leer["fecha_fact"],
                        nit = (string)leer["nit"],
                        direccion = (int)leer["direccion"],
                        id_cupon = (int)leer["id_cupon"],
                        id_estatus = (int)leer["id_estatus"],
                        usuario_creacion = (int)leer["usuario_creacion"]
                    };

                    lista.Add(M_Rol);
                }
            }
        }

        return lista;
    }

    public async Task<List<FacturasModel>> MostrarRoles()
    {
        return await EjecutarSP(4, 0, 0, "", 0, "", "", 0, 0, 0, 0);
    }
    public async Task<List<FacturasModel>> MostrarRol_id(int id)
    {

        return await EjecutarSP(5, id, 0, "", 0, "", "", 0, 0, 0, 0);
    }
    public async Task InsertarRol(FacturasModel parametros)
    {
        await EjecutarSP(1, 0, parametros.id_restaurante, parametros.serie, parametros.numero, parametros.fecha_fact, parametros.nit, parametros.direccion, parametros.id_cupon, parametros.id_estatus, parametros.usuario_creacion);
    }
    public async Task ModificarRol(FacturasModel parametros)
    {
        await EjecutarSP(2, parametros.id_factura, parametros.id_restaurante, parametros.serie, parametros.numero, parametros.fecha_fact, parametros.nit, parametros.direccion, parametros.id_cupon, parametros.id_estatus, parametros.usuario_creacion);
    }

    public async Task EliminarRol(FacturasModel parametros)
    {
        await EjecutarSP(3, parametros.id_factura, 0, "", 0, "", "", 0, 0, 0, 0);
    }
}

