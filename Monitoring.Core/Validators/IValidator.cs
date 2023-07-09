namespace Monitoring.Core.Validators
{
    public interface IValidator<T>
        where T : class
    {
        void ValidateOrThrow( T entity );
    }
}