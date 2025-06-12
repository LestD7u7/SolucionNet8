using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using MediatR;

namespace BancaLafise.Application.Features.AdminCatalogos
{
    public class InsertarCatalogosBaseCommand : IRequest<string>
    {
    }

    public class InsertarCatalogosBaseCommandHandler : IRequestHandler<InsertarCatalogosBaseCommand, string>
    {
        private readonly ISexoRepository _sexoRepository;
        private readonly ITipoCuentaRepository _tipoCuentaRepository;
        private readonly IEstadoUsuarioRepository _estadoUsuarioRepository;
        private readonly ITipoMovimientoRepository _tipoMovimientoRepository;
        private readonly ITipoTransaccionRepository _tipoTransaccionRepository;
        private readonly IReglaCumplimientoRepository _reglaCumplimientoRepository;
        
        public InsertarCatalogosBaseCommandHandler(ISexoRepository sexoRepository,
            ITipoCuentaRepository tipoCuentaRepository,
            IEstadoUsuarioRepository estadoUsuarioRepository,
            ITipoMovimientoRepository tipoMovimientoRepository,
            ITipoTransaccionRepository tipoTransaccionRepository,
            IReglaCumplimientoRepository reglaCumplimientoRepository
            )
        { 
            _sexoRepository = sexoRepository;
            _tipoCuentaRepository = tipoCuentaRepository;
            _estadoUsuarioRepository = estadoUsuarioRepository;
            _tipoMovimientoRepository = tipoMovimientoRepository;
            _tipoTransaccionRepository = tipoTransaccionRepository;
            _reglaCumplimientoRepository = reglaCumplimientoRepository;
        }

        public async Task<string> Handle(InsertarCatalogosBaseCommand request, CancellationToken cancellationToken)
        {
            string mensaje = "";
            try
            {
                var sexos = new List<Sexo> 
                {
                    new() { Descripcion = "NO_DEFINIDO" },
                    new() { Descripcion = "MASCULINO" },
                    new() { Descripcion = "FEMENINO" }
                };

                await _sexoRepository.CreateRange(sexos, cancellationToken);

                var tiposCuenta = new List<TipoCuenta>
                {
                    new() { Descripcion = "AHORRO" },
                    new() { Descripcion = "CORRIENTE" },
                    new() { Descripcion = "BLACK_CARD" }
                };
                
                await _tipoCuentaRepository.CreateRange(tiposCuenta, cancellationToken);

                var estadosEntidad = new List<EstadoUsuario>
                {
                    new() { Descripcion = "INACTIVO" },
                    new() { Descripcion = "ACTIVO" },
                    new() { Descripcion = "ELIMINADO" }
                };

                await _estadoUsuarioRepository.CreateRange(estadosEntidad, cancellationToken);

                var tiposMovimiento = new List<TipoMovimiento>
                {
                    new() { Descripcion = "CREDITO" },
                    new() { Descripcion = "DEBITO" }
                };

                await _tipoMovimientoRepository.CreateRange(tiposMovimiento, cancellationToken);

                var tiposTransaccion = new List<TipoTransaccion>
                {
                    new() { Descripcion = "DEPOSITO TERCERO" , TipoMovimiento = tiposMovimiento.Where(x => x.Descripcion == "DEBITO").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "DEPOSITO CUENTAS PROPIAS", TipoMovimiento = tiposMovimiento.Where(x => x.Descripcion == "DEBITO").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "DEPOSITO EN EFECTIVO", TipoMovimiento = tiposMovimiento.Where(x => x.Descripcion == "CREDITO").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "RETIRO EN EFECTIVO", TipoMovimiento = tiposMovimiento.Where(x => x.Descripcion == "DEBITO").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "e", TipoMovimiento = tiposMovimiento.Where(x => x.Descripcion == "CREDITO").Select(x => x.Id).FirstOrDefault() },
                };

                await _tipoTransaccionRepository.CreateRange(tiposTransaccion, cancellationToken);

                var reglas = new List<ReglaCumplimiento>
                {
                    new() { Descripcion = "SALARIO MIN APERTURA", Valor = "1000", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "AHORRO").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MONTO INICIAL", Valor = "1000", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "AHORRO").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MAX DEBITOS POR DIA", Valor = "1", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "AHORRO").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MAX CREDITOS POR DIA", Valor = "10", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "AHORRO").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MONTO MAX POR DEBITO", Valor = "10000", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "AHORRO").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MONTO MAX POR CREDITO", Valor = "10000", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "AHORRO").Select(x => x.Id).FirstOrDefault() },

                    new() { Descripcion = "SALARIO MIN APERTURA", Valor = "500", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "CORRIENTE").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MONTO INICIAL", Valor = "500", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "CORRIENTE").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MAX DEBITOS POR DIA", Valor = "100", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "CORRIENTE").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MAX CREDITOS POR DIA", Valor = "100", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "CORRIENTE").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MONTO MAX POR DEBITO", Valor = "10000", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "CORRIENTE").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MONTO MAX POR CREDITO", Valor = "10000", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "CORRIENTE").Select(x => x.Id).FirstOrDefault() },
                    
                    new() { Descripcion = "SALARIO MIN APERTURA", Valor = "0", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "BLACK_CARD").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MONTO INICIAL", Valor = "1000000000", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "BLACK_CARD").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MAX DEBITOS POR DIA", Valor = "100000", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "BLACK_CARD").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MAX CREDITOS POR DIA", Valor = "100000", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "BLACK_CARD").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MONTO MAX POR DEBITO", Valor = "1000000", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "BLACK_CARD").Select(x => x.Id).FirstOrDefault() },
                    new() { Descripcion = "MONTO MAX POR CREDITO", Valor = "1000000", TipoCuenta = tiposCuenta.Where(x => x.Descripcion == "BLACK_CARD").Select(x => x.Id).FirstOrDefault() },
                };

                await _reglaCumplimientoRepository.CreateRange(reglas, cancellationToken);

                mensaje = "Catalogos insertados con exito en la base de datos!!!";
            }
            catch (Exception ex) 
            {
                mensaje = $"Error generado al insertar los catalogos iniciales {ex.Message} {ex.InnerException?.Message}";
            }

            return mensaje;
        }
    }
}
