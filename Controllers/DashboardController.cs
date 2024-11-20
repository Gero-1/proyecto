using MiradorB.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MiradorB.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Data;

namespace EcoGlam.Controllers
{
    
    public class DashboardController : Controller
    {
        private readonly BdMiradorContext _context;

        public DashboardController(BdMiradorContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Metodos

        public IActionResult resumenReservas(int formatoFecha)
        {
            DateTime fechaDateTime = DateTime.Today;
            DateOnly fechaInicio = DateOnly.FromDateTime(fechaDateTime).AddDays(-2);
            DateOnly FechaFin = DateOnly.FromDateTime(fechaDateTime);

            switch (formatoFecha)
            {
                case 1:
                    fechaInicio = ObtenerFechaInicioUltimosDias(fechaDateTime, 7);
                    break;
                case 2:
                    fechaInicio = ObtenerFechaInicioUltimoMes(fechaDateTime);
                    break;
                case 3:
                    fechaInicio = ObtenerFechaInicioUltimosMeses(fechaDateTime, 3);
                    break;
                case 4:
                    fechaInicio = ObtenerFechaInicioUltimoAnio(fechaDateTime);
                    break;
                case 5:
                    fechaInicio = ObtenerPrimerDiaMesActual(fechaDateTime);
                    break;
                case 6:
                    (fechaInicio, FechaFin) = ObtenerRangoSemanaActual(fechaDateTime);
                    break;
                default:
                    break;
            }

            List<DashReservaVM> Lista = new List<DashReservaVM>();

            if (formatoFecha == 3 || formatoFecha == 4)
            {
                CultureInfo culture = new CultureInfo("es-ES");

                for (DateOnly fecha = new DateOnly(fechaInicio.Year, fechaInicio.Month, 1); fecha <= FechaFin; fecha = fecha.AddMonths(1))
                {
                    int cantidadReservas = _context.Reservas
                        .Count(r => r.FechaReserva.Date.Year == fecha.Year && r.FechaReserva.Date.Month == fecha.Month);

                    string nombreMes = culture.DateTimeFormat.GetMonthName(fecha.Month);
                    Lista.Add(new DashReservaVM
                    {
                        Fecha = $"{nombreMes} {fecha.Year}",
                        Cantidad = cantidadReservas.ToString()
                    });
                }
            }
            else
            {
                for (DateOnly fecha = fechaInicio; fecha <= FechaFin; fecha = fecha.AddDays(1))
                {
                    int cantidadReservas = _context.Reservas
                 .Count(r => DateOnly.FromDateTime(r.FechaReserva) == fecha);


                    Lista.Add(new DashReservaVM
                    {
                        Fecha = fecha.ToString("dd-MM-yyyy"),
                        Cantidad = cantidadReservas.ToString()
                    });
                }
            }

            return Json(Lista);
        }

        public IActionResult resumenPaquetes(int formatoFecha)
        {
            DateTime fechaDateTime = DateTime.Today;
            DateOnly fechaInicio = DateOnly.FromDateTime(fechaDateTime).AddDays(-2);
            DateOnly FechaFin = DateOnly.FromDateTime(fechaDateTime);

            switch (formatoFecha)
            {
                case 1:
                    fechaInicio = ObtenerFechaInicioUltimosDias(fechaDateTime, 7);
                    break;
                case 2:
                    fechaInicio = ObtenerFechaInicioUltimoMes(fechaDateTime);
                    break;
                case 3:
                    fechaInicio = ObtenerFechaInicioUltimosMeses(fechaDateTime, 3);
                    break;
                case 4:
                    fechaInicio = ObtenerFechaInicioUltimoAnio(fechaDateTime);
                    break;
                case 5:
                    fechaInicio = ObtenerPrimerDiaMesActual(fechaDateTime);
                    break;
                case 6:
                    (fechaInicio, FechaFin) = ObtenerRangoSemanaActual(fechaDateTime);
                    break;
                default:
                    break;
            }



            var resultados = _context.Paquetes
            .Select(p => new
            {
             NombrePaquete = p.NomPaquete,
             CantidadReservas = p.DetalleReservaPaquetes.Count()
            }).ToList();

            List<DashPaqueteVM> lista = resultados.Select(r => new DashPaqueteVM
            {
                NombrePaquete = r.NombrePaquete,
                Cantidad = r.CantidadReservas.ToString()
            }).ToList();

            return Json(lista);

        }

        public IActionResult resumenServicios(int formatoFecha)
        {
            DateTime fechaDateTime = DateTime.Today;
            DateOnly fechaInicio = DateOnly.FromDateTime(fechaDateTime).AddDays(-2);
            DateOnly FechaFin = DateOnly.FromDateTime(fechaDateTime);

            switch (formatoFecha)
            {
                case 1:
                    fechaInicio = ObtenerFechaInicioUltimosDias(fechaDateTime, 7);
                    break;
                case 2:
                    fechaInicio = ObtenerFechaInicioUltimoMes(fechaDateTime);
                    break;
                case 3:
                    fechaInicio = ObtenerFechaInicioUltimosMeses(fechaDateTime, 3);
                    break;
                case 4:
                    fechaInicio = ObtenerFechaInicioUltimoAnio(fechaDateTime);
                    break;
                case 5:
                    fechaInicio = ObtenerPrimerDiaMesActual(fechaDateTime);
                    break;
                case 6:
                    (fechaInicio, FechaFin) = ObtenerRangoSemanaActual(fechaDateTime);
                    break;
                default:
                    break;
            }



            
            var resultados = _context.Servicios
                .Select(s => new
                {
                    NombreServicio = s.NomServicio, 
                    CantidadReservas = s.DetalleReservaServicios.Count() 
                }).ToList();

            
            List<DashServiciosVM> lista = resultados.Select(r => new DashServiciosVM
            {
                NombreServicio = r.NombreServicio,
                Cantidad = r.CantidadReservas.ToString()
            }).ToList();

            return Json(lista);

        }

        public IActionResult resumenTipoHabi(int formatoFecha)
        {
            DateTime fechaDateTime = DateTime.Today;
            DateOnly fechaInicio = DateOnly.FromDateTime(fechaDateTime).AddDays(-2);
            DateOnly FechaFin = DateOnly.FromDateTime(fechaDateTime);

            switch (formatoFecha)
            {
                case 1:
                    fechaInicio = ObtenerFechaInicioUltimosDias(fechaDateTime, 7);
                    break;
                case 2:
                    fechaInicio = ObtenerFechaInicioUltimoMes(fechaDateTime);
                    break;
                case 3:
                    fechaInicio = ObtenerFechaInicioUltimosMeses(fechaDateTime, 3);
                    break;
                case 4:
                    fechaInicio = ObtenerFechaInicioUltimoAnio(fechaDateTime);
                    break;
                case 5:
                    fechaInicio = ObtenerPrimerDiaMesActual(fechaDateTime);
                    break;
                case 6:
                    (fechaInicio, FechaFin) = ObtenerRangoSemanaActual(fechaDateTime);
                    break;
                default:
                    break;
            }



            
            var resultados = _context.TipoHabitaciones
                .Select(th => new
                {
                    NombreTipoHabitacion = th.NomTipoHabitacion, 
                    CantidadReservas = th.Habitaciones.Count() 
                }).ToList();

            
            List<DashTipoHabitacionVM> lista = resultados.Select(r => new DashTipoHabitacionVM
            {
                NombreTipoHabitacion = r.NombreTipoHabitacion,
                Cantidad = r.CantidadReservas.ToString()
            }).ToList();

            return Json(lista);

        }

        public IActionResult resumenEstadosReserva(int formatoFecha)
        {
            DateTime fechaDateTime = DateTime.Today;
            DateOnly fechaInicio = DateOnly.FromDateTime(fechaDateTime).AddDays(-2);
            DateOnly FechaFin = DateOnly.FromDateTime(fechaDateTime);

            switch (formatoFecha)
            {
                case 1:
                    fechaInicio = ObtenerFechaInicioUltimosDias(fechaDateTime, 7);
                    break;
                case 2:
                    fechaInicio = ObtenerFechaInicioUltimoMes(fechaDateTime);
                    break;
                case 3:
                    fechaInicio = ObtenerFechaInicioUltimosMeses(fechaDateTime, 3);
                    break;
                case 4:
                    fechaInicio = ObtenerFechaInicioUltimoAnio(fechaDateTime);
                    break;
                case 5:
                    fechaInicio = ObtenerPrimerDiaMesActual(fechaDateTime);
                    break;
                case 6:
                    (fechaInicio, FechaFin) = ObtenerRangoSemanaActual(fechaDateTime);
                    break;
                default:
                    break;
            }



            
            var resultados = _context.Reservas
                .GroupBy(r => r.IdEstadoReserva) 
                .Select(g => new
                {
                    NombreEstado = g.Key,                
                    CantidadReservas = g.Count()         
                })
                .ToList();

            // Ahora puedes usar 'resultados' en tu mapeo a DashEstadoReservaVM
            List<DashEstadoReservaVM> lista = resultados.Select(r => new DashEstadoReservaVM
            {
                NombreEstado = r.NombreEstado.ToString(),
                Cantidad = r.CantidadReservas.ToString()
            }).ToList();

            return Json(lista);

        }

        public IActionResult infoBasicaDash(int formatoFecha)
        {
            DateTime fechaDateTime = DateTime.Today;
            DateOnly fechaInicio = DateOnly.FromDateTime(fechaDateTime).AddDays(-2);
            DateOnly FechaFin = DateOnly.FromDateTime(fechaDateTime);

            switch (formatoFecha)
            {
                case 1:
                    fechaInicio = ObtenerFechaInicioUltimosDias(fechaDateTime, 7);
                    break;
                case 2:
                    fechaInicio = ObtenerFechaInicioUltimoMes(fechaDateTime);
                    break;
                case 3:
                    fechaInicio = ObtenerFechaInicioUltimosMeses(fechaDateTime, 3);
                    break;
                case 4:
                    fechaInicio = ObtenerFechaInicioUltimoAnio(fechaDateTime);
                    break;
                case 5:
                    fechaInicio = ObtenerPrimerDiaMesActual(fechaDateTime);
                    break;
                case 6:
                    (fechaInicio, FechaFin) = ObtenerRangoSemanaActual(fechaDateTime);
                    break;
                default:
                    break;
            }

            var reservasActivas = _context.Reservas
            .Count(r => DateOnly.FromDateTime(r.FechaReserva) >= fechaInicio && DateOnly.FromDateTime(r.FechaReserva) <= FechaFin &&
            r.IdEstadoReserva != 5 && r.IdEstadoReserva != 6);

            var reservasRealizadas = _context.Reservas
                .Count(r => DateOnly.FromDateTime(r.FechaReserva) >= fechaInicio && DateOnly.FromDateTime(r.FechaReserva) <= FechaFin);

            var reservasFinalizadas = _context.Reservas
                .Count(r => DateOnly.FromDateTime(r.FechaReserva) >= fechaInicio && DateOnly.FromDateTime(r.FechaReserva) <= FechaFin &&
                    r.IdEstadoReserva == 6);

            var Ingresos = _context.Abonos
                .Where(a => a.IdAbono != 0 && DateOnly.FromDateTime(a.IdReservaNavigation.FechaReserva) >= fechaInicio && DateOnly.FromDateTime(a.IdReservaNavigation.FechaReserva) <= FechaFin &&
                    (a.IdReservaNavigation.IdEstadoReserva == 5 || a.IdReservaNavigation.IdEstadoReserva == 6))
                .Sum(a => a.CantAbono);


            var info = new
            {
                ReservasActivas = reservasActivas,
                ReservasRealizadas = reservasRealizadas,
                ReservasFinalizadas = reservasFinalizadas,
                Ingresos = Ingresos
            };

            return Json(info);
        }

        //Obtencion de Fechas

        private DateOnly ObtenerFechaInicioUltimosDias(DateTime fechaDateTime, int cantidadDias)
        {
            return DateOnly.FromDateTime(fechaDateTime).AddDays(-cantidadDias);
        }

        private DateOnly ObtenerFechaInicioUltimoMes(DateTime fechaDateTime)
        {
            return DateOnly.FromDateTime(fechaDateTime.AddMonths(-1));
        }

        private DateOnly ObtenerFechaInicioUltimosMeses(DateTime fechaDateTime, int cantidadMeses)
        {
            return DateOnly.FromDateTime(fechaDateTime.AddMonths(-cantidadMeses + 1));
        }

        private DateOnly ObtenerFechaInicioUltimoAnio(DateTime fechaDateTime)
        {
            return DateOnly.FromDateTime(fechaDateTime.AddYears(-1));
        }

        private DateOnly ObtenerPrimerDiaMesActual(DateTime fechaDateTime)
        {
            return new DateOnly(fechaDateTime.Year, fechaDateTime.Month, 1);
        }

        private (DateOnly, DateOnly) ObtenerRangoSemanaActual(DateTime fechaDateTime)
        {
            DayOfWeek primerDiaSemana = DayOfWeek.Monday;
            int diferenciaDias = (fechaDateTime.DayOfWeek - primerDiaSemana + 7) % 7;
            DateTime primerDiaSemanaActual = fechaDateTime.AddDays(-diferenciaDias);
            DateTime ultimoDiaSemanaActual = primerDiaSemanaActual.AddDays(6);
            return (DateOnly.FromDateTime(primerDiaSemanaActual), DateOnly.FromDateTime(ultimoDiaSemanaActual));
        }

    }
}
