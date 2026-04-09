using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PescaderiaAPP.Models
{
    public class OrdenMesa
    {
        public string Mesa { get; set; } = "";
        public List<ItemOrden> Items { get; set; } = new();

        public DateTime FechaHora { get; set; } = DateTime.Now;
        public decimal Total => Items.Sum(i => i.Precio);

        public string Resumen => Items
            .GroupBy(i => i.Nombre)
            .Select(g => $"  • {g.Key} x{g.Count()}  —  ${g.Sum(i => i.Precio):0.00}")
            .Aggregate((a, b) => $"{a}\n{b}");

        public string HoraFormato => FechaHora.ToString("HH:mm tt");
    }

    public class ItemOrden
    {
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
    }
















    }
