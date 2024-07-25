using System;

namespace MathyConsole
{

    public delegate Vec FirstOrder(double t, Vec y);
    public delegate Vec SecondOrder(double t, Vec y, Vec yp);

    public enum OdeScheme
    {
        Euler,
        Midpoint,
        Rk4,
    }

    public class Ode
    {
        public Ode(OdeScheme scheme, IModel model)
            : this(scheme, model.Rate, model.GetInitalState()) { }
        public Ode(OdeScheme scheme, FirstOrder rate, Vec initialState)
        {
            Scheme = scheme;
            Rate = rate;
            Reset(initialState);
        }
        public double Time { get; set; }
        public Vec State { get; set; }
        public FirstOrder Rate { get; }
        public OdeScheme Scheme { get; set; }

        public void Reset(Vec initialState)
        {
            Time = 0;
            State = initialState;
        }

        public void Step(ref double timeStep)
        {
            switch (Scheme)
            {
                case OdeScheme.Euler:
                State = EulerOdeStep(Rate, Time, State, timeStep);
                break;
                case OdeScheme.Midpoint:
                State = MidpointOdeStep(Rate, Time, State, timeStep);
                break;
                case OdeScheme.Rk4:
                State = Rk4OdeStep(Rate, Time, State, timeStep);
                break;
                default:
                throw new NotSupportedException();
            }
            Time += timeStep;
        }

        public static Vec EulerOdeStep(FirstOrder f, double time, Vec state, double h)
        {
            return state + h * f(time, state);
        }
        public static Vec MidpointOdeStep(FirstOrder f, double time, Vec state, double h)
        {
            Vec K0 = h * f(time, state);
            Vec K1 = h * f(time + h, state + (h) * K0);
            return state + (K0 + K1) / 2;
        }
        public static Vec Rk4OdeStep(FirstOrder f, double time, Vec state, double h)
        {
            Vec K0 = h * f(time, state);
            Vec K1 = h * f(time + h / 2, state + (h / 2) * K0);
            Vec K2 = h * f(time + h / 2, state + (h / 2) * K1);
            Vec K3 = h * f(time + h, state + (h) * K2);

            Vec result = state.Copy();

            for (int i = 0; i < result.Count; i++)
            {
                result[i] += (K0[i] + 2 * K1[i] + 2 * K2[i] + K3[i])/6;
            }

            return result;
        }
    }
}
