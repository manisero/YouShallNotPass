namespace Manisero.YouShallNotPass
{
    public class ValidationContext
    {
        public ISubvalidationEngine Engine { get; set; }

        public IReadonlyValidationData Data { get; set; }
    }
}
