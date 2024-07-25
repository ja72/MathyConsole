using System;
using System.Text;
using System.Threading.Tasks;

using static System.Math;

namespace MathyConsole
{
    public class Program
    {
        public const double pi = Math.PI;

        public static void Main(string[] args)
        {
            // Example code
            const double f = 5;
            const double m = 1.0, omg_n = f*(2*pi), zeta = 0.05;
            const double k = m*omg_n*omg_n, c = 2*zeta*m*omg_n;
            var model = new SpringDamper(m, k, c);
            const double t_end = 0.5/f;
            const int n_steps = 400;
            double dt = t_end/n_steps;
            ReportHead();
            var ode = new Ode(OdeScheme.Rk4, model);
            ReportState(model, ode);
            while (ode.Time < t_end)
            {
                ode.Step(ref dt);
                ReportState(model, ode);
            }
        }
        public static void ReportHead()
        {
            Console.WriteLine($"{"time [s]",12} {"pos [m]",12} {"vel [m/s]",12} {"pe [J]",12} {"ke [J]",12} {"tot [J]",12}");
        }
        public static void ReportState(IModel model, Ode ode)
        {
            double t = ode.Time, x = ode.State[0], v = ode.State[1];
            if (model is SpringDamper spr)
            {
                double pe = spr.GetPotentialEnergy(ode.State);
                double ke = spr.GetKineticEnergy(ode.State);
                double tot = pe + ke;
                Console.WriteLine($"{t,12:g6} {x,12:g6} {v,12:g6} {pe,12:g6} {ke,12:g6} {tot,12:g6}");
            }
            else
            {
                Console.WriteLine($"{t,12:g6} {x,12:g6} {v,12:g6}");
            }
        }
    }
}
