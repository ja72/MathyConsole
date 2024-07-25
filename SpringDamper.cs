namespace MathyConsole
{
    public interface IModel
    {
        Vec GetInitalState();
        Vec Rate(double time, Vec state);
    }

    public class SpringDamper : IModel
    {
        public SpringDamper(double mass, double stiffness, double damping)
        {
            Mass = mass;
            Stiffness=stiffness;
            Damping=damping;
        }
        public double Mass { get; set; }
        public double Stiffness { get; set; }
        public double Damping { get; set; }
        public Vec Rate(double time, Vec state)
        {
            double x = state[0], v = state[1];
            double m = Mass, k = Stiffness, c = Damping;
            return new Vec(
              v,
              -(k*x+c*v)/m
            );
        }
        public double InitalPosition { get; set; } = 1;
        public double InitalVelocity { get; set; } = 0;
        public Vec GetInitalState()
            => new Vec(InitalPosition, InitalVelocity);
        public double GetKineticEnergy(Vec state)
        {
            double x = state[0], v = state[1];
            double m = Mass, k = Stiffness, c = Damping;

            return 0.5*m*v*v;
        }
        public double GetPotentialEnergy(Vec state)
        {
            double x = state[0], v = state[1];
            double m = Mass, k = Stiffness, c = Damping;

            return 0.5*k*x*x;
        }
    }
}
