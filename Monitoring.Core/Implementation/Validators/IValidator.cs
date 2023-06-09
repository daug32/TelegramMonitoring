namespace Monitoring.Core.Implementation.Validators
{
    public interface IValidator<T>
        where T : class
    {
        void ValidateOrThrow( T config );
    }
}