using MediatR;
using EmbPortal.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EmbPortal.Shared.Enums;

namespace Application.CQRS.Uoms.Query
{
    public class GetUomDimensionsQuery : IRequest<IReadOnlyList<UomDimensionResponse>>
    {
    }

    public class GetUomDimensionsQueryHandler : IRequestHandler<GetUomDimensionsQuery, IReadOnlyList<UomDimensionResponse>>
    {
        public Task<IReadOnlyList<UomDimensionResponse>> Handle(GetUomDimensionsQuery request, CancellationToken cancellationToken)
        {
            var accountTypes = new List<UomDimensionResponse>();
            foreach (var item in Enum.GetValues(typeof(UomDimension)))
            {
                accountTypes.Add(new UomDimensionResponse
                {
                    Id = (int)item,
                    Name = item.ToString()
                });
            }

            IReadOnlyList<UomDimensionResponse> accTypes = accountTypes;
            return Task.FromResult(accTypes);
        }
    }
}
