using App.Services.Dtos;
using Domain.Entities.Skeleton;

namespace App.Services.Mappers
{
    internal class SkeletonMapper : IMapper<Skeleton, CreateSkeletonRequestDto, SkeletonResponse>
    {
        public SkeletonResponse MapFromDomainToOutDto(Skeleton domain)
        {
            var outDto = new SkeletonResponse
            {
                Id = domain.Id.ToString(),
                Name = domain.Name.Value
            };

            return outDto;

        }

        public Skeleton MapFromCreateDtoToDomain(CreateSkeletonRequestDto createSkeletonRequestDto)
        {
            return new Skeleton(createSkeletonRequestDto.Name);
        }
    }
}
