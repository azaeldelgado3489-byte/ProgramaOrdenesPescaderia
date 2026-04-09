using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PescaderiaAPP.Models;
using PescaderiaAPP.Services;

namespace PescaderiaAPP.Services
{
    public class OrdenesManager
    {
        public static OrdenesManager Instance { get; } = new();

        public List<OrdenMesa> OrdenesActivas { get; } = new();

        public List<OrdenMesa> Historial { get; } = new();

        public event Action? OrdenesActualizadas;

        public void GuardarOrden(OrdenMesa orden)
        {
            var existente = OrdenesActivas.FirstOrDefault(o => o.Mesa == orden.Mesa);

            if (existente != null)
            {
                OrdenesActivas.Remove(existente);
            }
            OrdenesActivas.Add(orden);
            OrdenesActualizadas?.Invoke();

        }

        public void CerrarOrden(string mesa)
        {
            var orden = OrdenesActivas.FirstOrDefault(o => o.Mesa == mesa);

            if (orden != null)
            {
                OrdenesActivas.Remove(orden);
                //Guardar en el historial
                Historial.Insert(0, orden);
                OrdenesActualizadas?.Invoke();
            }
        }

    }



















}
