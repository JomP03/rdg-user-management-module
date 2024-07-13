namespace App.Services.Mappers
{
    /// <summary>
    /// Interface a Mapper
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IMapper<TDomain, TIn, TOut>
    {
        /// <summary>
        /// Maps from domain to out dto.
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        TOut MapFromDomainToOutDto(TDomain domain);

        /// <summary>
        /// Maps from in dto to domain.
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        TDomain MapFromCreateDtoToDomain(TIn createDto);
    }
}